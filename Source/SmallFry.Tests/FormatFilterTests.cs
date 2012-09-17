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
            FormatFilter textFilter = new FormatFilter("text/plain", new PlainTextFormat());

            Assert.AreEqual(jsonFilter, new FormatFilter("application/json", new JsonFormat()));
            Assert.AreEqual(textFilter, new FormatFilter("text/plain", new PlainTextFormat()));
        }

        [Test]
        public void FormatFilterNotEquals()
        {
            FormatFilter jsonFilter = new FormatFilter("application/json", new JsonFormat());
            FormatFilter textFilter = new FormatFilter("text/plain", new PlainTextFormat());

            Assert.AreNotEqual(jsonFilter, textFilter);
            Assert.AreNotEqual(jsonFilter, new FormatFilter("application/json", new PlainTextFormat()));
            Assert.AreNotEqual(jsonFilter, new FormatFilter("text/json", new JsonFormat()));

            Assert.AreNotEqual(textFilter, new FormatFilter("text/xml", new JsonFormat()));
            Assert.AreNotEqual(textFilter, new FormatFilter("application/xml", new PlainTextFormat()));
        }
    }
}
