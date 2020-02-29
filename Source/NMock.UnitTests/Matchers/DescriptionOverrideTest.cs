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
	public class DescriptionOverrideTest
	{
		[TestMethod]
		public void DelegatesMatchingToAnotherMatcher()
		{
			Matcher m1 = new DescriptionOverride("irrelevant", new AlwaysMatcher(true, "always true"));
			Matcher m2 = new DescriptionOverride("irrelevant", new AlwaysMatcher(false, "always false"));

			Assert.IsTrue(m1.Matches(new object()), "m1");
			Assert.IsFalse(m2.Matches(new object()), "m2");
		}

		[TestMethod]
		public void OverridesDescriptionOfOtherMatcherWithThatPassedToConstructor()
		{
			Matcher m1 = new DescriptionOverride("m1", new AlwaysMatcher(true, "always true"));
			Matcher m2 = new DescriptionOverride("m2", new AlwaysMatcher(false, "always false"));

			AssertDescription.IsEqual(m1, "m1");
			AssertDescription.IsEqual(m2, "m2");
		}
	}
}