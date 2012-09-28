//-----------------------------------------------------------------------------
// <copyright file="ServiceResolver.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    internal sealed class ServiceResolver
    {
        private ServiceCollection serviceCollection;
        private IDictionary<MethodType, IEnumerable<ResolvedService>> serviceLookup;

        public ServiceResolver(ServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException("serviceCollection", "serviceCollection cannot be null.");
            }

            this.serviceCollection = serviceCollection;
            this.serviceLookup = ServiceResolver.ResolveAllServices(serviceCollection);
        }

        public bool ExistsForAnyMethodType(string url)
        {
            IDictionary<string, object> routeValues;

            foreach (ResolvedService service in this.serviceLookup.Values.SelectMany(c => c))
            {
                routeValues = service.Method.Endpoint.Route.Match(url);

                if (routeValues != null)
                {
                    if (ServiceResolver.RoutePatternsMatch(service, routeValues))
                    {
                        return this.serviceCollection.RouteValueBinder.Bind(routeValues, service.Method.Endpoint.ParameterTypes) != null;
                    }
                }
            }

            return false;
        }

        public ResolvedService Find(MethodType methodType, string url)
        {
            IDictionary<string, object> routeValues;

            foreach (ResolvedService service in this.serviceLookup[methodType])
            {
                routeValues = service.Method.Endpoint.Route.Match(url);

                if (routeValues != null)
                {
                    if (ServiceResolver.RoutePatternsMatch(service, routeValues))
                    {
                        routeValues = this.serviceCollection.RouteValueBinder.Bind(routeValues, service.Method.Endpoint.ParameterTypes);

                        if (routeValues != null)
                        {
                            return new ResolvedService(service, routeValues);
                        }
                    }
                }
            }

            return null;
        }

        private static IDictionary<MethodType, IEnumerable<ResolvedService>> ResolveAllServices(ServiceCollection serviceCollection)
        {
            const string MultipleMethodsMessage = "There are multiple {0} methods registered for endpoint {1}. You may only register one method type per endpoint.";

            List<ResolvedService> deleteServices = new List<ResolvedService>();
            List<ResolvedService> getServices = new List<ResolvedService>();
            List<ResolvedService> postServices = new List<ResolvedService>();
            List<ResolvedService> putServices = new List<ResolvedService>();

            if (serviceCollection != null)
            {
                foreach (Service service in serviceCollection)
                {
                    foreach (Endpoint endpoint in service.Endpoints as EndpointCollection)
                    {
                        bool hasDelete = false, hasGet = false, hasPost = false, hasPut = false;

                        foreach (Method method in endpoint.Methods as MethodCollection)
                        {
                            IEnumerable<FilterAction> beforeActions = ServiceResolver.ResolveBeforeActions(serviceCollection.Pipeline, service.Pipeline, endpoint.Pipeline, method.Pipeline);
                            IEnumerable<FilterAction> afterActions = ServiceResolver.ResolveAfterActions(serviceCollection.Pipeline, service.Pipeline, endpoint.Pipeline, method.Pipeline);
                            IEnumerable<FilterAction> errorActions = ServiceResolver.ResolveErrorActions(serviceCollection.Pipeline, service.Pipeline, endpoint.Pipeline, method.Pipeline);

                            ResolvedService resolvedService = new ResolvedService(
                                service.Name,
                                method,
                                afterActions,
                                beforeActions,
                                ServiceResolver.ResolveEncodings(serviceCollection.Pipeline, service.Pipeline, endpoint.Pipeline, method.Pipeline),
                                errorActions,
                                ServiceResolver.ResolveFormats(serviceCollection.Pipeline, service.Pipeline, endpoint.Pipeline, method.Pipeline),
                                ServiceResolver.ResolveRequestType(method, beforeActions, afterActions, errorActions));

                            bool throwMultipleMethods = false;

                            switch (method.MethodType)
                            {
                                case MethodType.Delete:
                                    deleteServices.Add(resolvedService);
                                    throwMultipleMethods = hasDelete;
                                    hasDelete = true;
                                    break;
                                case MethodType.Get:
                                    getServices.Add(resolvedService);
                                    throwMultipleMethods = hasGet;
                                    hasGet = true;
                                    break;
                                case MethodType.Post:
                                    postServices.Add(resolvedService);
                                    throwMultipleMethods = hasPost;
                                    hasPost = true;
                                    break;
                                case MethodType.Put:
                                    putServices.Add(resolvedService);
                                    throwMultipleMethods = hasPut;
                                    hasPut = true;
                                    break;
                                default:
                                    throw new NotImplementedException();
                            }

                            if (throwMultipleMethods)
                            {
                                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, MultipleMethodsMessage, method.MethodType, endpoint.Route));
                            }
                        }
                    }
                }
            }

            Dictionary<MethodType, IEnumerable<ResolvedService>> result = new Dictionary<MethodType, IEnumerable<ResolvedService>>();
            result.Add(MethodType.Delete, deleteServices);
            result.Add(MethodType.Get, getServices);
            result.Add(MethodType.Post, postServices);
            result.Add(MethodType.Put, putServices);
            return result;
        }

        private static IEnumerable<FilterAction> ResolveActions(
            IEnumerable<FilterAction> hostActions,
            IEnumerable<FilterAction> serviceExcludeActions,
            IEnumerable<FilterAction> serviceActions,
            IEnumerable<FilterAction> endpointExcludeActions,
            IEnumerable<FilterAction> endpointActions,
            IEnumerable<FilterAction> methodExcludeActions,
            IEnumerable<FilterAction> methodActions)
        {
            List<FilterAction> actions = new List<FilterAction>(hostActions);

            actions.RemoveAll(a => serviceExcludeActions.Any(ea => a.Equals(ea)));
            actions.RemoveAll(a => serviceActions.Any(sa => a.Equals(sa)));
            actions.AddRange(serviceActions);

            actions.RemoveAll(a => endpointExcludeActions.Any(ea => a == ea));
            actions.RemoveAll(a => endpointActions.Any(ea => a == ea));
            actions.AddRange(endpointActions);

            actions.RemoveAll(a => methodExcludeActions.Any(ea => a.Equals(ea)));
            actions.RemoveAll(a => methodActions.Any(ma => a.Equals(ma)));
            actions.AddRange(methodActions);

            return actions;
        }

        private static IEnumerable<FilterAction> ResolveAfterActions(Pipeline hostPipeline, Pipeline servicePipeline, Pipeline endpointPipeline, Pipeline methodPipeline)
        {
            return ServiceResolver.ResolveActions(
                hostPipeline.AfterActions,
                servicePipeline.ExcludeAfterActions,
                servicePipeline.AfterActions,
                endpointPipeline.ExcludeAfterActions,
                endpointPipeline.AfterActions,
                methodPipeline.ExcludeAfterActions,
                methodPipeline.AfterActions);
        }

        private static IEnumerable<FilterAction> ResolveBeforeActions(Pipeline hostPipeline, Pipeline servicePipeline, Pipeline endpointPipeline, Pipeline methodPipeline)
        {
            return ServiceResolver.ResolveActions(
                hostPipeline.BeforeActions,
                servicePipeline.ExcludeBeforeActions,
                servicePipeline.BeforeActions,
                endpointPipeline.ExcludeBeforeActions,
                endpointPipeline.BeforeActions,
                methodPipeline.ExcludeBeforeActions,
                methodPipeline.BeforeActions);
        }

        private static IEnumerable<IEncoding> ResolveEncodings(Pipeline hostPipeline, Pipeline servicePipeline, Pipeline endpointPipeline, Pipeline methodPipeline)
        {
            List<IEncoding> encodings = new List<IEncoding>(hostPipeline.Encodings);

            encodings.RemoveAll(e => servicePipeline.ExcludeEncodings.Any(ee => e.Equals(ee)));
            encodings.RemoveAll(e => servicePipeline.Encodings.Any(se => e.Equals(se)));
            encodings.AddRange(servicePipeline.Encodings);

            encodings.RemoveAll(e => endpointPipeline.ExcludeEncodings.Any(ee => e.Equals(ee)));
            encodings.RemoveAll(e => endpointPipeline.Encodings.Any(ee => e.Equals(ee)));
            encodings.AddRange(endpointPipeline.Encodings);

            encodings.RemoveAll(e => methodPipeline.ExcludeEncodings.Any(ee => e.Equals(ee)));
            encodings.RemoveAll(e => methodPipeline.Encodings.Any(me => e.Equals(me)));
            encodings.AddRange(methodPipeline.Encodings);

            encodings.Reverse();
            return encodings;
        }

        private static IEnumerable<FilterAction> ResolveErrorActions(Pipeline hostPipeline, Pipeline servicePipeline, Pipeline endpointPipeline, Pipeline methodPipeline)
        {
            return ServiceResolver.ResolveActions(
                hostPipeline.ErrorActions,
                servicePipeline.ExcludeErrorActions,
                servicePipeline.ErrorActions,
                endpointPipeline.ExcludeErrorActions,
                endpointPipeline.ErrorActions,
                methodPipeline.ExcludeErrorActions,
                methodPipeline.ErrorActions);
        }

        private static IEnumerable<IFormat> ResolveFormats(Pipeline hostPipeline, Pipeline servicePipeline, Pipeline endpointPipeline, Pipeline methodPipeline)
        {
            List<IFormat> formats = new List<IFormat>(hostPipeline.Formats);

            formats.RemoveAll(f => servicePipeline.ExcludeFormats.Any(ef => f.Equals(ef)));
            formats.RemoveAll(f => servicePipeline.Formats.Any(sf => f.Equals(sf)));
            formats.AddRange(servicePipeline.Formats);

            formats.RemoveAll(f => endpointPipeline.ExcludeFormats.Any(ef => f.Equals(ef)));
            formats.RemoveAll(f => endpointPipeline.Formats.Any(ef => f.Equals(ef)));
            formats.AddRange(endpointPipeline.Formats);

            formats.RemoveAll(f => methodPipeline.ExcludeFormats.Any(ef => f.Equals(ef)));
            formats.RemoveAll(f => methodPipeline.Formats.Any(mf => f.Equals(mf)));
            formats.AddRange(methodPipeline.Formats);

            formats.Reverse();
            return formats;
        }

        private static Type ResolveRequestType(Method method, IEnumerable<FilterAction> beforeActions, IEnumerable<FilterAction> afterActions, IEnumerable<FilterAction> errorActions)
        {
            Type type = method.TypeArguments.FirstOrDefault();

            if (type == null)
            {
                foreach (FilterAction filter in beforeActions)
                {
                    type = filter.TypeArguments.FirstOrDefault();

                    if (type != null)
                    {
                        break;
                    }
                }

                if (type == null)
                {
                    foreach (FilterAction filter in afterActions)
                    {
                        type = filter.TypeArguments.FirstOrDefault();

                        if (type != null)
                        {
                            break;
                        }
                    }

                    if (type == null)
                    {
                        foreach (FilterAction filter in errorActions)
                        {
                            type = filter.TypeArguments.FirstOrDefault();

                            if (type != null)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return type;
        }

        private static bool RoutePatternsMatch(ResolvedService service, IDictionary<string, object> routeValues)
        {
            bool success = true;

            foreach (KeyValuePair<string, object> pair in routeValues)
            {
                if (service.Method.Endpoint.ParameterPatterns.ContainsKey(pair.Key))
                {
                    if (!service.Method.Endpoint.ParameterPatterns[pair.Key].IsMatch(pair.Value as string ?? string.Empty))
                    {
                        success = false;
                        break;
                    }
                }
            }

            return success;
        }
    }
}