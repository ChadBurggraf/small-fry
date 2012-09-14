namespace SmallFry
{
    using System;
    using System.Collections.Specialized;

    public interface IRequestMessage
    {
        NameValueCollection Cookies { get; }

        NameValueCollection Headers { get; }

        Uri RequestUri { get; }

        T RouteParameter<T>(string name);
    }
}