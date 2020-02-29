#region Using

using System;
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
	public class MethodNameMatcherTest
	{
		public interface I
		{
			void m();
			int m(int i);
			bool m(string s, string t);
			void n();
		}

		[TestMethod]
		public void DoesNotMatchObjectsThatAreNotMethodInfo()
		{
			Matcher matcher = new MethodNameMatcher("m", typeof (I));
			Assert.IsFalse(matcher.Matches("m"));
		}

		[TestMethod]
		public void MatchesMethodsWithAGivenName()
		{
			Matcher matcher = new MethodNameMatcher("m", typeof (I));

			Assert.IsTrue(matcher.Matches(typeof (I).GetMethod("m", new Type[0])), "m()");
			Assert.IsTrue(matcher.Matches(typeof (I).GetMethod("m", new[] {typeof (int)})), "m(int)");
			Assert.IsTrue(matcher.Matches(typeof (I).GetMethod("m", new[] {typeof (string), typeof (string)})), "m(string,string)");
			Assert.IsFalse(matcher.Matches(typeof (I).GetMethod("n", new Type[0])), "n()");
		}

		[TestMethod]
		public void UsesMethodNameAsDescription()
		{
			AssertDescription.IsEqual(new MethodNameMatcher("m", typeof (I)), "m");
		}
	}
}