namespace SmallFry.Tests
{
    using System;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public sealed class MethodCollectionTests
    {
        [Test]
        public void MethodCollectionAfterEndpoint()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;
            MethodCollection methods = endpoints.WithEndpoint("endpoint/route") as MethodCollection;

            methods
                .AfterEndpoint(() => true)
                .AfterEndpoint((req, res) => true)
                .AfterEndpoint<string>((req, res) => true);

            Assert.AreEqual(3, endpoints.First().Pipeline.AfterActions.Count);
        }

        [Test]
        public void MethodCollectionAfterMethod()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;
            MethodCollection methods = endpoints.WithEndpoint("endpoint/route") as MethodCollection;

            methods.Get(() => { });

            methods
                .AfterMethod(() => true)
                .AfterMethod((req, res) => true)
                .AfterMethod<string>((req, res) => true);

            Assert.AreEqual(3, methods.First().Pipeline.AfterActions.Count);
        }

        [Test]
        public void MethodCollectionAfterService()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;
            MethodCollection methods = endpoints.WithEndpoint("endpoint/route") as MethodCollection;

            methods
                .AfterService(() => true)
                .AfterService((req, res) => true)
                .AfterService<string>((req, res) => true);

            Assert.AreEqual(3, services.First().Pipeline.AfterActions.Count);
        }

        [Test]
        public void MethodCollectionBeforeEndpoint()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;
            MethodCollection methods = endpoints.WithEndpoint("endpoint/route") as MethodCollection;

            methods
                .BeforeEndpoint(() => true)
                .BeforeEndpoint((req, res) => true)
                .BeforeEndpoint<string>((req, res) => true);

            Assert.AreEqual(3, endpoints.First().Pipeline.BeforeActions.Count);
        }

        [Test]
        public void MethodCollectionBeforeMethod()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;
            MethodCollection methods = endpoints.WithEndpoint("endpoint/route") as MethodCollection;

            methods.Get(() => { });

            methods
                .BeforeMethod(() => true)
                .BeforeMethod((req, res) => true)
                .BeforeMethod<string>((req, res) => true);

            Assert.AreEqual(3, methods.First().Pipeline.BeforeActions.Count);
        }

        [Test]
        public void MethodCollectionBeforeService()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;
            MethodCollection methods = endpoints.WithEndpoint("endpoint/route") as MethodCollection;

            methods
                .BeforeService(() => true)
                .BeforeService((req, res) => true)
                .BeforeService<string>((req, res) => true);

            Assert.AreEqual(3, services.First().Pipeline.BeforeActions.Count);
        }

        [Test]
        public void MethodCollectionDelete()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;
            MethodCollection methods = endpoints.WithEndpoint("endpoint/route") as MethodCollection;

            methods.Delete(() => { });
            Assert.AreEqual(methods.Count, 1);
            Assert.AreEqual(MethodType.Delete, methods.CurrentMethod.MethodType);

            methods.Clear();
            Assert.AreEqual(0, methods.Count);
            methods.Delete((req) => { });
            Assert.AreEqual(methods.Count, 1);
            Assert.AreEqual(MethodType.Delete, methods.CurrentMethod.MethodType);

            methods.Clear();
            Assert.AreEqual(0, methods.Count);
            methods.Delete((req, res) => { });
            Assert.AreEqual(methods.Count, 1);
            Assert.AreEqual(MethodType.Delete, methods.CurrentMethod.MethodType);
        }

        [Test]
        public void MethodCollectionErrorEndpoint()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;
            MethodCollection methods = endpoints.WithEndpoint("endpoint/route") as MethodCollection;

            methods
                .ErrorEndpoint((ex) => true)
                .ErrorEndpoint((ex, req, res) => true)
                .ErrorEndpoint<string>((ex, req, res) => true);

            Assert.AreEqual(3, endpoints.First().Pipeline.ErrorActions.Count);
        }

        [Test]
        public void MethodCollectionErrorMethod()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;
            MethodCollection methods = endpoints.WithEndpoint("endpoint/route") as MethodCollection;

            methods.Get(() => { });

            methods
                .ErrorMethod((ex) => true)
                .ErrorMethod((ex, req, res) => true)
                .ErrorMethod<string>((ex, req, res) => true);

            Assert.AreEqual(3, methods.First().Pipeline.ErrorActions.Count);
        }

        [Test]
        public void MethodCollectionErrorService()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;
            MethodCollection methods = endpoints.WithEndpoint("endpoint/route") as MethodCollection;

            services
                .ErrorService((ex) => true)
                .ErrorService((ex, req, res) => true)
                .ErrorService<string>((ex, req, res) => true);

            Assert.AreEqual(3, services.First().Pipeline.ErrorActions.Count);
        }

        [Test]
        public void MethodCollectionGet()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;
            MethodCollection methods = endpoints.WithEndpoint("endpoint/route") as MethodCollection;

            methods.Get(() => { });
            Assert.AreEqual(methods.Count, 1);
            Assert.AreEqual(MethodType.Get, methods.CurrentMethod.MethodType);

            methods.Clear();
            Assert.AreEqual(0, methods.Count);
            methods.Get((req) => { });
            Assert.AreEqual(methods.Count, 1);
            Assert.AreEqual(MethodType.Get, methods.CurrentMethod.MethodType);

            methods.Clear();
            Assert.AreEqual(0, methods.Count);
            methods.Get((req, res) => { });
            Assert.AreEqual(methods.Count, 1);
            Assert.AreEqual(MethodType.Get, methods.CurrentMethod.MethodType);
        }

        [Test]
        public void MethodCollectionPost()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;
            MethodCollection methods = endpoints.WithEndpoint("endpoint/route") as MethodCollection;

            methods.Post(() => { });
            Assert.AreEqual(methods.Count, 1);
            Assert.AreEqual(MethodType.Post, methods.CurrentMethod.MethodType);

            methods.Clear();
            Assert.AreEqual(0, methods.Count);
            methods.Post<string>((string s) => { });
            Assert.AreEqual(methods.Count, 1);
            Assert.AreEqual(MethodType.Post, methods.CurrentMethod.MethodType);

            methods.Clear();
            Assert.AreEqual(0, methods.Count);
            methods.Post((req) => { });
            Assert.AreEqual(methods.Count, 1);
            Assert.AreEqual(MethodType.Post, methods.CurrentMethod.MethodType);

            methods.Clear();
            Assert.AreEqual(0, methods.Count);
            methods.Post<string>((IRequestMessage<string> req) => { });
            Assert.AreEqual(methods.Count, 1);
            Assert.AreEqual(MethodType.Post, methods.CurrentMethod.MethodType);

            methods.Clear();
            Assert.AreEqual(0, methods.Count);
            methods.Post((req, res) => { });
            Assert.AreEqual(methods.Count, 1);
            Assert.AreEqual(MethodType.Post, methods.CurrentMethod.MethodType);

            methods.Clear();
            Assert.AreEqual(0, methods.Count);
            methods.Post<string>((req, res) => { });
            Assert.AreEqual(methods.Count, 1);
            Assert.AreEqual(MethodType.Post, methods.CurrentMethod.MethodType);
        }

        [Test]
        public void MethodCollectionPut()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;
            MethodCollection methods = endpoints.WithEndpoint("endpoint/route") as MethodCollection;

            methods.Put(() => { });
            Assert.AreEqual(methods.Count, 1);
            Assert.AreEqual(MethodType.Put, methods.CurrentMethod.MethodType);

            methods.Clear();
            Assert.AreEqual(0, methods.Count);
            methods.Put<string>((string s) => { });
            Assert.AreEqual(methods.Count, 1);
            Assert.AreEqual(MethodType.Put, methods.CurrentMethod.MethodType);

            methods.Clear();
            Assert.AreEqual(0, methods.Count);
            methods.Put((req) => { });
            Assert.AreEqual(methods.Count, 1);
            Assert.AreEqual(MethodType.Put, methods.CurrentMethod.MethodType);

            methods.Clear();
            Assert.AreEqual(0, methods.Count);
            methods.Put<string>((IRequestMessage<string> req) => { });
            Assert.AreEqual(methods.Count, 1);
            Assert.AreEqual(MethodType.Put, methods.CurrentMethod.MethodType);

            methods.Clear();
            Assert.AreEqual(0, methods.Count);
            methods.Put((req, res) => { });
            Assert.AreEqual(methods.Count, 1);
            Assert.AreEqual(MethodType.Put, methods.CurrentMethod.MethodType);

            methods.Clear();
            Assert.AreEqual(0, methods.Count);
            methods.Put<string>((req, res) => { });
            Assert.AreEqual(methods.Count, 1);
            Assert.AreEqual(MethodType.Put, methods.CurrentMethod.MethodType);
        }

        [Test]
        public void MethodCollectionWithHostEncoding()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;
            MethodCollection methods = endpoints.WithEndpoint("endpoint/route") as MethodCollection;
            methods.WithHostEncoding("gzip", new GzipDeflateEncoding());
            Assert.AreEqual(1, services.Pipeline.Encodings.Count);
        }

        [Test]
        public void MethodCollectionWithHostFormat()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;
            MethodCollection methods = endpoints.WithEndpoint("endpoint/route") as MethodCollection;
            methods.WithHostFormat("application/json", new JsonFormat());
            Assert.AreEqual(1, services.Pipeline.Formats.Count);
        }

        [Test]
        public void MethodCollectionWithoutAfterEndpoint()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;
            MethodCollection methods = endpoints.WithEndpoint("endpoint/route") as MethodCollection;

            methods
                .WithoutAfterEndpoint(() => true)
                .WithoutAfterEndpoint((req, res) => true)
                .WithoutAfterEndpoint<string>((req, res) => true);

            Assert.AreEqual(3, endpoints.First().Pipeline.ExcludeAfterActions.Count);
        }

        [Test]
        public void MethodCollectionWithoutAfterMethod()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;
            MethodCollection methods = endpoints.WithEndpoint("endpoint/route") as MethodCollection;

            methods.Get(() => { });

            methods
                .WithoutAfterMethod(() => true)
                .WithoutAfterMethod((req, res) => true)
                .WithoutAfterMethod<string>((req, res) => true);

            Assert.AreEqual(3, methods.First().Pipeline.ExcludeAfterActions.Count);
        }

        [Test]
        public void MethodCollectionWithoutBeforeEndpoint()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;
            MethodCollection methods = endpoints.WithEndpoint("endpoint/route") as MethodCollection;

            methods
                .WithoutBeforeEndpoint(() => true)
                .WithoutBeforeEndpoint((req, res) => true)
                .WithoutBeforeEndpoint<string>((req, res) => true);

            Assert.AreEqual(3, endpoints.First().Pipeline.ExcludeBeforeActions.Count);
        }

        [Test]
        public void MethodCollectionWithoutBeforeMethod()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;
            MethodCollection methods = endpoints.WithEndpoint("endpoint/route") as MethodCollection;

            methods.Get(() => { });

            methods
                .WithoutBeforeMethod(() => true)
                .WithoutBeforeMethod((req, res) => true)
                .WithoutBeforeMethod<string>((req, res) => true);

            Assert.AreEqual(3, methods.First().Pipeline.ExcludeBeforeActions.Count);
        }

        [Test]
        public void MethodCollectionWithoutErrorEndpoint()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;
            MethodCollection methods = endpoints.WithEndpoint("endpoint/route") as MethodCollection;

            methods
                .WithoutErrorEndpoint(ex => true)
                .WithoutErrorEndpoint((ex, req, res) => true)
                .WithoutErrorEndpoint<string>((ex, req, res) => true);

            Assert.AreEqual(3, endpoints.First().Pipeline.ExcludeErrorActions.Count);
        }

        [Test]
        public void MethodCollectionWithoutErrorMethod()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;
            MethodCollection methods = endpoints.WithEndpoint("endpoint/route") as MethodCollection;

            methods.Get(() => { });

            methods
                .WithoutErrorMethod(ex => true)
                .WithoutErrorMethod((ex, req, res) => true)
                .WithoutErrorMethod<string>((ex, req, res) => true);

            Assert.AreEqual(3, methods.First().Pipeline.ExcludeErrorActions.Count);
        }

        [Test]
        public void MethodCollectionWithoutServiceEncoding()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;
            MethodCollection methods = endpoints.WithEndpoint("endpoint/route") as MethodCollection;
            methods.WithoutServiceEncoding("gzip", new GzipDeflateEncoding());
            Assert.AreEqual(1, services.First().Pipeline.ExcludeEncodings.Count);
        }

        [Test]
        public void MethodCollectionWithoutServiceFormat()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;
            MethodCollection methods = endpoints.WithEndpoint("endpoint/route") as MethodCollection;
            methods.WithoutServiceFormat("application/json", new JsonFormat());
            Assert.AreEqual(1, services.First().Pipeline.ExcludeFormats.Count);
        }

        [Test]
        public void MethodCollectionWithService()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;
            MethodCollection methods = endpoints.WithEndpoint("endpoint/route") as MethodCollection;
            methods.WithService("Test1", "/api");
            Assert.AreEqual(2, services.Count);
        }

        [Test]
        public void MethodCollectionWithServiceEncoding()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;
            MethodCollection methods = endpoints.WithEndpoint("endpoint/route") as MethodCollection;
            methods.WithServiceEncoding("gzip", new GzipDeflateEncoding());
            Assert.AreEqual(1, services.First().Pipeline.Encodings.Count);
        }

        [Test]
        public void MethodCollectionWithServiceFormat()
        {
            ServiceCollection services = new ServiceCollection();
            EndpointCollection endpoints = services.WithService("Test", "/") as EndpointCollection;
            MethodCollection methods = endpoints.WithEndpoint("endpoint/route") as MethodCollection;
            methods.WithServiceFormat("application/json", new JsonFormat());
            Assert.AreEqual(1, services.First().Pipeline.Formats.Count);
        }
    }
}