#region Using

using System.IO;
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
	internal class MockMatcher : Matcher
	{
		public int DescribeToCallCount;
		public string DescribeToOutput = "";
		public TextWriter ExpectedDescribeToWriter;
		public object ExpectedMatchesArg;
		public int MatchesCallCount;
		public bool MatchesResult;

		public override bool Matches(object o)
		{
			MatchesCallCount++;
			Assert.AreEqual(ExpectedMatchesArg, o, "Matches arg");
			return MatchesResult;
		}

		public void AssertMatchesCalled(string messageFormat, params object[] formatArgs)
		{
			AssertMatchesCalled(1, messageFormat, formatArgs);
		}

		public void AssertMatchesCalled(int times, string messageFormat, params object[] formatArgs)
		{
			Assert.AreEqual(times, MatchesCallCount, messageFormat, formatArgs);
		}

		public override void DescribeTo(TextWriter writer)
		{
			DescribeToCallCount++;
			if (ExpectedDescribeToWriter != null)
			{
				Assert.AreSame(ExpectedDescribeToWriter, writer, "DescribeTo writer");
			}
			writer.Write(DescribeToOutput);
		}
	}
}