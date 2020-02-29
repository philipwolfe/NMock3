#region Using

using System;
using NMock;
using NMock.Actions;
using NMockTests.MockTests;
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
namespace NMockTests.Actions35
#else
#if SILVERLIGHT
namespace NMockTests.ActionsSL
#else
namespace NMockTests.Actions
#endif
#endif
{
	[TestClass]
	public class LazyReturnActionTests : BasicTestBase
	{
		[TestInitialize]
		public void TestInit()
		{
			Initalize();
		}

		[TestCleanup]
		public void TestClean()
		{
			Cleanup();
		}

		[TestMethod]
		public void Test1()
		{
			mocks.ForEach(Test1);
		}

		private void Test1(Mock<IParentInterface> mock)
		{
			mock.Expects.One.MethodWith(_ => _.Method<Version>()).Will(new LazyReturnAction(GetVersion));

			var version = mock.MockObject.Method<Version>();

			Assert.AreEqual(1, version.Major);
		}

		private Version GetVersion()
		{
			return new Version(1, 2, 3, 4);
		}
	}
}
