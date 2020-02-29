#region Using

using System;
using System.Collections.Generic;
using NMock;
using NMock.Matchers;
using NMockTests.MockTests;
using NMockTests._TestStructures;

#if NUNIT
using Assert = NUnit.Framework.Assert;
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
namespace NMockTests.Matchers35
#else
#if SILVERLIGHT
namespace NMockTests.MatchersSL
#else
namespace NMockTests.Matchers
#endif
#endif
{
	[TestClass]
	public class CallbackMatcherTests : BasicTestBase
	{
		[TestInitialize]
		public void TestInit()
		{
			Initalize();
		}

		[TestCleanup]
		public void TestClean()
		{
			Cleanup();
		}

		[TestMethod]
		public void CallbackMatcherTest1()
		{
			mocks.ForEach(CallbackMatcherTest1);
		}

		private void CallbackMatcherTest1(Mock<IParentInterface> mock)
		{
			var callback = new CallbackMatcher<Action>();

			mock.Expects.One.Method(_ => _.AsyncMethod((Action)null)).With(callback);

			var presenter = new TestPresenter(mock.MockObject);

			presenter.BeginInvoke();

			Assert.IsTrue(string.IsNullOrEmpty(presenter.Status));

			callback.Callback();

			Assert.AreEqual(TestPresenter.END_INVOKE, presenter.Status);
		}

		[TestMethod]
		public void CallbackMatcherTest2()
		{
			mocks.ForEach(CallbackMatcherTest2);
		}

		public void CallbackMatcherTest2(Mock<IParentInterface> mock)
		{
			var callback = new CallbackMatcher<Action<IEnumerable<Version>>>();

			mock.Expects.One.Method(_ => _.AsyncMethod((Action<IEnumerable<Version>>)null)).With(callback);

			var presenter = new TestPresenter(mock.MockObject);

			presenter.BeginIEnumerableInvoke();

			Assert.IsTrue(string.IsNullOrEmpty(presenter.Status));

			var versions = new List<Version> { new Version(1, 1, 1, 1), new Version(2, 2, 2, 2), new Version(3, 3, 3, 3) };

			callback.Callback(versions);

			Assert.AreEqual(string.Format(TestPresenter.END_INVOKE_COUNT, 3), presenter.Status);
		}

		[TestMethod]
		public void CallbackMatcherTest3()
		{
			mocks.ForEach(CallbackMatcherTest3);
		}

		private void CallbackMatcherTest3(Mock<IParentInterface> mock)
		{
			var callback = new CallbackMatcher<Action>();

			mock.Expects.One.Method(_ => _.AsyncMethod((Action)null)).With(callback);

			var presenter = new TestPresenter(mock.MockObject);

			presenter.ShowDialog();

			Assert.IsNull(presenter.Status);

			callback.Callback();

			Assert.AreEqual(TestPresenter.SHOW_DIALOG, presenter.Status);
		}

		[TestMethod]
		public void CallbackMatcherTest4()
		{
			mocks.ForEach(CallbackMatcherTest4);
		}

		private void CallbackMatcherTest4(Mock<IParentInterface> mock)
		{
			var v = new Version(1, 2, 3, 4);
			var c = new CallbackMatcher<Action>();
			var called = false;
			mock.Expects.One.Method(_ => _.AsyncMethod(null, null)).With(v, c);

			mock.MockObject.AsyncMethod(v, () => { called = true; });

			Assert.IsFalse(called);

			c.Callback();

			Assert.IsTrue(called);
		}

		[TestMethod]
		public void CallbackMatcherTest5()
		{
			mocks.ForEach(CallbackMatcherTest5);
		}

		private void CallbackMatcherTest5(Mock<IParentInterface> mock)
		{
			var c = new CallbackMatcher<Action>();
			var v = new Version(1, 2, 3, 4);

			mock.Expects.AtLeastOne.SetProperty(_ => _.ReadWriteObjectProperty).To(Is.TypeOf<Version>());

			using (Factory.Ordered())
			{
				mock.Expects.AtLeastOne.SetProperty(_ => _[4, 5]).To(Is.TypeOf<DateTime>());
				mock.Expects.One.Method(_ => _.AsyncMethod((Action)null)).With(c);
			}

			var p = new TestPresenter(mock.MockObject);
			p.SetPropertyOfMockToInternalValue();
			p.SetPropertyOfMockToInternalValue();
			p.SetIndexerPropertyOfMockToInternalValue();
			p.SetIndexerPropertyOfMockToInternalValue();
			p.BeginInvoke();

			c.Callback();

			Assert.AreEqual(TestPresenter.END_INVOKE, p.Status);
		}

#if NetFx45
		[TestMethod]
		public void CallbackMatcherTest6()
		{
            var factory = new MockFactory();
            var mock = factory.CreateMock<IMyService>();

            var matcher = new CallbackMatcher<System.Threading.Tasks.Task<int>>();

            mock.Expects.One.Method(_ => _.GetAsync()).With(matcher);

            var sut = new SystemUnderTest(mock.MockObject);

            var result = sut.RetrieveValueAsync();

            //matcher.c

            Assert.AreEqual(47, result);
		}
#endif

    }

#if NetFx45
    public interface IMyService
    {
        System.Threading.Tasks.Task<int> GetAsync();
    }

    public sealed class SystemUnderTest
    {
        private readonly IMyService _service;

        public SystemUnderTest(IMyService service)
        {
            _service = service;
        }

        public async System.Threading.Tasks.Task<int> RetrieveValueAsync()
        {
            return 42 + await _service.GetAsync();
        }
    }
#endif
}
