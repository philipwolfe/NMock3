#region Using

using System;
using System.Collections.Generic;
using System.Diagnostics;
using NMock;
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

#if NetFx35
namespace NMockTests.MockTests.ExpectsTests.IMethodSyntaxTests35
#else
#if SILVERLIGHT
namespace NMockTests.MockTests.ExpectsTests.IMethodSyntaxTestsSL
#else
namespace NMockTests.MockTests.ExpectsTests.IMethodSyntaxTests
#endif
#endif
{
	[TestClass]
	public class ProtectedMethodTests : BasicTestBase
	{
		[TestMethod]
		public void CanCallNonMockedMethodOfTransparentClassMock()
		{
			var mock = Factory.CreateMock<ParentClass>(MockStyle.Transparent);
			mock.Expects.One.ProtectedMethod("DoAdd").WithAnyArguments().Will(Return.Value(7));
			Assert.AreEqual(7, mock.MockObject.Add(1, 2));

			// Call to non-mocked method "Divide"
			decimal remainder;
			decimal divideResult = mock.MockObject.Divide(10, 2, out remainder);
			Assert.AreEqual(5m, divideResult, "Expected 5 as result of calling Divide (10/2).");
		}

		[TestMethod]
		public void CanSetExpectationOnMethodOfTransparentClassMock()
		{
			var mock = Factory.CreateMock<ParentClass>(MockStyle.Transparent);

			mock.Expects.One.ProtectedMethod("DoAdd").WithAnyArguments().Will(Return.Value(7));

			Assert.AreEqual(7, mock.MockObject.Add(1, 2));
		}

		[TestMethod]
		public void CanSetExpectationOnMethodOnSuperClassOfMockedClass()
		{
			var mock = Factory.CreateMock<ChildClass>(DefinedAs.Named("classMock").WithArgs("Phil"));

			mock.Expects.One.ProtectedMethod("DoAdd").WithAnyArguments().Will(Return.Value(5));

			Assert.AreEqual(10, mock.MockObject.AddThenDouble(1, 2));
		}

		[TestMethod]
		public void LocalCallsWithinMockedClassCanSatisfyExpectations()
		{
			Mock<ParentClass> mock = Factory.CreateMock<ParentClass>();

			mock.Expects.One.ProtectedMethod("DoAdd").WithAnyArguments().Will(Return.Value(7)); // Should be called from Add()

			Assert.AreEqual(7, mock.MockObject.Add(1, 2));
		}


	}
}
