#region Using

using System;
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
	public class GetArugmentsActionTest
	{
		private readonly object receiver = new object();
		private readonly MethodInfo methodInfo = typeof (GetArugmentsActionTest).GetMethod("DummyMethod");


		public void DummyMethod(int i, string s, Action<string> response)
		{
		}

		[TestMethod]
		public void GetArgumentsOfInvokdedMethod()
		{
			ParameterList arugmentList = null;
			GetArgumentsAction action = new GetArgumentsAction(argments => arugmentList = argments);
			Action<string> responseHanlder = response => { };

			((IAction)action).Invoke(new Invocation(receiver, methodInfo, new object[] {123, "hello", responseHanlder}));

			Assert.AreEqual(3, arugmentList.Count);
			Assert.AreEqual(123, arugmentList[0]);
			Assert.AreEqual("hello", arugmentList[1]);
			Assert.AreEqual(responseHanlder, arugmentList[2]);
		}
	}
}