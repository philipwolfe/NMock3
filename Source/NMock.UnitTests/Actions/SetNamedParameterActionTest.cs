#region Using

using System;
using System.Reflection;
using NMock;
using NMock.Actions;
using NMock.Matchers;
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
	public class SetNamedParameterActionTest
	{
		[TestMethod]
		public void HasReadableDescription()
		{
			string name = "param";
			object value = new NamedObject("value");

			SetNamedParameterAction action = new SetNamedParameterAction(name, value);

			AssertDescription.IsEqual(action, "set param=<value>(NMockTests.NamedObject)");
		}

		[TestMethod]
		public void SetsNamedParameterOnInvocation()
		{
			object receiver = new object();
			MethodInfoStub methodInfo = new MethodInfoStub("method",
			                                               new ParameterInfoStub("p1", ParameterAttributes.In),
			                                               new ParameterInfoStub("p2", ParameterAttributes.Out));
			string name = "p2";
			object value = new object();
			Invocation invocation = new Invocation(receiver, methodInfo, new object[] {null, null});

			SetNamedParameterAction action = new SetNamedParameterAction(name, value);

			((IAction)action).Invoke(invocation);

			Assert.AreSame(value, invocation.Parameters[1], "set value");
		}

		[TestMethod]
		public void SetsNamedParameterOnInvocationWrong()
		{
			object receiver = new object();
			MethodInfoStub methodInfo = new MethodInfoStub("method",
			                                               new ParameterInfoStub("p1", ParameterAttributes.In),
			                                               new ParameterInfoStub("p2", ParameterAttributes.Out));
			string name = "p2_wrong";
			object value = new object();
			Invocation invocation = new Invocation(receiver, methodInfo, new object[] {null, null});

			SetNamedParameterAction action = new SetNamedParameterAction(name, value);

			Expect.That(() => ((IAction)action).Invoke(invocation)).Throws<ArgumentException>(new StringContainsMatcher("no such parameter\r\nParameter name: p2_wrong"));

		}
	}
#endif
}