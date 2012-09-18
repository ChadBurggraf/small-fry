namespace SmallFry.Tests
{
    using System;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public sealed class RoutePatternTests
    {
        [Test]
        [ExpectedException(typeof(FormatException))]
        public void RoutePatternFailInitializeDuplicateParameters()
        {
            new RoutePattern("devices/{deviceLibraryIdentifier}/{deviceLibraryIdentifier}");
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void RoutePatternFailInitializeEmptyParameter()
        {
            new RoutePattern("devices/{}/registrations");
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void RoutePatternFailInitializeUnmatchedClosingBracket()
        {
            new RoutePattern("devices/deviceLibraryIdentifier}/registrations");
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void RoutePatternFailInitializeUnmatchedOpeningBracket()
        {
            new RoutePattern("devices/{deviceLibraryIdentifier/registrations");
        }

        [Test]
        public void RoutePatternInitializeBasicRoute()
        {
            RoutePattern pattern = new RoutePattern("devices/{deviceLibraryIdentifier}/registrations/{passTypeIdentifier}");
            Assert.AreEqual(4, pattern.Segments.Count());
            Assert.AreEqual(2, pattern.Segments.Count(s => s.Tokens.Any(t => t.TokenType == RouteTokenType.Literal)));
            Assert.AreEqual(2, pattern.Segments.Count(s => s.Tokens.Any(t => t.TokenType == RouteTokenType.Named)));
            Assert.IsTrue(pattern.Segments.ElementAt(0).Tokens.Any(t => "devices".Equals(t.Value, StringComparison.Ordinal)));
            Assert.IsTrue(pattern.Segments.ElementAt(0).Tokens.Any(t => t.TokenType == RouteTokenType.Literal));
            Assert.IsTrue(pattern.Segments.ElementAt(1).Tokens.Any(t => "deviceLibraryIdentifier".Equals(t.Value, StringComparison.Ordinal)));
            Assert.IsTrue(pattern.Segments.ElementAt(1).Tokens.Any(t => t.TokenType == RouteTokenType.Named));
            Assert.IsTrue(pattern.Segments.ElementAt(2).Tokens.Any(t => "registrations".Equals(t.Value, StringComparison.Ordinal)));
            Assert.IsTrue(pattern.Segments.ElementAt(2).Tokens.Any(t => t.TokenType == RouteTokenType.Literal));
            Assert.IsTrue(pattern.Segments.ElementAt(3).Tokens.Any(t => "passTypeIdentifier".Equals(t.Value, StringComparison.Ordinal)));
            Assert.IsTrue(pattern.Segments.ElementAt(3).Tokens.Any(t => t.TokenType == RouteTokenType.Named));
        }

        [Test]
        public void RoutePatternInitializeMultiTokenSegmentRoute()
        {
            RoutePattern pattern = new RoutePattern("foo{token1}bar{?token2}");
            Assert.AreEqual(1, pattern.Segments.Count());
            Assert.AreEqual(4, pattern.Segments.First().Tokens.Count());
            Assert.AreEqual(RouteTokenType.Literal, pattern.Segments.First().Tokens.ElementAt(0).TokenType);
            Assert.AreEqual("foo", pattern.Segments.First().Tokens.ElementAt(0).Value);
            Assert.AreEqual(RouteTokenType.Named, pattern.Segments.First().Tokens.ElementAt(1).TokenType);
            Assert.AreEqual("token1", pattern.Segments.First().Tokens.ElementAt(1).Value);
            Assert.AreEqual(RouteTokenType.Literal, pattern.Segments.First().Tokens.ElementAt(2).TokenType);
            Assert.AreEqual("bar", pattern.Segments.First().Tokens.ElementAt(2).Value);
            Assert.AreEqual(RouteTokenType.Named, pattern.Segments.First().Tokens.ElementAt(3).TokenType);
            Assert.AreEqual("token2", pattern.Segments.First().Tokens.ElementAt(3).Value);
            Assert.IsTrue(pattern.Segments.First().Tokens.ElementAt(3).IsOptional);
        }

        [Test]
        public void RoutePatternInitializeOptionalRoute()
        {
            RoutePattern pattern = new RoutePattern("devices/{?deviceLibraryIdentifier}");
            Assert.AreEqual(2, pattern.Segments.Count());
            Assert.AreEqual(1, pattern.Segments.Count(s => s.Tokens.Any(t => t.TokenType == RouteTokenType.Literal)));
            Assert.AreEqual(1, pattern.Segments.Count(s => s.Tokens.Any(t => t.IsOptional)));
        }

        [Test]
        public void RoutePatternInitializeWildcardRoute()
        {
            RoutePattern pattern = new RoutePattern("devices/{*pathInfo}");
            Assert.AreEqual(2, pattern.Segments.Count());
            Assert.AreEqual(1, pattern.Segments.Count(s => s.Tokens.Any(t => t.TokenType == RouteTokenType.Literal)));
            Assert.AreEqual(1, pattern.Segments.Count(s => s.Tokens.Any(t => t.TokenType == RouteTokenType.Wildcard)));
        }
    }
}
