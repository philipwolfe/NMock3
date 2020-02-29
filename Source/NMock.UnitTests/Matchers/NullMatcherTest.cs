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
	public class NullMatcherTest
	{
		[TestMethod]
		public void MatchesNullReferences()
		{
			Matcher matcher = new NullMatcher();

			Assert.IsTrue(matcher.Matches(null), "null");
			Assert.IsFalse(matcher.Matches(new object()), "not null");
		}

		[TestMethod]
		public void ProvidesAReadableDescription()
		{
			AssertDescription.IsEqual(new NullMatcher(), "null");
		}
	}
}