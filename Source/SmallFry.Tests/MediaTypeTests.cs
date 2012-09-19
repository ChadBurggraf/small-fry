namespace SmallFry.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public sealed class MediaTypeTests
    {
        [Test]
        public void MediaTypeAcceptParametersEquals()
        {
            Assert.AreEqual(MediaType.AcceptParameters.Parse("q=0.5"), MediaType.AcceptParameters.Parse("q=0.5"));
            Assert.AreEqual(MediaType.AcceptParameters.Parse("q=0.5;token"), MediaType.AcceptParameters.Parse("q=0.5;token"));
            Assert.AreEqual(MediaType.AcceptParameters.Parse("q=0.5;token;token=value"), MediaType.AcceptParameters.Parse("q=0.5;token;token=value"));
            Assert.AreEqual(MediaType.AcceptParameters.Parse("q=0.5;token;token=value;token=\"quoted value\""), MediaType.AcceptParameters.Parse("q=0.5;token;token=value;token=\"quoted value\""));
        }

        [Test]
        public void MediaTypeAcceptParametersNotEquals()
        {
            Assert.AreNotEqual(MediaType.AcceptParameters.Parse("q=0.5"), MediaType.AcceptParameters.Parse("q=0.6"));
            Assert.AreNotEqual(MediaType.AcceptParameters.Parse("q=0.5;token"), MediaType.AcceptParameters.Parse("q=0.5;token1"));
            Assert.AreNotEqual(MediaType.AcceptParameters.Parse("q=0.5;token;token=value"), MediaType.AcceptParameters.Parse("q=0.5;token;token=value1"));
            Assert.AreNotEqual(MediaType.AcceptParameters.Parse("q=0.5;token;token=value;token=\"quoted value\""), MediaType.AcceptParameters.Parse("q=0.5;token;token=value;token=\"quoted value 1\""));
        }

        [Test]
        public void MediaTypeAcceptParametersParse()
        {
            MediaType.AcceptParameters p;

            Assert.IsTrue(MediaType.AcceptParameters.TryParse("q=0.5", out p));
            Assert.AreEqual(.5f, p.Value);
            Assert.IsNotNull(p.Extensions);
            Assert.IsFalse(p.Extensions.Any());

            Assert.IsTrue(MediaType.AcceptParameters.TryParse("q=0.5;token", out p));
            Assert.AreEqual(.5f, p.Value);
            Assert.IsNotNull(p.Extensions);
            Assert.AreEqual(1, p.Extensions.Count());
            Assert.AreEqual(MediaType.Extension.Parse("token"), p.Extensions.First());

            Assert.IsTrue(MediaType.AcceptParameters.TryParse("q=0.5;token;token=value", out p));
            Assert.AreEqual(.5f, p.Value);
            Assert.IsNotNull(p.Extensions);
            Assert.AreEqual(2, p.Extensions.Count());
            Assert.AreEqual(MediaType.Extension.Parse("token"), p.Extensions.First());
            Assert.AreEqual(MediaType.Extension.Parse("token=value"), p.Extensions.Last());

            Assert.IsTrue(MediaType.AcceptParameters.TryParse("q=0.5;token;token=value;token=\"quoted value\"", out p));
            Assert.AreEqual(.5f, p.Value);
            Assert.IsNotNull(p.Extensions);
            Assert.AreEqual(3, p.Extensions.Count());
            Assert.AreEqual(MediaType.Extension.Parse("token"), p.Extensions.First());
            Assert.AreEqual(MediaType.Extension.Parse("token=value"), p.Extensions.ElementAt(1));
            Assert.AreEqual(MediaType.Extension.Parse("token=\"quoted value\""), p.Extensions.Last());
        }

        [Test]
        public void MediaTypeAcceptParametersToString()
        {
            Assert.AreEqual("q=0.5", MediaType.AcceptParameters.Parse("q=0.5").ToString());
            Assert.AreEqual("q=0.5;token", MediaType.AcceptParameters.Parse("q=0.5;token").ToString());
            Assert.AreEqual("q=0.5;token;token=value", MediaType.AcceptParameters.Parse("q=0.5;token;token=value").ToString());
            Assert.AreEqual("q=0.5;token;token=value;token=\"quoted value\"", MediaType.AcceptParameters.Parse("q=0.5;token;token=value;token=\"quoted value\"").ToString());
        }

        [Test]
        public void MediaTypeCompare()
        {
            List<MediaType> list = new List<MediaType>(new MediaType[]
            {
                MediaType.Parse("text/*"),
                MediaType.Parse("text/html"),
                MediaType.Parse("text/html;level=1"),
                MediaType.Parse("*/*")
            });

            list.Sort();
            list.Reverse();

            Assert.AreEqual(MediaType.Parse("text/html;level=1"), list[0]);
            Assert.AreEqual(MediaType.Parse("text/html"), list[1]);
            Assert.AreEqual(MediaType.Parse("text/*"), list[2]);
            Assert.AreEqual(MediaType.Parse("*/*"), list[3]);

            list.Clear();

            list.AddRange(
                new MediaType[]
                {
                    MediaType.Parse("text/*;q=0.3"),
                    MediaType.Parse("text/html;q=0.7"),
                    MediaType.Parse("text/html;level=1"),
                    MediaType.Parse("text/html;level=2;q=0.4"),
                    MediaType.Parse("*/*;q=0.5")
                });

            list.Sort();
            list.Reverse();

            Assert.AreEqual("text/html;level=1", list[0].ToString());
            Assert.AreEqual("text/html;q=0.7", list[1].ToString());
            Assert.AreEqual("*/*;q=0.5", list[2].ToString());
            Assert.AreEqual("text/html;level=2;q=0.4", list[3].ToString());
            Assert.AreEqual("text/*;q=0.3", list[4].ToString());
        }

        [Test]
        public void MediaTypeEquals()
        {
            Assert.AreEqual(MediaType.Parse("application/json"), MediaType.Parse("application/json"));
            Assert.AreEqual(MediaType.Parse("text/html;level=1"), MediaType.Parse("text/html;level=1"));
            Assert.AreEqual(MediaType.Parse("text/html;q=0.5"), MediaType.Parse("text/html;q=0.5"));
            Assert.AreEqual(MediaType.Parse("text/html;level=2;q=0.4"), MediaType.Parse("text/html;level=2;q=0.4"));
            Assert.AreEqual(MediaType.Parse("text/html;level=1;level=2;q=0.5;token;token=value;token=\"quoted value\""), MediaType.Parse("text/html;level=1;level=2;q=0.5;token;token=value;token=\"quoted value\""));
        }

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
        public void MediaTypeNotEquals()
        {
            Assert.AreNotEqual(MediaType.Parse("application/json"), MediaType.Parse("text/html"));
            Assert.AreNotEqual(MediaType.Parse("text/html"), MediaType.Parse("text/html;level=1"));
            Assert.AreNotEqual(MediaType.Parse("text/html"), MediaType.Parse("text/html;q=0.5"));
            Assert.AreNotEqual(MediaType.Parse("text/html"), MediaType.Parse("text/html;level=1;q=0.5"));
        }

        [Test]
        public void MediaTypeParse()
        {
            MediaType mediaType;
            Assert.IsTrue(MediaType.TryParse("*/*", out mediaType));
            Assert.AreEqual("*", mediaType.RootType);
            Assert.AreEqual("*", mediaType.SubType);
            Assert.IsNotNull(mediaType.RangeParams);
            Assert.IsFalse(mediaType.RangeParams.Any());
            Assert.IsNotNull(mediaType.AcceptParams);
            Assert.AreEqual(MediaType.AcceptParameters.Empty, mediaType.AcceptParams);

            Assert.IsTrue(MediaType.TryParse(null, out mediaType));
            Assert.AreEqual("*", mediaType.RootType);
            Assert.AreEqual("*", mediaType.SubType);
            Assert.IsNotNull(mediaType.RangeParams);
            Assert.IsFalse(mediaType.RangeParams.Any());
            Assert.IsNotNull(mediaType.AcceptParams);
            Assert.AreEqual(MediaType.AcceptParameters.Empty, mediaType.AcceptParams);

            Assert.IsTrue(MediaType.TryParse("application/json", out mediaType));
            Assert.AreEqual("application", mediaType.RootType);
            Assert.AreEqual("json", mediaType.SubType);
            Assert.IsNotNull(mediaType.RangeParams);
            Assert.IsFalse(mediaType.RangeParams.Any());
            Assert.IsNotNull(mediaType.AcceptParams);
            Assert.AreEqual(MediaType.AcceptParameters.Empty, mediaType.AcceptParams);

            Assert.IsTrue(MediaType.TryParse("text/html;level=1", out mediaType));
            Assert.AreEqual("text", mediaType.RootType);
            Assert.AreEqual("html", mediaType.SubType);
            Assert.IsNotNull(mediaType.RangeParams);
            Assert.AreEqual(1, mediaType.RangeParams.Count());
            Assert.AreEqual("level=1", mediaType.RangeParams.First());
            Assert.IsNotNull(mediaType.AcceptParams);
            Assert.AreEqual(MediaType.AcceptParameters.Empty, mediaType.AcceptParams);

            Assert.IsTrue(MediaType.TryParse("text/html; q=0.5", out mediaType));
            Assert.AreEqual("text", mediaType.RootType);
            Assert.AreEqual("html", mediaType.SubType);
            Assert.IsNotNull(mediaType.RangeParams);
            Assert.IsFalse(mediaType.RangeParams.Any());
            Assert.IsNotNull(mediaType.AcceptParams);
            Assert.AreEqual(MediaType.AcceptParameters.Parse("q=0.5"), mediaType.AcceptParams);

            Assert.IsTrue(MediaType.TryParse("text/html;level=2;q=0.4", out mediaType));
            Assert.AreEqual("text", mediaType.RootType);
            Assert.AreEqual("html", mediaType.SubType);
            Assert.IsNotNull(mediaType.RangeParams);
            Assert.AreEqual(1, mediaType.RangeParams.Count());
            Assert.AreEqual("level=2", mediaType.RangeParams.First());
            Assert.IsNotNull(mediaType.AcceptParams);
            Assert.AreEqual(MediaType.AcceptParameters.Parse("q=0.4"), mediaType.AcceptParams);

            Assert.IsTrue(MediaType.TryParse("text/html;level=1;level=2;q=0.5;token;token=value;token=\"quoted value\"", out mediaType));
            Assert.AreEqual("text", mediaType.RootType);
            Assert.AreEqual("html", mediaType.SubType);
            Assert.IsNotNull(mediaType.RangeParams);
            Assert.AreEqual(2, mediaType.RangeParams.Count());
            Assert.AreEqual("level=1", mediaType.RangeParams.First());
            Assert.AreEqual("level=2", mediaType.RangeParams.Last());
            Assert.IsNotNull(mediaType.AcceptParams);
            Assert.AreEqual(MediaType.AcceptParameters.Parse("q=0.5;token;token=value;token=\"quoted value\""), mediaType.AcceptParams);
        }

        [Test]
        public void MediaTypeToString()
        {
            Assert.AreEqual("application/json", MediaType.Parse("application/json").ToString());
            Assert.AreEqual("text/html;level=1", MediaType.Parse("text/html;level=1").ToString());
            Assert.AreEqual("text/html;q=0.5", MediaType.Parse("text/html;q=0.5").ToString());
            Assert.AreEqual("text/html;level=2;q=0.4", MediaType.Parse("text/html;level=2;q=0.4").ToString());
            Assert.AreEqual("text/html;level=1;level=2;q=0.5;token;token=value;token=\"quoted value\"", MediaType.Parse("text/html;level=1;level=2;q=0.5;token;token=value;token=\"quoted value\"").ToString());
        }
    }
}