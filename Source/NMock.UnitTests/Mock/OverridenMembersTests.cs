#region Using

using System;
using System.Collections.Generic;
using System.Diagnostics;
using NMock;
using NMock.Matchers;
using NMockTests._TestStructures;
using Is = NMock.Is;
using NMockTests.MockTests;

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
namespace NMockTests.MockTests35
#else
#if SILVERLIGHT
namespace NMockTests.MockTestsSL
#else
namespace NMockTests.MockTests
#endif
#endif
{
	[TestClass]
	public class OverridenMembersTests : BasicTestBase
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
		[Ignore]
		public void MockHashTest()
		{
			mocks.ForEach(MockHashTest);
		}

		private void MockHashTest(Mock<IParentInterface> mock)
		{
			int i = mock.GetHashCode();

			Assert.AreEqual(33650554, i);
		}

		[TestMethod]
		//[Ignore]
		public void MockToStringTest()
		{
			mocks.ForEach(MockToStringTest);
		}

		private void MockToStringTest(Mock<IParentInterface> mock)
		{
			mock.ThrowToStringException = false;
			string s = mock.ToString();

			Assert.AreEqual(mock.Name, s);
		}
	}
}
