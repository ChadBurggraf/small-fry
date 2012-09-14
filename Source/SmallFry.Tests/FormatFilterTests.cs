namespace SmallFry.Tests
{
    using System;
    using System.IO;
    using NUnit.Framework;

    [TestFixture]
    public sealed class FormatFilterTests
    {
        [Test]
        public void FormatFilterEquals()
        {
            FormatFilter jsonFilter = new FormatFilter("application/json", new JsonFormat());
            FormatFilter xmlFilter = new FormatFilter("text/xml", new XmlFormat());

            Assert.AreEqual(jsonFilter, new FormatFilter("application/json", new JsonFormat()));
            Assert.AreEqual(xmlFilter, new FormatFilter("text/xml", new XmlFormat()));
        }

        [Test]
        public void FormatFilterNotEquals()
        {
            FormatFilter jsonFilter = new FormatFilter("application/json", new JsonFormat());
            FormatFilter xmlFilter = new FormatFilter("text/xml", new XmlFormat());

            Assert.AreNotEqual(jsonFilter, xmlFilter);
            Assert.AreNotEqual(jsonFilter, new FormatFilter("application/json", new XmlFormat()));
            Assert.AreNotEqual(jsonFilter, new FormatFilter("text/json", new JsonFormat()));

            Assert.AreNotEqual(xmlFilter, new FormatFilter("text/xml", new JsonFormat()));
            Assert.AreNotEqual(xmlFilter, new FormatFilter("application/xml", new XmlFormat()));
        }

        private sealed class JsonFormat : IFormat
        {
            public object Deserialize(Type type, Stream stream)
            {
                throw new NotImplementedException();
            }

            public void Serialize(object value, Stream stream)
            {
                throw new NotImplementedException();
            }
        }

        private sealed class XmlFormat : IFormat
        {
            public object Deserialize(Type type, Stream stream)
            {
                throw new NotImplementedException();
            }

            public void Serialize(object value, Stream stream)
            {
                throw new NotImplementedException();
            }
        }
    }
}
