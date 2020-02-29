#region Using

using System;
using System.Collections.Generic;
using NMock;
using NMock.Matchers;
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
	public class CallbackListMatcherTests
	{
		MockFactory _factory = new MockFactory();
		private Mock<IParentInterface> _mock;

		[TestInitialize]
		public void TestInit()
		{
			_mock = _factory.CreateMock<IParentInterface>();
		}

		[TestMethod]
		public void CallbackListMatcherTest1()
		{
			var callback = new CallbackListMatcher<Action<IEnumerable<Version>>>();

			_mock.Expects.Exactly(2).Method(_ => _.AsyncMethod((Action<IEnumerable<Version>>)null)).With(callback);

			TestPresenter presenter1 = new TestPresenter(_mock.MockObject);
			TestPresenter presenter2 = new TestPresenter(_mock.MockObject);

			presenter1.BeginIEnumerableInvoke();
			presenter2.BeginIEnumerableInvoke();

			Assert.IsTrue(string.IsNullOrEmpty(presenter1.Status));
			Assert.IsTrue(string.IsNullOrEmpty(presenter2.Status));

			var versions = new List<Version> {new Version(1, 1, 1, 1), new Version(2, 2, 2, 2), new Version(3, 3, 3, 3)};

			callback.Callbacks.ForEach(_=>_.Invoke(versions));

			Assert.AreEqual(string.Format(TestPresenter.END_INVOKE_COUNT, 3), presenter1.Status);
			Assert.AreEqual(string.Format(TestPresenter.END_INVOKE_COUNT, 3), presenter2.Status);
		}
	}
}
