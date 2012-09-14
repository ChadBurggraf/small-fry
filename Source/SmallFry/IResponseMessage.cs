namespace SmallFry
{
    using System;
    using System.Collections.Specialized;

    public interface IResponseMessage
    {
        NameValueCollection Cookies { get; }

        NameValueCollection Headers { get; }

        object Response { get; set; }

        int StatusCode { get; set; }

        string StatusMessage { get; set; }
    }
}