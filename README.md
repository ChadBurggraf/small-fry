# Small Fry
#### Fluent API design for C# and .NET.

## Installation

Install with NuGet:

    PM> Install-Package SmallFry

If your application doesn't have one already, create a `Global.asax` file and
add `using SmallFry;` to your list of namespace registrations.

Add or update `void Application_Start()`:

    void Application_Start(object sender, EventArgs e)
    {
        SmallFryConfig.RegisterServices(ServiceHost.Instance.Services);
    }

Open `~/App_Start/SmallFryConfig.cs` (which was created for you) and define
all of your services.

## Overview

Your API is built from a collection of services, based on REST-like semantics.
Services contain a collection of endpoints, which can be thought of as routes,
e.g.:

    /accounts
    /entries/{?id}
    /labels/{?id}
    /entries/{entryId}/labels/{?labelId}

Each of these routes can respond to one or more methods, which correspond to
HTTP verbs:

    DELETE
    GET
    POST
    PUT

At each level (service collections, endpoint collections, and method 
collections), you can fluently manipulate the definition of your current
element, or the elements in your parent tree.

In other words, you can define an entire collection of services in one
fluent line:

    services
        .WithHostEncoding(new GzipDeflateEncoding())
        .WithHostFormat(new JsonFormat())
        .WithService("Hello, Service!", "/api/v1")
            .BeforeService(Credentials)
            .BeforeService(Throttle)
            .BeforeService(Authorize)
            .WithEndpoint("accounts")
                .Get(() => { ... })
                .Post(() => { ... })
                    .WithoutBeforeMethod(Authorize)
                .Delete(() => { ... })
            .WithEndpoint("preferences")
                .Put(() => { ... })
            .WithEndpoint("entries/{entryId}/labels/{?labelId}")
                .Post(() => { ... })
                .Delete(() => { ... })
            .WithEndpoint("entries/{?id}")
                .Post(() => { ... })
                .Put(() => { ... })
                .Delete(() => { ... })
            .WithEndpoint("labels/{?id}")
                .Post(() => { ... })
                .Put(() => { ... })
                .Delete(() => { ... })
            .AfterService(Log)
            .ErrorService(LogError);

Breaking up the above "line" into more manageable pieces, it would look like
this:

    services
        .WithHostEncoding(new GzipDeflateEncoding())
        .WithHostFormat(new JsonFormat());

    IEndpointCollection endpoints = services.WithService("Hello, Service!", "/api/v1");
    endpoints
        .BeforeService(Credentials)
        .BeforeService(Throttle)
        .BeforeService(Authorize)
        .AfterService(Log)
        .ErrorService(LogError);

    IMethodCollection methods = endpoints.WithEndpoint("accounts");
    methods
        .Get(() => { ... })
        .Post(() => { ... })
            .WithoutBeforeMethod(Authorize)
        .Delete(() => { ... });

    methods = methods.WithEndpoint("preferences");
    methods.Put(() => { ... });

... and so forth.

## Endpoints

Endpoints are defined by their routes and a collection of methods and filters.
Routes work almost exactly like ASP.NET MVC routes do:

  * Contain a mix of literals and tokens
  * Tokens are named, and enclosed in brackets
  * A wildcard token is prefixed with `*`, and must be the last element in the
    route

Additionally, optional tokens are supported. However, they are denoted by
the `?` prefix, unlike in MVC.

### Examples

    /literal/route
    /standard/{parameterized}/route
    /{tokens}/and/{*pathInfo}
    /{required}/tokens/and/{?optional}/tokens

Tokens need not be separated by path segments, but if they are not, they 
must be separated by literal characters.

### Route Parameter Types and Patterns

Routes can further be constrained by types and regular expression patterns, 
using the appropriate overloads if `WithEndpoint()`. For example:

    services.WithEndpoint(
        "geography/{id}/{state}",
        new { id = typeof(int) },
        new { state = @"^[a-zA-Z]{2}$" }
    );

Custom parameter parsers can be registered to provide type conversion, but all
.NET primitive types, plus `enum`, `Guid`, and `DateTime` are supported by 
default.

## Methods

Methods define the action to take when an endpoint is requested with a specific
HTTP verb. Methods can be defined with lambda expressions, or by referencing
an existing method definition containing the correct signature.

A basic method can be defined with an empty `Action` delegate:

    .Get(() => { ... })

Or, you can get information about the request and response:

    .Get((req, res) => { ... })

In the above call, `req` is of type `IRequestMessage`, and `res` is of type
`IResponseMessage`.

If you've registered a format (see **Encodings and Formats**), you can ask for
a typed request:

    .Post<RequestType>((obj) => { ... })

Or, with request and response information included:

    .Post<RequestType>((req, res) => { ... })

Where `req` is of type `IRequestMessage<RequestType` and `res` is of type
`IResponseMessage`.

## Encodings and Formats

By default only the *identity* encoding and *plain text* formats are available.
To send and receive JSON, register it as a format:

    services
        .WithHostFormat(new JsonFormat());

Or, just register JSON for a specific service:

    services
        .WithService("Hello, Service!", "/api")
            .WithServiceFormat(new JsonFormat());

Formats implement `IFormat`. The `Content-Type` header is used to choose a 
format when de-serializing a request, and the `Accept` header is used to choose
a format when serializing the response.

Encodings work almost exactly the same, except they implement `IEncoding`.
To send and/or receive GZip and Deflate compression, for example, register
the appropriate encoding:

    services
        .WithHostEncoding(new GzipDeflateEncoding());

The `Content-Encoding` header is used to choose an encoding when reading the
request, and the `Accept-Encoding` header is used to choose an encoding when
writing the response.

## Before and After Filters

Filter actions behave in almost exactly the same way as `Method`s, except they
are fired either before or after the actual method invocation.

You can register filters for the entire host, or for individual services, 
endpoints, or methods. Similarly, you can exclude filters for specific
parts of your service, provided you're passing delegates that have reference
equality (see the example in **Overview** where `Authorize` is excluded).

Finally, filters require a `bool` return value. This value indicates whether
the system should continue processing the current request. You can use this 
to bail out during an authentication failure, for example:

    services
        .WithService("Hello, Service!", "/api")
            .BeforeService(
                (req, res) =>
                {
                    bool authorized = false;

                    ...

                    // Do authorization check here.

                    ...

                    if (!authorized) 
                    {
                        res.SetStatus(StatusCode.Unauthorized);
                    }

                    return authorized;
                });

## Error Filters

Error filters can similarly be defined for services, endpoints, and/or
methods. Errors filters are invoked in the order they are defined, and accept
the collection of exceptions that occurred.

Error filters are guaranteed to be called for any exceptions that occur while
processing the request, unless a serialization or encoding error occurs
while writing the response back to the client. In that case, an unhandled 
exception of type `SmallFry.PipelineException` is thrown.

## Examples

Please see the example applications at 
<https://github.com/ChadBurggraf/small-fry/tree/master/Source/Examples>
for a more detailed view of building services using Small Fry.

## License

Licensed under the [MIT](http://www.opensource.org/licenses/mit-license.html) 
license.

Copyright (c) 2012 Chad Burggraf.