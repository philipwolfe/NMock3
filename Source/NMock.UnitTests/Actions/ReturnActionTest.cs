#region Using

using NMock;
using NMock.Actions;
using NMock.Monitoring;
using NMockTests.Monitoring;

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
#if !SILVERLIGHT
	[TestClass]
	public class ReturnActionTest
	{
		public static object ResultOfAction(IAction action)
		{
			object receiver = new NamedObject("receiver");
			MethodInfoStub methodInfo = new MethodInfoStub("method");
			Invocation invocation = new Invocation(receiver, methodInfo, new object[0]);

			action.Invoke(invocation);

			return invocation.Result;
		}

		[TestMethod]
		public void HasAReadableDescription()
		{
			object result = new NamedObject("result");
			ReturnAction action = new ReturnAction(result);

			AssertDescription.IsEqual(action, "return <result>(NMockTests.NamedObject)");
		}

		[TestMethod]
		public void SetsReturnValueOfInvocation()
		{
			object result = new NamedObject("result");
			ReturnAction action = new ReturnAction(result);

			Assert.AreSame(result, ResultOfAction(action), "result");
		}
	}
#endif
}