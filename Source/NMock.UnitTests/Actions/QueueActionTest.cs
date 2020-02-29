#region Using

using System;
using System.Collections.Generic;
using NMock;
using NMock.Actions;
using NMock.Matchers;
using NMockTests.MockTests;
using NMockTests._TestStructures;

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
namespace NMockTests.Actions35
#else
#if SILVERLIGHT
namespace NMockTests.ActionsSL
#else
namespace NMockTests.Actions
#endif
#endif
{
	[TestClass]
	public class QueueActionTest : BasicTestBase
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
		public void QueueTest1()
		{
			mocks.ForEach(QueueTest1);
		}

		private void QueueTest1(Mock<IParentInterface> mock)
		{
			var queue = new Queue<Version>();
			queue.Enqueue(new Version(1, 1, 1, 1));
			queue.Enqueue(new Version(2, 2, 2, 2));
			queue.Enqueue(new Version(3, 3, 3, 3));

			mock.Expects.Exactly(3).Method(_ => _.Method<Version>()).Will(new QueueAction<Version>(queue));

			Assert.AreEqual(1, mock.MockObject.Method<Version>().Major);
			Assert.AreEqual(2, mock.MockObject.Method<Version>().Major);
			Assert.AreEqual(3, mock.MockObject.Method<Version>().Major);
		}

		[TestMethod]
		public void QueueTest2()
		{
			mocks.ForEach(QueueTest2);
		}

		private void QueueTest2(Mock<IParentInterface> mock)
		{
			var queue = new Queue<Version>();
			queue.Enqueue(new Version(1, 1, 1, 1));
			queue.Enqueue(new Version(2, 2, 2, 2));
			queue.Enqueue(new Version(3, 3, 3, 3));

			mock.Expects.Exactly(3).Method(_ => _.Method<Version>()).Will(new QueueAction<Version>(queue));

			Assert.AreEqual(1, mock.MockObject.Method<Version>().Major);
			Assert.AreEqual(2, mock.MockObject.Method<Version>().Major);
			Assert.AreEqual(3, mock.MockObject.Method<Version>().Major);

			Expect.That(() => { var v = mock.MockObject.Method<Version>().Major; }).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected invocation of:
  System.Version {0}.Method<System.Version>()
MockFactory Expectations:
  System.Version {0}.Method<System.Version>() will return a 'Version' from a queue [EXPECTED: 3 times CALLED: 4 times]
", mock.Name)));

			mock.ClearExpectations();
		}
	}
}