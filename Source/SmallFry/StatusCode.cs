//-----------------------------------------------------------------------------
// <copyright file="StatusCode.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents an HTTP status code.
    /// </summary>
    public enum StatusCode
    {
        /// <summary>
        /// Identifies an empty status code.
        /// </summary>
        None = 0,

        /// <summary>
        /// This means that the server has received the request headers, and that the client should proceed to send the request body (in the case of a request for which a body needs to be sent; for example, a POST request).
        /// </summary>
        [Description("Continue")]
        Continue = 100,

        /// <summary>
        /// This means the requester has asked the server to switch protocols and the server is acknowledging that it will do so.
        /// </summary>
        [Description("Switching Protocols")]
        SwitchingProtocols = 101,

        /// <summary>
        /// As a WebDAV request may contain many sub-requests involving file operations, it may take a long time to complete the request.
        /// </summary>
        [Description("Processing")]
        Processing = 102,

        /// <summary>
        /// Standard response for successful HTTP requests.
        /// </summary>
        [Description("OK")]
        OK = 200,

        /// <summary>
        /// The request has been fulfilled and resulted in a new resource being created.
        /// </summary>
        [Description("Created")]
        Created = 201,

        /// <summary>
        /// The request has been accepted for processing, but the processing has not been completed.
        /// </summary>
        [Description("Accepted")]
        Accepted = 202,

        /// <summary>
        /// The server successfully processed the request, but is returning information that may be from another source.
        /// </summary>
        [Description("Non-Authoritative Information")]
        NonAuthoritativeInformation = 203,

        /// <summary>
        /// The server successfully processed the request, but is not returning any content.
        /// </summary>
        [Description("No Content")]
        NoContent = 204,

        /// <summary>
        /// The server successfully processed the request, but is not returning any content. Unlike a 204 response, this response requires that the requester reset the document view.
        /// </summary>
        [Description("Reset Content")]
        ResetContent = 205,

        /// <summary>
        /// The server is delivering only part of the resource due to a range header sent by the client.
        /// </summary>
        [Description("Partial Content")]
        PartialContent = 206,

        /// <summary>
        /// The message body that follows is an XML message and can contain a number of separate response codes, depending on how many sub-requests were made.
        /// </summary>
        [Description("Multi Status")]
        MultiStatus = 207,

        /// <summary>
        /// The members of a DAV binding have already been enumerated in a previous reply to this request, and are not being included again.
        /// </summary>
        [Description("Already Reported")]
        AlreadyReported = 208,

        /// <summary>
        /// The server has fulfilled a GET request for the resource, and the response is a representation of the result of one or more instance-manipulations applied to the current instance.
        /// </summary>
        [Description("IM Used")]
        IMUsed = 226,

        /// <summary>
        /// Indicates multiple options for the resource that the client may follow.
        /// </summary>
        [Description("Multiple Choices")]
        MultipleChoices = 300,

        /// <summary>
        /// This and all future requests should be directed to the given URI.
        /// </summary>
        [Description("Moved Permanently")]
        MovedPermanently = 301,

        /// <summary>
        /// The HTTP/1.0 specification (RFC 1945) required the client to perform a temporary redirect (the original describing phrase was "Moved Temporarily").
        /// </summary>
        [Description("Found")]
        Found = 302,

        /// <summary>
        /// The response to the request can be found under another URI using a GET method.
        /// </summary>
        [Description("See Other")]
        SeeOther = 303,

        /// <summary>
        /// Indicates the resource has not been modified since last requested.
        /// </summary>
        [Description("Not Modified")]
        NotModified = 304,

        /// <summary>
        /// Many HTTP clients (such as Mozilla[9] and Internet Explorer) do not correctly handle responses with this status code, primarily for security reasons.
        /// </summary>
        [Description("Use Proxy")]
        UseProxy = 305,

        /// <summary>
        /// No longer used. Originally meant "Subsequent requests should use the specified proxy."
        /// </summary>
        [Description("Switch Proxy")]
        SwitchProxy = 306,

        /// <summary>
        /// In this case, the request should be repeated with another URI; however, future requests can still use the original URI.
        /// </summary>
        [Description("Temporary Redirect")]
        TemporaryRedirect = 307,

        /// <summary>
        /// The request, and all future requests should be repeated using another URI. 307 and 308 (as proposed) parallel the behaviors of 302 and 301, but do not allow the HTTP method to change.
        /// </summary>
        [Description("Permanent Redirect")]
        PermanentRedirect = 308,

        /// <summary>
        /// The request cannot be fulfilled due to bad syntax.
        /// </summary>
        [Description("Bad Request")]
        BadRequest = 400,

        /// <summary>
        /// Similar to 403 Forbidden, but specifically for use when authentication is required and has failed or has not yet been provided.
        /// </summary>
        [Description("Unauthorized")]
        Unauthorized = 401,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        [Description("Payment Required")]
        PaymentRequired = 402,

        /// <summary>
        /// The request was a valid request, but the server is refusing to respond to it.
        /// </summary>
        [Description("Forbidden")]
        Forbidden = 403,

        /// <summary>
        /// The requested resource could not be found but may be available again in the future.
        /// </summary>
        [Description("Not Found")]
        NotFound = 404,

        /// <summary>
        /// A request was made of a resource using a request method not supported by that resource; for example, using GET on a form which requires data to be presented via POST, or using PUT on a read-only resource.
        /// </summary>
        [Description("Method Not Allowed")]
        MethodNotAllowed = 405,

        /// <summary>
        /// The requested resource is only capable of generating content not acceptable according to the Accept headers sent in the request.
        /// </summary>
        [Description("Not Acceptable")]
        NotAcceptable = 406,

        /// <summary>
        /// The client must first authenticate itself with the proxy.
        /// </summary>
        [Description("Proxy Authentication Required")]
        ProxyAuthenticationRequired = 407,

        /// <summary>
        /// The server timed out waiting for the request.
        /// </summary>
        [Description("Request Timeout")]
        RequestTimeout = 408,

        /// <summary>
        /// Indicates that the request could not be processed because of conflict in the request, such as an edit conflict.
        /// </summary>
        [Description("Conflict")]
        Conflict = 409,

        /// <summary>
        /// Indicates that the resource requested is no longer available and will not be available again.
        /// </summary>
        [Description("Gone")]
        Gone = 410,

        /// <summary>
        /// The request did not specify the length of its content, which is required by the requested resource.
        /// </summary>
        [Description("Length Required")]
        LengthRequired = 411,

        /// <summary>
        /// The server does not meet one of the preconditions that the requester put on the request.
        /// </summary>
        [Description("Precondition Failed")]
        PreconditionFailed = 412,

        /// <summary>
        /// The request is larger than the server is willing or able to process.
        /// </summary>
        [Description("Request Entity Too Large")]
        RequestEntityTooLarge = 413,

        /// <summary>
        /// The URI provided was too long for the server to process.
        /// </summary>
        [Description("Request URI Too Long")]
        RequestUriTooLong = 414,

        /// <summary>
        /// The request entity has a media type which the server or resource does not support.
        /// </summary>
        [Description("Unsupported Media Type")]
        UnsupportedMediaType = 415,

        /// <summary>
        /// The client has asked for a portion of the file, but the server cannot supply that portion.
        /// </summary>
        [Description("Requested Range Not Satisfiable")]
        RequestedRangeNotSatisfiable = 416,

        /// <summary>
        /// The server cannot meet the requirements of the Expect request-header field.
        /// </summary>
        [Description("Expectation Failed")]
        ExpectationFailed = 417,

        /// <summary>
        /// This code was defined in 1998 as one of the traditional IETF April Fools' jokes, in RFC 2324, Hyper Text Coffee Pot Control Protocol, and is not expected to be implemented by actual HTTP servers.
        /// </summary>
        [Description("I'm A Teapot")]
        ImATeapot = 418,

        /// <summary>
        /// Not part of the HTTP standard, but returned by the Twitter Search and Trends API when the client is being rate limited.
        /// </summary>
        [Description("Enhance Your Calm")]
        EnhanceYourCalm = 420,

        /// <summary>
        /// The request was well-formed but was unable to be followed due to semantic errors.
        /// </summary>
        [Description("Unprocessable Entity")]
        UnprocessableEntity = 422,

        /// <summary>
        /// The resource that is being accessed is locked.
        /// </summary>
        [Description("Locked")]
        Locked = 423,

        /// <summary>
        /// The request failed due to failure of a previous request (e.g. a PROPPATCH).
        /// </summary>
        [Description("Failed Dependency")]
        FailedDependency = 424,

        /// <summary>
        /// Indicates the method was not executed on a particular resource within its scope because some part of the method's execution failed causing the entire method to be aborted.
        /// </summary>
        [Description("Method Failure")]
        MethodFailure = 424,

        /// <summary>
        /// Defined in drafts of "WebDAV Advanced Collections Protocol",[14] but not present in "Web Distributed Authoring and Versioning (WebDAV) Ordered Collections Protocol".
        /// </summary>
        [Description("Unordered Collection")]
        UnorderedCollection = 425,

        /// <summary>
        /// The client should switch to a different protocol such as TLS/1.0.
        /// </summary>
        [Description("Upgrade Required")]
        UpgradeRequired = 426,

        /// <summary>
        /// The origin server requires the request to be conditional. Intended to prevent "the 'lost update' problem, where a client GETs a resource's state, modifies it, and PUTs it back to the server, when meanwhile a third party has modified the state on the server, leading to a conflict."
        /// </summary>
        [Description("Precondition Required")]
        PreconditionRequired = 428,

        /// <summary>
        /// The user has sent too many requests in a given amount of time. Intended for use with rate limiting schemes.
        /// </summary>
        [Description("Too Many Requests")]
        TooManyRequests = 429,

        /// <summary>
        /// The server is unwilling to process the request because either an individual header field, or all the header fields collectively, are too large.
        /// </summary>
        [Description("Request Header Fields Too Large")]
        RequestHeaderFieldsTooLarge = 431,

        /// <summary>
        /// Used in Nginx logs to indicate that the server has returned no information to the client and closed the connection (useful as a deterrent for malware).
        /// </summary>
        [Description("No Response")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
        NoResponse = 444,

        /// <summary>
        /// A Microsoft extension. The request should be retried after performing the appropriate action.
        /// </summary>
        [Description("Retry With")]
        RetryWith = 449,

        /// <summary>
        /// A Microsoft extension. This error is given when Windows Parental Controls are turned on and are blocking access to the given webpage.
        /// </summary>
        [Description("Blocked By Windows Parental Controls")]
        BlockedByWindowsParentalControls = 450,

        /// <summary>
        /// Defined in the internet draft "A New HTTP Status Code for Legally-restricted Resources".[20] Intended to be used when resource access is denied for legal reasons, e.g. censorship or government-mandated blocked access. A reference to the 1953 dystopian novel Fahrenheit 451, where books are outlawed.
        /// </summary>
        [Description("Unavailable For Legal Reasons")]
        UnavailableForLegalReasons = 451,

        /// <summary>
        /// Nginx internal code similar to 431 but it was introduced earlier.
        /// </summary>
        [Description("Request Header Too Large")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
        RequestHeaderTooLarge = 494,

        /// <summary>
        /// Nginx internal code used when SSL client certificate error occured to distinguish it from 4XX in a log and an error page redirection.
        /// </summary>
        [Description("Cert Error")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
        CertError = 495,

        /// <summary>
        /// Nginx internal code used when client didn't provide certificate to distinguish it from 4XX in a log and an error page redirection.
        /// </summary>
        [Description("No Cert")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
        NoCert = 496,

        /// <summary>
        /// Nginx internal code used for the plain HTTP requests that are sent to HTTPS port to distinguish it from 4XX in a log and an error page redirection.
        /// </summary>
        [Description("HTTP To HTTPS")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
        HttpToHttps = 497,

        /// <summary>
        /// Used in Nginx logs to indicate when the connection has been closed by client while the server is still processing its request, making server unable to send a status code back.
        /// </summary>
        [Description("Client Closed Request")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
        ClientClosedRequest = 499,

        /// <summary>
        /// A generic error message, given when no more specific message is suitable.
        /// </summary>
        [Description("Internal Server Error")]
        InternalServerError = 500,

        /// <summary>
        /// The server either does not recognize the request method, or it lacks the ability to fulfill the request.
        /// </summary>
        [Description("Not Implemented")]
        NotImplemented = 501,

        /// <summary>
        /// The server was acting as a gateway or proxy and received an invalid response from the upstream server.
        /// </summary>
        [Description("Bad Gateway")]
        BadGateway = 502,

        /// <summary>
        /// The server is currently unavailable (because it is overloaded or down for maintenance). Generally, this is a temporary state.
        /// </summary>
        [Description("Service Unavailable")]
        ServiceUnavailable = 503,

        /// <summary>
        /// The server was acting as a gateway or proxy and did not receive a timely response from the upstream server.
        /// </summary>
        [Description("Gateway Timeout")]
        GatewayTimeout = 504,

        /// <summary>
        /// The server does not support the HTTP protocol version used in the request.
        /// </summary>
        [Description("HTTP Version Not Supported")]
        HttpVersionNotSupported = 505,

        /// <summary>
        /// Transparent content negotiation for the request results in a circular reference.
        /// </summary>
        [Description("Variant Also Negotiates")]
        VariantAlsoNegotiates = 506,

        /// <summary>
        /// The server is unable to store the representation needed to complete the request.
        /// </summary>
        [Description("Unsufficient Storage")]
        InsufficientStorage = 507,

        /// <summary>
        /// The server detected an infinite loop while processing the request (sent in lieu of 208).
        /// </summary>
        [Description("Loop Detected")]
        LoopDetected = 508,

        /// <summary>
        /// This status code, while used by many servers, is not specified in any RFCs.
        /// </summary>
        [Description("Bandwidth Limit Exceeded")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
        BandwidthLimitExceeded = 509,

        /// <summary>
        /// Further extensions to the request are required for the server to fulfill it.
        /// </summary>
        [Description("Not Extended")]
        NotExtended = 510,

        /// <summary>
        /// The client needs to authenticate to gain network access. Intended for use by intercepting proxies used to control access to the network (e.g. "captive portals" used to require agreement to Terms of Service before granting full Internet access via a Wi-Fi hotspot).
        /// </summary>
        [Description("Network Authentication Required")]
        NetworkAuthenticationRequired = 511,

        /// <summary>
        /// This status code is not specified in any RFCs, but is used by Microsoft Corp. HTTP proxies to signal a network read timeout behind the proxy to a client in front of the proxy.
        /// </summary>
        [Description("Network Read Timeout Error")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
        NetworkReadTimeoutError = 598,

        /// <summary>
        /// This status code is not specified in any RFCs, but is used by Microsoft Corp. HTTP proxies to signal a network connect timeout behind the proxy to a client in front of the proxy.
        /// </summary>
        [Description("Network Connection Timeout Error")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
        NetworkConnectionTimeoutError = 599
    }
}