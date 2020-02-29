#region Using

using System;
using System.Collections;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using NMock;
using NMock.AcceptanceTests;
using NMock.Matchers;
using NMockTests._TestStructures;
using Is = NMock.Is;

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

namespace NMock.AcceptanceTests
{
	[TestClass]
	public class ErrorCheckingAcceptanceTest : AcceptanceTestBase
	{
		private Mock<IHelloWorld> mock;

		[TestInitialize]
		public void TestClassSetUp()
		{
			mock = Mocks.CreateMock<IHelloWorld>();
			SkipVerificationForThisFixture();
		}

		[TestMethod]
		public void CorrectMessageWhenReturnValueNotSet()
		{
			Mock<IParentInterface> myHelloWorld = Mocks.CreateMock<IParentInterface>();
			myHelloWorld.Expects.One.Method(_ => _.Method(0)).WithAnyArguments(); //.Will(Return.Value(true));//, new SetNamedParameterAction("number", 3)); //.With();

			Expect.That(() => myHelloWorld.MockObject.Method(3)).Throws<IncompleteExpectationException>("You have to set the return value for method 'IsPrime' on 'IMyHelloWorld' mock.");
		}

		//[TestMethod]
		//[ExpectedException(typeof (ArgumentException))]
		////[Ignore] //"This was turned off because it is possible as far as I can tell when the InterfaceBase has a ToString()"
		//public void MockingInterfaceWithToStringMethodThrowsException()
		//{
		//    Mocks.CreateMock<InterfaceWithToStringMethod>();
		//}

		[TestMethod]
		public void NullReturnValue()
		{
			mock.Expects.One.Method(_ => _.Ask(string.Empty)).WithAnyArguments().Will(Return.Value(null as string));

			string s = mock.MockObject.Ask("What?");

			Assert.IsNull(s);
		}

		[TestMethod]
		public void UnnecessaryReturnValue()
		{
			mock.Expects.One.Method(_ => _.Hello()).Will(Return.Value("What?"));

			Expect.That(mock.MockObject.Hello).Throws<ArgumentException>();
		}
	}
}