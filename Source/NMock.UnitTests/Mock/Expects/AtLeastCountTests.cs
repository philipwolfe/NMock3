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
	public class AtLeastCountTests : BasicTestBase
	{
		private int N = 5;

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
		public void PassesTestIfMethodExpectedAtLeastNTimesAndCalledNTimes()
		{
			mocks.ForEach(PassesTestIfMethodExpectedAtLeastNTimesAndCalledNTimes);
		}

		private void PassesTestIfMethodExpectedAtLeastNTimesAndCalledNTimes(Mock<IParentInterface> mock)
		{
			mock.Expects.AtLeast(N).Method(_ => _.MethodVoid());

			for (int i = 0; i < N; i++) 
				mock.MockObject.MethodVoid();
		}

		[TestMethod]
		public void FailsTestIfMethodExpectedAtLeastNTimesButCalledLessThanNTimes()
		{
			mocks.ForEach(FailsTestIfMethodExpectedAtLeastNTimesButCalledLessThanNTimes);
		}

		private void FailsTestIfMethodExpectedAtLeastNTimesButCalledLessThanNTimes(Mock<IParentInterface> mock)
		{
			mock.Expects.AtLeast(N).Method(_ => _.MethodVoid());

			for (int i = 0; i < N - 1; i++)
				mock.MockObject.MethodVoid();

			try
			{
				Factory.VerifyAllExpectationsHaveBeenMet();
				
				Assert.Fail("Expected an UnmetExpectationException");
			}
			catch(Exception ex)
			{
				Assert.AreEqual(string.Format(@"Not all expected invocations were performed.
MockFactory Expectations:
  System.Void {0}.MethodVoid() [EXPECTED: at least 5 times CALLED: 4 times]
", mock.Name), ex.Message);
			}
		}

		[TestMethod]
		public void PassesTestIfMethodExpectedAtLeastNTimesAndCalledMoreThanNTimes()
		{
			mocks.ForEach(PassesTestIfMethodExpectedAtLeastNTimesAndCalledMoreThanNTimes);
		}

		private void PassesTestIfMethodExpectedAtLeastNTimesAndCalledMoreThanNTimes(Mock<IParentInterface> mock)
		{
			mock.Expects.AtLeast(N).Method(_ => _.MethodVoid());

			for (int i = 0; i < N + 1; i++) 
				mock.MockObject.MethodVoid();
		}

		//ordered
		[TestMethod]
		public void PassesTestIfMethodExpectedAtLeastNTimesAndCalledNTimesOrdered()
		{
			mocks.ForEach(PassesTestIfMethodExpectedAtLeastNTimesAndCalledNTimesOrdered);
		}

		private void PassesTestIfMethodExpectedAtLeastNTimesAndCalledNTimesOrdered(Mock<IParentInterface> mock)
		{
			using(Factory.Ordered())
			{
				mock.Expects.AtLeast(N).Method(_ => _.MethodVoid());
			}

			for (int i = 0; i < N; i++)
				mock.MockObject.MethodVoid();
		}

		[TestMethod]
		public void FailsTestIfMethodExpectedAtLeastNTimesButCalledLessThanNTimesOrdered()
		{
			mocks.ForEach(FailsTestIfMethodExpectedAtLeastNTimesButCalledLessThanNTimesOrdered);
		}

		private void FailsTestIfMethodExpectedAtLeastNTimesButCalledLessThanNTimesOrdered(Mock<IParentInterface> mock)
		{
			using(Factory.Ordered())
			{
				mock.Expects.AtLeast(N).Method(_ => _.MethodVoid());
			}

			for (int i = 0; i < N - 1; i++)
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
    System.Void {0}.MethodVoid() [EXPECTED: at least 5 times CALLED: 4 times]
  }}
", mock.Name), ex.Message);
			}
		}

		[TestMethod]
		public void PassesTestIfMethodExpectedAtLeastNTimesAndCalledMoreThanNTimesOrdered()
		{
			mocks.ForEach(PassesTestIfMethodExpectedAtLeastNTimesAndCalledMoreThanNTimesOrdered);
		}

		private void PassesTestIfMethodExpectedAtLeastNTimesAndCalledMoreThanNTimesOrdered(Mock<IParentInterface> mock)
		{
			using (Factory.Ordered())
			{
				mock.Expects.AtLeast(N).Method(_ => _.MethodVoid());
			}

			for (int i = 0; i < N + 1; i++)
				mock.MockObject.MethodVoid();
		}

	}
}