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
	public class ExactlyTests : BasicTestBase
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
		public void TestShouldPassIfExpectedExactlyNTimesAndCalledNTimes()
		{
			mocks.ForEach(TestShouldPassIfExpectedExactlyNTimesAndCalledNTimes);
		}

		private void TestShouldPassIfExpectedExactlyNTimesAndCalledNTimes(Mock<IParentInterface> mock)
		{
			mock.Expects.Exactly(N).Method(_ => _.MethodVoid());

			for (int i = 0; i < N; i++)
				mock.MockObject.MethodVoid();
		}

		[TestMethod]
		public void TestShouldFailIfExpectedExactlyNTimesAndCalledLessThanNTimes()
		{
			mocks.ForEach(TestShouldFailIfExpectedExactlyNTimesAndCalledLessThanNTimes);
		}

		public void TestShouldFailIfExpectedExactlyNTimesAndCalledLessThanNTimes(Mock<IParentInterface> mock)
		{
			mock.Expects.Exactly(N).Method(_ => _.MethodVoid());

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
  System.Void {0}.MethodVoid() [EXPECTED: 5 times CALLED: 4 times]
", mock.Name), ex.Message);
			}
		}

		[TestMethod]
		public void TestShouldFailIfExpectedExactlyNTimesAndCalledMoreThanNTimes()
		{
			mocks.ForEach(TestShouldFailIfExpectedExactlyNTimesAndCalledMoreThanNTimes);
		}

		private void TestShouldFailIfExpectedExactlyNTimesAndCalledMoreThanNTimes(Mock<IParentInterface> mock)
		{
			mock.Expects.Exactly(N).Method(_ => _.MethodVoid());

			for (int i = 0; i < N; i++)
				mock.MockObject.MethodVoid();

			//do one more than N
			Expect.That(mock.MockObject.MethodVoid).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected invocation of:
  System.Void {0}.MethodVoid()
MockFactory Expectations:
  System.Void {0}.MethodVoid() [EXPECTED: 5 times CALLED: 6 times]
", mock.Name)));

			mock.ClearExpectations();
		}

		//ordered
		[TestMethod]
		public void TestShouldPassIfExpectedExactlyNTimesAndCalledNTimesOrdered()
		{
			mocks.ForEach(TestShouldPassIfExpectedExactlyNTimesAndCalledNTimesOrdered);
		}

		private void TestShouldPassIfExpectedExactlyNTimesAndCalledNTimesOrdered(Mock<IParentInterface> mock)
		{
			using(Factory.Ordered())
			{
				mock.Expects.Exactly(N).Method(_ => _.MethodVoid());
			}

			for (int i = 0; i < N; i++)
				mock.MockObject.MethodVoid();
		}

		[TestMethod]
		public void TestShouldFailIfExpectedExactlyNTimesAndCalledLessThanNTimesOrdered()
		{
			mocks.ForEach(TestShouldFailIfExpectedExactlyNTimesAndCalledLessThanNTimesOrdered);
		}

		public void TestShouldFailIfExpectedExactlyNTimesAndCalledLessThanNTimesOrdered(Mock<IParentInterface> mock)
		{
			using (Factory.Ordered())
			{
				mock.Expects.Exactly(N).Method(_ => _.MethodVoid());
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
    System.Void {0}.MethodVoid() [EXPECTED: 5 times CALLED: 4 times]
  }}
", mock.Name), ex.Message);
			}
		}

		[TestMethod]
		public void TestShouldFailIfExpectedExactlyNTimesAndCalledMoreThanNTimesOrdered()
		{
			mocks.ForEach(TestShouldFailIfExpectedExactlyNTimesAndCalledMoreThanNTimesOrdered);
		}

		private void TestShouldFailIfExpectedExactlyNTimesAndCalledMoreThanNTimesOrdered(Mock<IParentInterface> mock)
		{
			using (Factory.Ordered())
			{
				mock.Expects.Exactly(N).Method(_ => _.MethodVoid());
			}

			for (int i = 0; i < N; i++)
				mock.MockObject.MethodVoid();

			//do one more than N
			Expect.That(mock.MockObject.MethodVoid).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected invocation of:
  System.Void {0}.MethodVoid()
MockFactory Expectations:
  Ordered {{
    System.Void {0}.MethodVoid() [EXPECTED: {1} times CALLED: {1} times]
  }}
", mock.Name, N)));

			mock.ClearExpectations();
		}
	}
}