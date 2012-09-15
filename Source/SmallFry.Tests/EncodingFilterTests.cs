namespace SmallFry.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public sealed class EncodingFilterTests
    {
        [Test]
        public void EncodingFilterEquals()
        {
            Assert.AreEqual(new EncodingFilter("gzip, deflate", new GzipDeflateEncoding()), new EncodingFilter("gzip, deflate", new GzipDeflateEncoding()));
            Assert.AreEqual(new EncodingFilter("*", new IdentityEncoding()), new EncodingFilter("*", new IdentityEncoding()));
        }

        [Test]
        public void EncodingFilterNotEquals()
        {
            Assert.AreNotEqual(new EncodingFilter("gzip, deflate", new GzipDeflateEncoding()), new EncodingFilter("gzip, deflate", new IdentityEncoding()));
            Assert.AreNotEqual(new EncodingFilter("gzip", new GzipDeflateEncoding()), new EncodingFilter("gzip, deflate", new GzipDeflateEncoding()));
        }

        [Test]
        public void EncodingFilterParseAcceptTypes()
        {
            IEnumerable<EncodingType> acceptTypes = EncodingFilter.ParseAcceptTypes(null);
            Assert.IsNotNull(acceptTypes);
            Assert.AreEqual(1, acceptTypes.Count());
            Assert.AreEqual(EncodingType.Parse("*"), acceptTypes.First());

            acceptTypes = EncodingFilter.ParseAcceptTypes("compress, gzip");
            Assert.IsNotNull(acceptTypes);
            Assert.AreEqual(2, acceptTypes.Count());
            Assert.AreEqual(EncodingType.Parse("compress"), acceptTypes.First());
            Assert.AreEqual(EncodingType.Parse("gzip"), acceptTypes.Last());

            acceptTypes = EncodingFilter.ParseAcceptTypes("gzip;q=1.0, identity; q=0.5, *;q=0");
            Assert.IsNotNull(acceptTypes);
            Assert.AreEqual(3, acceptTypes.Count());
            Assert.AreEqual(EncodingType.Parse("gzip;q=1.0"), acceptTypes.First());
            Assert.AreEqual(EncodingType.Parse("identity;q=0.5"), acceptTypes.ElementAt(1));
            Assert.AreEqual(EncodingType.Parse("*;q=0"), acceptTypes.Last());
        }

        private sealed class IdentityEncoding : IEncoding
        {
            public void Decode(Stream inputStream, Stream outputStream)
            {
                throw new NotImplementedException();
            }

            public void Encode(Stream inputStream, Stream outputStream)
            {
                throw new NotImplementedException();
            }

            public bool Equals(IEncoding other)
            {
                if ((object)other != null)
                {
                    return this.GetType().Equals(other.GetType());
                }

                return false;
            }
        }
    }
}
