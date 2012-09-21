//-----------------------------------------------------------------------------
// <copyright file="ResolvedService.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    internal sealed class ResolvedService
    {
        public ResolvedService(
            string name,
            Method method,
            IEnumerable<FilterAction> afterActions,
            IEnumerable<FilterAction> beforeActions,
            IEnumerable<IEncoding> encodings,
            IEnumerable<FilterAction> errorActions,
            IEnumerable<IFormat> formats,
            Type requestType)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method", "method cannot be null.");
            }

            if (afterActions == null)
            {
                throw new ArgumentNullException("afterActions", "afterActions cannot be null.");
            }

            if (beforeActions == null)
            {
                throw new ArgumentNullException("beforeActions", "beforeActions cannot be null.");
            }

            if (encodings == null)
            {
                throw new ArgumentNullException("encodings", "encodings cannot be null.");
            }

            if (errorActions == null)
            {
                throw new ArgumentNullException("errorActions", "errorActions cannot be null.");
            }

            if (formats == null)
            {
                throw new ArgumentNullException("formats", "formats cannot be null.");
            }

            this.Name = name ?? string.Empty;
            this.Method = method;
            this.AfterActions = afterActions;
            this.BeforeActions = beforeActions;
            this.Encodings = encodings;
            this.ErrorActions = errorActions;
            this.Formats = formats;
            this.RequestType = requestType;
            this.RouteValues = new Dictionary<string, object>();
        }

        public ResolvedService(ResolvedService service, IDictionary<string, object> routeValues)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service", "service cannot be null.");
            }

            this.AfterActions = new List<FilterAction>(service.AfterActions);
            this.BeforeActions = new List<FilterAction>(service.BeforeActions);
            this.Encodings = new List<IEncoding>(service.Encodings);
            this.ErrorActions = new List<FilterAction>(service.ErrorActions);
            this.Formats = new List<IFormat>(service.Formats);
            this.Method = service.Method;
            this.Name = service.Name;
            this.RequestType = service.RequestType;
            this.RouteValues = routeValues ?? new Dictionary<string, object>();
        }

        public IEnumerable<FilterAction> AfterActions { get; private set; }

        public IEnumerable<FilterAction> BeforeActions { get; private set; }

        public IEnumerable<IEncoding> Encodings { get; private set; }

        public IEnumerable<FilterAction> ErrorActions { get; private set; }

        public IEnumerable<IFormat> Formats { get; private set; }

        public Method Method { get; private set; }

        public string Name { get; private set; }

        public Type RequestType { get; private set; }

        public IDictionary<string, object> RouteValues { get; private set; }

        public EncodingLookupResult GetRequestDecoder(string contentEncoding)
        {
            EncodingLookupResult result = null;

            foreach (EncodingType encodingType in contentEncoding.AsEncodingTypes())
            {
                foreach (IEncoding encoding in this.Encodings)
                {
                    if (encoding.CanDecode(encodingType))
                    {
                        result = new EncodingLookupResult()
                        {
                            Encoding = encoding,
                            EncodingType = encodingType
                        };

                        break;
                    }
                }
            }

            return result ?? new EncodingLookupResult() { Encoding = new IdentityEncoding(), EncodingType = EncodingType.Empty };
        }

        public FormatLookupResult GetRequestDeserializer(string contentType)
        {
            FormatLookupResult result = null;

            foreach (MediaType mediaType in contentType.AsMediaTypes())
            {
                foreach (IFormat format in this.Formats)
                {
                    if (format.CanDeserialize(mediaType))
                    {
                        result = new FormatLookupResult()
                        {
                            Format = format,
                            MediaType = mediaType
                        };

                        break;
                    }
                }
            }

            return result ?? new FormatLookupResult() { Format = new PlainTextFormat(), MediaType = MediaType.Empty };
        }

        public EncodingLookupResult GetResponseEncoder(string acceptEncoding)
        {
            EncodingLookupResult result = null;

            foreach (EncodingType encodingType in acceptEncoding.AsEncodingTypes())
            {
                foreach (IEncoding encoding in this.Encodings)
                {
                    if (encoding.CanEncode(encodingType))
                    {
                        result = new EncodingLookupResult()
                        {
                            Encoding = encoding,
                            EncodingType = encodingType
                        };

                        break;
                    }
                }
            }

            return result ?? new EncodingLookupResult() { Encoding = new IdentityEncoding(), EncodingType = EncodingType.Empty };
        }

        public FormatLookupResult GetResponseSerializer(string accept)
        {
            FormatLookupResult result = null;

            foreach (MediaType mediaType in accept.AsMediaTypes())
            {
                foreach (IFormat format in this.Formats)
                {
                    if (format.CanSerialize(mediaType))
                    {
                        result = new FormatLookupResult()
                        {
                            Format = format,
                            MediaType = mediaType
                        };

                        break;
                    }
                }
            }

            return result ?? new FormatLookupResult() { Format = new PlainTextFormat(), MediaType = MediaType.Empty };
        }

        public ICollection<FilterActionResult> InvokeAfterActions(IRequestMessage request, IResponseMessage response)
        {
            return ResolvedService.InvokeActions(this.AfterActions, request, response, null);
        }

        public ICollection<FilterActionResult> InvokeBeforeActions(IRequestMessage request, IResponseMessage response)
        {
            return ResolvedService.InvokeActions(this.BeforeActions, request, response, null);
        }

        public ICollection<FilterActionResult> InvokeErrors(IRequestMessage request, IResponseMessage response, IEnumerable<Exception> exceptions)
        {
            return ResolvedService.InvokeActions(this.ErrorActions, request, response, exceptions);
        }

        public bool TryReadRequestObject(string contentEncoding, string contentType, Stream inputStream, out object requestObject, out Exception exception)
        {
            bool success = true;
            requestObject = null;
            exception = null;

            try
            {
                EncodingLookupResult encoding = this.GetRequestDecoder(contentEncoding);
                FormatLookupResult format = this.GetRequestDeserializer(contentType);

                using (MemoryStream stream = new MemoryStream())
                {
                    encoding.Encoding.Decode(encoding.EncodingType, inputStream, stream);
                    stream.Position = 0;
                    requestObject = format.Format.Deserialize(format.MediaType, this.RequestType, stream);
                }
            }
            catch (Exception ex)
            {
                success = false;
                exception = ex;

                IDisposable d = requestObject as IDisposable;

                if (d != null)
                {
                    d.Dispose();
                }

                requestObject = null;
            }

            return success;
        }

        public bool TryWriteResponseObject(string acceptEncoding, string accept, object responseObject, Stream outputStream, out Exception exception)
        {
            bool success = true;
            exception = null;

            try
            {
                EncodingLookupResult encoding = this.GetResponseEncoder(acceptEncoding);
                FormatLookupResult format = this.GetResponseSerializer(accept);

                using (MemoryStream stream = new MemoryStream())
                {
                    format.Format.Serialize(format.MediaType, responseObject, stream);
                    stream.Position = 0;
                    encoding.Encoding.Encode(encoding.EncodingType, stream, outputStream);
                }
            }
            catch (Exception ex)
            {
                success = false;
                exception = ex;
            }

            return success;
        }

        private static ICollection<FilterActionResult> InvokeActions(IEnumerable<FilterAction> actions, IRequestMessage request, IResponseMessage response, IEnumerable<Exception> exceptions)
        {
            List<FilterActionResult> results = new List<FilterActionResult>();

            foreach (FilterAction action in actions)
            {
                FilterActionResult result = action.Invoke(request, response, exceptions);
                results.Add(result);

                if (!result.Continue)
                {
                    break;
                }
            }

            return results;
        }
    }
}