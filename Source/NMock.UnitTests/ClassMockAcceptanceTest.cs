#region Using

using System;
using System.Collections;
using System.Data;
using System.Runtime.CompilerServices;
using NMock;
using NMock.AcceptanceTests;
using NMock.Matchers;
using NMockTests;
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
	public class ClassMockAcceptanceTest : AcceptanceTestBase
	{
		[TestMethod]
		public void CanHaveClassWithIMockObjectMembers()
		{
			Mock<SampleClassWithIMockObjectMembers> mock = Mocks.CreateMock<SampleClassWithIMockObjectMembers>();
			mock.Expects.One.Method(_ => _.Multiply(0, 0)).WithAnyArguments().Will(Return.Value(133));
			int result = mock.MockObject.Multiply(12, 12);
			//// even if 12 by 12 is 144, we mocked the method with 133:
			Assert.AreEqual(133, result, "Mock wasn't created successful.");
		}

		[TestMethod]
		public void CanSetExpectationOnAbstractMethod()
		{
			Mock<SampleClassWithVirtualAndNonVirtualMethods> mock = Mocks.CreateMock<SampleClassWithVirtualAndNonVirtualMethods>();

			// Abstract target method
			mock.Expects.One.Method(_ => _.Add(1.3m, 2.4m)).With(1.1m, 2.1m).Will(Return.Value(10m));
			Assert.AreEqual(10, mock.MockObject.Add(1.1m, 2.1m));

			mock.Expects.One.MethodWith(_ => _.Add(1.1m, 2.1m)).WillReturn(10);
			Assert.AreEqual(10, mock.MockObject.Add(1.1m, 2.1m));
		}

		[TestMethod]
		public void CanSetExpectationOnAbstractMethodOfClassMock()
		{
			Mock<SampleAbstractClass> mock = Mocks.CreateMock<SampleAbstractClass>();

			mock.Expects.One.Method(_ => _.Add(0, 0)).WithAnyArguments().Will(Return.Value(7));

			Assert.AreEqual(7, mock.MockObject.Add(1, 2));
		}

		[TestMethod]
		public void CanSetExpectationOnGenericMethodOnMockedClass()
		{
			Mock<SampleClassWithGenericMethods> mock = Mocks.CreateMock<SampleClassWithGenericMethods>();

			mock.Expects.One.Method(_ => _.GetStringValue("123")).WithAnyArguments().Will(Return.Value("ABC"));

			Assert.AreEqual("ABC", mock.MockObject.GetStringValue("XYZ"));
		}

		[TestMethod]
		public void CanSetExpectationOnGenericMethodWithConstraintOnMockedClass()
		{
			Mock<SampleClassWithGenericMethods> mock = Mocks.CreateMock<SampleClassWithGenericMethods>();

			mock.Expects.One.Method(_ => _.GetCount(new[] { 0 })).WithAnyArguments().Will(Return.Value(3));

			Assert.AreEqual(3, mock.MockObject.GetCount(new int[5]));
		}

		[TestMethod]
		public void CanSetExpectationOnMethodOnMockedGenericClass()
		{
			Mock<GenericClass<string>> mock = Mocks.CreateMock<GenericClass<string>>();

			mock.Expects.One.Method(_ => _.GetT()).Will(Return.Value("ABC"));

			Assert.AreEqual("ABC", mock.MockObject.GetT());
		}

		[TestMethod]
		public void CanSetExpectationOnMethodWhenAtLeastOneMatchedOverloadIsVirtualOrAbstract()
		{
			Mock<SampleClassWithVirtualAndNonVirtualMethods> mock = Mocks.CreateMock<SampleClassWithVirtualAndNonVirtualMethods>();

			// Three possible matches here...
			mock.Expects.One.Method(_ => _.Add(0, 0)).WithAnyArguments().Will(Return.Value(10));
			mock.Expects.One.Method(_ => _.Add(0m, 0m)).WithAnyArguments().Will(Return.Value(10m));
			Assert.AreEqual(10, mock.MockObject.Add(1, 2), "Virtual method expectation failed");
			Assert.AreEqual(10, mock.MockObject.Add(1.1m, 2.1m), "Abstract method expectation failed");
			Assert.AreEqual(6, mock.MockObject.Add(1, 2, 3), "Expected call to non-virtual method to go to implementation");
		}

		[TestMethod]
		public void CanSetExpectationOnMethodWithOutParameterOnMockedClass()
		{
			Mock<ParentClass> mock = Mocks.CreateMock<ParentClass>();

			decimal d;
			mock.Expects.One.Method(_ => _.Divide(0, 0, out d)).WithAnyArguments().Will(Return.Value(3), Return.OutValue("remainder", 15m));

			decimal remainder;
			mock.MockObject.Divide(7, 2, out remainder);

			Assert.AreEqual(15m, remainder);
		}

		[TestMethod]
		public void CanSetExpectationOnOverriddenObjectMembersOnClassMock()
		{
			Mock<SampleClassWithObjectOverrides> mock = Mocks.CreateMock<SampleClassWithObjectOverrides>();

			mock.Expects.One.Method(_ => _.ToString()).Will(Return.Value("ABC"));
			mock.Expects.One.Method(_ => _.Equals(null)).WithAnyArguments().Will(Return.Value(false));
			mock.Expects.One.Method(_ => _.GetHashCode()).Will(Return.Value(17));

			Assert.AreEqual("ABC", mock.MockObject.ToString(), "unexpected ToString() value");
			Assert.IsFalse(mock.MockObject.Equals(mock), "unexpected Equals() value");
			Assert.AreEqual(17, mock.MockObject.GetHashCode(), "unexpected GetHashCode() value");
		}

		[TestMethod]
		public void CanSetExpectationOnVirtualMethod()
		{
			Mock<SampleClassWithVirtualAndNonVirtualMethods> mock = Mocks.CreateMock<SampleClassWithVirtualAndNonVirtualMethods>();

			// Virtual target method
			mock.Expects.One.Method(_ => _.Add(0, 0)).With(1, 2).Will(Return.Value(10));
			Assert.AreEqual(10, mock.MockObject.Add(1, 2));
		}


		[TestMethod]
		public void SettingExpectationOnInheritedNonVirtualImplOfInterfaceMethodThrowsArgumentException()
		{
			Mock<SampleInterfaceImpl> mock = Mocks.CreateMock<SampleInterfaceImpl>("myMock");
			mock.Expects.No.Method(_ => _.SomeOtherMethod());
		}

		[TestMethod, ExpectedException(typeof(ArgumentException))]
		public void SettingExpectationOnNonVirtualImplOfInterfaceGenericMethodThrowsArgumentException()
		{
			Mock<SampleInterfaceImpl> mock = Mocks.CreateMock<SampleInterfaceImpl>("myMock");

			mock.Expects.No.Method(_ => _.SomeGenericMethod(1)).With(new ArgumentsMatcher(new TypeMatcher(typeof(int))));
		}

		[TestMethod]
		public void SettingExpectationOnNonVirtualImplOfInterfaceMethodThrowsArgumentException()
		{
			Mock<SampleInterfaceImpl> mock = Mocks.CreateMock<SampleInterfaceImpl>("myMock");

			mock.Expects.No.Method(_ => _.SomeMethod());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void SettingExpectationOnNonVirtualMethodThrowsArgumentException()
		{
			Mock<SampleClassWithVirtualAndNonVirtualMethods> mock = Mocks.CreateMock<SampleClassWithVirtualAndNonVirtualMethods>("myMock");

			// The target method is non-virtual, so we expect an exception.
			// (we use 'Never' here to avoid an undesired failure in teardown).
			mock.Expects.No.Method(_ => _.Subtract(0, 0)).With(2, 1).Will(Return.Value(10));
		}

		// This is a problem, as we can't easily use args to identify overload
		// as expectation is defined, and we can't do it at execution time, because
		// call will not be intercepted (proxy is only for virtual members).
		[TestMethod]
		public void SettingExpectationOnNonVirtualOverloadOfVirtualMethodThrowsArgumentException()
		{
			Mock<SampleClassWithVirtualAndNonVirtualMethods> mock = Mocks.CreateMock<SampleClassWithVirtualAndNonVirtualMethods>();

			Expect.That(() => mock.Expects.One.MethodWith(_ => _.Add(1, 2, 3)).WillReturn(10)).Throws<ArgumentException>();

			Mocks.ClearExpectations();
		}
	}
}