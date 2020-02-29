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
	public class BetweenTests : BasicTestBase
	{
		private int N = 10;
		private int M = 20;

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
		public void PassesTestIfMethodExpectedBetweenNAndMTimesAndCalledBetweenNAndMTimes()
		{
			mocks.ForEach(PassesTestIfMethodExpectedBetweenNAndMTimesAndCalledBetweenNAndMTimes);
		}

		private void PassesTestIfMethodExpectedBetweenNAndMTimesAndCalledBetweenNAndMTimes(Mock<IParentInterface> mock)
		{
			mock.Expects.Between(N, M).Method(_ => _.MethodVoid());

			for (int i = 0; i < (N + M) / 2; i++)
				mock.MockObject.MethodVoid();
		}

		[TestMethod]
		public void FailsTestIfMethodExpectedBetweenNAndMTimesAndCalledMoreThanMTimes()
		{
			mocks.ForEach(FailsTestIfMethodExpectedBetweenNAndMTimesAndCalledMoreThanMTimes);
		}

		private void FailsTestIfMethodExpectedBetweenNAndMTimesAndCalledMoreThanMTimes(Mock<IParentInterface> mock)
		{
			mock.Expects.Between(N, M).Method(_ => _.MethodVoid());

			for (int i = 0; i < M; i++)
				mock.MockObject.MethodVoid();

			//do one more than M
			Expect.That(mock.MockObject.MethodVoid).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected invocation of:
  System.Void {0}.MethodVoid()
MockFactory Expectations:
  System.Void {0}.MethodVoid() [EXPECTED: 10 to 20 times CALLED: 21 times]
", mock.Name)));

			mock.ClearExpectations();
		}

		[TestMethod]
		public void FailsTestIfMethodExpectedBetweenNAndMTimesButCalledLessThanNTimes()
		{
			mocks.ForEach(FailsTestIfMethodExpectedBetweenNAndMTimesButCalledLessThanNTimes);
		}

		private void FailsTestIfMethodExpectedBetweenNAndMTimesButCalledLessThanNTimes(Mock<IParentInterface> mock)
		{
			mock.Expects.Between(N, M).Method(_ => _.MethodVoid());

			for (int i = 0; i < N - 1; i++) mock.MockObject.MethodVoid();

			try
			{
				Factory.VerifyAllExpectationsHaveBeenMet();
			}
			catch (Exception ex)
			{
				Assert.AreEqual(string.Format(@"Not all expected invocations were performed.
MockFactory Expectations:
  System.Void {0}.MethodVoid() [EXPECTED: 10 to 20 times CALLED: 9 times]
", mock.Name), ex.Message);
			}
		}

		//ordered
		[TestMethod]
		public void PassesTestIfMethodExpectedBetweenNAndMTimesAndCalledBetweenNAndMTimesOrdered()
		{
			mocks.ForEach(PassesTestIfMethodExpectedBetweenNAndMTimesAndCalledBetweenNAndMTimesOrdered);
		}

		private void PassesTestIfMethodExpectedBetweenNAndMTimesAndCalledBetweenNAndMTimesOrdered(Mock<IParentInterface> mock)
		{
			using (Factory.Ordered())
			{
				mock.Expects.Between(N, M).Method(_ => _.MethodVoid());
			}

			for (int i = 0; i < (N + M) / 2; i++)
				mock.MockObject.MethodVoid();
		}

		[TestMethod]
		public void FailsTestIfMethodExpectedBetweenNAndMTimesAndCalledMoreThanMTimesOrdered()
		{
			mocks.ForEach(FailsTestIfMethodExpectedBetweenNAndMTimesAndCalledMoreThanMTimesOrdered);
		}

		private void FailsTestIfMethodExpectedBetweenNAndMTimesAndCalledMoreThanMTimesOrdered(Mock<IParentInterface> mock)
		{
			using (Factory.Ordered())
			{
				mock.Expects.Between(N, M).Method(_ => _.MethodVoid());
			}

			for (int i = 0; i < M; i++)
				mock.MockObject.MethodVoid();

			//do one more than M
			Expect.That(mock.MockObject.MethodVoid).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected invocation of:
  System.Void {0}.MethodVoid()
MockFactory Expectations:
  Ordered {{
    System.Void {0}.MethodVoid() [EXPECTED: 10 to 20 times CALLED: 20 times]
  }}
", mock.Name)));

			mock.ClearExpectations();
		}

		[TestMethod]
		public void FailsTestIfMethodExpectedBetweenNAndMTimesButCalledLessThanNTimesOrdered()
		{
			mocks.ForEach(FailsTestIfMethodExpectedBetweenNAndMTimesButCalledLessThanNTimesOrdered);
		}

		private void FailsTestIfMethodExpectedBetweenNAndMTimesButCalledLessThanNTimesOrdered(Mock<IParentInterface> mock)
		{
			using (Factory.Ordered())
			{
				mock.Expects.Between(N, M).Method(_ => _.MethodVoid());
			}

			for (int i = 0; i < N - 1; i++) mock.MockObject.MethodVoid();

			try
			{
				Factory.VerifyAllExpectationsHaveBeenMet();
			}
			catch (Exception ex)
			{
				Assert.AreEqual(string.Format(@"Not all expected invocations were performed.
MockFactory Expectations:
  Ordered {{
    System.Void {0}.MethodVoid() [EXPECTED: 10 to 20 times CALLED: 9 times]
  }}
", mock.Name), ex.Message);
			}
		}
	}
}