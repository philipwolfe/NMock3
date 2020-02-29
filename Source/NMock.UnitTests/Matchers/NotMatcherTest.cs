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
	public class NotMatcherTest
	{
		private static readonly object ignored = new object();
		private static readonly Matcher TRUE = new AlwaysMatcher(true, "TRUE");
		private static readonly Matcher FALSE = new AlwaysMatcher(false, "FALSE");

		[TestMethod]
		public void CalculatesTheLogicalNegationOfAMatcher()
		{
			Assert.IsTrue(new NotMatcher(FALSE).Matches(ignored), "not false");
			Assert.IsFalse(new NotMatcher(TRUE).Matches(ignored), "not true");
		}

		[TestMethod]
		public void CanUseOperatorOverloadingAsSyntacticSugar()
		{
			Assert.IsTrue((!FALSE).Matches(ignored), "not false");
			Assert.IsFalse((!TRUE).Matches(ignored), "not true");
		}

		[TestMethod]
		public void HasAReadableDescription()
		{
			Matcher negated = new MatcherWithDescription("<negated>");
			NotMatcher notMatcher = new NotMatcher(negated);
			AssertDescription.IsComposed(notMatcher, "not {0}", negated);
		}
	}
}