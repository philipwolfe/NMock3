#region Using

using System.Reflection;
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
	public class SetIndexedParameterActionTest
	{
		[TestMethod]
		public void HasReadableDescription()
		{
			int index = 1;
			object value = new NamedObject("value");

			SetIndexedParameterAction action = new SetIndexedParameterAction(index, value);

			AssertDescription.IsEqual(action, "set arg 1=<value>(NMockTests.NamedObject)");
		}

		[TestMethod]
		public void SetsIndexedParameterOnInvocation()
		{
			object receiver = new object();
			MethodInfoStub methodInfo = new MethodInfoStub("method",
			                                               new ParameterInfoStub("p1", ParameterAttributes.In),
			                                               new ParameterInfoStub("p2", ParameterAttributes.Out));
			int index = 1;
			object value = new object();
			Invocation invocation = new Invocation(receiver, methodInfo, new object[] {null, null});

			SetIndexedParameterAction action = new SetIndexedParameterAction(index, value);

			((IAction)action).Invoke(invocation);

			Assert.AreSame(value, invocation.Parameters[index], "set value");
		}
	}
#endif
}