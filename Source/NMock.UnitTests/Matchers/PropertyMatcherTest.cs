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
	public class PropertyMatcherTest
	{
		public class ObjectWithProperties
		{
			public object A { get; set; }

			public object WriteOnlyProperty
			{
				set
				{
				}
			}

			private object PrivateProperty
			{
				get
				{
					return "value";
				}
				set
				{
				}
			}

			public void AMethodToGetAroundCompilerWarnings()
			{
				PrivateProperty = "something";
			}
		}

		[TestMethod]
		public void DoesNotMatchObjectIfItDoesNotHaveNamedProperty()
		{
			ObjectWithProperties o = new ObjectWithProperties();

			Matcher m = new PropertyMatcher("OtherProperty", new AlwaysMatcher(true, "anything"));

			Assert.IsFalse(m.Matches(o), "should not match o");
		}

		[TestMethod]
		public void DoesNotMatchObjectIfPropertyMatcherDoesNotMatch()
		{
			ObjectWithProperties o = new ObjectWithProperties();
			object aValue = new NamedObject("aValue");
			object otherValue = new NamedObject("otherValue");
			o.A = aValue;

			Matcher m = new PropertyMatcher("A", new EqualMatcher(otherValue));

			Assert.IsFalse(m.Matches(o), "should not match o");
		}


		[TestMethod]
		public void DoesNotMatchPrivateProperty()
		{
			ObjectWithProperties o = new ObjectWithProperties();

			Matcher m = new PropertyMatcher("PrivateProperty", new AlwaysMatcher(true, "anything"));

			Assert.IsFalse(m.Matches(o), "should not match o");
		}

		[TestMethod]
		public void DoesNotMatchWriteOnlyProperty()
		{
			ObjectWithProperties o = new ObjectWithProperties();

			Matcher m = new PropertyMatcher("WriteOnlyProperty", new AlwaysMatcher(true, "anything"));

			Assert.IsFalse(m.Matches(o), "should not match o");
		}

		[TestMethod]
		public void HasAReadableDescription()
		{
			Matcher matcher = new EqualMatcher("foo");

			AssertDescription.IsEqual(new PropertyMatcher("A", matcher), "property 'A' " + matcher);
		}

		[TestMethod]
		public void MatchesObjectWithNamedPropertyAndMatchingPropertyValue()
		{
			ObjectWithProperties o = new ObjectWithProperties();
			object aValue = new NamedObject("aValue");
			o.A = aValue;

			Matcher m = new PropertyMatcher("A", Is.EqualTo(aValue));

			Assert.IsTrue(m.Matches(o), "should match o");
		}
	}
}