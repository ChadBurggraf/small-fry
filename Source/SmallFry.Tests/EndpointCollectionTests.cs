namespace SmallFry.Tests
{
    using System;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public sealed class EndpointCollectionTests
    {
        [Test]
        public void EndpointCollectionAfterEndpoint()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;

            endpoints.WithEndpoint("endpoint/route");

            endpoints
                .AfterEndpoint(() => true)
                .AfterEndpoint((req, res) => true)
                .AfterEndpoint<string>((req, res) => true);

            Assert.AreEqual(3, endpoints.First().Pipeline.AfterActions.Count);
        }

        [Test]
        public void EndpointCollectionAfterService()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;

            endpoints.WithEndpoint("endpoint/route");

            endpoints
                .AfterService(() => true)
                .AfterService((req, res) => true)
                .AfterService<string>((req, res) => true);

            Assert.AreEqual(3, services.First().Pipeline.AfterActions.Count);
        }

        [Test]
        public void EndpointCollectionBeforeEndpoint()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;

            endpoints.WithEndpoint("endpoint/route");

            endpoints
                .BeforeEndpoint(() => true)
                .BeforeEndpoint((req, res) => true)
                .BeforeEndpoint<string>((req, res) => true);

            Assert.AreEqual(3, endpoints.First().Pipeline.BeforeActions.Count);
        }

        [Test]
        public void EndpointCollectionBeforeService()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;

            endpoints.WithEndpoint("endpoint/route");

            endpoints
                .BeforeService(() => true)
                .BeforeService((req, res) => true)
                .BeforeService<string>((req, res) => true);

            Assert.AreEqual(3, services.First().Pipeline.BeforeActions.Count);
        }

        [Test]
        public void EndpointCollectionErrorEndpoint()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;

            endpoints.WithEndpoint("endpoint/route");

            endpoints
                .ErrorEndpoint((ex) => true)
                .ErrorEndpoint((ex, req, res) => true)
                .ErrorEndpoint<string>((ex, req, res) => true);

            Assert.AreEqual(3, endpoints.First().Pipeline.ErrorActions.Count);
        }

        [Test]
        public void EndpointCollectionErrorService()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;

            endpoints.WithEndpoint("endpoint/route");

            endpoints
                .ErrorService((ex) => true)
                .ErrorService((ex, req, res) => true)
                .ErrorService<string>((ex, req, res) => true);

            Assert.AreEqual(3, services.First().Pipeline.ErrorActions.Count);
        }

        [Test]
        public void EndpointCollectionWithEndpoint()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;

            endpoints.WithEndpoint("accounts/{id}");
            Assert.AreEqual(1, endpoints.Count());
            Assert.AreEqual(0, endpoints.First().ParameterTypes.Count);

            endpoints.WithEndpoint("foo/{id}/bar", new { id = typeof(long) });
            Assert.AreEqual(2, endpoints.Count());
            Assert.IsTrue(endpoints.Last().ParameterTypes.ContainsKey("id"));
            Assert.AreEqual(typeof(long), endpoints.Last().ParameterTypes["id"]);
        }

        [Test]
        public void EndpointCollectionWithHostEncoding()
        {
            ServiceCollection services = new ServiceCollection();
            int count = services.Pipeline.Encodings.Count;
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;
            endpoints.WithHostEncoding(new GzipDeflateEncoding());
            Assert.AreEqual(count + 1, services.Pipeline.Encodings.Count);
        }

        [Test]
        public void EndpointCollectionWithHostFormat()
        {
            ServiceCollection services = new ServiceCollection();
            int count = services.Pipeline.Formats.Count;
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;
            endpoints.WithHostFormat("application/json", new JsonFormat());
            Assert.AreEqual(count + 1, services.Pipeline.Formats.Count);
        }

        [Test]
        public void EndpointCollectionWithHostParameterParser()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;
            Assert.IsFalse(services.RouteValueBinder.HasParserForType(typeof(Service)));
            endpoints.WithHostParameterParser(new NoOpRouteParameterParser(new Type[] { typeof(Service) }));
            Assert.IsTrue(services.RouteValueBinder.HasParserForType(typeof(Service)));
        }

        [Test]
        public void EndpointCollectionWithoutAfterEndpoint()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;

            endpoints.WithEndpoint("endpoint/route");

            endpoints
                .WithoutAfterEndpoint(() => true)
                .WithoutAfterEndpoint((req, res) => true)
                .WithoutAfterEndpoint<string>((req, res) => true);

            Assert.AreEqual(3, endpoints.First().Pipeline.ExcludeAfterActions.Count);
        }

        [Test]
        public void EndpointCollectionWithoutBeforeEndpoint()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;

            endpoints.WithEndpoint("endpoint/route");

            endpoints
                .WithoutBeforeEndpoint(() => true)
                .WithoutBeforeEndpoint((req, res) => true)
                .WithoutBeforeEndpoint<string>((req, res) => true);

            Assert.AreEqual(3, endpoints.First().Pipeline.ExcludeBeforeActions.Count);
        }

        [Test]
        public void EndpointCollectionWithoutErrorEndpoint()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;

            endpoints.WithEndpoint("endpoint/route");

            endpoints
                .WithoutErrorEndpoint(ex => true)
                .WithoutErrorEndpoint((ex, req, res) => true)
                .WithoutErrorEndpoint<string>((ex, req, res) => true);

            Assert.AreEqual(3, endpoints.First().Pipeline.ExcludeErrorActions.Count);
        }

        [Test]
        public void EndpointCollectionWithoutServiceEncoding()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;

            endpoints.WithEndpoint("endpoint/route");

            endpoints.WithoutServiceEncoding(new GzipDeflateEncoding());
            Assert.AreEqual(1, services.First().Pipeline.ExcludeEncodings.Count);
        }

        [Test]
        public void EndpointCollectionWithoutServiceFormat()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;

            endpoints.WithEndpoint("endpoint/route");

            endpoints.WithoutServiceFormat("application/json", new JsonFormat());
            Assert.AreEqual(1, services.First().Pipeline.ExcludeFormats.Count);
        }

        [Test]
        public void EndpointCollectionWithService()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;

            endpoints.WithEndpoint("endpoint/route");

            endpoints.WithService("Test2", "/api");
            Assert.AreEqual(2, services.Count);
        }

        [Test]
        public void EndpointCollectionWithServiceEncoding()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;

            endpoints.WithEndpoint("endpoint/route");

            endpoints.WithServiceEncoding(new GzipDeflateEncoding());
            Assert.AreEqual(1, services.First().Pipeline.Encodings.Count);
        }

        [Test]
        public void EndpointCollectionWithServiceFormat()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;

            endpoints.WithEndpoint("endpoint/route");

            endpoints.WithServiceFormat("application/json", new JsonFormat());
            Assert.AreEqual(1, services.First().Pipeline.Formats.Count);
        }
    }
}
