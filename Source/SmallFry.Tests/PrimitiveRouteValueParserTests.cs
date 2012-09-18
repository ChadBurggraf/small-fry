namespace SmallFry.Tests
{
    using System;
    using System.Reflection;
    using NUnit.Framework;

    [TestFixture]
    public sealed class PrimitiveRouteValueParserTests
    {
        [Test]
        public void PrimitiveRouteValueParserCanParse()
        {
            IRouteValueParser parser = new PrimitiveRouteValueParser();
            Assert.IsTrue(parser.CanParse<bool>());
            Assert.IsTrue(parser.CanParse<byte>());
            Assert.IsTrue(parser.CanParse<char>());
            Assert.IsTrue(parser.CanParse<DateTime>());
            Assert.IsTrue(parser.CanParse<decimal>());
            Assert.IsTrue(parser.CanParse<double>());
            Assert.IsTrue(parser.CanParse<short>());
            Assert.IsTrue(parser.CanParse<int>());
            Assert.IsTrue(parser.CanParse<long>());
            Assert.IsTrue(parser.CanParse<sbyte>());
            Assert.IsTrue(parser.CanParse<float>());
            Assert.IsTrue(parser.CanParse<string>());
            Assert.IsTrue(parser.CanParse<UInt16>());
            Assert.IsTrue(parser.CanParse<uint>());
            Assert.IsTrue(parser.CanParse<UInt64>());

            Assert.IsFalse(parser.CanParse<object>());
            Assert.IsFalse(parser.CanParse<PrimitiveRouteValueParserTests>());
        }

        [Test]
        public void PrimitiveRouteValueParserTryParse()
        {
            IRouteValueParser parser = new PrimitiveRouteValueParser();

            bool boolResult;
            Assert.IsTrue(parser.TryParse<bool>("test", "true", out boolResult));
            Assert.AreEqual(true, boolResult);

            byte byteResult;
            Assert.IsTrue(parser.TryParse<byte>("test", "128", out byteResult));
            Assert.AreEqual((byte)128, byteResult);

            char charResult;
            Assert.IsTrue(parser.TryParse<char>("test", "x", out charResult));
            Assert.AreEqual('x', charResult);

            DateTime dateTimeResult;
            Assert.IsTrue(parser.TryParse<DateTime>("test", "2012-09-17T16:39:00Z", out dateTimeResult));
            Assert.AreEqual(new DateTime(2012, 09, 17, 16, 39, 0, DateTimeKind.Utc), dateTimeResult);

            decimal decimalResult;
            Assert.IsTrue(parser.TryParse<decimal>("test", ".12345", out decimalResult));
            Assert.AreEqual((decimal).12345, decimalResult);

            double doubleResult;
            Assert.IsTrue(parser.TryParse<double>("test", ".12345", out doubleResult));
            Assert.AreEqual(.12345, doubleResult);

            short shortResult;
            Assert.IsTrue(parser.TryParse<short>("test", "5", out shortResult));
            Assert.AreEqual((short)5, shortResult);

            int intResult;
            Assert.IsTrue(parser.TryParse<int>("test", "42", out intResult));
            Assert.AreEqual(42, intResult);

            long longResult;
            Assert.IsTrue(parser.TryParse<long>("test", "9999999", out longResult));
            Assert.AreEqual(9999999, longResult);

            sbyte sbyteResult;
            Assert.IsTrue(parser.TryParse<sbyte>("test", "3", out sbyteResult));
            Assert.AreEqual((sbyte)3, sbyteResult);

            float floatResult;
            Assert.IsTrue(parser.TryParse<float>("test", ".25", out floatResult));
            Assert.AreEqual(.25, floatResult);

            string stringResult;
            Assert.IsTrue(parser.TryParse<string>("test", "Hello, world!", out stringResult));
            Assert.AreEqual("Hello, world!", stringResult);

            UInt16 shortUIntResult;
            Assert.IsTrue(parser.TryParse<UInt16>("test", "5", out shortUIntResult));
            Assert.AreEqual((UInt16)5, shortUIntResult);

            uint uintResult;
            Assert.IsTrue(parser.TryParse<uint>("test", "42", out uintResult));
            Assert.AreEqual((uint)42, uintResult);

            UInt64 longUIntResult;
            Assert.IsTrue(parser.TryParse<UInt64>("test", "9999999", out longUIntResult));
            Assert.AreEqual((UInt64)9999999, longUIntResult);
        }
    }
}