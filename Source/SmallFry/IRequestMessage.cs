namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;

    public interface IRequestMessage : IDisposable
    {
        NameValueCollection Cookies { get; }

        NameValueCollection Headers { get; }

        IDictionary<string, object> Properties { get; }

        Uri RequestUri { get; }

        T RouteParameter<T>(string name);
    }
}