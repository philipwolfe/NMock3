#region Using

using System;
using System.Collections;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using NMock;
using NMock.AcceptanceTests;
using NMock.Matchers;
using NMockTests.MockTests;
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
	/// Tests for generic method parameters and return values.
	/// <see cref="GenericMethodTypeParamAcceptanceTest"/> for acceptance tests about
	/// generic type parameters.
	/// </summary>
	/// <remarks>
	/// Created on user request for Generic return types.
	/// Request was fed by Adrian Krummenacher on 18-JAN-2008.
	/// </remarks>
	[TestClass]
	public class GenericMethodParameterAcceptanceTest : BasicTestBase
	{
		[TestInitialize]
		public void Init()
		{
			Initalize();
		}

		[TestCleanup]
		public void Clean()
		{
			Cleanup();
		}

		[TestMethod]
		public void CanMockGenericMethod()
		{
			mocks.ForEach(CanMockGenericMethodOnClass);
		}

		private void CanMockGenericMethodOnClass(Mock<IParentInterface> mock)
		{
			Mock<IServiceOne> serviceOneMock = Factory.CreateMock<IServiceOne>();
			Mock<IServiceTwo> serviceTwoMock = Factory.CreateMock<IServiceTwo>();

			// That works only with Expect and if the order of calls to Get match the order of the expectations:
			mock.Expects.One.Method(_ => _.Method<IServiceOne>()).WillReturn(serviceOneMock.MockObject);
			mock.Expects.One.Method(_ => _.Method<IServiceTwo>()).Will(Return.Value(serviceTwoMock.MockObject));

			serviceOneMock.Expects.One.Method(_ => _.ServiceOneGetsName()).Will(Return.Value("ServiceOne"));
			serviceTwoMock.Expects.One.Method(_ => _.ServiceTwoSaves()).Will(Return.Value(true));

			// real call now; only works in same order as the expectations
			IServiceOne serviceOne = mock.MockObject.Method<IServiceOne>();
			string name = serviceOne.ServiceOneGetsName();
			Assert.AreEqual("ServiceOne", name, "Service one returned wrong name.");

			IServiceTwo serviceTwo = mock.MockObject.Method<IServiceTwo>();
			bool res = serviceTwo.ServiceTwoSaves();
			Assert.AreEqual(true, res, "Service two returned wrong boolean value.");
		}

		[TestMethod]
		public void CanMockGenericMethodWithGenericParameter()
		{
			mocks.ForEach(CanMockGenericMethodWithGenericParameter);
		}

		public void CanMockGenericMethodWithGenericParameter(Mock<IParentInterface> mock)
		{
			var version = new Version();

			mock.Expects.One.Method(_ => _.Method(null as Version)).With(version).WillReturn(true);

			var saveResult = mock.MockObject.Method(version);
			Assert.IsTrue(saveResult, "Generic method 'Save' with PersistentClass did not return correct value.");
		}

		[TestMethod]
		public void CanMockGenericMethodWithGenericParameterUsingValueType()
		{
			mocks.ForEach(CanMockGenericMethodWithGenericParameterUsingValueType);
		}

		private void CanMockGenericMethodWithGenericParameterUsingValueType(Mock<IParentInterface> mock)
		{
			mock.Expects.One.Method(_ => _.Method(5m)).With(5m).WillReturn(true);

			var saveResult = mock.MockObject.Method(5m);
			Assert.IsTrue(saveResult, "Generic method 'Save' with decimal value did not return correct value.");
		}

		[TestMethod]
		public void CanMockGenericMethodWithGenericReturnValueUsingMixedTypes()
		{
			mocks.ForEach(CanMockGenericMethodWithGenericReturnValueUsingMixedTypes);
		}

		private void CanMockGenericMethodWithGenericReturnValueUsingMixedTypes(Mock<IParentInterface> mock)
		{
			const int integerValue = 12;
			const string stringValue = "Hello World";

			mock.Expects.One.Method(_ => _.Method<int>()).Will(Return.Value(integerValue));
			mock.Expects.One.Method(_ => _.Method<string>()).Will(Return.Value(stringValue));

			int integerFindResult = mock.MockObject.Method<int>();
			Assert.AreEqual(integerValue, integerFindResult,"Generic method did not return correct Value Type value.");

			string stringFindResult = mock.MockObject.Method<string>();
			Assert.AreEqual(stringValue, stringFindResult,"Generic method did not return correct Reference Type value.");
		}

		[TestMethod]
		public void CanMockGenericMethodWithGenericReturnValueUsingReferenceType()
		{
			mocks.ForEach(CanMockGenericMethodWithGenericReturnValueUsingReferenceType);
		}

		private void CanMockGenericMethodWithGenericReturnValueUsingReferenceType(Mock<IParentInterface> mock)
		{
			const string stringValue = "Hello World";

			mock.Expects.One.Method(_ => _.Method<string>()).Will(Return.Value(stringValue));

			string findResult = mock.MockObject.Method<string>();
			Assert.AreEqual(stringValue, findResult, "Generic method did not return correct Reference Type value.");
		}

		[TestMethod]
		public void CanMockGenericMethodWithGenericReturnValueUsingValueType()
		{
			mocks.ForEach(CanMockGenericMethodWithGenericReturnValueUsingValueType);
		}

		private void CanMockGenericMethodWithGenericReturnValueUsingValueType(Mock<IParentInterface> mock)
		{
			const int integerValue = 12;

			mock.Expects.One.Method(_ => _.Method<int>()).Will(Return.Value(integerValue));

			int findResult = mock.MockObject.Method<int>();
			Assert.AreEqual(integerValue, findResult, "Generic method did not return correct Value Type value.");
		}

		[TestMethod]
		public void CanMockGenericMethodWithMixedParameters()
		{
			mocks.ForEach(CanMockGenericMethodWithMixedParameters);
		}

		private void CanMockGenericMethodWithMixedParameters(Mock<IParentInterface> mock)
		{
			string variableA = "Contents of variable a";
			string variableB = "Contents of variable b";
			mock.Expects.One.Method(_ => _.Set(null, null as string)).With("A", "Contents of variable a");
			mock.Expects.One.MethodWith(_ => _.Set("B", "Contents of variable b"));
			mock.Expects.One.Method(_ => _.Get<string>(null)).With("A").Will(Return.Value(variableA));
			mock.Expects.One.MethodWith(_ => _.Get<string>("B")).Will(Return.Value(variableB));

			mock.MockObject.Set("A", "Contents of variable a");
			string resultA = mock.MockObject.Get<string>("A");
			Assert.AreEqual("Contents of variable a", resultA, "Variable 'A' was not read correctly.");

			mock.MockObject.Set("B", "Contents of variable b");
			string resultB = mock.MockObject.Get<string>("B");
			Assert.AreEqual("Contents of variable b", resultB, "Variable 'B' was not read correctly.");
		}
	}
}