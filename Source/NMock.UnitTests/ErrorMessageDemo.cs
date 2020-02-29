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
	public class ErrorMessageDemo : AcceptanceTestBase
	{
		[TestInitialize]
		public void TestClassSetUp()
		{
			SkipVerificationForThisFixture();
		}

		private void DoAction()
		{
			throw new NotSupportedException();
		}

		[TestMethod, ExpectedException(typeof (UnexpectedInvocationException))]
		public void EventAdd()
		{
			Mock<ISyntacticSugar> sugar = Mocks.CreateMock<ISyntacticSugar>("sugar");

			sugar.Expects.One.EventBinding(s => s.Actions -= null);

			sugar.MockObject.Actions += DoAction;
		}


		[TestMethod, ExpectedException(typeof (UnexpectedInvocationException))]
		public void UnexpectedInvocation()
		{
			Mock<IHelloWorld> helloWorld = Mocks.CreateMock<IHelloWorld>();

			helloWorld.Expects.One.Method(_ => _.Hello());
			helloWorld.Expects.Between(2, 4).Method(_ => _.Ask(null)).With("What color is the fish?")
				.Will(Return.Value("purple"));
			helloWorld.Expects.AtLeast(1).Method(_ => _.Ask(null)).With("How big is the fish?")
				.Will(Throw.Exception(new InvalidOperationException("stop asking about the fish!")));

			helloWorld.MockObject.Hello();
			helloWorld.MockObject.Ask("What color is the fish?");
			helloWorld.MockObject.Ask("What color is the hippo?");
		}

		[TestMethod, ExpectedException(typeof (UnmetExpectationException))]
		public void VerifyFailure()
		{
			Mock<IHelloWorld> helloWorld = Mocks.CreateMock<IHelloWorld>();

			helloWorld.Expects.One.Method(_ => _.Hello());
			helloWorld.Expects.Between(2, 4).Method(_ => _.Ask(null)).With("What color is the fish?")
				.Will(Return.Value("purple"));
			helloWorld.Expects.AtLeast(1).Method(_ => _.Ask(null)).With("How big is the fish?")
				.Will(Throw.Exception(new InvalidOperationException("stop asking about the fish!")));

			helloWorld.MockObject.Hello();
			helloWorld.MockObject.Ask("What color is the fish?");

			Mocks.VerifyAllExpectationsHaveBeenMet();
		}
	}
}