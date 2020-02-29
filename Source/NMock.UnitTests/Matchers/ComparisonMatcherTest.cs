#region Using

using System;
using System.Reflection;
using NMock;
using NMock.Matchers;
using NMockTests._TestStructures;
using Is = NMock.Is;

#if NUNIT
using NUnit.Framework;
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
	public class ComparisonMatcherTest
	{
		[TestMethod]
		public void CanSpecifyMinAndMaxComparisonResultInAnyOrder()
		{
			Matcher matcher = new ComparisonMatcher(10, 0, -1);
			Assert.IsTrue(matcher.Matches(9), "less");
			Assert.IsTrue(matcher.Matches(10), "equal");
			Assert.IsFalse(matcher.Matches(11), "greater");
		}

		[TestMethod]
		public void CannotCreateComparisonThatMatchesAnything()
		{
			Expect.That(() => new ComparisonMatcher(10, -1, 1)).Throws<ArgumentException>();
		}

		[TestMethod]
		public void DoesNotMatchObjectOfDifferentType()
		{
			Assert.IsFalse((new ComparisonMatcher(10, 0, 0)).Matches("a string"));
		}

		[TestMethod]
		public void HasReadableDescription()
		{
			AssertDescription.IsEqual(new ComparisonMatcher(10, -1, -1), "? < <10>(System.Int32)");
			AssertDescription.IsEqual(new ComparisonMatcher(10, -1, 0), "? <= <10>(System.Int32)");
			AssertDescription.IsEqual(new ComparisonMatcher(10, 0, 0), "? = <10>(System.Int32)");
			AssertDescription.IsEqual(new ComparisonMatcher(10, 0, 1), "? >= <10>(System.Int32)");
			AssertDescription.IsEqual(new ComparisonMatcher(10, 1, 1), "? > <10>(System.Int32)");
		}

		[TestMethod]
		public void MatchesAComparisonOfAComparableValue()
		{
			Matcher matcher;

			matcher = new ComparisonMatcher(10, -1, 0);
			Assert.IsTrue(matcher.Matches(9), "less");
			Assert.IsTrue(matcher.Matches(10), "equal");
			Assert.IsFalse(matcher.Matches(11), "greater");

			matcher = new ComparisonMatcher(10, -1, -1);
			Assert.IsTrue(matcher.Matches(9), "less");
			Assert.IsFalse(matcher.Matches(10), "equal");
			Assert.IsFalse(matcher.Matches(11), "greater");

			matcher = new ComparisonMatcher(10, 0, 1);
			Assert.IsFalse(matcher.Matches(9), "less");
			Assert.IsTrue(matcher.Matches(10), "equal");
			Assert.IsTrue(matcher.Matches(11), "greater");
		}
	}
}