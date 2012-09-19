namespace SmallFry.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public sealed class RouteValueBinderTests
    {
        [Test]
        public void RouteValueBinderBindPrimitives()
        {
            RouteValueBinder binder = new RouteValueBinder();
            IDictionary<string, object> inputValues = new Dictionary<string, object>();
            IDictionary<string, Type> types = new Dictionary<string, Type>();

            inputValues.AddDynamic(new { id = "42", date = "2012-09-18T18:30:00Z" });
            types.AddDynamic(new { id = typeof(long), date = typeof(DateTime) });

            IDictionary<string, object> boundValues = binder.Bind(inputValues, types);
            Assert.IsNotNull(boundValues);
            Assert.AreEqual(42L, boundValues["id"]);
            Assert.AreEqual(new DateTime(2012, 9, 18, 18, 30, 0, DateTimeKind.Utc), boundValues["date"]);
        }

        [Test]
        public void RouteValueBinderOverrideType()
        {
            RouteValueBinder binder = new RouteValueBinder();
            binder.AddParser(new NoOpRouteParameterParser(new Type[] { typeof(DateTime) }));
            IDictionary<string, object> inputValues = new Dictionary<string, object>();
            IDictionary<string, Type> types = new Dictionary<string, Type>();

            inputValues.AddDynamic(new { id = "42", date = "2012-09-18T18:30:00Z" });
            types.AddDynamic(new { id = typeof(long), date = typeof(DateTime) });

            // NoOpRouteParameterParser throws a NotImplementedException during parsing.
            // This should cause the binder to fail the bind on the DateTime constraint.
            IDictionary<string, object> boundValues = binder.Bind(inputValues, types);
            Assert.IsNull(boundValues);
        }
    }
}