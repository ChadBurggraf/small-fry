namespace SmallFry.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
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

        [Test]
        public void ResolvedServiceInvokeAfterActions()
        {
            Payload payload = new Payload()
            {
                Date = DateTime.UtcNow,
                Number = 42,
                Text = "Hello, world!"
            };

            ServiceCollection services = new ServiceCollection();
            services
                .WithService("Test", "/")
                    .WithEndpoint("{action}")
                        .Post<Payload>((Payload p) => { });

            ResolvedService service = new ServiceResolver(services).Find(MethodType.Post, "foo");

            using (IRequestMessage request = new RequestMessage<Payload>("Test", new Uri("http://example.com/foo"), payload))
            {
                using (IResponseMessage response = new ResponseMessage())
                {
                    InvokeActionsResult result = service.InvokeAfterActions(request, response);
                    Assert.IsNotNull(result);
                    Assert.IsTrue(result.Continue);
                    Assert.IsTrue(result.Success);
                    Assert.IsFalse(result.Results.Any());

                    services.AfterService<Payload>(
                        (req, resp) =>
                        {
                            Assert.AreEqual(payload, req.RequestObject);
                            return true;
                        });

                    service = new ServiceResolver(services).Find(MethodType.Post, "foo");
                    result = service.InvokeAfterActions(request, response);
                    Assert.IsTrue(result.Continue);
                    Assert.IsTrue(result.Success);
                    Assert.AreEqual(1, result.Results.Count);

                    services.AfterService(() => true);

                    service = new ServiceResolver(services).Find(MethodType.Post, "foo");
                    result = service.InvokeAfterActions(request, response);
                    Assert.IsTrue(result.Continue);
                    Assert.IsTrue(result.Success);
                    Assert.AreEqual(2, result.Results.Count);
                }
            }
        }

        [Test]
        public void ResolvedServiceInvokeAfterActionsExceptionContinue()
        {
            Payload payload = new Payload()
            {
                Date = DateTime.UtcNow,
                Number = 42,
                Text = "Hello, world!"
            };

            ServiceCollection services = new ServiceCollection();
            services
                .WithService("Test", "/")
                    .WithEndpoint("{action}")
                        .Post<Payload>((Payload p) => { })
                .AfterService<Payload>(
                    (req, resp) =>
                    {
                        throw new Exception();
                    })
                .AfterService(
                    () =>
                    {
                        return true;
                    });

            using (IRequestMessage request = new RequestMessage<Payload>("Test", new Uri("http://example.com/foo"), payload))
            {
                using (IResponseMessage response = new ResponseMessage())
                {
                    ResolvedService service = new ServiceResolver(services).Find(MethodType.Post, "foo");
                    InvokeActionsResult result = service.InvokeAfterActions(request, response);
                    Assert.IsTrue(result.Continue);
                    Assert.IsFalse(result.Success);
                    Assert.AreEqual(2, result.Results.Count);
                }
            }
        }

        [Test]
        public void ResolvedServiceInvokeAfterActionsNoContinue()
        {
            Payload payload = new Payload()
            {
                Date = DateTime.UtcNow,
                Number = 42,
                Text = "Hello, world!"
            };

            ServiceCollection services = new ServiceCollection();
            services
                .WithService("Test", "/")
                    .WithEndpoint("{action}")
                        .Post<Payload>((Payload p) => { })
                .AfterService<Payload>(
                    (req, resp) =>
                    {
                        Assert.AreEqual(payload, req.RequestObject);
                        return false;
                    })
                .AfterService(
                    () =>
                    {
                        Assert.Fail();
                        return true;
                    });

            using (IRequestMessage request = new RequestMessage<Payload>("Test", new Uri("http://example.com/foo"), payload))
            {
                using (IResponseMessage response = new ResponseMessage())
                {
                    ResolvedService service = new ServiceResolver(services).Find(MethodType.Post, "foo");
                    InvokeActionsResult result = service.InvokeAfterActions(request, response);
                    Assert.IsFalse(result.Continue);
                    Assert.IsTrue(result.Success);
                    Assert.AreEqual(1, result.Results.Count);

                }
            }
        }

        [Test]
        public void ResolvedServiceInvokeBeforeActions()
        {
            Payload payload = new Payload()
            {
                Date = DateTime.UtcNow,
                Number = 42,
                Text = "Hello, world!"
            };

            ServiceCollection services = new ServiceCollection();
            services
                .WithService("Test", "/")
                    .WithEndpoint("{action}")
                        .Post<Payload>((Payload p) => { });

            ResolvedService service = new ServiceResolver(services).Find(MethodType.Post, "foo");

            using (IRequestMessage request = new RequestMessage<Payload>("Test", new Uri("http://example.com/foo"), payload))
            {
                using (IResponseMessage response = new ResponseMessage())
                {
                    InvokeActionsResult result = service.InvokeBeforeActions(request, response);
                    Assert.IsNotNull(result);
                    Assert.IsTrue(result.Continue);
                    Assert.IsTrue(result.Success);
                    Assert.IsFalse(result.Results.Any());

                    services.BeforeService<Payload>(
                        (req, resp) =>
                        {
                            Assert.AreEqual(payload, req.RequestObject);
                            return true;
                        });

                    service = new ServiceResolver(services).Find(MethodType.Post, "foo");
                    result = service.InvokeBeforeActions(request, response);
                    Assert.IsTrue(result.Continue);
                    Assert.IsTrue(result.Success);
                    Assert.AreEqual(1, result.Results.Count);

                    services.BeforeService(() => true);

                    service = new ServiceResolver(services).Find(MethodType.Post, "foo");
                    result = service.InvokeBeforeActions(request, response);
                    Assert.IsTrue(result.Continue);
                    Assert.IsTrue(result.Success);
                    Assert.AreEqual(2, result.Results.Count);
                }
            }
        }

        [Test]
        public void ResolvedServiceInvokeBeforeActionsExceptionContinue()
        {
            Payload payload = new Payload()
            {
                Date = DateTime.UtcNow,
                Number = 42,
                Text = "Hello, world!"
            };

            ServiceCollection services = new ServiceCollection();
            services
                .WithService("Test", "/")
                    .WithEndpoint("{action}")
                        .Post<Payload>((Payload p) => { })
                .BeforeService<Payload>(
                    (req, resp) =>
                    {
                        throw new Exception();
                    })
                .BeforeService(
                    () =>
                    {
                        return true;
                    });

            using (IRequestMessage request = new RequestMessage<Payload>("Test", new Uri("http://example.com/foo"), payload))
            {
                using (IResponseMessage response = new ResponseMessage())
                {
                    ResolvedService service = new ServiceResolver(services).Find(MethodType.Post, "foo");
                    InvokeActionsResult result = service.InvokeBeforeActions(request, response);
                    Assert.IsTrue(result.Continue);
                    Assert.IsFalse(result.Success);
                    Assert.AreEqual(2, result.Results.Count);
                }
            }
        }

        [Test]
        public void ResolvedServiceInvokeBeforeActionsNoContinue()
        {
            Payload payload = new Payload()
            {
                Date = DateTime.UtcNow,
                Number = 42,
                Text = "Hello, world!"
            };

            ServiceCollection services = new ServiceCollection();
            services
                .WithService("Test", "/")
                    .WithEndpoint("{action}")
                        .Post<Payload>((Payload p) => { })
                .BeforeService<Payload>(
                    (req, resp) =>
                    {
                        Assert.AreEqual(payload, req.RequestObject);
                        return false;
                    })
                .BeforeService(
                    () =>
                    {
                        Assert.Fail();
                        return true;
                    });

            using (IRequestMessage request = new RequestMessage<Payload>("Test", new Uri("http://example.com/foo"), payload))
            {
                using (IResponseMessage response = new ResponseMessage())
                {
                    ResolvedService service = new ServiceResolver(services).Find(MethodType.Post, "foo");
                    InvokeActionsResult result = service.InvokeBeforeActions(request, response);
                    Assert.IsFalse(result.Continue);
                    Assert.IsTrue(result.Success);
                    Assert.AreEqual(1, result.Results.Count);

                }
            }
        }

        [Test]
        public void ResolvedServiceInvokeErrorActions()
        {
            Payload payload = new Payload()
            {
                Date = DateTime.UtcNow,
                Number = 42,
                Text = "Hello, world!"
            };

            ServiceCollection services = new ServiceCollection();
            services
                .WithService("Test", "/")
                    .WithEndpoint("{action}")
                        .Post<Payload>((Payload p) => { });

            ResolvedService service = new ServiceResolver(services).Find(MethodType.Post, "foo");

            using (IRequestMessage request = new RequestMessage<Payload>("Test", new Uri("http://example.com/foo"), payload))
            {
                using (IResponseMessage response = new ResponseMessage())
                {
                    InvokeActionsResult result = service.InvokeErrorActions(request, response, new Exception[0]);
                    Assert.IsNotNull(result);
                    Assert.IsTrue(result.Continue);
                    Assert.IsTrue(result.Success);
                    Assert.IsFalse(result.Results.Any());

                    services.ErrorService<Payload>(
                        (ex, req, resp) =>
                        {
                            Assert.AreEqual(payload, req.RequestObject);
                            return true;
                        });

                    service = new ServiceResolver(services).Find(MethodType.Post, "foo");
                    result = service.InvokeErrorActions(request, response, new Exception[0]);
                    Assert.IsTrue(result.Continue);
                    Assert.IsTrue(result.Success);
                    Assert.AreEqual(1, result.Results.Count);

                    services.ErrorService(ex => true);

                    service = new ServiceResolver(services).Find(MethodType.Post, "foo");
                    result = service.InvokeErrorActions(request, response, new Exception[0]);
                    Assert.IsTrue(result.Continue);
                    Assert.IsTrue(result.Success);
                    Assert.AreEqual(2, result.Results.Count);
                }
            }
        }

        [Test]
        public void ResolvedServiceInvokeErrorActionsExceptionContinue()
        {
            Payload payload = new Payload()
            {
                Date = DateTime.UtcNow,
                Number = 42,
                Text = "Hello, world!"
            };

            ServiceCollection services = new ServiceCollection();
            services
                .WithService("Test", "/")
                    .WithEndpoint("{action}")
                        .Post<Payload>((Payload p) => { })
                .ErrorService<Payload>(
                    (ex, req, resp) =>
                    {
                        throw new Exception();
                    })
                .ErrorService(
                    ex =>
                    {
                        return true;
                    });

            using (IRequestMessage request = new RequestMessage<Payload>("Test", new Uri("http://example.com/foo"), payload))
            {
                using (IResponseMessage response = new ResponseMessage())
                {
                    ResolvedService service = new ServiceResolver(services).Find(MethodType.Post, "foo");
                    InvokeActionsResult result = service.InvokeErrorActions(request, response, new Exception[0]);
                    Assert.IsTrue(result.Continue);
                    Assert.IsFalse(result.Success);
                    Assert.AreEqual(2, result.Results.Count);
                }
            }
        }

        [Test]
        public void ResolvedServiceInvokeErrorActionsNoContinue()
        {
            Payload payload = new Payload()
            {
                Date = DateTime.UtcNow,
                Number = 42,
                Text = "Hello, world!"
            };

            ServiceCollection services = new ServiceCollection();
            services
                .WithService("Test", "/")
                    .WithEndpoint("{action}")
                        .Post<Payload>((Payload p) => { })
                .ErrorService<Payload>(
                    (ex, req, resp) =>
                    {
                        Assert.AreEqual(payload, req.RequestObject);
                        return false;
                    })
                .ErrorService(
                    ex =>
                    {
                        Assert.Fail();
                        return true;
                    });

            using (IRequestMessage request = new RequestMessage<Payload>("Test", new Uri("http://example.com/foo"), payload))
            {
                using (IResponseMessage response = new ResponseMessage())
                {
                    ResolvedService service = new ServiceResolver(services).Find(MethodType.Post, "foo");
                    InvokeActionsResult result = service.InvokeErrorActions(request, response, new Exception[0]);
                    Assert.IsFalse(result.Continue);
                    Assert.IsTrue(result.Success);
                    Assert.AreEqual(1, result.Results.Count);

                }
            }
        }

        [Test]
        public void ResolvedServiceReadRequest()
        {
            ServiceCollection services = new ServiceCollection();
            services
                .WithHostFormat(new JsonFormat())
                .WithService("Test", "/")
                    .WithEndpoint("{action}")
                        .Get(() => { })
                        .Post<Payload>((Payload p) => { });

            ResolvedService service = new ServiceResolver(services).Find(MethodType.Get, "foo");
            ReadRequestResult result = service.ReadRequest(0, null, null, null);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.AreEqual(StatusCode.None, result.StatusCode);
            Assert.IsNull(result.RequestObject);

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes("{\"date\":\"2012-09-22T18:46:00Z\",\"number\":42,\"text\":\"Hello, world!\"}")))
            {
                service = new ServiceResolver(services).Find(MethodType.Post, "foo");
                result = service.ReadRequest((int)stream.Length, null, "application/json", stream);
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Success);
                Assert.IsNull(result.Exception);
                Assert.AreEqual(StatusCode.None, result.StatusCode);
                Assert.IsNotNull(result.RequestObject);

                Payload payload = result.RequestObject as Payload;
                Assert.IsNotNull(payload);
                Assert.AreEqual(new DateTime(2012, 9, 22, 18, 46, 0, DateTimeKind.Utc), payload.Date);
                Assert.AreEqual(42L, payload.Number);
                Assert.AreEqual("Hello, world!", payload.Text);
            }
        }

        private class Payload
        {
            public DateTime Date { get; set; }

            public long Number { get; set; }

            public string Text { get; set; }
        }
    }
}