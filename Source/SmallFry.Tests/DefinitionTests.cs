namespace SmallFry.Tests
{
    using System;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public sealed class DefinitionTests
    {
        [Test]
        public void DefinitionVirtualKeychain()
        {
            new Mock<IServiceCollection>().Object
                .WithServicesEncoding("gzip, deflate", new GzipDeflateEncoding())
                .WithServicesFormat("application/json, text/json, application/javascript, text/javascript", new JsonFormat())
                .WithService("Virtual Keychain")
                    .BeforeService()
                    .BeforeService()
                    .BeforeService()
                    .WithEndpoint("accounts")
                        .Get()
                        .Post()
                            .WithoutBeforeMethod()
                        .Delete()
                    .WithEndpoint("preferences")
                        .Put()
                    .WithEndpoint("entries/{?id}")
                        .Post()
                        .Put()
                        .Delete()
                    .WithEndpoint("labels/{?id}")
                        .Post()
                        .Put()
                        .Delete()
                    .WithEndpoint("entries/{entryId}/labels/{?labelId}")
                        .Post()
                        .Delete()
                    .AfterService()
                    .ErrorService();
        }
    }
}