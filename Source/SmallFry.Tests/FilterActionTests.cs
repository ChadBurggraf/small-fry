namespace SmallFry.Tests
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public sealed class FilterActionTests
    {
        [Test]
        public void FilterActionEquals()
        {
            FilterAction requestResponse = new FilterAction(FilterActionTests.RequestResponseAction);
            FilterAction requestResponseTyped = new FilterAction<TypedRequest>(FilterActionTests.RequestResponseAction<TypedRequest>);
            FilterAction requestResponseException = new FilterAction(FilterActionTests.RequestResponseExceptionAction);
            FilterAction requestResponseExceptionTyped = new FilterAction<TypedRequest>(FilterActionTests.RequestResponseExceptionAction<TypedRequest>);
            FilterAction simple = new FilterAction(FilterActionTests.SimpleAction);
            FilterAction simpleException = new FilterAction(FilterActionTests.SimpleExceptionAction);

            Assert.AreEqual(new FilterAction(FilterActionTests.RequestResponseAction), requestResponse);
            Assert.AreEqual(new FilterAction<TypedRequest>(FilterActionTests.RequestResponseAction<TypedRequest>), requestResponseTyped);
            Assert.AreEqual(new FilterAction(FilterActionTests.RequestResponseExceptionAction), requestResponseException);
            Assert.AreEqual(new FilterAction<TypedRequest>(FilterActionTests.RequestResponseExceptionAction<TypedRequest>), requestResponseExceptionTyped);
            Assert.AreEqual(new FilterAction(FilterActionTests.SimpleAction), simple);
            Assert.AreEqual(new FilterAction(FilterActionTests.SimpleExceptionAction), simpleException); 
        }

        [Test]
        public void FilterActionNotEquals()
        {
            FilterAction requestResponse = new FilterAction(FilterActionTests.RequestResponseAction);
            FilterAction requestResponseTyped = new FilterAction<TypedRequest>(FilterActionTests.RequestResponseAction<TypedRequest>);
            FilterAction requestResponseException = new FilterAction(FilterActionTests.RequestResponseExceptionAction);
            FilterAction requestResponseExceptionTyped = new FilterAction<TypedRequest>(FilterActionTests.RequestResponseExceptionAction<TypedRequest>);
            FilterAction simple = new FilterAction(FilterActionTests.SimpleAction);
            FilterAction simpleException = new FilterAction(FilterActionTests.SimpleExceptionAction);

            Assert.AreNotEqual(requestResponse, requestResponseTyped);
            Assert.AreNotEqual(requestResponse, requestResponseException);
            Assert.AreNotEqual(requestResponse, requestResponseExceptionTyped);
            Assert.AreNotEqual(requestResponse, simple);
            Assert.AreNotEqual(requestResponse, simpleException);

            Assert.AreNotEqual(requestResponseTyped, requestResponse);
            Assert.AreNotEqual(requestResponseTyped, requestResponseException);
            Assert.AreNotEqual(requestResponseTyped, requestResponseExceptionTyped);
            Assert.AreNotEqual(requestResponseTyped, simple);
            Assert.AreNotEqual(requestResponseTyped, simpleException);

            Assert.AreNotEqual(requestResponseException, requestResponse);
            Assert.AreNotEqual(requestResponseException, requestResponseTyped);
            Assert.AreNotEqual(requestResponseException, requestResponseExceptionTyped);
            Assert.AreNotEqual(requestResponseException, simple);
            Assert.AreNotEqual(requestResponseException, simpleException);

            Assert.AreNotEqual(requestResponseExceptionTyped, requestResponse);
            Assert.AreNotEqual(requestResponseExceptionTyped, requestResponseTyped);
            Assert.AreNotEqual(requestResponseExceptionTyped, requestResponseException);
            Assert.AreNotEqual(requestResponseExceptionTyped, simple);
            Assert.AreNotEqual(requestResponseExceptionTyped, simpleException);

            Assert.AreNotEqual(simple, requestResponse);
            Assert.AreNotEqual(simple, requestResponseTyped);
            Assert.AreNotEqual(simple, requestResponseException);
            Assert.AreNotEqual(simple, requestResponseExceptionTyped);
            Assert.AreNotEqual(simple, simpleException);

            Assert.AreNotEqual(simpleException, requestResponse);
            Assert.AreNotEqual(simpleException, requestResponseTyped);
            Assert.AreNotEqual(simpleException, requestResponseException);
            Assert.AreNotEqual(simpleException, requestResponseExceptionTyped);
            Assert.AreNotEqual(simpleException, simple);

            Assert.AreNotEqual(requestResponseTyped, new FilterAction<TypedRequest2>(FilterActionTests.RequestResponseAction<TypedRequest2>));
        }

        private static bool RequestResponseAction(IRequestMessage request, IResponseMessage response)
        {
            return true;
        }

        private static bool RequestResponseAction<T>(IRequestMessage<T> request, IResponseMessage response)
        {
            return true;
        }

        private static bool RequestResponseExceptionAction(IEnumerable<Exception> exceptions, IRequestMessage request, IResponseMessage response)
        {
            return true;
        }

        private static bool RequestResponseExceptionAction<T>(IEnumerable<Exception> exceptions, IRequestMessage<T> request, IResponseMessage response)
        {
            return true;
        }

        private static bool SimpleAction()
        {
            return true;
        }

        private static bool SimpleExceptionAction(IEnumerable<Exception> exceptions)
        {
            return true;
        }

        private sealed class TypedRequest
        {
        }

        private sealed class TypedRequest2
        {
        }
    }
}