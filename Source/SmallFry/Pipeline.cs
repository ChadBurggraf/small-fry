﻿namespace SmallFry
{
    using System;
    using System.Collections.Generic;

    internal sealed class Pipeline
    {
        public Pipeline()
        {
            this.AfterActions = new List<FilterAction>();
            this.BeforeActions = new List<FilterAction>();
            this.Encodings = new List<EncodingFilter>();
            this.ErrorActions = new List<FilterAction>();
            this.ExcludeAfterActions = new List<FilterAction>();
            this.ExcludeBeforeActions = new List<FilterAction>();
            this.ExcludeEncodings = new List<EncodingFilter>();
            this.ExcludeErrorActions = new List<FilterAction>();
            this.ExcludeFormats = new List<FormatFilter>();
            this.Formats = new List<FormatFilter>();
        }

        public IList<FilterAction> AfterActions { get; private set; }

        public IList<FilterAction> BeforeActions { get; private set; }

        public IList<EncodingFilter> Encodings { get; private set; }

        public IList<FilterAction> ErrorActions { get; private set; }

        public IList<FilterAction> ExcludeAfterActions { get; private set; }

        public IList<FilterAction> ExcludeBeforeActions { get; private set; }

        public IList<EncodingFilter> ExcludeEncodings { get; private set; }

        public IList<FilterAction> ExcludeErrorActions { get; private set; }

        public IList<FormatFilter> ExcludeFormats { get; private set; }

        public IList<FormatFilter> Formats { get; private set; }
    }
}