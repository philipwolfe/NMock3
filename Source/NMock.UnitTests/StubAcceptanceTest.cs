#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
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
	public class StubAcceptanceTest : AcceptanceTestBase
	{
		private const int ANY_NUMBER = 10;

		[TestMethod]
		public void StubsCanBeCalledAnyNumberOfTimes()
		{
			Mock<IHelloWorld> helloWorld = Mocks.CreateMock<IHelloWorld>();

			helloWorld.Stub.Out.Method(_ => _.Hello());

			for (int i = 0; i < ANY_NUMBER; i++) helloWorld.MockObject.Hello();
		}

		[TestMethod]
		public void StubsDoNotHaveToBeCalled()
		{
			Mock<IHelloWorld> helloWorld = Mocks.CreateMock<IHelloWorld>();

			helloWorld.Stub.Out.Method(_ => _.Hello());
		}

		[TestMethod]
		public void StubsMatchArgumentsAndPerformActionsJustLikeAnExpectation()
		{
			Mock<IHelloWorld> helloWorld = Mocks.CreateMock<IHelloWorld>();

			helloWorld.Stub.Out.Method(_ => _.Ask(null)).With("Name").Will(Return.Value("Bob"));
			helloWorld.Stub.Out.Method(_ => _.Ask(null)).With("Age").Will(Return.Value("30"));

			for (int i = 0; i < ANY_NUMBER; i++)
			{
				Verify.That(helloWorld.MockObject.Ask("Name"), Is.EqualTo("Bob"), "Name");
				Verify.That(helloWorld.MockObject.Ask("Age"), Is.EqualTo("30"), "Age");
			}
		}
	}
}