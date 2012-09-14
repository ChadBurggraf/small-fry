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
            VirtualKeychainServices vkc = new VirtualKeychainServices();

            new Mock<IServiceCollection>().Object
                .WithServicesEncoding("gzip, deflate", new GzipDeflateEncoding())
                .WithServicesFormat("application/json, */*", new JsonFormat())
                .WithService("Virtual Keychain")
                //.BeforeService()
                //.BeforeService()
                //.BeforeService()
                    .WithEndpoint("accounts")
                        .Get(vkc.GetAccounts)
                        .Post(vkc.PostAccounts)
                            .WithoutBeforeMethod()
                        .Delete(vkc.DeleteAccounts)
                    .WithEndpoint("preferences")
                        .Put<VirtualKeychainServices.Preferences>(vkc.PutPreferences)
                    .WithEndpoint("entries/{?id}")
                        .Post<VirtualKeychainServices.Entry>(vkc.PostEntries)
                        .Put<VirtualKeychainServices.Entry>(vkc.PutEntries)
                        .Delete(vkc.DeleteEntries)
                    .WithEndpoint("labels/{?id}")
                        .Post<VirtualKeychainServices.Label>(vkc.PostLabels)
                        .Put<VirtualKeychainServices.Label>(vkc.PutLabels)
                        .Delete(vkc.DeleteLabels)
                    .WithEndpoint("entries/{entryId}/labels/{?labelId}")
                        .Post<VirtualKeychainServices.EntryLabel>(vkc.PostEntryLabels)
                        .Delete(vkc.DeleteEntryLabels)
                    .AfterService((req, res) => true);
                    //.ErrorService();
        }
    }
}