namespace SmallFry.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using SmallFry;

    [TestFixture]
    public sealed class ExtensionsTests
    {
        [Test]
        public void ExtensionsAsContentEncodings()
        {
            IEnumerable<EncodingType> acceptTypes = Extensions.AsContentEncodings(null);
            Assert.IsNotNull(acceptTypes);
            Assert.AreEqual(1, acceptTypes.Count());
            Assert.AreEqual(EncodingType.Parse("*"), acceptTypes.First());

            acceptTypes = "compress, gzip".AsContentEncodings();
            Assert.IsNotNull(acceptTypes);
            Assert.AreEqual(2, acceptTypes.Count());
            Assert.AreEqual(EncodingType.Parse("compress"), acceptTypes.First());
            Assert.AreEqual(EncodingType.Parse("gzip"), acceptTypes.Last());

            acceptTypes = "gzip;q=1.0, identity; q=0.5, *;q=0".AsContentEncodings();
            Assert.IsNotNull(acceptTypes);
            Assert.AreEqual(3, acceptTypes.Count());
            Assert.AreEqual(EncodingType.Parse("gzip;q=1.0"), acceptTypes.First());
            Assert.AreEqual(EncodingType.Parse("identity;q=0.5"), acceptTypes.ElementAt(1));
            Assert.AreEqual(EncodingType.Parse("*;q=0"), acceptTypes.Last());
        }
    }
}