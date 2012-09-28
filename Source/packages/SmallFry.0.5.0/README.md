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
        .WithService("My Service", "/api/v1")
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
            .ErrorService(() => { ... });

Breaking up the above "line" into more manageable pieces, it would look like
this:

    services
        .WithHostEncoding(new GzipDeflateEncoding())
        .WithHostFormat(new JsonFormat());

    IEndpointCollection endpoints = ServiceHost.Instance.Services.WithService("My Service", "/api/v1");
    endpoints
        .BeforeService(Credentials)
        .BeforeService(Throttle)
        .BeforeService(Authorize)
        .AfterService(Log)
        .ErrorService(() => { ... });

    IMethodCollection methods = endpoints.WithEndpoint("accounts");
    methods
        .Get(() => { ... })
        .Post(() => { ... })
            .WithoutBeforeMethod(Authorize)
        .Delete(() => { ... });

    methods = methods.WithEndpoint("preferences");
    methods.Put(() => { ... });

... and so forth.
