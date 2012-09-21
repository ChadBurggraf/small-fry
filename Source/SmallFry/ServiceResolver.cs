//-----------------------------------------------------------------------------
// <copyright file="ServiceResolver.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;
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

        public ResolvedService Find(MethodType methodType, string url)
        {
            IDictionary<string, object> routeValues;

            foreach (ResolvedService service in this.serviceLookup[methodType])
            {
                routeValues = service.Method.Endpoint.Route.Match(url);

                if (routeValues != null)
                {
                    routeValues = this.serviceCollection.RouteValueBinder.Bind(routeValues, service.Method.Endpoint.ParameterTypes);

                    if (routeValues != null)
                    {
                        return new ResolvedService(service, routeValues);
                    }
                }
            }

            return null;
        }

        private static IDictionary<MethodType, IEnumerable<ResolvedService>> ResolveAllServices(ServiceCollection serviceCollection)
        {
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
                        foreach (Method method in endpoint.Methods as MethodCollection)
                        {
                            ResolvedService resolvedService = new ResolvedService(
                                method,
                                ServiceResolver.ResolveAfterActions(serviceCollection.Pipeline, service.Pipeline, endpoint.Pipeline, method.Pipeline),
                                ServiceResolver.ResolveBeforeActions(serviceCollection.Pipeline, service.Pipeline, endpoint.Pipeline, method.Pipeline),
                                ServiceResolver.ResolveEncodings(serviceCollection.Pipeline, service.Pipeline, endpoint.Pipeline, method.Pipeline),
                                ServiceResolver.ResolveErrorActions(serviceCollection.Pipeline, service.Pipeline, endpoint.Pipeline, method.Pipeline),
                                ServiceResolver.ResolveFormats(serviceCollection.Pipeline, service.Pipeline, endpoint.Pipeline, method.Pipeline));

                            switch (method.MethodType)
                            {
                                case MethodType.Delete:
                                    deleteServices.Add(resolvedService);
                                    break;
                                case MethodType.Get:
                                    getServices.Add(resolvedService);
                                    break;
                                case MethodType.Post:
                                    postServices.Add(resolvedService);
                                    break;
                                case MethodType.Put:
                                    putServices.Add(resolvedService);
                                    break;
                                default:
                                    throw new NotImplementedException();
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

            return formats;
        }
    }
}