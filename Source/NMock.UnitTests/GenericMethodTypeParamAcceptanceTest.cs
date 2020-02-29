#region Using

using System;
using System.Collections;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using NMock;
using NMock.AcceptanceTests;
using NMock.Internal;
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
	/// <summary>
	/// Acceptance tests for generic method type parameters.
	/// <see cref="GenericMethodParameterAcceptanceTest"/> for acceptance tests
	/// about generic method parameters.
	/// </summary>
	[TestClass]
	public class GenericMethodTypeParamAcceptanceTest : AcceptanceTestBase
	{
		private void AssertCanMockGenericMethodWithSpecifiedTypeParameter(Mock<IGenericHelloWorld> genericHelloWorld)
		{
			const int iValue = 3;
			const string sValue = "test";

			genericHelloWorld.Stub.Out.Method(_ => _.Method<int>()).Will(Return.Value(iValue));
			genericHelloWorld.Stub.Out.Method(_ => _.Method<string>()).Will(Return.Value(sValue));
			genericHelloWorld.Stub.Out.Method(_ => _.Method<IHelloWorld, bool>()).Will(Return.Value(Mocks.CreateMock<IHelloWorld>().MockObject));

			string s = genericHelloWorld.MockObject.Method<string>();
			int i = genericHelloWorld.MockObject.Method<int>();
			IHelloWorld helloWorld = genericHelloWorld.MockObject.Method<IHelloWorld, bool>();

			Assert.AreEqual(iValue, i);
			Assert.AreEqual(sValue, s);
			Assert.IsNotNull(helloWorld);
		}

		private void AssertCanMockGenericMethodWithUnspecifiedTypeParameter(Mock<IGenericHelloWorld> helloWorld)
		{
			const int iValue = 3;

			helloWorld.Stub.Out.Method(_ => _.Method<int>()).Will(Return.Value(iValue));

			int i = helloWorld.MockObject.Method<int>();

			Assert.AreEqual(iValue, i);
		}

		private void AssertCanMockGenericMethodWithMultipleTypeParameters(Mock<IGenericHelloWorld> genericHelloWorld)
		{
			genericHelloWorld.Stub.Out.Method(_ => _.Method<int, string>(0)).With(3).Will(Return.Value("three"));

			string s = genericHelloWorld.MockObject.Method<int, string>(3);

			Assert.AreEqual("three", s);
		}


		private void AssertHasCorrectErrorMessageOnUnexpectedInvocation(Mock<IGenericHelloWorld> helloWorld)
		{
			Expect.That(() => helloWorld.MockObject.Method<int, bool>()).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected invocation of:
  System.Int32 {0}.Method<System.Int32, System.Boolean>()
MockFactory Expectations:
  " + ExpectationListBase.NO_EXPECTATIONS + @"
", helloWorld.Name)));

			helloWorld.ClearExpectations();
		}

		private void AssertHasCorrectErrorMessageOnNotMetExpectation(Mock<IGenericHelloWorld> helloWorld)
		{
			SkipVerificationForThisTest();

			helloWorld.Expects.One.Method(_ => _.Method<int>()).Will(Return.Value(3));

			try
			{
				Mocks.VerifyAllExpectationsHaveBeenMet();

				Assert.Fail("An ExpectationException should have been thrown");
			}
			catch (UnmetExpectationException ex)
			{
				Assert.AreEqual("Not all expected invocations were performed.\r\nMockFactory Expectations:\r\n  System.Int32 genericHelloWorld.Method<System.Int32>() will return <3>(System.Int32) [EXPECTED: 1 time CALLED: 0 times]\r\n", ex.Message);
			}
		}

		[TestMethod]
		public void CanMockGenericMethodWithMultipleTypeParametersOnClass()
		{
			AssertCanMockGenericMethodWithMultipleTypeParameters(Mocks.CreateMock<GenericHelloWorld>().As<IGenericHelloWorld>());
		}

		[TestMethod]
		public void CanMockGenericMethodWithMultipleTypeParametersOnInterface()
		{
			AssertCanMockGenericMethodWithMultipleTypeParameters(Mocks.CreateMock<IGenericHelloWorld>());
		}

		[TestMethod]
		public void CanMockGenericMethodWithSpecifiedTypeParameterOnClass()
		{
			AssertCanMockGenericMethodWithSpecifiedTypeParameter(Mocks.CreateMock<GenericHelloWorld>().As<IGenericHelloWorld>());
		}

		[TestMethod]
		public void CanMockGenericMethodWithSpecifiedTypeParameterOnInterface()
		{
			AssertCanMockGenericMethodWithSpecifiedTypeParameter(Mocks.CreateMock<IGenericHelloWorld>());
		}

		[TestMethod]
		public void CanMockGenericMethodWithUnspecifiedTypeParameterOnClass()
		{
			AssertCanMockGenericMethodWithUnspecifiedTypeParameter(Mocks.CreateMock<GenericHelloWorld>().As<IGenericHelloWorld>());
		}

		[TestMethod]
		public void CanMockGenericMethodWithUnspecifiedTypeParameterOnInterface()
		{
			AssertCanMockGenericMethodWithUnspecifiedTypeParameter(Mocks.CreateMock<IGenericHelloWorld>());
		}

		[TestMethod]
		public void HasCorrectErrorMessageOnNotMetExpectationOnClass()
		{
			AssertHasCorrectErrorMessageOnNotMetExpectation(Mocks.CreateMock<GenericHelloWorld>().As<IGenericHelloWorld>());
		}

		[TestMethod]
		public void HasCorrectErrorMessageOnNotMetExpectationOnInterface()
		{
			AssertHasCorrectErrorMessageOnNotMetExpectation(Mocks.CreateMock<IGenericHelloWorld>());
		}

		[TestMethod]
		public void HasCorrectErrorMessageOnUnexpectedInvocationOnClass()
		{
			AssertHasCorrectErrorMessageOnUnexpectedInvocation(Mocks.CreateMock<GenericHelloWorld>().As<IGenericHelloWorld>());
		}

		[TestMethod]
		public void HasCorrectErrorMessageOnUnexpectedInvocationOnInterface()
		{
			AssertHasCorrectErrorMessageOnUnexpectedInvocation(Mocks.CreateMock<IGenericHelloWorld>());
		}
	}
}