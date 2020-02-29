#region Using

using NMock;
using NMock.Matchers;

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
	public class AndMatcherTest
	{
		private static readonly object ignored = new object();
		private static readonly Matcher TRUE = new AlwaysMatcher(true, "TRUE");
		private static readonly Matcher FALSE = new AlwaysMatcher(false, "FALSE");

		private static readonly object[,] truthTable = {
		                                               	{FALSE, FALSE, false},
		                                               	{FALSE, TRUE, false},
		                                               	{TRUE, FALSE, false},
		                                               	{TRUE, TRUE, true}
		                                               };

		[TestMethod]
		public void CalculatesLogicalConjunctionOfTwoMatchers()
		{
			for (int i = 0; i < truthTable.GetLength(0); i++)
			{
				Matcher matcher = new AndMatcher((Matcher) truthTable[i, 0], (Matcher) truthTable[i, 1]);

				Assert.AreEqual(truthTable[i, 2], matcher.Matches(ignored));
			}
		}

		[TestMethod]
		public void CanUseOperatorOverloadingAsSyntacticSugar()
		{
			for (int i = 0; i < truthTable.GetLength(0); i++)
			{
				Matcher arg1 = (Matcher) truthTable[i, 0];
				Matcher arg2 = (Matcher) truthTable[i, 1];
				Matcher matcher = arg1 & arg2;

				Assert.AreEqual(truthTable[i, 2], matcher.Matches(ignored));
			}
		}

		[TestMethod]
		public void HasAReadableDescription()
		{
			Matcher left = new MatcherWithDescription("<left>");
			Matcher right = new MatcherWithDescription("<right>");

			AssertDescription.IsComposed(new AndMatcher(left, right), "'{0}' and '{1}'", left, right);
		}
	}
}