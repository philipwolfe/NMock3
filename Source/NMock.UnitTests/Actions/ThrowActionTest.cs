#region Using

using System;
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
	public class ThrowActionTest
	{
		[TestMethod]
		public void HasReadableDescription()
		{
			Exception exception = new Exception();
			ThrowAction action = new ThrowAction(exception);

			AssertDescription.IsEqual(action, "throw <" + exception + ">(System.Exception)");
		}

		[TestMethod]
		public void SetsExceptionOfInvocation()
		{
			Exception exception = new Exception();
			ThrowAction action = new ThrowAction(exception);

			object receiver = new object();
			MethodInfoStub methodInfo = new MethodInfoStub("method");
			Invocation invocation = new Invocation(receiver, methodInfo, new object[0]);

			((IAction)action).Invoke(invocation);

			Assert.AreSame(exception, invocation.Exception, "exception");
		}
	}
#endif
}