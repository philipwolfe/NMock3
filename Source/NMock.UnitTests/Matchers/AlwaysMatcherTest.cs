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
	public class AlwaysMatcherTest
	{
		[TestMethod]
		public void AlwaysReturnsFixedBooleanValueFromMatchesMethod()
		{
			Matcher matcher = new AlwaysMatcher(true, "");
			Assert.IsTrue(matcher.Matches("something"));
			Assert.IsTrue(matcher.Matches("something else"));
			Assert.IsTrue(matcher.Matches(null));
			Assert.IsTrue(matcher.Matches(1));
			Assert.IsTrue(matcher.Matches(1.0));
			Assert.IsTrue(matcher.Matches(new object()));

			matcher = new AlwaysMatcher(false, "");
			Assert.IsFalse(matcher.Matches("something"));
			Assert.IsFalse(matcher.Matches("something else"));
			Assert.IsFalse(matcher.Matches(null));
			Assert.IsFalse(matcher.Matches(1));
			Assert.IsFalse(matcher.Matches(1.0));
			Assert.IsFalse(matcher.Matches(new object()));
		}

		[TestMethod]
		public void IsGivenADescription()
		{
			string description = "DESCRIPTION";
			bool irrelevantFlag = false;

			AssertDescription.IsEqual(new AlwaysMatcher(irrelevantFlag, description), description);
		}
	}
}