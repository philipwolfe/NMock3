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
	public class StringContainsMatcherTest
	{
		[TestMethod]
		public void HasAReadableDescription()
		{
			string substring = "substring";
			AssertDescription.IsEqual(new StringContainsMatcher(substring),
			                          "string containing \"" + substring + "\"");
		}

		[TestMethod]
		public void MatchesIfStringArgumentContainsGivenSubstring()
		{
			string substring = "SUBSTRING";
			Matcher matcher = new StringContainsMatcher(substring);

			Assert.IsTrue(matcher.Matches(substring), "arg is substring");
			Assert.IsTrue(matcher.Matches(substring + "X"), "arg starts with substring");
			Assert.IsTrue(matcher.Matches("X" + substring), "arg ends with substring");
			Assert.IsTrue(matcher.Matches("X" + substring + "X"), "arg contains substring");
			Assert.IsFalse(matcher.Matches("XX"), "arg does not contain substring");
			Assert.IsFalse(matcher.Matches(null), "arg is null");
			Assert.IsFalse(matcher.Matches(new object()), "arg is not a string");
		}
	}
}