#region Using

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
	public class AtMostCountTests : BasicTestBase
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
		public void PassesTestIfMethodExpectedAtMostNTimesAndCalledLessThanNTimes()
		{
			mocks.ForEach(PassesTestIfMethodExpectedAtMostNTimesAndCalledLessThanNTimes);
		}

		private void PassesTestIfMethodExpectedAtMostNTimesAndCalledLessThanNTimes(Mock<IParentInterface> mock)
		{
			mock.Expects.AtMost(N).Method(_ => _.MethodVoid());

			for (var i = 0; i < N - 1; i++)
				mock.MockObject.MethodVoid();
		}

		[TestMethod]
		public void PassesTestIfMethodExpectedAtMostNTimesAndCalledNTimes()
		{
			mocks.ForEach(PassesTestIfMethodExpectedAtMostNTimesAndCalledNTimes);
		}

		private void PassesTestIfMethodExpectedAtMostNTimesAndCalledNTimes(Mock<IParentInterface> mock)
		{
			mock.Expects.AtMost(N).Method(_ => _.MethodVoid());

			for (var i = 0; i < N; i++)
				mock.MockObject.MethodVoid();
		}

		[TestMethod]
		public void FailsTestIfMethodExpectedAtMostNTimesAndCalledMoreThanNTimes()
		{
			mocks.ForEach(FailsTestIfMethodExpectedAtMostNTimesAndCalledMoreThanNTimes);
		}

		private void FailsTestIfMethodExpectedAtMostNTimesAndCalledMoreThanNTimes(Mock<IParentInterface> mock)
		{
			mock.Expects.AtMost(N).Method(_ => _.MethodVoid());

			for (var i = 0; i < N; i++)
				mock.MockObject.MethodVoid();

			//do one more than N
			Expect.That(mock.MockObject.MethodVoid).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected invocation of:
  System.Void {0}.MethodVoid()
MockFactory Expectations:
  System.Void {0}.MethodVoid() [EXPECTED: at most 5 times CALLED: 6 times]
", mock.Name)));

			mock.ClearExpectations();
		}

		//ordered
		[TestMethod]
		public void PassesTestIfMethodExpectedAtMostNTimesAndCalledLessThanNTimesOrdered()
		{
			mocks.ForEach(PassesTestIfMethodExpectedAtMostNTimesAndCalledLessThanNTimesOrdered);
		}

		private void PassesTestIfMethodExpectedAtMostNTimesAndCalledLessThanNTimesOrdered(Mock<IParentInterface> mock)
		{
			using(Factory.Ordered())
			{
				mock.Expects.AtMost(N).Method(_ => _.MethodVoid());
			}

			for (var i = 0; i < N - 1; i++)
				mock.MockObject.MethodVoid();
		}

		[TestMethod]
		public void PassesTestIfMethodExpectedAtMostNTimesAndCalledNTimesOrdered()
		{
			mocks.ForEach(PassesTestIfMethodExpectedAtMostNTimesAndCalledNTimesOrdered);
		}

		private void PassesTestIfMethodExpectedAtMostNTimesAndCalledNTimesOrdered(Mock<IParentInterface> mock)
		{
			using(Factory.Ordered())
			{
				mock.Expects.AtMost(N).Method(_ => _.MethodVoid());
			}

			for (var i = 0; i < N; i++)
				mock.MockObject.MethodVoid();
		}

		[TestMethod]
		public void FailsTestIfMethodExpectedAtMostNTimesAndCalledMoreThanNTimesOrdered()
		{
			mocks.ForEach(FailsTestIfMethodExpectedAtMostNTimesAndCalledMoreThanNTimesOrdered);
		}

		private void FailsTestIfMethodExpectedAtMostNTimesAndCalledMoreThanNTimesOrdered(Mock<IParentInterface> mock)
		{
			using (Factory.Ordered())
			{
				mock.Expects.AtMost(N).Method(_ => _.MethodVoid());
			}

			for (var i = 0; i < N; i++)
				mock.MockObject.MethodVoid();

			//do one more than N
			Expect.That(mock.MockObject.MethodVoid).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected invocation of:
  System.Void {0}.MethodVoid()
MockFactory Expectations:
  Ordered {{
    System.Void {0}.MethodVoid() [EXPECTED: at most 5 times CALLED: 5 times]
  }}
", mock.Name)));

			mock.ClearExpectations();
		}

		[TestMethod]
		public void FailsTestIfMethodExpectedAtMostNTimesAndCalledMoreThan5TimesVeryOrdered()
		{
			mocks.ForEach(FailsTestIfMethodExpectedAtMostNTimesAndCalledMoreThan5TimesVeryOrdered);
		}

		private void FailsTestIfMethodExpectedAtMostNTimesAndCalledMoreThan5TimesVeryOrdered(Mock<IParentInterface> mock)
		{
			using (Factory.Ordered())
			{
				mock.Expects.AtMost(1).Method(_ => _.MethodVoid());
				using (Factory.Ordered())
				{
					mock.Expects.AtMost(1).Method(_ => _.MethodVoid());
					using (Factory.Ordered())
					{
						mock.Expects.AtMost(1).Method(_ => _.MethodVoid());
						using (Factory.Ordered())
						{
							mock.Expects.AtMost(1).Method(_ => _.MethodVoid());
							using (Factory.Ordered())
							{
								mock.Expects.AtMost(1).Method(_ => _.MethodVoid());
							}
						}
					}
				}
			}

			mock.MockObject.MethodVoid();
			mock.MockObject.MethodVoid();
			mock.MockObject.MethodVoid();
			mock.MockObject.MethodVoid();
			mock.MockObject.MethodVoid();

			//do one more than 5
			Expect.That(mock.MockObject.MethodVoid).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected invocation of:
  System.Void {0}.MethodVoid()
MockFactory Expectations:
  Ordered {{
    System.Void {0}.MethodVoid() [EXPECTED: at most 1 time CALLED: 1 time]
    Ordered {{
      System.Void {0}.MethodVoid() [EXPECTED: at most 1 time CALLED: 1 time]
      Ordered {{
        System.Void {0}.MethodVoid() [EXPECTED: at most 1 time CALLED: 1 time]
        Ordered {{
          System.Void {0}.MethodVoid() [EXPECTED: at most 1 time CALLED: 1 time]
          Ordered {{
            System.Void {0}.MethodVoid() [EXPECTED: at most 1 time CALLED: 1 time]
          }}
        }}
      }}
    }}
  }}

", mock.Name)));

			Factory.ClearExpectations();
		}
	}
}
