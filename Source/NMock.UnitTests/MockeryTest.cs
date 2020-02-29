#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using NMock;
using NMock.AcceptanceTests;
using NMock.Actions;
using NMock.Internal;
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

namespace NMockTests
{
	[TestClass]
	public class MockFactoryTest
	{
		#region Setup/Teardown

		[TestInitialize]
		public void SetUp()
		{
			_mockFactory = new MockFactory();
			_mock = _mockFactory.CreateMock<IMockedType>("mock");

			_expectation1 = new MockExpectation();
			_expectation2 = new MockExpectation();
		}

		[TestCleanup]
		public void TearDown()
		{
			// We're mucking around with changing the default IMockObjectFactory in some tests
			// in this fixture. Here we restore things back to normal after each test.
			_mockFactory.ClearExpectations();//.ChangeDefaultMockObjectFactory(typeof(CastleMockObjectFactory));
		}

		#endregion

		public interface InterfaceWithoutIPrefix
		{
		}

		public interface ARSEIInterfaceWithAdditionalPrefixBeforeI
		{
		}

		public interface INTERFACE_WITH_UPPER_CLASS_NAME
		{
		}

		private MockFactory _mockFactory;
		private Mock<IMockedType> _mock;
		private MockExpectation _expectation1;
		private MockExpectation _expectation2;

		private void AddExpectationsToMockFactory()
		{
			IMockObject mockObjectControl = (IMockObject)_mock.MockObject;
			mockObjectControl.AddExpectation(_expectation1);
			mockObjectControl.AddExpectation(_expectation2);
		}

		// AKR Code provided by Adrian Krummenacher, Roche Rotkreuz
		public interface IMockObjectWithGenericMethod : IMockedType
		{
			T Find<T>();
			T Search<T>(T instance, string Name);
		}

		// AKR Code provided by Adrian Krummenacher, Roche Rotkreuz


		public class TestingMockObjectFactoryWithNoDefaultConstructor : IMockObjectFactory
		{
			public TestingMockObjectFactoryWithNoDefaultConstructor(string someArg)
			{
			}

			#region IMockObjectFactory Members

			public object CreateMock(MockFactory mockFactory, CompositeType mockedTypes, string name, MockStyle mockStyle, object[] constructorArgs)
			{
				return null;
			}

			public string SaveAssembly()
			{
				throw new NotImplementedException();
			}

			#endregion
		}

		public sealed class SampleSealedClass
		{
			public int Add(int a, int b)
			{
				return a + b;
			}
		}

		public class SampleClass
		{
			public int Add(int a, int b)
			{
				return DoAdd(a, b);
			}

			protected virtual int DoAdd(int a, int b)
			{
				return a + b;
			}

			public virtual int Divide(int a, int b, out decimal remainder)
			{
				remainder = a % b;

				return a / b;
			}
		}

		public class SampleClassWithConstructorArgs
		{
			public SampleClassWithConstructorArgs(int i, string s)
			{
			}
		}

		public class SampleClassWithGenericMethods
		{
			public virtual string GetStringValue<T>(T input)
			{
				return input.ToString();
			}

			public virtual int GetCount<T>(T list) where T : IList
			{
				return list.Count;
			}
		}

		[TestMethod]
		public void AssertionExceptionThrownWhenNoExpectationsMatchContainsDescriptionOfActiveExpectations()
		{
			var mockObjectControl = (IMockObject)_mock.MockObject;

			var expectation3 = new MockExpectation();

			_expectation1.Description = "expectation1";
			_expectation1.IsActive = true;
			_expectation1.Matches_Result = false;

			_expectation2.Description = "expectation2";
			_expectation2.IsActive = false;
			_expectation2.Matches_Result = false;

			expectation3.Description = "expectation3";
			expectation3.IsActive = true;
			expectation3.Matches_Result = false;

			mockObjectControl.AddExpectation(_expectation1);
			mockObjectControl.AddExpectation(_expectation2);
			mockObjectControl.AddExpectation(expectation3);

			try
			{
				_mock.MockObject.DoStuff();
			}
			catch (UnexpectedInvocationException e)
			{
				string newLine = Environment.NewLine;

				Assert.AreEqual(
					newLine + "Unexpected invocation of:\r\n  System.Void mock.DoStuff()" + newLine +
					"MockFactory Expectations:" + newLine +
					"  expectation1" + newLine +
					"  expectation2" + newLine +
					"  expectation3" + newLine,
					e.Message);
			}
		}

		[TestMethod]
		public void AssertionExceptionThrownWhenSomeExpectationsHaveNotBeenMetContainsDescriptionOfUnMetExpectations()
		{
			IMockObject mockObjectControl = (IMockObject)_mock.MockObject;

			MockExpectation expectation3 = new MockExpectation();

			_expectation1.Description = "expectation1";
			_expectation1.HasBeenMet = false;
			_expectation1.IsActive = true;
			_expectation2.Description = "expectation2";
			_expectation2.HasBeenMet = true;
			_expectation2.IsActive = true;
			expectation3.Description = "expectation3";
			expectation3.HasBeenMet = false;
			expectation3.IsActive = true;

			mockObjectControl.AddExpectation(_expectation1);
			mockObjectControl.AddExpectation(_expectation2);
			mockObjectControl.AddExpectation(expectation3);

			try
			{
				_mockFactory.VerifyAllExpectationsHaveBeenMet();
			}
			catch (UnmetExpectationException e)
			{
				string newLine = Environment.NewLine;

				Assert.AreEqual(
					"Not all expected invocations were performed." + newLine +
					"MockFactory Expectations:" + newLine +
					"  expectation1" + newLine +
					"  expectation3" + newLine,
					e.Message);
			}
		}

		[TestMethod]
		public void CanCreateClassMockWithConstructor()
		{
			Mock<SampleClassWithConstructorArgs> mock = _mockFactory.CreateMock<SampleClassWithConstructorArgs>(1, "A");
		}

		[TestMethod]
		public void CanCreateMockWithoutCastingBySpecifingTypeAsGenericParameter()
		{
			Mock<IMockedType> mock = _mockFactory.CreateMock<IMockedType>();
			Console.WriteLine("Generic mock create test ran");
		}

		[TestMethod]
		public void CanMockClassWithGenericMethods()
		{
			Mock<SampleClassWithGenericMethods> mock = _mockFactory.CreateMock<SampleClassWithGenericMethods>();
		}

		[TestMethod]
		public void CanMockGenericClass()
		{
			Mock<GenericClass<string>> mock = _mockFactory.CreateMock<GenericClass<string>>();
		}

		[TestMethod, ExpectedException(typeof(ArgumentException))]
		public void ChangingDefaultMockObjectFactoryToClassThatDoesnotImplementIMockObjectFactoryThrowsArgumentException()
		{
			_mockFactory.ChangeDefaultMockObjectFactory(GetType());
		}

		[TestMethod, ExpectedException(typeof(ArgumentException))]
		public void ChangingDefaultMockObjectFactoryToClassWithNoDefaultConstructorThrowsArgumentException()
		{
			_mockFactory.ChangeDefaultMockObjectFactory(typeof(TestingMockObjectFactoryWithNoDefaultConstructor));
		}

		[TestMethod]
		public void ClassMockReturnsDefaultNameFromMockNameProperty()
		{
			IMockObject mock = (IMockObject)_mockFactory.CreateMock<SampleClass>().MockObject;

			Assert.AreEqual("sampleClass", mock.MockName);
		}

		[TestMethod]
		public void ClearExpectation()
		{
			MockFactory testee = new MockFactory();
			Mock<ISelfDescribing> mock = testee.CreateMock<ISelfDescribing>();

			mock.Expects.One.Method(_ => _.DescribeTo(null));

			testee.ClearExpectations();

			testee.VerifyAllExpectationsHaveBeenMet();
		}

		[TestMethod]
		public void CreateMocksWithGenericMethod()
		{
			object mock = _mockFactory.CreateMock<IMockObjectWithGenericMethod>();
		}

		[TestMethod]
		public void CreatedClassMockComparesReferenceIdentityWithEqualsMethod()
		{
			object mock1 = _mockFactory.CreateMock<SampleClass>();
			object mock2 = _mockFactory.CreateMock<SampleClass>();

			Assert.IsTrue(mock1.Equals(mock1), "same object should be equal");
			Assert.IsFalse(mock1.Equals(mock2), "different objects should not be equal");
		}

		[TestMethod]
		public void CreatedMockComparesReferenceIdentityWithEqualsMethod()
		{
			object mock1 = _mockFactory.CreateMock<IMockedType>("mock1");
			object mock2 = _mockFactory.CreateMock<IMockedType>("mock2");

			Assert.IsTrue(mock1.Equals(mock1), "same object should be equal");
			Assert.IsFalse(mock1.Equals(mock2), "different objects should not be equal");
		}

		[TestMethod]
		public void CreatedMockReturnsNameFromToString()
		{
			Mock<IMockedType> mock1 = _mockFactory.CreateMock<IMockedType>("mock1");
			Mock<IMockedType> mock2 = _mockFactory.CreateMock<IMockedType>("mock2");

			Assert.AreEqual("mock1", mock1.MockObject.ToString(), "mock1.ToString()");
			Assert.AreEqual("mock2", mock2.MockObject.ToString(), "mock2.ToString()");
		}

		[TestMethod]
		public void CreatesClassMocksThatCanBeCastToIMockObject()
		{
			Mock<SampleClass> mock = _mockFactory.CreateMock<SampleClass>();

			Assert.IsTrue(mock.MockObject is IMockObject, "should be instance of IMock");
		}

		[TestMethod]
		public void CreatesMocksThatCanBeCastToIMockObject()
		{
			Mock<IMockedType> mock = _mockFactory.CreateMock<IMockedType>();

			Assert.IsTrue(mock.MockObject is IMockObject, "should be instance of IMock");
		}

		[TestMethod]
		public void CreatesMocksThatCanBeCastToMockedType()
		{
			Mock<IMockedType> mock = _mockFactory.CreateMock<IMockedType>();

			Assert.IsTrue(mock.MockObject is IMockedType, "should be instance of mocked type");
		}

		[TestMethod]
		[ExpectedException(typeof(UnmetExpectationException))]
		public void DetectsWhenFirstExpectationHasNotBeenMet()
		{
			AddExpectationsToMockFactory();
			_expectation1.HasBeenMet = false;
			_expectation1.IsValid = true;
			_expectation2.HasBeenMet = true;
			_expectation2.IsValid = true;
			_mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[TestMethod]
		[ExpectedException(typeof(UnmetExpectationException))]
		public void DetectsWhenSecondExpectationHasNotBeenMet()
		{
			AddExpectationsToMockFactory();
			_expectation1.HasBeenMet = true;
			_expectation1.IsValid = true;
			_expectation2.HasBeenMet = false;
			_expectation2.IsValid = true;
			_mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[TestMethod]
		public void DispatchesInvocationBySearchingForMatchingExpectationInOrderOfAddition()
		{
			AddExpectationsToMockFactory();

			_expectation2.Previous = _expectation1;

			_expectation1.ExpectedInvokedObject = _expectation2.ExpectedInvokedObject = _mock.MockObject;
			_expectation1.ExpectedInvokedMethod = _expectation2.ExpectedInvokedMethod =
												  typeof(IMockedType).GetMethod("DoStuff", new Type[0]);
			_expectation1.Matches_Result = false;
			_expectation2.Matches_Result = true;

			_mock.MockObject.DoStuff();

			Assert.IsTrue(_expectation1.Matches_HasBeenCalled, "should have tried to match expectation1");
			Assert.IsFalse(_expectation1.Perform_HasBeenCalled, "should not have performed expectation1");

			Assert.IsTrue(_expectation2.Matches_HasBeenCalled, "should have tried to match expectation2");
			Assert.IsTrue(_expectation2.Perform_HasBeenCalled, "should have performed expectation2");
		}

		[TestMethod, ExpectedException(typeof(UnexpectedInvocationException))]
		public void FailsTestIfNoExpectationsMatch()
		{
			AddExpectationsToMockFactory();
			_expectation1.Matches_Result = false;
			_expectation2.Matches_Result = false;
			_mock.MockObject.DoStuff();
		}

		[TestMethod]
		public void GivesMocksDefaultNameIfNoNameSpecified()
		{
			Assert.AreEqual("mockedType", _mockFactory.CreateMock<IMockedType>().MockObject.ToString());
			Assert.AreEqual("interfaceWithoutIPrefix", _mockFactory.CreateMock<InterfaceWithoutIPrefix>().MockObject.ToString());
			Assert.AreEqual("interfaceWithAdditionalPrefixBeforeI", _mockFactory.CreateMock<ARSEIInterfaceWithAdditionalPrefixBeforeI>().MockObject.ToString());
			Assert.AreEqual("interface_with_upper_class_name", _mockFactory.CreateMock<INTERFACE_WITH_UPPER_CLASS_NAME>().MockObject.ToString());
		}

		[TestMethod]
		public void MockReturnsNameFromMockNameProperty()
		{
			IMockObject mock = (IMockObject)_mockFactory.CreateMock<IMockedType>("mock").MockObject;

			Assert.AreEqual("mock", mock.MockName);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void MockingSealedClassThrowsArgumentException()
		{
			Mock<SampleSealedClass> mock = _mockFactory.CreateMock<SampleSealedClass>();
		}

		[TestMethod]
		public void ShouldBeAbleToInvokeMethodOnInheritedInterface()
		{
			MockFactory mockFactory = new MockFactory();
			Mock<IChildInterface> childMock = mockFactory.CreateMock<IChildInterface>();

			childMock.Expects.AtLeastOne.Method(_ => _.MethodVoid());
			childMock.MockObject.MethodVoid();
			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[TestMethod]
		public void StopsSearchingForMatchingExpectationAsSoonAsOneMatches()
		{
			AddExpectationsToMockFactory();
			_expectation2.Previous = _expectation1;

			_expectation1.ExpectedInvokedObject = _expectation2.ExpectedInvokedObject = _mock.MockObject;
			_expectation1.ExpectedInvokedMethod = _expectation2.ExpectedInvokedMethod =
												  typeof(IMockedType).GetMethod("DoStuff", new Type[0]);
			_expectation1.Matches_Result = true;

			_mock.MockObject.DoStuff();

			Assert.IsTrue(_expectation1.Matches_HasBeenCalled, "should have tried to match expectation1");
			Assert.IsTrue(_expectation1.Perform_HasBeenCalled, "should have performed expectation1");

			Assert.IsFalse(_expectation2.Matches_HasBeenCalled, "should not have tried to match expectation2");
			Assert.IsFalse(_expectation2.Perform_HasBeenCalled, "should not have performed expectation2");
		}

		[TestMethod]
		public void TransparentClassMockWithoutExpectationsExposesRealMethodImplementations()
		{
			Mock<SampleClass> mock = _mockFactory.CreateMock<SampleClass>(MockStyle.Transparent);

			Assert.AreEqual(3, mock.MockObject.Add(1, 2));
		}

		[TestMethod]
		public void VerifiesWhenAllExpectationsHaveBeenMet()
		{
			AddExpectationsToMockFactory();
			_expectation1.HasBeenMet = true;
			_expectation2.HasBeenMet = true;
			_mockFactory.VerifyAllExpectationsHaveBeenMet();
		}
	}

	internal class MockExpectation : IExpectation
	{
		public string Description = "";
		public MethodInfo ExpectedInvokedMethod;
		public object ExpectedInvokedObject;

		public bool Matches_HasBeenCalled;
		public bool Matches_Result;
		public bool Perform_HasBeenCalled;
		public MockExpectation Previous;

		#region IExpectation Members

		/// <summary>
		/// Checks whether stored expectations matches the specified invocation.
		/// </summary>
		/// <param name="invocation">The invocation to check.</param>
		/// <returns>Returns whether one of the stored expectations has met the specified invocation.</returns>
		public bool Matches(Invocation invocation)
		{
			CheckInvocation(invocation);
			Assert.IsTrue(Previous == null || Previous.Matches_HasBeenCalled,
						  "called out of order");
			Matches_HasBeenCalled = true;
			return Matches_Result;
		}

		public bool MatchesIgnoringIsActive(Invocation invocation)
		{
			CheckInvocation(invocation);
			Assert.IsTrue(Previous == null || Previous.Matches_HasBeenCalled,
						  "called out of order");
			Matches_HasBeenCalled = true;
			return Matches_Result;
		}

		public bool Perform(Invocation invocation)
		{
			CheckInvocation(invocation);
			Assert.IsTrue(Matches_HasBeenCalled, "Matches should have been called");
			Perform_HasBeenCalled = true;
			return true;
		}

		public void DescribeActiveExpectationsTo(TextWriter writer)
		{
			writer.WriteLine(Description);
		}

		public void DescribeUnmetExpectationsTo(TextWriter writer)
		{
			writer.WriteLine(Description);
		}

		public void DescribeTo(TextWriter writer)
		{
			writer.WriteLine(Description);
		}

		public void QueryExpectationsBelongingTo(IMockObject mock, IList<IExpectation> result)
		{
		}

		public bool IsActive { get; set; }

		public bool HasBeenMet { get; set; }

		public bool IsValid
		{
			get
			{
				return true;

			}
			set
			{
				//no-op
			}
		}

		public string ValidationErrors()
		{
			return string.Empty;
		}

		#endregion

		private void CheckInvocation(Invocation invocation)
		{
			Assert.IsTrue(ExpectedInvokedObject == null || ExpectedInvokedObject == invocation.Receiver,
						  "should have received invocation on expected object");
			Assert.IsTrue(ExpectedInvokedMethod == null || ExpectedInvokedMethod == invocation.Method,
						  "should have received invocation of expected method");
		}

		#region Implementation of IVerifyableExpectation

		void IVerifyableExpectation.Assert()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}