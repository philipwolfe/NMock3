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
	public class OneTests : BasicTestBase
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
		public void PassesTestIfMethodExpectedOnceAndCalled()
		{
			mocks.ForEach(PassesTestIfMethodExpectedOnceAndCalled);
			instances.ForEach(PassesTestIfMethodExpectedOnceAndCalled);
		}

		private void PassesTestIfMethodExpectedOnceAndCalled(Mock<IParentInterface> mock)
		{
			mock.Expects.One.Method(_ => _.MethodVoid());

			mock.MockObject.MethodVoid();
		}

		private void PassesTestIfMethodExpectedOnceAndCalled(IParentInterface instance)
		{
			Expect.On(instance).One.Method(_ => _.MethodVoid());

			instance.MethodVoid();
		}

		[TestMethod]
		public void FailsTestIfMethodExpectedOnceButCalledTwice()
		{
			mocks.ForEach(FailsTestIfMethodExpectedOnceButCalledTwice);
		}

		private void FailsTestIfMethodExpectedOnceButCalledTwice(Mock<IParentInterface> mock)
		{
			mock.Expects.One.Method(_ => _.MethodVoid());

			mock.MockObject.MethodVoid();

			//do one more than N
			Expect.That(mock.MockObject.MethodVoid).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected invocation of:
  System.Void {0}.MethodVoid()
MockFactory Expectations:
  System.Void {0}.MethodVoid() [EXPECTED: 1 time CALLED: 2 times]
", mock.Name)));

			mock.ClearExpectations();
		}

		[TestMethod]
		public void FailsTestIfMethodExpectedOnceButNotCalled()
		{
			mocks.ForEach(FailsTestIfMethodExpectedOnceButNotCalled);
		}

		private void FailsTestIfMethodExpectedOnceButNotCalled(Mock<IParentInterface> mock)
		{
			mock.Expects.One.Method(_ => _.MethodVoid());

			try
			{
				Factory.VerifyAllExpectationsHaveBeenMet();
			}
			catch (Exception ex)
			{
				Assert.AreEqual(string.Format(@"Not all expected invocations were performed.
MockFactory Expectations:
  System.Void {0}.MethodVoid() [EXPECTED: 1 time CALLED: 0 times]
", mock.Name), ex.Message);
			}
		}

		//ordered
		[TestMethod]
		public void PassesTestIfMethodExpectedOnceAndCalledOrdered()
		{
			mocks.ForEach(PassesTestIfMethodExpectedOnceAndCalledOrdered);
			instances.ForEach(PassesTestIfMethodExpectedOnceAndCalledOrdered);
		}

		private void PassesTestIfMethodExpectedOnceAndCalledOrdered(Mock<IParentInterface> mock)
		{
			using (Factory.Ordered())
				mock.Expects.One.Method(_ => _.MethodVoid());

			mock.MockObject.MethodVoid();
		}

		private void PassesTestIfMethodExpectedOnceAndCalledOrdered(IParentInterface instance)
		{
			using (Factory.Ordered())
				Expect.On(instance).One.Method(_ => _.MethodVoid());

			instance.MethodVoid();
		}

		[TestMethod]
		public void FailsTestIfMethodExpectedOnceButCalledTwiceOrdered()
		{
			mocks.ForEach(FailsTestIfMethodExpectedOnceButCalledTwiceOrdered);
		}

		private void FailsTestIfMethodExpectedOnceButCalledTwiceOrdered(Mock<IParentInterface> mock)
		{
			using (Factory.Ordered())
				mock.Expects.One.Method(_ => _.MethodVoid());

			mock.MockObject.MethodVoid();

			//do one more than N
			Expect.That(mock.MockObject.MethodVoid).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected invocation of:
  System.Void {0}.MethodVoid()
MockFactory Expectations:
  Ordered {{
    System.Void {0}.MethodVoid() [EXPECTED: 1 time CALLED: 1 time]
  }}
", mock.Name)));

			mock.ClearExpectations();
		}

		[TestMethod]
		public void FailsTestIfMethodExpectedOnceButNotCalledOrdered()
		{
			mocks.ForEach(FailsTestIfMethodExpectedOnceButNotCalledOrdered);
		}

		private void FailsTestIfMethodExpectedOnceButNotCalledOrdered(Mock<IParentInterface> mock)
		{
			using (Factory.Ordered())
				mock.Expects.One.Method(_ => _.MethodVoid());

			try
			{
				Factory.VerifyAllExpectationsHaveBeenMet();
			}
			catch (Exception ex)
			{
				Assert.AreEqual(string.Format(@"Not all expected invocations were performed.
MockFactory Expectations:
  Ordered {{
    System.Void {0}.MethodVoid() [EXPECTED: 1 time CALLED: 0 times]
  }}
", mock.Name), ex.Message);
			}
		}
	}
}