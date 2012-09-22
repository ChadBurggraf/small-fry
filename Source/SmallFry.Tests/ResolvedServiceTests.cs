namespace SmallFry.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public sealed class ResolvedServiceTests
    {
        [Test]
        public void ResolvedServiceGetRequestDecoder()
        {
            ServiceCollection services = new ServiceCollection();
            services
                .WithService("Test", "/")
                    .WithEndpoint("{action}")
                        .Post<Payload>((Payload p) => { });

            ResolvedService service = new ServiceResolver(services).Find(MethodType.Post, "foo");
            EncodingLookupResult result = service.GetRequestDecoder("gzip");
            Assert.IsNull(result);

            result = service.GetRequestDecoder(string.Empty);
            Assert.IsNotNull(result);
            Assert.AreEqual(EncodingType.Empty, result.EncodingType);
            Assert.AreEqual(new IdentityEncoding(), result.Encoding);

            services.WithHostEncoding(new GzipDeflateEncoding());
            service = new ServiceResolver(services).Find(MethodType.Post, "foo");
            result = service.GetRequestDecoder("gzip");
            Assert.IsNotNull(result);
            Assert.AreEqual(EncodingType.Parse("gzip"), result.EncodingType);
            Assert.AreEqual(new GzipDeflateEncoding(), result.Encoding);
        }

        [Test]
        public void ResolvedServiceGetRequestDeserializer()
        {
            ServiceCollection services = new ServiceCollection();
            services
                .WithService("Test", "/")
                    .WithEndpoint("{action}")
                        .Post<Payload>((Payload p) => { });

            ResolvedService service = new ServiceResolver(services).Find(MethodType.Post, "foo");
            FormatLookupResult result = service.GetRequestDeserializer("application/json");
            Assert.IsNull(result);

            result = service.GetRequestDeserializer(string.Empty);
            Assert.IsNotNull(result);
            Assert.AreEqual(MediaType.Empty, result.MediaType);
            Assert.AreEqual(new PlainTextFormat(), result.Format);

            services.WithHostFormat(new JsonFormat());
            service = new ServiceResolver(services).Find(MethodType.Post, "foo");
            result = service.GetRequestDeserializer("application/json");
            Assert.IsNotNull(result);
            Assert.AreEqual(MediaType.Parse("application/json"), result.MediaType);
            Assert.AreEqual(new JsonFormat(), result.Format);
        }

        [Test]
        public void ResolvedServiceGetResponseEncoder()
        {
            ServiceCollection services = new ServiceCollection();
            services
                .WithService("Test", "/")
                    .WithEndpoint("{action}")
                        .Post<Payload>((Payload p) => { });

            ResolvedService service = new ServiceResolver(services).Find(MethodType.Post, "foo");
            EncodingLookupResult result = service.GetResponseEncoder("gzip");
            Assert.IsNull(result);

            result = service.GetResponseEncoder("gzip, *");
            Assert.IsNotNull(result);
            Assert.AreEqual(EncodingType.Empty, result.EncodingType);
            Assert.AreEqual(new IdentityEncoding(), result.Encoding);

            services.WithHostEncoding(new GzipDeflateEncoding());
            service = new ServiceResolver(services).Find(MethodType.Post, "foo");
            result = service.GetResponseEncoder("gzip, *");
            Assert.IsNotNull(result);
            Assert.AreEqual(EncodingType.Parse("gzip"), result.EncodingType);
            Assert.AreEqual(new GzipDeflateEncoding(), result.Encoding);
        }

        [Test]
        public void ResolvedServiceGetResponseSerializer()
        {
            ServiceCollection services = new ServiceCollection();
            services
                .WithService("Test", "/")
                    .WithEndpoint("{action}")
                        .Post<Payload>((Payload p) => { });

            ResolvedService service = new ServiceResolver(services).Find(MethodType.Post, "foo");
            FormatLookupResult result = service.GetResponseSerializer("application/json");
            Assert.IsNull(result);

            result = service.GetResponseSerializer("application/json, */*");
            Assert.IsNotNull(result);
            Assert.AreEqual(MediaType.Empty, result.MediaType);
            Assert.AreEqual(new PlainTextFormat(), result.Format);

            services.WithHostFormat(new JsonFormat());
            service = new ServiceResolver(services).Find(MethodType.Post, "foo");
            result = service.GetResponseSerializer("application/json, */*");
            Assert.IsNotNull(result);
            Assert.AreEqual(MediaType.Parse("application/json"), result.MediaType);
            Assert.AreEqual(new JsonFormat(), result.Format);
        }

        private class Payload
        {
            public DateTime Date { get; set; }

            public long Number { get; set; }

            public string Text { get; set; }
        }
    }
}