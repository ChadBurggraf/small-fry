namespace SmallFry.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public sealed class EncodingTypeTests
    {
        [Test]
        public void EncodingTypeEquals()
        {
            Assert.AreEqual(EncodingType.Parse(null), EncodingType.Parse("*"));
            Assert.AreEqual(EncodingType.Parse("gzip"), EncodingType.Parse("gzip;q=1"));
            Assert.AreEqual(EncodingType.Parse("deflate;q=0.5"), EncodingType.Parse("deflate; q=0.5"));
        }

        [Test]
        public void EncodingTypeNotEquals()
        {
            Assert.AreNotEqual(EncodingType.Parse("*"), EncodingType.Parse("gzip"));
            Assert.AreNotEqual(EncodingType.Parse("gzip;q=0.5"), EncodingType.Parse("gzip;q=0.3"));
        }

        [Test]
        public void EncodingTypeParse()
        {
            EncodingType encoding;
            Assert.IsTrue(EncodingType.TryParse("*", out encoding));
            Assert.IsNotNull(encoding);
            Assert.AreEqual("*", encoding.Name);
            Assert.AreEqual(1f, encoding.QValue);

            Assert.IsTrue(EncodingType.TryParse("gzip", out encoding));
            Assert.IsNotNull(encoding);
            Assert.AreEqual("gzip", encoding.Name);
            Assert.AreEqual(1f, encoding.QValue);

            Assert.IsTrue(EncodingType.TryParse("compress;q=0.5", out encoding));
            Assert.IsNotNull(encoding);
            Assert.AreEqual("compress", encoding.Name);
            Assert.AreEqual(.5f, encoding.QValue);

            Assert.IsTrue(EncodingType.TryParse("*; q = 0.5", out encoding));
            Assert.IsNotNull(encoding);
            Assert.AreEqual("*", encoding.Name);
            Assert.AreEqual(.5f, encoding.QValue);
        }

        [Test]
        public void EncodingTypeToString()
        {
            Assert.AreEqual("*", EncodingType.Parse(null).ToString());
            Assert.AreEqual("deflate", EncodingType.Parse("deflate").ToString());
            Assert.AreEqual("gzip", EncodingType.Parse("gzip;q=1").ToString());
            Assert.AreEqual("gzip;q=0.333", EncodingType.Parse("gzip;q=0.3334").ToString());
        }
    }
}
