namespace SmallFry.Tests
{
    using System;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public sealed class MediaTypeTests
    {
        [Test]
        public void MediaTypeExtensionEquals()
        {
            Assert.AreEqual(MediaType.Extension.Parse("token"), MediaType.Extension.Parse("token"));
            Assert.AreEqual(MediaType.Extension.Parse("token=value"), MediaType.Extension.Parse("token=value"));
            Assert.AreEqual(MediaType.Extension.Parse("token=\"quoted value\""), MediaType.Extension.Parse("token=\"quoted value\""));
        }

        [Test]
        public void MediaTypeExtensionNotEquals()
        {
            Assert.AreNotEqual(MediaType.Extension.Parse("token"), MediaType.Extension.Parse("token=value"));
            Assert.AreNotEqual(MediaType.Extension.Parse("token"), MediaType.Extension.Parse("token=\"quoted value\""));
            Assert.AreNotEqual(MediaType.Extension.Parse("token=value"), MediaType.Extension.Parse("token=\"quoted value\""));
        }

        [Test]
        public void MediaTypeExtensionParse()
        {
            MediaType.Extension extension;

            Assert.IsTrue(MediaType.Extension.TryParse("token", out extension));
            Assert.AreEqual("token", extension.Key);
            Assert.AreEqual(string.Empty, extension.Value);

            Assert.IsTrue(MediaType.Extension.TryParse("token=value", out extension));
            Assert.AreEqual("token", extension.Key);
            Assert.AreEqual("value", extension.Value);

            Assert.IsTrue(MediaType.Extension.TryParse("token=\"quoted value\"", out extension));
            Assert.AreEqual("token", extension.Key);
            Assert.AreEqual("\"quoted value\"", extension.Value);
        }

        [Test]
        public void MediaTypeExtensionToString()
        {
            MediaType.Extension extension;

            Assert.IsTrue(MediaType.Extension.TryParse("token", out extension));
            Assert.AreEqual("token", extension.ToString());

            Assert.IsTrue(MediaType.Extension.TryParse("token=value", out extension));
            Assert.AreEqual("token=value", extension.ToString());

            Assert.IsTrue(MediaType.Extension.TryParse("token=\"quoted value\"", out extension));
            Assert.AreEqual("token=\"quoted value\"", extension.ToString());
        }

        [Test]
        public void MediaTypeParamsEquals()
        {
            Assert.AreEqual(MediaType.Params.Parse("q=0.5"), MediaType.Params.Parse("q=0.5"));
            Assert.AreEqual(MediaType.Params.Parse("q=0.5;token"), MediaType.Params.Parse("q=0.5;token"));
            Assert.AreEqual(MediaType.Params.Parse("q=0.5;token;token=value"), MediaType.Params.Parse("q=0.5;token;token=value"));
            Assert.AreEqual(MediaType.Params.Parse("q=0.5;token;token=value;token=\"quoted value\""), MediaType.Params.Parse("q=0.5;token;token=value;token=\"quoted value\""));
        }

        [Test]
        public void MediaTypeParamsNotEquals()
        {
            Assert.AreNotEqual(MediaType.Params.Parse("q=0.5"), MediaType.Params.Parse("q=0.6"));
            Assert.AreNotEqual(MediaType.Params.Parse("q=0.5;token"), MediaType.Params.Parse("q=0.5;token1"));
            Assert.AreNotEqual(MediaType.Params.Parse("q=0.5;token;token=value"), MediaType.Params.Parse("q=0.5;token;token=value1"));
            Assert.AreNotEqual(MediaType.Params.Parse("q=0.5;token;token=value;token=\"quoted value\""), MediaType.Params.Parse("q=0.5;token;token=value;token=\"quoted value 1\""));
        }

        [Test]
        public void MediaTypeParamsParse()
        {
            MediaType.Params p;

            Assert.IsTrue(MediaType.Params.TryParse("q=0.5", out p));
            Assert.AreEqual("0.5", p.Value);
            Assert.IsNotNull(p.Extensions);
            Assert.IsFalse(p.Extensions.Any());

            Assert.IsTrue(MediaType.Params.TryParse("q=0.5;token", out p));
            Assert.AreEqual("0.5", p.Value);
            Assert.IsNotNull(p.Extensions);
            Assert.AreEqual(1, p.Extensions.Count());
            Assert.AreEqual(MediaType.Extension.Parse("token"), p.Extensions.First());

            Assert.IsTrue(MediaType.Params.TryParse("q=0.5;token;token=value", out p));
            Assert.AreEqual("0.5", p.Value);
            Assert.IsNotNull(p.Extensions);
            Assert.AreEqual(2, p.Extensions.Count());
            Assert.AreEqual(MediaType.Extension.Parse("token"), p.Extensions.First());
            Assert.AreEqual(MediaType.Extension.Parse("token=value"), p.Extensions.Last());

            Assert.IsTrue(MediaType.Params.TryParse("q=0.5;token;token=value;token=\"quoted value\"", out p));
            Assert.AreEqual("0.5", p.Value);
            Assert.IsNotNull(p.Extensions);
            Assert.AreEqual(3, p.Extensions.Count());
            Assert.AreEqual(MediaType.Extension.Parse("token"), p.Extensions.First());
            Assert.AreEqual(MediaType.Extension.Parse("token=value"), p.Extensions.ElementAt(1));
            Assert.AreEqual(MediaType.Extension.Parse("token=\"quoted value\""), p.Extensions.Last());
        }

        [Test]
        public void MediaTypeParamsToString()
        {
            Assert.AreEqual("q=0.5", MediaType.Params.Parse("q=0.5").ToString());
            Assert.AreEqual("q=0.5;token", MediaType.Params.Parse("q=0.5;token").ToString());
            Assert.AreEqual("q=0.5;token;token=value", MediaType.Params.Parse("q=0.5;token;token=value").ToString());
            Assert.AreEqual("q=0.5;token;token=value;token=\"quoted value\"", MediaType.Params.Parse("q=0.5;token;token=value;token=\"quoted value\"").ToString());
        }
    }
}
