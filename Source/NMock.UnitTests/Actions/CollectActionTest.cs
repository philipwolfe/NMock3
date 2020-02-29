#region Using

using System.Reflection;
using NMock;
using NMock.Actions;
using NMock.Monitoring;

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
	public class CollectActionTest
	{
		private readonly object receiver = new object();
		private readonly MethodInfo methodInfo = typeof (CollectActionTest).GetMethod("DummyMethod");

		public void DummyMethod(int i, string s)
		{
		}

		[TestMethod]
		public void CollectsParameterValueAtSpecifiedIndex()
		{
			CollectAction action = new CollectAction(1);

			((IAction)action).Invoke(new Invocation(receiver, methodInfo, new object[] {123, "hello"}));

			Assert.AreEqual("hello", action.Parameter);
		}

		[TestMethod]
		public void HasAReadableDescription()
		{
			IAction action = new CollectAction(37);

			AssertDescription.IsEqual(action, "collect argument at index 37");
		}

		[TestMethod]
		public void ReturnsParameterValueFromMostRecentOfMultipleCalls()
		{
			CollectAction action = new CollectAction(1);

			((IAction)action).Invoke(new Invocation(receiver, methodInfo, new object[] {123, "hello"}));
			((IAction)action).Invoke(new Invocation(receiver, methodInfo, new object[] {456, "goodbye"}));

			Assert.AreEqual("goodbye", action.Parameter);
		}

		// MethodInfoStub breaks with non-empty parameters array
	}
}