#region Using

using NMock;
using NMock.Matchers;
using NMockTests._TestStructures;
using Is = NMock.Is;

#if NUNIT
using Assert = NUnit.Framework.Assert;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;

#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
#endif

#endregion

#if NetFx35
namespace NMockTests.Matchers35
#else
#if SILVERLIGHT
namespace NMockTests.MatchersSL
#else
namespace NMockTests.Matchers
#endif
#endif
{
	[TestClass]
	public class FieldMatcherTest
	{
		private class ObjectWithFields
		{
			public static object StaticField = "static value";
			private readonly object privateField = "private value";
			public object PublicField;
			protected object protectedField = "protected value";

			public object PersuadeCompilerToShutUp()
			{
				return privateField;
			}
		}

		[TestMethod]
		public void DoesNotMatchNonPublicField()
		{
			ObjectWithFields o = new ObjectWithFields();
			Matcher m = new FieldMatcher("protectedField", new AlwaysMatcher(true, "anything"));

			Assert.IsFalse(m.Matches(o), "should not match o; field is protected");

			m = new FieldMatcher("privateField", new AlwaysMatcher(true, "anything"));

			Assert.IsFalse(m.Matches(o), "should not match o; field is private");
		}

		[TestMethod]
		public void DoesNotMatchObjectIfItDoesNotHaveNamedField()
		{
			ObjectWithFields o = new ObjectWithFields();
			Matcher m = new FieldMatcher("FavouriteColour", new AlwaysMatcher(true, "anything"));

			Assert.IsFalse(m.Matches(o), "should not match o; field does not exist");
		}

		[TestMethod]
		public void DoesNotMatchObjectIfValueMatcherDoesNotMatch()
		{
			ObjectWithFields o = new ObjectWithFields();
			o.PublicField = "actual value";
			Matcher m = new FieldMatcher("PublicField", new EqualMatcher("some other value"));

			Assert.IsFalse(m.Matches(o), "should match o; value is different");
		}

		[TestMethod]
		public void DoesNotMatchStaticField()
		{
			ObjectWithFields o = new ObjectWithFields();
			Matcher m = new FieldMatcher("StaticField", new AlwaysMatcher(true, "anything"));

			Assert.IsFalse(m.Matches(o), "should not match o; field is static");
		}

		[TestMethod]
		public void HasAReadableDescription()
		{
			Matcher matcher = new EqualMatcher("foo");

			AssertDescription.IsEqual(new FieldMatcher("A", matcher), "field 'A' " + matcher);
		}

		[TestMethod]
		public void MatchesObjectWithMatchingField()
		{
			ObjectWithFields o = new ObjectWithFields();
			o.PublicField = "actual value";
			Matcher m = new FieldMatcher("PublicField", Is.EqualTo("actual value"));

			Assert.IsTrue(m.Matches(o), "should match o");
		}
	}
}