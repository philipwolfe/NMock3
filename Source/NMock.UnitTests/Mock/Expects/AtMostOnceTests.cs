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
	public class AtMostOnceTests : BasicTestBase
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
		public void PassesTestIfMethodExpectedAtMostOnceAndCalledOneTime()
		{
			mocks.ForEach(PassesTestIfMethodExpectedAtMostOnceAndCalledOneTime);
		}

		private void PassesTestIfMethodExpectedAtMostOnceAndCalledOneTime(Mock<IParentInterface> mock)
		{
			mock.Expects.AtMostOne.Method(_ => _.MethodVoid());

			for (int i = 0; i < 1; i++)
				mock.MockObject.MethodVoid();
		}

		[TestMethod]
		public void PassesTestIfMethodExpectedAtMostOnceAndCalled0Times()
		{
			mocks.ForEach(PassesTestIfMethodExpectedAtMostOnceAndCalled0Times);
		}

		public void PassesTestIfMethodExpectedAtMostOnceAndCalled0Times(Mock<IParentInterface> mock)
		{
			mock.Expects.AtMostOne.Method(_ => _.MethodVoid());

			for (int i = 0; i < 0; i++)
				mock.MockObject.MethodVoid();
		}

		[TestMethod]
		public void FailsTestIfMethodExpectedAtMostOncesAndCalledMoreThanOneTimes()
		{
			mocks.ForEach(FailsTestIfMethodExpectedAtMostOncesAndCalledMoreThanOneTimes);
		}

		public void FailsTestIfMethodExpectedAtMostOncesAndCalledMoreThanOneTimes(Mock<IParentInterface> mock)
		{
			mock.Expects.AtMostOne.Method(_ => _.MethodVoid());

			mock.MockObject.MethodVoid();

			//do one more
			Expect.That(mock.MockObject.MethodVoid).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected invocation of:
  System.Void {0}.MethodVoid()
MockFactory Expectations:
  System.Void {0}.MethodVoid() [EXPECTED: at most 1 time CALLED: 2 times]
", mock.Name)));

			mock.ClearExpectations();
		}

		//ordered
		[TestMethod]
		public void PassesTestIfMethodExpectedAtMostOnceAndCalledOneTimeOrdered()
		{
			mocks.ForEach(PassesTestIfMethodExpectedAtMostOnceAndCalledOneTimeOrdered);
		}

		private void PassesTestIfMethodExpectedAtMostOnceAndCalledOneTimeOrdered(Mock<IParentInterface> mock)
		{
			using (Factory.Ordered())
			{
				mock.Expects.AtMostOne.Method(_ => _.MethodVoid());
			}

			for (int i = 0; i < 1; i++)
				mock.MockObject.MethodVoid();
		}

		[TestMethod]
		public void PassesTestIfMethodExpectedAtMostOnceAndCalled0TimesOrdered()
		{
			mocks.ForEach(PassesTestIfMethodExpectedAtMostOnceAndCalled0TimesOrdered);
		}

		public void PassesTestIfMethodExpectedAtMostOnceAndCalled0TimesOrdered(Mock<IParentInterface> mock)
		{
			using (Factory.Ordered())
			{
				mock.Expects.AtMostOne.Method(_ => _.MethodVoid());
			}

			for (int i = 0; i < 0; i++)
				mock.MockObject.MethodVoid();
		}

		[TestMethod]
		public void FailsTestIfMethodExpectedAtMostOncesAndCalledMoreThanOneTimesOrdered()
		{
			mocks.ForEach(FailsTestIfMethodExpectedAtMostOncesAndCalledMoreThanOneTimesOrdered);
		}

		public void FailsTestIfMethodExpectedAtMostOncesAndCalledMoreThanOneTimesOrdered(Mock<IParentInterface> mock)
		{
			using (Factory.Ordered())
			{
				mock.Expects.AtMostOne.Method(_ => _.MethodVoid());
			}

			mock.MockObject.MethodVoid();

			//do one more
			Expect.That(mock.MockObject.MethodVoid).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected invocation of:
  System.Void {0}.MethodVoid()
MockFactory Expectations:
  Ordered {{
    System.Void {0}.MethodVoid() [EXPECTED: at most 1 time CALLED: 1 time]
  }}
", mock.Name)));

			mock.ClearExpectations();
		}
	}
}
