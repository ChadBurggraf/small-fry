namespace SmallFry.Tests
{
    using System;
    using NUnit.Framework;
    using SmallFry;

    [TestFixture]
    public sealed class FunctionTests
    {
        [Test]
        public void FunctionActionApply()
        {
            Action<string, IRequestMessage, IResponseMessage> filter = (roles, req, resp) =>
            {
                Assert.AreEqual("Administrators", roles);
            };

            filter.Apply("Administrators")(null, null);

            Action<string, string, IRequestMessage, IResponseMessage> filter2 = (users, roles, req, resp) =>
            {
                Assert.AreEqual("TestUser", users);
                Assert.AreEqual("Administrators", roles);
            };

            filter2.Apply("TestUser", "Administrators")(null, null);
        }

        [Test]
        public void FunctionFuncApply()
        {
            Func<string, IRequestMessage, IResponseMessage, bool> filter = (roles, req, resp) =>
            {
                Assert.AreEqual("Administrators", roles);
                return true;
            };

            Assert.AreEqual(true, filter.Apply("Administrators")(null, null));

            Func<string, string, IRequestMessage, IResponseMessage, bool> filter2 = (users, roles, req, resp) =>
            {
                Assert.AreEqual("TestUser", users);
                Assert.AreEqual("Administrators", roles);
                return true;
            };

            Assert.AreEqual(true, filter2.Apply("TestUser", "Administrators")(null, null));
        }
    }
}