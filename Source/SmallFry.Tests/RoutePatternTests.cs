namespace SmallFry.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public sealed class RoutePatternTests
    {
        [Test]
        public void RoutePatternFailMatchBasicRoute()
        {
            RoutePattern pattern = RoutePattern.Parse("devices/{deviceLibraryIdentifier}/registrations/{passTypeIdentifier}");
            Assert.IsNull(pattern.Match("devices/1234/registrations"));
            Assert.IsNull(pattern.Match("api/devices/1234/registrations/5678"));
            Assert.IsNull(pattern.Match("foo/1234/bar/5678"));
            Assert.IsNull(pattern.Match("1234/registrations/5677"));
        }

        [Test]
        public void RoutePatternFailMatchMultiTokenSegmentRoute()
        {
            RoutePattern pattern = RoutePattern.Parse("foo{token1}bar{?token2}");
            Assert.IsNull(pattern.Match("foobar5678"));
            Assert.IsNull(pattern.Match("1234bar5678"));
            Assert.IsNull(pattern.Match("foo1234"));
            Assert.IsNull(pattern.Match("foo"));
            Assert.IsNull(pattern.Match("1234bar"));
            Assert.IsNull(pattern.Match("bar"));
        }

        [Test]
        public void RoutePatternFailMatchOptionalRoute()
        {
            RoutePattern pattern = RoutePattern.Parse("devices/{?deviceLibraryIdentifier}");
            Assert.IsNull(pattern.Match("1234"));
            Assert.IsNull(pattern.Match("devices/1234/registrations"));
            Assert.IsNull(pattern.Match("foo/devices/1234"));
            Assert.IsNull(pattern.Match("foo/devices"));
        }

        [Test]
        public void RoutePatternFailMatchWildcardRoute()
        {
            RoutePattern pattern = RoutePattern.Parse("devices/{*pathInfo}");
            Assert.IsNull(pattern.Match("foo/bar"));
            Assert.IsNull(pattern.Match(string.Empty));
            Assert.IsNull(pattern.Match("foo"));
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void RoutePatternFailParseDuplicateParameters()
        {
            RoutePattern.Parse("devices/{deviceLibraryIdentifier}/{deviceLibraryIdentifier}");
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void RoutePatternFailParseEmptyParameter()
        {
            RoutePattern.Parse("devices/{}/registrations");
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void RoutePatternFailParseMultiSegmentWithoutSeparator()
        {
            RoutePattern.Parse("foo/{bar}{baz}/{*pathInfo}");
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void RoutePatternFailParseUnmatchedClosingBracket()
        {
            RoutePattern.Parse("devices/deviceLibraryIdentifier}/registrations");
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void RoutePatternFailParseUnmatchedOpeningBracket()
        {
            RoutePattern.Parse("devices/{deviceLibraryIdentifier/registrations");
        }

        [Test]
        public void RoutePatternMatchBasicRoute()
        {
            RoutePattern pattern = RoutePattern.Parse("devices/{deviceLibraryIdentifier}/registrations/{passTypeIdentifier}");
            IDictionary<string, object> routeValues = pattern.Match("devices/1234/registrations/5678");
            Assert.IsNotNull(routeValues);
            Assert.IsTrue(routeValues.ContainsKey("deviceLibraryIdentifier"));
            Assert.AreEqual("1234", routeValues["deviceLibraryIdentifier"]);
            Assert.IsTrue(routeValues.ContainsKey("passTypeIdentifier"));
            Assert.AreEqual("5678", routeValues["passTypeIdentifier"]);
        }

        [Test]
        public void RoutePatternMatchMultiTokenSegmentRoute()
        {
            RoutePattern pattern = RoutePattern.Parse("foo{token1}bar{?token2}");
            IDictionary<string, object> routeValues = pattern.Match("foo1234bar5678");
            Assert.IsNotNull(routeValues);
            Assert.IsTrue(routeValues.ContainsKey("token1"));
            Assert.AreEqual("1234", routeValues["token1"]);
            Assert.IsTrue(routeValues.ContainsKey("token2"));
            Assert.AreEqual("5678", routeValues["token2"]);

            pattern = RoutePattern.Parse("foo{token1}bar{?token2}");
            routeValues = pattern.Match("foo1234bar");
            Assert.IsNotNull(routeValues);
            Assert.IsTrue(routeValues.ContainsKey("token1"));
            Assert.AreEqual("1234", routeValues["token1"]);
            Assert.IsTrue(routeValues.ContainsKey("token2"));
            Assert.IsNull(routeValues["token2"]);
        }

        [Test]
        public void RoutePatternMatchOptionalRoute()
        {
            RoutePattern pattern = RoutePattern.Parse("devices/{?deviceLibraryIdentifier}");
            IDictionary<string, object> routeValues = pattern.Match("devices/1234");
            Assert.IsNotNull(routeValues);
            Assert.IsTrue(routeValues.ContainsKey("deviceLibraryIdentifier"));
            Assert.AreEqual("1234", routeValues["deviceLibraryIdentifier"]);

            routeValues = pattern.Match("devices");
            Assert.IsNotNull(routeValues);
            Assert.IsTrue(routeValues.ContainsKey("deviceLibraryIdentifier"));
            Assert.IsNull(routeValues["deviceLibraryIdentifier"]);

            routeValues = pattern.Match("devices/");
            Assert.IsNotNull(routeValues);
            Assert.IsTrue(routeValues.ContainsKey("deviceLibraryIdentifier"));
            Assert.IsNull(routeValues["deviceLibraryIdentifier"]);

            pattern = RoutePattern.Parse("foo{token1}bar{?token2}/{?token3}");
            routeValues = pattern.Match("foo1234bar");
            Assert.IsNotNull(routeValues);
            Assert.IsTrue(routeValues.ContainsKey("token1"));
            Assert.AreEqual("1234", routeValues["token1"]);
            Assert.IsTrue(routeValues.ContainsKey("token2"));
            Assert.IsNull(routeValues["token2"]);
            Assert.IsTrue(routeValues.ContainsKey("token3"));
            Assert.IsNull(routeValues["token3"]);

            pattern = RoutePattern.Parse("foo{token1}bar{?token2}/{?token3}");
            routeValues = pattern.Match("foo1234bar/910");
            Assert.IsNotNull(routeValues);
            Assert.IsTrue(routeValues.ContainsKey("token1"));
            Assert.AreEqual("1234", routeValues["token1"]);
            Assert.IsTrue(routeValues.ContainsKey("token2"));
            Assert.IsNull(routeValues["token2"]);
            Assert.IsTrue(routeValues.ContainsKey("token3"));
            Assert.AreEqual("910", routeValues["token3"]);
        }

        [Test]
        public void RoutePatternMatchWildcardRoute()
        {
            RoutePattern pattern = RoutePattern.Parse("devices/{*pathInfo}");
            IDictionary<string, object> routeValues = pattern.Match("devices");
            Assert.IsNotNull(routeValues);
            Assert.IsTrue(routeValues.ContainsKey("pathInfo"));
            Assert.IsNull(routeValues["pathInfo"]);

            routeValues = pattern.Match("devices/foo");
            Assert.IsNotNull(routeValues);
            Assert.IsTrue(routeValues.ContainsKey("pathInfo"));
            Assert.AreEqual("foo", routeValues["pathInfo"]);

            routeValues = pattern.Match("devices/some/path/info/here");
            Assert.IsNotNull(routeValues);
            Assert.IsTrue(routeValues.ContainsKey("pathInfo"));
            Assert.AreEqual("some/path/info/here", routeValues["pathInfo"]);
        }

        [Test]
        public void RoutePatternParseBasicRoute()
        {
            RoutePattern pattern = RoutePattern.Parse("devices/{deviceLibraryIdentifier}/registrations/{passTypeIdentifier}");
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
        public void RoutePatternParseMultiTokenSegmentRoute()
        {
            RoutePattern pattern = RoutePattern.Parse("foo{token1}bar{?token2}");
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
        public void RoutePatternParseOptionalRoute()
        {
            RoutePattern pattern = RoutePattern.Parse("devices/{?deviceLibraryIdentifier}");
            Assert.AreEqual(2, pattern.Segments.Count());
            Assert.AreEqual(1, pattern.Segments.Count(s => s.Tokens.Any(t => t.TokenType == RouteTokenType.Literal)));
            Assert.AreEqual(1, pattern.Segments.Count(s => s.Tokens.Any(t => t.IsOptional)));
        }

        [Test]
        public void RoutePatternParseWildcardRoute()
        {
            RoutePattern pattern = RoutePattern.Parse("devices/{*pathInfo}");
            Assert.AreEqual(2, pattern.Segments.Count());
            Assert.AreEqual(1, pattern.Segments.Count(s => s.Tokens.Any(t => t.TokenType == RouteTokenType.Literal)));
            Assert.AreEqual(1, pattern.Segments.Count(s => s.Tokens.Any(t => t.TokenType == RouteTokenType.Wildcard)));
        }
    }
}
