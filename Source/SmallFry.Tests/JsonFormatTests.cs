namespace SmallFry.Tests
{
    using System;
    using System.IO;
    using NUnit.Framework;

    [TestFixture]
    public sealed class JsonFormatTests
    {
        [Test]
        public void JsonFormatCanDeserialize()
        {
            Assert.IsTrue(new JsonFormat().CanDeserialize(MediaType.Parse("application/json")));
            Assert.IsFalse(new JsonFormat().CanDeserialize(MediaType.Parse("*/*")));
            Assert.IsFalse(new JsonFormat().CanDeserialize(MediaType.Parse("application/xml")));
        }

        [Test]
        public void JsonFormatCanSerialize()
        {
            Assert.IsTrue(new JsonFormat().CanSerialize(MediaType.Parse("*/*")));
            Assert.IsTrue(new JsonFormat().CanSerialize(MediaType.Parse("application/json")));
            Assert.IsFalse(new JsonFormat().CanSerialize(MediaType.Parse("application/xml")));
        }

        [Test]
        public void JsonFormatDeserialize()
        {
            Test test = new Test()
            {
                Date = new DateTime(2012, 9, 24, 10, 0, 0, DateTimeKind.Utc),
                Number = 42,
                Text = "Hello, world!"
            };

            using (MemoryStream stream = new MemoryStream())
            {
                Assert.AreEqual(null, new JsonFormat().Deserialize(MediaType.Parse("application/json"), typeof(Test), stream));
            }

            using (MemoryStream stream = new MemoryStream())
            {
                new JsonFormat().Serialize(MediaType.Parse("application/json"), test, stream);
                stream.Position = 0;

                Test deserialized = new JsonFormat().Deserialize(MediaType.Parse("application/json"), typeof(Test), stream) as Test;
                Assert.IsNotNull(deserialized);
                Assert.AreEqual(test.Date, deserialized.Date);
                Assert.AreEqual(test.Number, deserialized.Number);
                Assert.AreEqual(test.Text, deserialized.Text);
            }
        }

        [Test]
        public void JsonFormatSerialize()
        {
            Assert.AreEqual("{}", JsonFormatTests.Serialize(MediaType.Parse("application/json"), null));
            Assert.AreEqual("{\"pushToken\":42}", JsonFormatTests.Serialize(MediaType.Parse("application/json"), "{\"pushToken\":42}"));

            Test test = new Test()
            {
                Date = new DateTime(2012, 9, 24, 10, 0, 0, DateTimeKind.Utc),
                Number = 42,
                Text = "Hello, world!"
            };

            Assert.AreEqual("{\"date\":\"2012-09-24T10:00:00.0000000Z\",\"number\":42,\"text\":\"Hello, world!\"}", JsonFormatTests.Serialize(MediaType.Parse("application/json"), test));
        }

        private static string Serialize(MediaType mediaType, object value)
        {
            Stream stream = null;

            try
            {
                stream = new MemoryStream();

                new JsonFormat().Serialize(mediaType, value, stream);
                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream))
                {
                    stream = null;
                    return reader.ReadToEnd();
                }
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                }
            }
        }

        private class Test
        {
            public DateTime Date { get; set; }

            public long Number { get; set; }

            public string Text { get; set; }
        }
    }
}