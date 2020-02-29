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
	public abstract class BindingTestsBase : BasicTestBase
	{
		[TestMethod]
		public abstract void AddHandlerTest();

		[TestMethod]
		public abstract void RemoveHandlerTest();

		[TestMethod]
		public abstract void AddHandlerAndInvokeTest();

		[TestMethod]
		public abstract void RemoveHandlerAndInvokeTest();

		[TestMethod]
		public abstract void ThrowErrorOnInvokeTest();

		[TestMethod]
		public abstract void ExpectBindingOfSpecificDelegateTest();

		[TestMethod]
		public abstract void MultipleBindingTest();

		[TestMethod]
		public abstract void AddHandlerAnonymousBindingTest();

		[TestMethod]
		public abstract void AddHandlerEmptyDelegateBindingTest();

		[TestMethod]
		public abstract void AddHandlerNullBindingTest();

		[TestMethod]
		public abstract void UnexpectedBindingTest();

		[TestMethod]
		public abstract void InvokedHandlerUnbindsTest();
	}
}