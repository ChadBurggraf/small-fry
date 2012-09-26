namespace SmallFry.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public sealed class ServiceResolverTests
    {
        [Test]
        public void ServiceResolverExistsForAnyMethodType()
        {
            ServiceCollection services = new ServiceCollection();
            services
                .WithService("Test", "/")
                    .WithEndpoint("foo/{param}/bar")
                        .Get(() => { });

            ServiceResolver resolver = new ServiceResolver(services);
            Assert.IsTrue(resolver.ExistsForAnyMethodType("foo/baz/bar"));
            Assert.IsFalse(resolver.ExistsForAnyMethodType("foo/bar"));
        }

        [Test]
        public void ServiceResolverFind()
        {
            ServiceCollection services = new ServiceCollection();
            services
                .WithService("Test", "/")
                    .WithEndpoint("foo/{param}/bar")
                        .Get(() => { });

            ServiceResolver resolver = new ServiceResolver(services);
            Assert.IsNotNull(resolver.Find(MethodType.Get, "foo/baz/bar"));
            Assert.IsNull(resolver.Find(MethodType.Post, "foo/baz/bar"));
        }

        [Test]
        public void ServiceResolverFindRoutePatterns()
        {
            ServiceCollection services = new ServiceCollection();
            services
                .WithService("Test", "/")
                    .WithEndpoint("foo/{param}/bar", null, new { param = @"^\d+$" })
                        .Get(() => { });

            ServiceResolver resolver = new ServiceResolver(services);
            Assert.IsNotNull(resolver.Find(MethodType.Get, "foo/42/bar"));
            Assert.IsNull(resolver.Find(MethodType.Get, "foo/baz/bar"));
        }

        [Test]
        public void ServiceResolverFindRouteTypes()
        {
            ServiceCollection services = new ServiceCollection();
            services
                .WithService("Test", "/")
                    .WithEndpoint("foo/{param}/bar", new { param = typeof(Guid) })
                        .Get(() => { });

            ServiceResolver resolver = new ServiceResolver(services);
            Assert.IsNotNull(resolver.Find(MethodType.Get, "foo/D118CE6F-36BE-4DB4-B0E7-D426809B22FE/bar"));
            Assert.IsNull(resolver.Find(MethodType.Get, "foo/42/bar"));
        }
    }
}