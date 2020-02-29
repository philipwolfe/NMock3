#region Using

using System.Collections;
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
	public class ElementMatcherTest
	{
		private ICollection CollectionOf(params object[] elements)
		{
			return elements;
		}

		[TestMethod]
		public void HasAReadableDescription()
		{
			AssertDescription.IsEqual(
				new ElementMatcher(CollectionOf("a", "b", 1, 2)),
				"element of [\"a\", \"b\", <1>(System.Int32), <2>(System.Int32)]");

			AssertDescription.IsEqual(
				new ElementMatcher(CollectionOf()),
				"element of []");
		}

		[TestMethod]
		public void IsNullSafe()
		{
			ICollection collection = CollectionOf(1, 2, null, 4);

			Matcher matcher = new ElementMatcher(collection);

			Assert.IsTrue(matcher.Matches(1), "should match 1");
			Assert.IsTrue(matcher.Matches(2), "should match 2");
			Assert.IsTrue(matcher.Matches(null), "should match null");
			Assert.IsTrue(matcher.Matches(4), "should match 4");
			Assert.IsFalse(matcher.Matches(0), "should not match 0");
		}

		[TestMethod]
		public void MatchesIfArgumentInCollection()
		{
			ICollection collection = CollectionOf(1, 2, 3, 4);

			Matcher matcher = new ElementMatcher(collection);

			Assert.IsTrue(matcher.Matches(1), "should match 1");
			Assert.IsTrue(matcher.Matches(2), "should match 2");
			Assert.IsTrue(matcher.Matches(3), "should match 3");
			Assert.IsTrue(matcher.Matches(4), "should match 4");
			Assert.IsFalse(matcher.Matches(0), "should not match 0");
		}
	}
}