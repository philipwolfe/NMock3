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
	public class TypeMatcherTest
	{
		private class B
		{
		}

		private class D : B
		{
		}

		[TestMethod]
		public void DoesNotMatchValueOfNonAssignableType()
		{
			Matcher m = new TypeMatcher(typeof (D));

			Assert.IsFalse(m.Matches(new B()), "should not match B");
			Assert.IsFalse(m.Matches(123), "should not match B");
			Assert.IsFalse(m.Matches("hello, world"), "should not match B");
		}

		[TestMethod]
		public void MatchesValueOfAssignableType()
		{
			Matcher m = new TypeMatcher(typeof (B));

			Assert.IsTrue(m.Matches(new B()), "should match B");
			Assert.IsTrue(m.Matches(new D()), "should match D");
		}
	}
}