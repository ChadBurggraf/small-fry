namespace SmallFry.Tests
{
    using System;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public sealed class ServiceCollectionTests
    {
        [Test]
        public void ServiceCollectionAfterService()
        {
            ServiceCollection services = new ServiceCollection();
            services.WithService("Test", "/");

            services
                .AfterService(() => true)
                .AfterService((req, res) => true)
                .AfterService<string>((req, res) => true);

            Assert.AreEqual(3, services.First().Pipeline.AfterActions.Count);
        }

        [Test]
        public void ServiceCollectionBeforeService()
        {
            ServiceCollection services = new ServiceCollection();
            services.WithService("Test", "/");

            services
                .BeforeService(() => true)
                .BeforeService((req, res) => true)
                .BeforeService<string>((req, res) => true);

            Assert.AreEqual(3, services.First().Pipeline.BeforeActions.Count);
        }

        [Test]
        public void ServiceCollectionErrorService()
        {
            ServiceCollection services = new ServiceCollection();
            services.WithService("Test", "/");

            services
                .ErrorService((ex) => true)
                .ErrorService((ex, req, res) => true)
                .ErrorService<string>((ex, req, res) => true);

            Assert.AreEqual(3, services.First().Pipeline.ErrorActions.Count);
        }

        [Test]
        public void ServiceCollectionWithEndpoint()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;

            services.WithEndpoint("accounts/{id}");
            Assert.AreEqual(1, endpoints.Count());
            Assert.AreEqual(0, endpoints.First().ParameterTypes.Count);

            services.WithEndpoint("foo/{id}/bar", new { id = typeof(long) });
            Assert.AreEqual(2, endpoints.Count());
            Assert.IsTrue(endpoints.Last().ParameterTypes.ContainsKey("id"));
            Assert.AreEqual(typeof(long), endpoints.Last().ParameterTypes["id"]);
        }

        [Test]
        public void ServiceCollectionWithHostEncoding()
        {
            ServiceCollection services = new ServiceCollection();
            int count = services.Pipeline.Encodings.Count;
            services.WithHostEncoding(new GzipDeflateEncoding());
            Assert.AreEqual(count + 1, services.Pipeline.Encodings.Count);
        }

        [Test]
        public void ServiceCollectionWithHostFormat()
        {
            ServiceCollection services = new ServiceCollection();
            int count = services.Pipeline.Formats.Count;
            services.WithHostFormat(new PlainTextFormat());
            Assert.AreEqual(count + 1, services.Pipeline.Formats.Count);
        }

        [Test]
        public void ServiceCollectionWithHostParameterParser()
        {
            ServiceCollection services = new ServiceCollection();
            Assert.IsFalse(services.RouteValueBinder.HasParserForType(typeof(Service)));
            services.WithHostParameterParser(new NoOpRouteParameterParser(new Type[] { typeof(Service) }));
            Assert.IsTrue(services.RouteValueBinder.HasParserForType(typeof(Service)));
        }

        [Test]
        public void ServiceCollectionWithoutServiceEncoding()
        {
            ServiceCollection services = new ServiceCollection();
            services.WithService("Test", "/");
            services.WithoutServiceEncoding(new GzipDeflateEncoding());
            Assert.AreEqual(1, services.First().Pipeline.ExcludeEncodings.Count);
        }

        [Test]
        public void ServiceCollectionWithoutServiceFormat()
        {
            ServiceCollection services = new ServiceCollection();
            services.WithService("Test", "/");
            services.WithoutServiceFormat(new PlainTextFormat());
            Assert.AreEqual(1, services.First().Pipeline.ExcludeFormats.Count);
        }

        [Test]
        public void ServiceCollectionWithService()
        {
            ServiceCollection services = new ServiceCollection();
            services.WithService("Test", "/");
            Assert.AreEqual(1, services.Count);
        }

        [Test]
        public void ServiceCollectionWithServiceEncoding()
        {
            ServiceCollection services = new ServiceCollection();
            services.WithService("Test", "/");
            services.WithServiceEncoding(new GzipDeflateEncoding());
            Assert.AreEqual(1, services.First().Pipeline.Encodings.Count);
        }

        [Test]
        public void ServiceCollectionWithServiceFormat()
        {
            ServiceCollection services = new ServiceCollection();
            services.WithService("Test", "/");
            services.WithServiceFormat(new PlainTextFormat());
            Assert.AreEqual(1, services.First().Pipeline.Formats.Count);
        }
    }
}