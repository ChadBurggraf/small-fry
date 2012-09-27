# Small Fry
#### Fluent API design for C# and .NET.

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

    ServiceHost.Instance.Services
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

    ServiceHost.Instance.Services
        .WithHostEncoding(new GzipDeflateEncoding())
        .WithHostFormat(new JsonFormat());

    IEndpointCollection endpoints = ServiceHost.Instance.Services.WithService("My Service", "/api/v1");
    endpoints
        .BeforeService(Credentials)
        .BeforeService(Throttle)
        .BeforeService(Authorize);

    IMethodCollection methods = endpoints.WithEndpoint("accounts");
    methods
        .Get(() => { ... })
        .Post(() => { ... })
            .WithoutBeforeMethod(Authorize)
        .Delete(() => { ... });

    methods = methods.WithEndpoint("preferences");
    methods.Put(() => { ... });

... and so forth.
