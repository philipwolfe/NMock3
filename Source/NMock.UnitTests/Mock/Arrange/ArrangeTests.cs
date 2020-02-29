#region Using

using System;
using System.Collections.Generic;
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
	public class ArrangeTests : BasicTestBase
	{
		[TestInitialize]
		public void Init()
		{
			Initalize();
		}

		//[TestCleanup]
		//public void Clean()
		//{
		//    Cleanup();
		//}

		[TestMethod]
		public void ArrangeTest1()
		{
			mocks.ForEach(ArrangeTest1);
		}

		private void ArrangeTest1(Mock<IParentInterface> mock)
		{
			var expectations = new Expectations(5);

			expectations[0] = mock.Arrange(_ => _.Method(3)).WillReturn(true);
			expectations[1] = mock.Arrange(_ => _.MethodVoid(new Version(1, 1, 1, 1))).With(new Version(3,3,3,3));
			expectations[2] = mock.Arrange(_ => _.ReadOnlyValueProperty).WillReturn(3);
			expectations[3] = mock.Arrange(_ => _.WriteOnlyObjectProperty = new Version(2, 2, 2, 2));
			expectations[4] = mock.Arrange(_ => _.StandardEvent1 += null);

			var p = new TestPresenter(mock.MockObject);
			var b1 = p.CallMethod(3);
			p.CallVoidMethod(new Version(3,3,3,3));
			var b2 = p.GetReadOnlyValueProperty();
			p.AssignWriteOnlyObjectProperty(new Version(2, 2, 2, 2));
			p.HookUpStandardEvent1();

			expectations.ForEach(_ => _.Assert());
			Assert.IsTrue(b1);
			Assert.AreEqual(3, b2);
		}
	}
}