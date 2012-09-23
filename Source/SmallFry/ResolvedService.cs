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
        private static readonly FormatLookupResult DefaultFormatLookupResult = new FormatLookupResult() { Format = new PlainTextFormat(), MediaType = MediaType.Empty };
        private static readonly EncodingLookupResult DefaultEncodingLookupResult = new EncodingLookupResult() { Encoding = new IdentityEncoding(), EncodingType = EncodingType.Empty };

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
            EncodingType encodingType = EncodingType.Parse(contentEncoding);

            if (encodingType != EncodingType.Empty)
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
            else
            {
                result = ResolvedService.DefaultEncodingLookupResult;
            }

            return result;
        }

        public FormatLookupResult GetRequestDeserializer(string contentType)
        {
            FormatLookupResult result = null;
            MediaType mediaType = MediaType.Parse(contentType);

            if (mediaType != MediaType.Empty)
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
            else
            {
                result = ResolvedService.DefaultFormatLookupResult;
            }

            return result;
        }

        public EncodingLookupResult GetResponseEncoder(string acceptEncoding)
        {
            EncodingLookupResult result = null;
            IEnumerable<EncodingType> encodingTypes = acceptEncoding.AsEncodingTypes();

            foreach (EncodingType encodingType in encodingTypes)
            {
                bool found = false;

                foreach (IEncoding encoding in this.Encodings)
                {
                    if (encoding.CanEncode(encodingType))
                    {
                        result = new EncodingLookupResult()
                        {
                            Encoding = encoding,
                            EncodingType = encodingType
                        };

                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    break;
                }
            }

            if (result == null && encodingTypes.Any(e => e == EncodingType.Empty))
            {
                result = ResolvedService.DefaultEncodingLookupResult;
            }

            return result;
        }

        public FormatLookupResult GetResponseSerializer(string accept)
        {
            FormatLookupResult result = null;
            IEnumerable<MediaType> mediaTypes = accept.AsMediaTypes();

            foreach (MediaType mediaType in mediaTypes)
            {
                bool found = false;

                foreach (IFormat format in this.Formats)
                {
                    if (format.CanSerialize(mediaType))
                    {
                        result = new FormatLookupResult()
                        {
                            Format = format,
                            MediaType = mediaType
                        };

                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    break;
                }
            }

            if (result == null && mediaTypes.Any(m => m == MediaType.Empty))
            {
                result = ResolvedService.DefaultFormatLookupResult;
            }

            return result;
        }

        public InvokeActionsResult InvokeAfterActions(IRequestMessage request, IResponseMessage response)
        {
            return ResolvedService.InvokeActions(this.AfterActions, request, response, null);
        }

        public InvokeActionsResult InvokeBeforeActions(IRequestMessage request, IResponseMessage response)
        {
            return ResolvedService.InvokeActions(this.BeforeActions, request, response, null);
        }

        public InvokeActionsResult InvokeErrorActions(IRequestMessage request, IResponseMessage response, IEnumerable<Exception> exceptions)
        {
            return ResolvedService.InvokeActions(this.ErrorActions, request, response, exceptions);
        }

        public ReadRequestResult ReadRequest(int contentLength, string contentEncoding, string contentType, Stream inputStream)
        {
            ReadRequestResult result = new ReadRequestResult() { Success = true };

            if ((this.Method.MethodType == MethodType.Post
                || this.Method.MethodType == MethodType.Put)
                && this.RequestType != null)
            {
                if (contentLength > 0)
                {
                    EncodingLookupResult encodingResult = null;
                    FormatLookupResult formatResult = null;

                    if (!string.IsNullOrEmpty(contentEncoding))
                    {
                        try
                        {
                            encodingResult = this.GetRequestDecoder(contentEncoding);

                            if (encodingResult == null)
                            {
                                result.StatusCode = StatusCode.UnsupportedMediaType;
                                result.Success = false;
                            }
                        }
                        catch (FormatException ex)
                        {
                            result.Exception = ex;
                            result.StatusCode = StatusCode.BadRequest;
                            result.Success = false;
                        }
                    }
                    else
                    {
                        encodingResult = ResolvedService.DefaultEncodingLookupResult;
                    }

                    if (result.Success)
                    {
                        if (!string.IsNullOrEmpty(contentType))
                        {
                            try
                            {
                                formatResult = this.GetRequestDeserializer(contentType);

                                if (formatResult == null)
                                {
                                    result.StatusCode = StatusCode.UnsupportedMediaType;
                                    result.Success = false;
                                }
                            }
                            catch (FormatException ex)
                            {
                                result.Exception = ex;
                                result.StatusCode = StatusCode.BadRequest;
                                result.Success = false;
                            }
                        }
                        else
                        {
                            formatResult = ResolvedService.DefaultFormatLookupResult;
                        }
                    }

                    if (result.Success)
                    {
                        object obj = null;

                        try
                        {
                            using (MemoryStream stream = new MemoryStream())
                            {
                                encodingResult.Encoding.Decode(encodingResult.EncodingType, inputStream, stream);
                                stream.Position = 0;
                                obj = formatResult.Format.Deserialize(formatResult.MediaType, this.RequestType, stream);
                            }

                            result.RequestObject = obj;
                            obj = null;
                        }
                        catch (Exception ex)
                        {
                            result.Exception = ex;
                            result.RequestObject = null;
                            result.StatusCode = StatusCode.BadRequest;
                            result.Success = false;
                            obj.DisposeIfPossible();
                            obj = null;
                        }
                    }
                }
                else
                {
                    result.StatusCode = StatusCode.LengthRequired;
                    result.Success = false;
                }
            }

            return result;
        }

        public WriteResponseResult WriteResponse(string acceptEncoding, string accept, object responseObject, Stream outputStream)
        {
            WriteResponseResult result = new WriteResponseResult() { Success = true };

            if (responseObject != null)
            {
                EncodingLookupResult encodingResult = this.GetResponseEncoder(acceptEncoding);

                if (encodingResult != null)
                {
                    FormatLookupResult formatResult = this.GetResponseSerializer(accept);

                    if (formatResult != null)
                    {
                        try
                        {
                            using (MemoryStream stream = new MemoryStream())
                            {
                                formatResult.Format.Serialize(formatResult.MediaType, responseObject, stream);
                                stream.Position = 0;
                                encodingResult.Encoding.Encode(encodingResult.EncodingType, stream, outputStream);
                            }
                        }
                        catch (Exception ex)
                        {
                            result.Exception = ex;
                            result.StatusCode = StatusCode.InternalServerError;
                            result.Success = false;
                        }
                    }
                    else
                    {
                        result.StatusCode = StatusCode.NotAcceptable;
                        result.Success = false;
                    }
                }
                else
                {
                    result.StatusCode = StatusCode.NotAcceptable;
                    result.Success = false;
                }
            }

            return result;
        }

        private static InvokeActionsResult InvokeActions(IEnumerable<FilterAction> actions, IRequestMessage request, IResponseMessage response, IEnumerable<Exception> exceptions)
        {
            List<FilterActionResult> results = new List<FilterActionResult>();
            bool success = true, cont = true;

            foreach (FilterAction action in actions)
            {
                FilterActionResult result = action.Invoke(request, response, exceptions);
                results.Add(result);
                success = success && result.Success;
                cont = cont && result.Continue;

                if (!cont)
                {
                    break;
                }
            }

            return new InvokeActionsResult(success, cont, results);
        }
    }
}