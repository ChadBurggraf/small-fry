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
        public void ServiceCollectionWithHostEncoding()
        {
            ServiceCollection services = new ServiceCollection();
            services.WithHostEncoding("gzip", new GzipDeflateEncoding());
            Assert.AreEqual(1, services.Pipeline.Encodings.Count);
        }

        [Test]
        public void ServiceCollectionWithHostFormat()
        {
            ServiceCollection services = new ServiceCollection();
            services.WithHostFormat("application/json", new JsonFormat());
            Assert.AreEqual(1, services.Pipeline.Formats.Count);
        }

        [Test]
        public void ServiceCollectionWithoutServiceEncoding()
        {
            ServiceCollection services = new ServiceCollection();
            services.WithService("Test", "/");
            services.WithoutServiceEncoding("gzip", new GzipDeflateEncoding());
            Assert.AreEqual(1, services.First().Pipeline.ExcludeEncodings.Count);
        }

        [Test]
        public void ServiceCollectionWithoutServiceFormat()
        {
            ServiceCollection services = new ServiceCollection();
            services.WithService("Test", "/");
            services.WithoutServiceFormat("application/json", new JsonFormat());
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
            services.WithServiceEncoding("gzip", new GzipDeflateEncoding());
            Assert.AreEqual(1, services.First().Pipeline.Encodings.Count);
        }

        [Test]
        public void ServiceCollectionWithServiceFormat()
        {
            ServiceCollection services = new ServiceCollection();
            services.WithService("Test", "/");
            services.WithServiceFormat("application/json", new JsonFormat());
            Assert.AreEqual(1, services.First().Pipeline.Formats.Count);
        }
    }
}