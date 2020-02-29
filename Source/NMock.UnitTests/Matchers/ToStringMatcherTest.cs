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
	public class ToStringMatcherTest
	{
		#region Setup/Teardown

		[TestInitialize]
		public void SetUp()
		{
			arg = new NamedObject("arg");
			stringMatcher = new MockMatcher();
			matcher = new ToStringMatcher(stringMatcher);
		}

		#endregion

		private NamedObject arg;
		private MockMatcher stringMatcher;
		private Matcher matcher;

		[TestMethod]
		public void PassesResultOfToStringToOtherMatcher()
		{
			stringMatcher.ExpectedMatchesArg = arg.ToString();
			stringMatcher.MatchesResult = true;

			Assert.AreEqual(stringMatcher.MatchesResult, matcher.Matches(arg), "result");
			stringMatcher.AssertMatchesCalled("should have passed string representation to stringMatcher");
		}

		[TestMethod]
		public void ReturnsAReadableDescription()
		{
			stringMatcher.DescribeToOutput = "<stringMatcher description>";
			AssertDescription.IsComposed(matcher, "an object with a string representation that is {0}", stringMatcher);
		}
	}
}