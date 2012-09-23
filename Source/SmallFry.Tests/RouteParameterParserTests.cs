namespace SmallFry.Tests
{
    using System;
    using System.Reflection;
    using NUnit.Framework;

    [TestFixture]
    public sealed class RouteParameterParserTests
    {
        [Test]
        public void PrimitiveRouteValueParserTryParse()
        {
            IRouteParameterParser parser = new PrimitiveRouteParameterParser();
            Assert.AreEqual(true, parser.Parse(typeof(bool), "test", "true"));
            Assert.AreEqual((byte)128, parser.Parse(typeof(byte), "test", "128"));
            Assert.AreEqual('x', parser.Parse(typeof(char), "test", "x"));
            Assert.AreEqual(new DateTime(2012, 9, 17, 16, 39, 0, DateTimeKind.Utc), parser.Parse(typeof(DateTime), "test", "2012-09-17T16:39:00Z"));
            Assert.AreEqual((decimal).12345, parser.Parse(typeof(decimal), "test", ".12345"));
            Assert.AreEqual(.12345, parser.Parse(typeof(double), "test", ".12345"));
            Assert.AreEqual((short)5, parser.Parse(typeof(short), "test", "5"));
            Assert.AreEqual(42, parser.Parse(typeof(int), "test", "42"));
            Assert.AreEqual(9999999, parser.Parse(typeof(long), "test", "9999999"));
            Assert.AreEqual((sbyte)3, parser.Parse(typeof(sbyte), "test", "3"));
            Assert.AreEqual(.25, parser.Parse(typeof(float), "test", ".25"));
            Assert.AreEqual("Hello, world!", parser.Parse(typeof(string), "test", "Hello, world!"));
            Assert.AreEqual((ushort)5, parser.Parse(typeof(ushort), "test", "5"));
            Assert.AreEqual((uint)42, parser.Parse(typeof(uint), "test", "42"));
            Assert.AreEqual((ulong)9999999, parser.Parse(typeof(ulong), "test", "9999999"));
            Assert.AreEqual(StatusCode.NotAcceptable, parser.Parse(typeof(StatusCode), "test", "NotAcceptable"));
            Assert.AreEqual(new Guid("67EBC5D6-A11D-4531-924D-4CBE85BB484E"), parser.Parse(typeof(Guid), "test", "67EBC5D6-A11D-4531-924D-4CBE85BB484E"));

            // Nullable?
            Assert.AreEqual(42, parser.Parse(typeof(int?), "test", "42"));
            Assert.AreEqual(null, parser.Parse(typeof(int?), "test", string.Empty));
        }
    }
}