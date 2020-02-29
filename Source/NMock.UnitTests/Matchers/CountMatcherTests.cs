using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMock;
using NMock.Matchers;
using NMockTests.MockTests;
using NMockTests._TestStructures;

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
	public class CountMatcherTests : BasicTestBase
	{
		[TestInitialize]
		public void Init()
		{
			Initalize();
		}

		[TestCleanup]
		public void Clean()
		{
			Cleanup();
		}

		[TestMethod]
		public void MatchesTest()
		{
			var matcher = new CountMatcher<IEnumerable<string>>(3);
			var list = new[] { "A", "B", "C" };

			Assert.IsTrue(matcher.Matches(list));
		}

		[TestMethod]
		public void MatchesTest2()
		{
			var matcher = new CountMatcher<IEnumerable<string>>(4);
			var list = new[] { "A", "B", "C" };

			Assert.IsFalse(matcher.Matches(list));
		}

		[TestMethod]
		public void MocksTest()
		{
			mocks.ForEach(MocksTest);
		}

		public void MocksTest(Mock<IParentInterface> mock)
		{
			var matcher = new CountMatcher<IEnumerable<string>>(3);

			mock.Expects.One.Method(_ => _.MethodVoid(null, 0)).With(matcher, 0);
			mock.Expects.One.Method(_ => _.MethodVoid());

			var list = new[] { "A", "B", "C" };
			mock.MockObject.MethodVoid(new List<string>(list), 0);
			mock.MockObject.MethodVoid();

		}
	}
}