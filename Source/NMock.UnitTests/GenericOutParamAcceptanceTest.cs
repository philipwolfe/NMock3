#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using NMock;
using NMock.AcceptanceTests;
using NMock.Actions;
using NMock.Matchers;
using NMockTests._TestStructures;
using Is = NMock.Is;

#if NUNIT
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
	public interface IGenericOutParamInterface
	{
		bool SomeMethod(string arg_in, out List<string> vals_out);
	}

	public abstract class OutParamClass : IGenericOutParamInterface
	{
		#region IGenericOutParamInterface Members

		public abstract bool SomeMethod(string arg_in, out List<string> vals_out);

		#endregion
	}


	[TestClass]
	public class GenericOutParamAcceptanceTest : AcceptanceTestBase
	{
		private void AssertCanMockMethodWithOutParam(Mock<IGenericOutParamInterface> someClass)
		{
			List<string> outVar;
			someClass.Expects.One.Method(_ => _.SomeMethod(null, out outVar)).WithAnyArguments().Will(Return.Value(true));

			List<string> myList = new List<string>();
			someClass.MockObject.SomeMethod("test", out myList);
		}

		private void AssertCanMockMethodWithOutParamWithIsAnything(Mock<IGenericOutParamInterface> someClass)
		{
			List<string> myList = new List<string>();

			someClass.Expects.One.Method(_ => _.SomeMethod(null, out myList)).With(Is.Anything, Is.Out).Will(
				Return.Value(false),
				new SetNamedParameterAction("vals_out", myList)
				);

			bool ret = someClass.MockObject.SomeMethod("test", out myList);
		}

		private void AssertCanMockMethodWithOutParamWithDefinedValue(Mock<IGenericOutParamInterface> someClass)
		{
			List<string> myList = new List<string>();

			someClass.Expects.One.Method(_ => _.SomeMethod(null, out myList)).With("test", Is.Out).Will(
				Return.Value(false),
				new SetNamedParameterAction("vals_out", myList)
				);

			bool ret = someClass.MockObject.SomeMethod("test", out myList);
		}

		[TestMethod]
		public void CanMockMethodWithOutParamOnClass()
		{
			AssertCanMockMethodWithOutParam(Mocks.CreateMock<OutParamClass>().As<IGenericOutParamInterface>());
		}

		[TestMethod]
		public void CanMockMethodWithOutParamOnInterface()
		{
			// InterfaceOnlyMockObjectFactory-generated mocks do not currently support this
			// - They expect out params to be explicitly set

			AssertCanMockMethodWithOutParam(Mocks.CreateMock<IGenericOutParamInterface>());
		}

		[TestMethod]
		public void CanMockMethodWithOutParamWithDefinedValueOnClass()
		{
			AssertCanMockMethodWithOutParamWithDefinedValue(Mocks.CreateMock<OutParamClass>().As<IGenericOutParamInterface>());
		}

		[TestMethod]
		public void CanMockMethodWithOutParamWithDefinedValueOnInterface()
		{
			AssertCanMockMethodWithOutParamWithDefinedValue(Mocks.CreateMock<IGenericOutParamInterface>());
		}

		[TestMethod]
		public void CanMockMethodWithOutParamWithIsAnythingOnClass()
		{
			AssertCanMockMethodWithOutParamWithIsAnything(Mocks.CreateMock<OutParamClass>().As<IGenericOutParamInterface>());
		}

		[TestMethod]
		public void CanMockMethodWithOutParamWithIsAnythingOnInterface()
		{
			AssertCanMockMethodWithOutParamWithIsAnything(Mocks.CreateMock<IGenericOutParamInterface>());
		}
	}
}