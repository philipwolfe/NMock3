#region Using

using System;
using System.Collections.Generic;
using System.Diagnostics;
using NMock;
using NMock.Internal;
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
namespace NMockTests.MockTests.ExpectsTests35
#else
#if SILVERLIGHT
namespace NMockTests.MockTests.ExpectsTestsSL
#else
namespace NMockTests.MockTests.ExpectsTests
#endif
#endif
{
	[TestClass]
	public class NoTests : BasicTestBase
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
		public void PassesTestIfMethodNeverExpectedIsNeverCalled()
		{
			mocks.ForEach(PassesTestIfMethodNeverExpectedIsNeverCalled);
		}

		private void PassesTestIfMethodNeverExpectedIsNeverCalled(Mock<IParentInterface> mock)
		{
			mock.Expects.No.Method(_ => _.MethodVoid());
		}

		//unordered
		[TestMethod]
		public void FailsTestIfMethodNeverExpectedIsActuallyCalled()
		{
			mocks.ForEach(FailsTestIfMethodNeverExpectedIsActuallyCalled);
		}

		public void FailsTestIfMethodNeverExpectedIsActuallyCalled(Mock<IParentInterface> mock)
		{
			mock.Expects.No.Method(_ => _.MethodVoid());

			Expect.That(mock.MockObject.MethodVoid).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected invocation of:
  System.Void {0}.MethodVoid()
MockFactory Expectations:
  System.Void {0}.MethodVoid() [EXPECTED: never CALLED: 1 time]
", mock.Name)));

			mock.ClearExpectations();
		}

		//ordered
		[TestMethod]
		public void FailsTestIfMethodNeverExpectedIsActuallyCalledOrdered()
		{
			mocks.ForEach(FailsTestIfMethodNeverExpectedIsActuallyCalledOrdered);
		}

		public void FailsTestIfMethodNeverExpectedIsActuallyCalledOrdered(Mock<IParentInterface> mock)
		{
			using (Factory.Ordered())
			{
				mock.Expects.No.Method(_ => _.MethodVoid());
			}

			Expect.That(mock.MockObject.MethodVoid).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected invocation of:
  System.Void {0}.MethodVoid()
MockFactory Expectations:
  Ordered {{
    System.Void {0}.MethodVoid() [EXPECTED: never CALLED: 0 times]
  }}
", mock.Name)));

			mock.ClearExpectations();
		}
	}
}