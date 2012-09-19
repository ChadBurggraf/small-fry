//-----------------------------------------------------------------------------
// <copyright file="ResolvedService.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;

    internal sealed class ResolvedService
    {
        public ResolvedService(
            Method method,
            IEnumerable<FilterAction> afterActions,
            IEnumerable<FilterAction> beforeActions,
            IEnumerable<EncodingFilter> encodings,
            IEnumerable<FilterAction> errorActions,
            IEnumerable<FormatFilter> formats)
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

            this.Method = method;
            this.AfterActions = afterActions;
            this.BeforeActions = beforeActions;
            this.Encodings = encodings;
            this.ErrorActions = errorActions;
            this.Formats = formats;
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
            this.Encodings = new List<EncodingFilter>(service.Encodings);
            this.ErrorActions = new List<FilterAction>(service.ErrorActions);
            this.Formats = new List<FormatFilter>(service.Formats);
            this.Method = service.Method;
            this.RouteValues = routeValues ?? new Dictionary<string, object>();
        }

        public IEnumerable<FilterAction> AfterActions { get; private set; }

        public IEnumerable<FilterAction> BeforeActions { get; private set; }

        public IEnumerable<EncodingFilter> Encodings { get; private set; }

        public IEnumerable<FilterAction> ErrorActions { get; private set; }

        public IEnumerable<FormatFilter> Formats { get; private set; }

        public Method Method { get; private set; }

        public IDictionary<string, object> RouteValues { get; private set; }
    }
}