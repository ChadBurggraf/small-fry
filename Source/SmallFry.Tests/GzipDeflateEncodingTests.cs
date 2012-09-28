namespace SmallFry.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;

    [TestFixture]
    public sealed class GzipDeflateEncodingTests
    {
        private const string Content = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse nec quam a lorem molestie eleifend. Pellentesque sit amet eros velit. Vivamus rhoncus, elit eu sollicitudin posuere, dolor ipsum tempus massa, semper faucibus lectus nunc eget ante. Proin dignissim, nisi eget tempor tincidunt, mauris felis adipiscing leo, quis pharetra lectus turpis id metus. Etiam aliquam rhoncus adipiscing. Pellentesque volutpat pellentesque pellentesque. Integer mattis mollis tortor, vel eleifend mi ultrices fringilla. Cras luctus est ut massa faucibus sit amet aliquet augue fermentum. Morbi dictum pulvinar ante nec molestie.

Nullam pretium rhoncus porta. Quisque vitae mauris sed nisl ornare bibendum eget ut diam. Integer enim magna, feugiat vel malesuada hendrerit, condimentum sed turpis. Fusce sodales, lacus vel consectetur tempus, arcu justo molestie erat, non placerat magna felis eget sem. Suspendisse imperdiet porttitor vestibulum. Suspendisse arcu massa, interdum a dignissim id, rutrum eu risus. Duis odio elit, pulvinar non rutrum sed, aliquet id ante. Mauris pretium quam in mi fermentum gravida.

Phasellus eget sem eu turpis iaculis viverra. Quisque eget ante orci, quis laoreet nisl. Etiam sit amet varius libero. Nunc placerat venenatis tempus. Praesent consectetur pretium erat, et dignissim nisl faucibus a. Curabitur turpis lorem, porttitor at tempor eu, venenatis eu urna. Fusce ultricies, tellus at ullamcorper sodales, velit lacus venenatis turpis, eu tincidunt nisl odio at neque. Etiam sollicitudin porttitor tortor a tincidunt. Integer varius vestibulum aliquam. Fusce non varius lectus. Mauris arcu ipsum, cursus vel volutpat at, tincidunt in justo. Donec auctor viverra malesuada.

Mauris libero elit, sodales nec tristique vel, vehicula quis odio. Mauris suscipit enim quis nisl tristique placerat eget a nisl. Proin odio ante, condimentum at cursus dictum, commodo et mi. Curabitur quis bibendum diam. Nunc convallis rutrum lacus, quis convallis dolor iaculis non. Maecenas nulla enim, placerat eleifend vestibulum eu, euismod et mi. Donec non diam eu lacus tincidunt scelerisque in et metus. Suspendisse elementum, orci sed imperdiet aliquam, ipsum magna porttitor tortor, non adipiscing nisl augue nec enim. Aliquam non mauris et lacus lobortis semper vitae quis velit.

Aliquam erat volutpat. Cras magna est, aliquet sit amet vestibulum facilisis, accumsan at nibh. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Vivamus nec ipsum nulla. Donec euismod sem et dolor auctor eu ultricies risus rutrum. Donec vel elit lorem. Vivamus ac pulvinar sem.";

        [Test]
        public void GzipDeflateEncodingCanDecode()
        {
            Assert.IsTrue(new GzipDeflateEncoding().CanDecode(EncodingType.Parse("deflate")));
            Assert.IsTrue(new GzipDeflateEncoding().CanDecode(EncodingType.Parse("gzip")));
            Assert.IsFalse(new GzipDeflateEncoding().CanDecode(EncodingType.Parse("compress")));
        }

        [Test]
        public void GzipDeflateEncodingCanEncode()
        {
            Assert.IsTrue(new GzipDeflateEncoding().CanEncode(EncodingType.Parse("deflate")));
            Assert.IsTrue(new GzipDeflateEncoding().CanEncode(EncodingType.Parse("gzip")));
            Assert.IsFalse(new GzipDeflateEncoding().CanEncode(EncodingType.Parse("compress")));
        }

        [Test]
        [SuppressMessage("Microsoft.Usage", "CA2202:DoNotDisposeObjectsMultipleTimess", Target = "outputStream", Justification = "DeflateStream does not dispose of its inner stream.")]
        public void GzipDeflateEncodingDecodeDeflate()
        {
            byte[] encoded;

            using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(GzipDeflateEncodingTests.Content)))
            {
                using (MemoryStream outputStream = new MemoryStream())
                {
                    using (DeflateStream compressionStream = new DeflateStream(outputStream, CompressionMode.Compress))
                    {
                        stream.CopyTo(compressionStream);
                    }

                    encoded = outputStream.ToArray();
                }
            }

            using (Stream stream = new MemoryStream(encoded))
            {
                Stream decodeStream = null;

                try
                {
                    decodeStream = new GzipDeflateEncoding().Decode(EncodingType.Parse("deflate"), stream);

                    using (StreamReader reader = new StreamReader(decodeStream, Encoding.Unicode))
                    {
                        decodeStream = null;
                        Assert.AreEqual(GzipDeflateEncodingTests.Content, reader.ReadToEnd());
                    }
                }
                finally
                {
                    if (decodeStream != null)
                    {
                        decodeStream.Dispose();
                    }
                }
            }
        }

        [Test]
        [SuppressMessage("Microsoft.Usage", "CA2202:DoNotDisposeObjectsMultipleTimess", Target = "outputStream", Justification = "GZipStream does not dispose of its inner stream.")]
        public void GzipDeflateEncodingDecodeGzip()
        {
            byte[] encoded;

            using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(GzipDeflateEncodingTests.Content)))
            {
                using (MemoryStream outputStream = new MemoryStream())
                {
                    using (GZipStream compressionStream = new GZipStream(outputStream, CompressionMode.Compress))
                    {
                        stream.CopyTo(compressionStream);
                    }

                    encoded = outputStream.ToArray();
                }
            }

            using (Stream stream = new MemoryStream(encoded))
            {
                Stream decodeStream = null;

                try
                {
                    decodeStream = new GzipDeflateEncoding().Decode(EncodingType.Parse("gzip"), stream);

                    using (StreamReader reader = new StreamReader(decodeStream, Encoding.Unicode))
                    {
                        decodeStream = null;
                        Assert.AreEqual(GzipDeflateEncodingTests.Content, reader.ReadToEnd());
                    }
                }
                finally
                {
                    if (decodeStream != null)
                    {
                        decodeStream.Dispose();
                    }
                }
            }
        }

        [Test]
        [SuppressMessage("Microsoft.Usage", "CA2202:DoNotDisposeObjectsMultipleTimess", Target = "outputStream", Justification = "DeflateStream does not dispose of its inner stream.")]
        public void GzipDeflateEncodingEncodeDeflate()
        {
            byte[] encoded, comparison;

            using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(GzipDeflateEncodingTests.Content)))
            {
                using (MemoryStream outputStream = new MemoryStream())
                {
                    using (Stream encodeStream = new GzipDeflateEncoding().Encode(EncodingType.Parse("deflate"), outputStream))
                    {
                        stream.CopyTo(encodeStream);
                    }

                    encoded = outputStream.ToArray();
                }
            }

            using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(GzipDeflateEncodingTests.Content)))
            {
                using (MemoryStream outputStream = new MemoryStream())
                {
                    using (DeflateStream compressionStream = new DeflateStream(outputStream, CompressionMode.Compress))
                    {
                        stream.CopyTo(compressionStream);
                    }

                    comparison = outputStream.ToArray();
                }
            }

            Assert.IsTrue(encoded.SequenceEqual(comparison));
        }

        [Test]
        [SuppressMessage("Microsoft.Usage", "CA2202:DoNotDisposeObjectsMultipleTimess", Target = "outputStream", Justification = "GZipStream does not dispose of its inner stream.")]
        public void GzipDeflateEncodingEncodeGzip()
        {
            byte[] encoded, comparison;

            using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(GzipDeflateEncodingTests.Content)))
            {
                using (MemoryStream outputStream = new MemoryStream())
                {
                    using (Stream encodeStream = new GzipDeflateEncoding().Encode(EncodingType.Parse("gzip"), outputStream))
                    {
                        stream.CopyTo(encodeStream);
                    }

                    encoded = outputStream.ToArray();
                }
            }

            using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(GzipDeflateEncodingTests.Content)))
            {
                using (MemoryStream outputStream = new MemoryStream())
                {
                    using (GZipStream compressionStream = new GZipStream(outputStream, CompressionMode.Compress))
                    {
                        stream.CopyTo(compressionStream);
                    }

                    comparison = outputStream.ToArray();
                }
            }

            Assert.IsTrue(encoded.SequenceEqual(comparison));
        }
    }
}
