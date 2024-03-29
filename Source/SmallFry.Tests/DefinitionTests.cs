﻿namespace SmallFry.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using Moq;
    using NUnit.Framework;
    
    [TestFixture]
    public sealed class DefinitionTests
    {
        [Test]
        public void DefinitionPassbook()
        {
            DefinitionTests.DefinePassbook(new ServiceCollection());
        }

        [Test]
        public void DefinitionPassbookContainsDeviceRegistrationEndpointAndMethods()
        {
            ServiceCollection services = new ServiceCollection();
            DefinitionTests.DefinePassbook(services);

            EndpointCollection endpoints = services.Last().Endpoints as EndpointCollection;
            Assert.IsNotNull(endpoints);
            Assert.IsTrue(2 <= endpoints.Count());
            Assert.AreEqual("v1/devices/{deviceLibraryIdentifier}/registrations/{passTypeIdentifier}", endpoints.ElementAt(1).Route.ToString());

            MethodCollection methods = endpoints.ElementAt(1).Methods as MethodCollection;
            Assert.IsNotNull(methods);
            Assert.AreEqual(2, methods.Count());
            Assert.AreEqual(MethodType.Get, methods.First().MethodType);
            Assert.AreEqual(MethodType.Post, methods.Last().MethodType);
        }

        [Test]
        public void DefinitionPassbookContainsDeviceUnregistrationEndpointAndMethods()
        {
            ServiceCollection services = new ServiceCollection();
            DefinitionTests.DefinePassbook(services);

            EndpointCollection endpoints = services.Last().Endpoints as EndpointCollection;
            Assert.IsNotNull(endpoints);
            Assert.IsTrue(3 <= endpoints.Count());
            Assert.AreEqual("v1/devices/{deviceLibraryIdentifier}/registrations/{passTypeIdentifier}/{serialNumber}", endpoints.ElementAt(2).Route.ToString());

            MethodCollection methods = endpoints.ElementAt(2).Methods as MethodCollection;
            Assert.IsNotNull(methods);
            Assert.AreEqual(1, methods.Count());
            Assert.AreEqual(MethodType.Delete, methods.First().MethodType);
        }

        [Test]
        public void DefinitionPassbookContainsGetLatestVersionOfPassEndpointAndMethods()
        {
            ServiceCollection services = new ServiceCollection();
            DefinitionTests.DefinePassbook(services);

            EndpointCollection endpoints = services.Last().Endpoints as EndpointCollection;
            Assert.IsNotNull(endpoints);
            Assert.IsTrue(endpoints.Any());
            Assert.AreEqual("v1/passes/{passTypeIdentifier}/{serialNumber}", endpoints.First().Route.ToString());

            MethodCollection methods = endpoints.First().Methods as MethodCollection;
            Assert.IsNotNull(methods);
            Assert.AreEqual(1, methods.Count());
            Assert.AreEqual(MethodType.Get, methods.First().MethodType);
        }

        [Test]
        public void DefinitionPassbookContainsService()
        {
            ServiceCollection services = new ServiceCollection();
            DefinitionTests.DefinePassbook(services);

            Assert.AreEqual(1, services.Count);
            Assert.IsNotNull(services.CurrentService);
            Assert.AreEqual(services.Last(), services.CurrentService);
            Assert.AreEqual("Passbook", services.CurrentService.Name);
        }

        private static void DefinePassbook(IServiceCollection services)
        {
            IEndpointCollection endpoints = services.WithService("Passbook", "/v1");
            
            IMethodCollection methods = endpoints.WithEndpoint("passes/{passTypeIdentifier}/{serialNumber}");
            methods = methods.Get(() => { });

            methods = methods = methods.WithEndpoint("devices/{deviceLibraryIdentifier}/registrations/{passTypeIdentifier}");
            methods = methods.Get(() => { });
            methods = methods.Post(() => { });

            methods = methods.WithEndpoint("devices/{deviceLibraryIdentifier}/registrations/{passTypeIdentifier}/{serialNumber}");
            methods = methods.Delete(() => { });
        }
    }
}