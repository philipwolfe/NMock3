#region Using

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
	public class AtLeastOneTests : BasicTestBase
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

		//unordered
		[TestMethod]
		public void PassesTestIfMethodExpectedAtLeastOneTimeAndCalledOneTime()
		{
			mocks.ForEach(PassesTestIfMethodExpectedAtLeastOneTimeAndCalledOneTime);
		}

		private void PassesTestIfMethodExpectedAtLeastOneTimeAndCalledOneTime(Mock<IParentInterface> mock)
		{
			mock.Expects.AtLeastOne.Method(_ => _.MethodVoid());

			for (int i = 0; i < 1; i++)
				mock.MockObject.MethodVoid();
		}

		[TestMethod]
		public void FailsTestIfMethodExpectedAtLeastOneTimeButCalledLessThanOneTimes()
		{
			mocks.ForEach(FailsTestIfMethodExpectedAtLeastOneTimeButCalledLessThanOneTimes);
		}

		private void FailsTestIfMethodExpectedAtLeastOneTimeButCalledLessThanOneTimes(Mock<IParentInterface> mock)
		{
			mock.Expects.AtLeastOne.Method(_ => _.MethodVoid());

			for (int i = 0; i < 0; i++)
				mock.MockObject.MethodVoid();

			try
			{
				Factory.VerifyAllExpectationsHaveBeenMet();

				Assert.Fail("Expected an UnmetExpectationException");
			}
			catch (Exception ex)
			{
				Assert.AreEqual(string.Format(@"Not all expected invocations were performed.
MockFactory Expectations:
  System.Void {0}.MethodVoid() [EXPECTED: at least 1 time CALLED: 0 times]
", mock.Name), ex.Message);
			}
		}

		[TestMethod]
		public void PassesTestIfMethodExpectedAtLeastOneTimeAndCalledMoreThanOneTime()
		{
			mocks.ForEach(PassesTestIfMethodExpectedAtLeastNTimesAndCalledMoreThanNTimes);
		}

		private void PassesTestIfMethodExpectedAtLeastNTimesAndCalledMoreThanNTimes(Mock<IParentInterface> mock)
		{
			mock.Expects.AtLeastOne.Method(_ => _.MethodVoid());

			for (int i = 0; i < 1 + 1; i++)
				mock.MockObject.MethodVoid();
		}

		//ordered
		[TestMethod]
		public void PassesTestIfMethodExpectedAtLeastOneTimeAndCalledOneTimeOrdered()
		{
			mocks.ForEach(PassesTestIfMethodExpectedAtLeastOneTimeAndCalledOneTimeOrdered);
		}

		private void PassesTestIfMethodExpectedAtLeastOneTimeAndCalledOneTimeOrdered(Mock<IParentInterface> mock)
		{
			using (Factory.Ordered())
			{
			mock.Expects.AtLeastOne.Method(_ => _.MethodVoid());
			}

			for (int i = 0; i < 1; i++)
				mock.MockObject.MethodVoid();
		}

		[TestMethod]
		public void FailsTestIfMethodExpectedAtLeastOneTimeButCalledLessThanOneTimesOrdered()
		{
			mocks.ForEach(FailsTestIfMethodExpectedAtLeastOneTimeButCalledLessThanOneTimesOrdered);
		}

		private void FailsTestIfMethodExpectedAtLeastOneTimeButCalledLessThanOneTimesOrdered(Mock<IParentInterface> mock)
		{
			using(Factory.Ordered())
			{
				mock.Expects.AtLeastOne.Method(_ => _.MethodVoid());
			}

			for (int i = 0; i < 0; i++)
				mock.MockObject.MethodVoid();

			try
			{
				Factory.VerifyAllExpectationsHaveBeenMet();

				Assert.Fail("Expected an UnmetExpectationException");
			}
			catch (Exception ex)
			{
				Assert.AreEqual(string.Format(@"Not all expected invocations were performed.
MockFactory Expectations:
  Ordered {{
    System.Void {0}.MethodVoid() [EXPECTED: at least 1 time CALLED: 0 times]
  }}
", mock.Name), ex.Message);
			}
		}

		[TestMethod]
		public void PassesTestIfMethodExpectedAtLeastOneTimeAndCalledMoreThanOneTimeOrdered()
		{
			mocks.ForEach(PassesTestIfMethodExpectedAtLeastNTimesAndCalledMoreThanNTimesOrdered);
		}

		private void PassesTestIfMethodExpectedAtLeastNTimesAndCalledMoreThanNTimesOrdered(Mock<IParentInterface> mock)
		{
			using(Factory.Ordered())
			{
				mock.Expects.AtLeastOne.Method(_ => _.MethodVoid());
			}

			for (int i = 0; i < 1 + 1; i++)
				mock.MockObject.MethodVoid();
		}

	}
}
