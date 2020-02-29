#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using NMock;
using NMock.AcceptanceTests;
using NMock.Actions;
using NMock.Matchers;
using NMock.Monitoring;
using NMock.Proxy;
using NMock.Proxy.Castle;
using NMockTests._TestStructures;
using Is = NMock.Is;

#if NUNIT
using NUnit.Framework;
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
	public class MockFactoryAcceptanceTest : AcceptanceTestBase
	{
		#region Setup/Teardown

		[TestCleanup]
		public void TearDown()
		{
			// We're mucking around with changing the default IMockObjectFactory in some tests
			// in this fixture. Here we restore things back to normal after each test.
			Mocks.ClearExpectations();//.ChangeDefaultMockObjectFactory(typeof (CastleMockObjectFactory));
		}

		#endregion

		[TestInitialize]
		public void TestClassSetUp()
		{
			SkipVerificationForThisFixture();
		}

		[TestMethod]
		public void CallingVerifyOnMockFactoryShouldEnableMockFactoryToBeUsedSuccessfullyForOtherTests()
		{
			Mock<IMockedType> mockWithUninvokedExpectations =  Mocks.CreateMock<IMockedType>();
			mockWithUninvokedExpectations.Expects.One.Method(_ => _.Method());
			try
			{
				Mocks.VerifyAllExpectationsHaveBeenMet();
				Assert.Fail("Expected ExpectationException to be thrown");
			}
			catch (UnmetExpectationException expected)
			{
				Assert.IsTrue(expected.Message.IndexOf("Not all expected invocations were performed.") != -1);
			}

			Mock<IMockedType> mockWithInvokedExpectations = Mocks.CreateMock<IMockedType>();
			mockWithInvokedExpectations.Expects.One.Method(_ => _.Method());
			mockWithInvokedExpectations.MockObject.Method();
			Mocks.VerifyAllExpectationsHaveBeenMet();
		}

		[TestMethod]
		public void CanMakeMultipleCallsToImplementingWhenCreatingMock()
		{
			Mock<IMockedType> mock = Mocks.CreateMock<IMockedType>(DefinedAs.Implementing<IEnumerable>().Implementing<IDisposable>());

			Assert.IsTrue(typeof(IMockedType).IsInstanceOfType(mock.MockObject));
			Assert.IsTrue(typeof(IEnumerable).IsInstanceOfType(mock.MockObject));
			Assert.IsTrue(typeof(IDisposable).IsInstanceOfType(mock.MockObject));
		}

		[TestMethod]
		public void ChangingDefaultMockObjectFactoryChangesBehaviourOfNewMockFactoryInstances()
		{
			var mocksA = new MockFactory();
			mocksA.ChangeDefaultMockObjectFactory(typeof(TestingMockObjectFactoryA));
			Assert.AreEqual("TestingMockObjectFactoryA", mocksA.CreateMock<INamed>().MockObject.GetName());

			var mocksB = new MockFactory();
			mocksB.ChangeDefaultMockObjectFactory(typeof(TestingMockObjectFactoryB));
			Assert.AreEqual("TestingMockObjectFactoryB", mocksB.CreateMock<INamed>().MockObject.GetName());
		}

		[TestMethod]
		public void MockObjectsMayBePlacedIntoServiceContainers()
		{
			var container = new ServiceContainer();
			var mockedType = Mocks.CreateMock<IMockedType>();

			container.AddService(typeof(IMockedType), mockedType.MockObject);

			Assert.AreSame(mockedType.MockObject, container.GetService(typeof(IMockedType)));
		}

		[TestMethod, ExpectedException(typeof (InvalidOperationException))]
		public void SpecifyingConstructorArgsTwiceWhenCreatingMockThrowsInvalidOperationException()
		{
			Mocks.CreateMock<ParentClass>(DefinedAs.WithArgs("ABC").WithArgs("DEF"));
		}

		[TestMethod, ExpectedException(typeof (InvalidOperationException))]
		public void SpecifyingMockStyleTwiceWhenCreatingMockThrowsInvalidOperationException()
		{
			Mocks.CreateMock<IMockedType>(DefinedAs.OfStyle(MockStyle.Stub).OfStyle(MockStyle.Transparent));
		}

		[TestMethod, ExpectedException(typeof (InvalidOperationException))]
		public void SpecifyingNameTwiceWhenCreatingMockThrowsInvalidOperationException()
		{
			Mocks.CreateMock<IMockedType>(DefinedAs.Named("A").Named("B"));
		}
	}
}