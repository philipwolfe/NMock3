#region Using

using System;
using NMock;
using NMock.Matchers;
using NMockTests._TestStructures;
using Is = NMock.Is;

#if NUNIT
using NUnit.Framework;
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
namespace NMockTests.MockTests.StubTests.IMethodSyntaxTests35
#else
#if SILVERLIGHT
namespace NMockTests.MockTests.StubTests.IMethodSyntaxTestsSL
#else
namespace NMockTests.MockTests.StubTests.IMethodSyntaxTests
#endif
#endif
{
	[TestClass]
	public class MethodTests : BasicTestBase
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
		public void TestMethod1()
		{
			mocks.ForEach(TestMethod1);
		}

		private void TestMethod1(Mock<IParentInterface> mock)
		{
			
		}
	}
}
