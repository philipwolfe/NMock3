#region Using

using System;
using System.Data;
using System.Runtime.CompilerServices;
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

namespace NMockTests
{
	[TestClass]
	public class VisibilityAcceptanceTest : AcceptanceTestBase
	{
		// Note: For the internal visibility tests, the assembly containing the internal
		// types must be strong-named and decorated with the InternalsVisibleToAttribute,
		// identifying the DynamicProxy2 runtime-generated assembly (DynamicProxyGenAssembly2).

		// Ideally this fixture should also work with the legacy InterfaceOnlyMockObjectFactory
		// implementation, but unfortunately, one of the two generated assemblies is not
		// strong-named. This prevents us using the InternalsVisibleToAttribute as we have
		// for the Castle implementation. For now we just exclude the affected test cases
		// for this scenario by decorating them with the CastleOnlyAttribute category.

		[TestMethod]
		public void CanMockClassWithInternalMembers()
		{
			Mock<VisibilityTestClass.SomeClassWithInternalMembers> mock = Mocks.CreateMock<VisibilityTestClass.SomeClassWithInternalMembers>();
			mock.Expects.One.Method(_ => _.DoWork());
			mock.MockObject.DoWork();
		}

		[TestMethod]
		public void CanMockClassWithProtectedInternalMembers()
		{
			Mock<VisibilityTestClass.SomeClassWithProtectedInternalMembers> mock = Mocks.CreateMock<VisibilityTestClass.SomeClassWithProtectedInternalMembers>();
			mock.Expects.One.Method(_ => _.DoWork());
			mock.MockObject.DoWork();
		}

		[TestMethod]
		public void CanMockInternalClass()
		{
			Mock<VisibilityTestClass.SomeInternalClass> mock = Mocks.CreateMock<VisibilityTestClass.SomeInternalClass>();
			mock.Expects.One.Method(_ => _.DoWork());
			mock.MockObject.DoWork();
		}

		[TestMethod]
		public void CanMockInternalInterface()
		{
			Mock<VisibilityTestClass.ISomeInternalInterface> mock = Mocks.CreateMock<VisibilityTestClass.ISomeInternalInterface>();
			mock.Expects.One.Method(_ => _.DoWork());
			mock.MockObject.DoWork();
		}

		[TestMethod]
		public void CanMockProtectedInternalClass()
		{
			Mock<VisibilityTestClass.SomeProtectedInternalClass> mock = Mocks.CreateMock<VisibilityTestClass.SomeProtectedInternalClass>();
			mock.Expects.One.Method(_ => _.DoWork());
			mock.MockObject.DoWork();
		}

		[TestMethod]
		public void CanMockProtectedInternalInterface()
		{
			Mock<VisibilityTestClass.ISomeProtectedInternalInterface> mock = Mocks.CreateMock<VisibilityTestClass.ISomeProtectedInternalInterface>();
			mock.Expects.One.Method(_ => _.DoWork());
			mock.MockObject.DoWork();
		}
	}
}