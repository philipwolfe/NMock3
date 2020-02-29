#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using NMock;
using NMock.AcceptanceTests;
using NMock.Actions;
using NMock.Matchers;
using NMockTests._TestStructures;
using Is = NMock.Is;

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

namespace NMock.AcceptanceTests
{
	[TestClass]
	public class ThreadSynchronizationAcceptanceTest : AcceptanceTestBase
	{
		/// <summary>
		/// You can use the <see cref="Signal"/> class to signal event handles when the action on an expectation is executed.
		/// </summary>
		[TestMethod]
		public void SynchronizeThreads()
		{
			AutoResetEvent signalFromBackgroundThread = new AutoResetEvent(false);
			AutoResetEvent signalFromForegroundThread = new AutoResetEvent(false);

			Mock<IHelloWorld> helloWorld = Mocks.CreateMock<IHelloWorld>();

			helloWorld.Expects.One.Method(_ => _.Hello()).Will(Signal.EventWaitHandle(signalFromBackgroundThread));

			ThreadPool.QueueUserWorkItem(delegate
			                             	{
			                             		if (signalFromForegroundThread.WaitOne(1000, false))
			                             		{
			                             			helloWorld.MockObject.Hello(); // this will set the signal
			                             		}
			                             	});

			signalFromForegroundThread.Set();
			bool signaled = signalFromBackgroundThread.WaitOne(1000, false);

			Assert.IsTrue(signaled, "did not receive signal from background thread.");
		}
	}
}