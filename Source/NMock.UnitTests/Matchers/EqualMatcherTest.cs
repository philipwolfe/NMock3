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
	public class EqualMatcherTest
	{
		private const string EXPECTED = "expected";

		[TestMethod]
		public void ComparesArgumentForEqualityToExpectedObject()
		{
			Matcher matcher = new EqualMatcher(EXPECTED);

			Assert.IsTrue(matcher.Matches(EXPECTED), "same object");
#if !SILVERLIGHT
			Assert.IsTrue(matcher.Matches(EXPECTED.Clone()), "equal object");
#endif
			Assert.IsFalse(matcher.Matches("not expected"), "unequal object");
		}

		[TestMethod]
		public void IsNullSafe()
		{
			Assert.IsTrue(new EqualMatcher(null).Matches(null), "null matches null");
			Assert.IsFalse(new EqualMatcher("not null").Matches(null), "not null does not match null");
			Assert.IsFalse(new EqualMatcher(null).Matches("not null"), "null does not match not null");
		}

		[TestMethod]
		public void ComparesArraysForEqualityByContents()
		{
			int[] expected = {1, 2};
			int[] equal = {1, 2};
			int[] inequal = {2, 3};
			int[] longer = {1, 2, 3};
			int[] shorter = {1};
			int[] empty = {};
			int[,] otherRank = {{1, 2}, {3, 4}};
			Matcher matcher = new EqualMatcher(expected);

			Assert.IsTrue(matcher.Matches(expected), "same array");
			Assert.IsTrue(matcher.Matches(equal), "same contents");
			Assert.IsFalse(matcher.Matches(inequal), "different contents");
			Assert.IsFalse(matcher.Matches(longer), "longer");
			Assert.IsFalse(matcher.Matches(shorter), "shorter");
			Assert.IsFalse(matcher.Matches(empty), "empty");
			Assert.IsFalse(matcher.Matches(otherRank), "other rank");
		}

		[TestMethod]
		public void ComparesMultidimensionalArraysForEquality()
		{
			int[,] expected = {{1, 2}, {3, 4}};
			int[,] equal = {{1, 2}, {3, 4}};
			int[,] inequal = {{3, 4}, {5, 6}};
			int[,] empty = new int[0,0];
			int[] otherRank = {1, 2};
			Matcher matcher = new EqualMatcher(expected);

			Assert.IsTrue(matcher.Matches(expected), "same array");
			Assert.IsTrue(matcher.Matches(equal), "same contents");
			Assert.IsFalse(matcher.Matches(inequal), "different contents");
			Assert.IsFalse(matcher.Matches(empty), "empty");
			Assert.IsFalse(matcher.Matches(otherRank), "other rank");
		}

		[TestMethod]
		public void RecursivelyComparesArrayContentsOfNestedArrays()
		{
			int[][] expected = new[] {new[] {1, 2}, new[] {3, 4}};
			int[][] equal = new[] {new[] {1, 2}, new[] {3, 4}};
			int[][] inequal = new[] {new[] {2, 3}, new[] {4, 5}};
			Matcher matcher = new EqualMatcher(expected);

			Assert.IsTrue(matcher.Matches(expected), "same array");
			Assert.IsTrue(matcher.Matches(equal), "same contents");
			Assert.IsFalse(matcher.Matches(inequal), "different contents");
		}

#if !SILVERLIGHT
		[TestMethod]
		public void RecursivelyComparesContentsOfNestedLists()
		{
			ArrayList expected = new ArrayList(new[]
			                                   	{
			                                   		new ArrayList(new[] {1, 2}),
			                                   		new ArrayList(new[] {3, 4})
			                                   	});
			ArrayList equal = new ArrayList(new[]
			                                	{
			                                		new ArrayList(new[] {1, 2}),
			                                		new ArrayList(new[] {3, 4})
			                                	});
			ArrayList inequal = new ArrayList(new[]
			                                  	{
			                                  		new ArrayList(new[] {2, 3}),
			                                  		new ArrayList(new[] {4, 5})
			                                  	});
			Matcher matcher = new EqualMatcher(expected);

			Assert.IsTrue(matcher.Matches(expected), "same array");
			Assert.IsTrue(matcher.Matches(equal), "same contents");
			Assert.IsFalse(matcher.Matches(inequal), "different contents");
		}
#endif

		[TestMethod]
		public void CanCompareAutoboxedValues()
		{
			Matcher matcher = new EqualMatcher(1);
			Assert.IsTrue(matcher.Matches(1), "equal value");
			Assert.IsFalse(matcher.Matches(2), "other value");
		}

		[TestMethod]
		public void HasAReadableDescription()
		{
			NamedObject value = new NamedObject("value");
			AssertDescription.IsEqual(new EqualMatcher(value), "equal to <" + value + ">(NMockTests.NamedObject)");
		}
	}
}