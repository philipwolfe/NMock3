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
	public interface IAdder
	{
		void Add(int a, int b, out int c);
		int Add(int a, int b);
	}

	public abstract class Adder : IAdder
	{
		#region IAdder Members

		public abstract void Add(int a, int b, out int c);
		public abstract int Add(int a, int b);

		#endregion
	}

	[TestClass]
	public class OutParamAcceptanceTest : AcceptanceTestBase
	{
		private void AssertCanMockInAndOutParamOnMethod(Mock<IAdder> adder)
		{
			int i;
			adder.Expects.One.Method(_ => _.Add(0, 0, out i)).With(3, 5, Is.Out).Will(
				new SetNamedParameterAction("c", 8)
				);

			int outValue;
			adder.MockObject.Add(3, 5, out outValue);
			Assert.AreEqual(8, outValue, "Outvalue was not set correctly.");
		}

		private void AssertCanMockInAndOutParamWithReturnValueOnMethod(Mock<IAdder> adder)
		{
			adder.Expects.One.Method(_ => _.Add(0, 0)).With(4, 7).Will(Return.Value(11));

			int c = adder.MockObject.Add(4, 7);
			Assert.AreEqual(11, c, "Result was not set correctly.");
		}

		[TestMethod]
		public void CanMockInAndOutParamOnClassMethod()
		{
			AssertCanMockInAndOutParamOnMethod(Mocks.CreateMock<Adder>().As<IAdder>());
		}

		[TestMethod]
		public void CanMockInAndOutParamOnInterfaceMethod()
		{
			AssertCanMockInAndOutParamOnMethod(Mocks.CreateMock<IAdder>());
		}

		[TestMethod]
		public void CanMockInAndOutParamWithReturnValueOnClassMethod()
		{
			AssertCanMockInAndOutParamWithReturnValueOnMethod(Mocks.CreateMock<Adder>().As<IAdder>());
		}

		[TestMethod]
		public void CanMockInAndOutParamWithReturnValueOnInterfaceMethod()
		{
			AssertCanMockInAndOutParamWithReturnValueOnMethod(Mocks.CreateMock<IAdder>());
		}

		[TestMethod]
		public void CanMockOutParameterUsingShortcutOnReturnClass()
		{
			Mock<IAdder> adder = Mocks.CreateMock<IAdder>();

			int i;
			adder.Expects.One.Method(_ => _.Add(0, 0, out i)).With(3, 5, Is.Out).Will(Return.OutValue("c", 8));

			int outValue;
			adder.MockObject.Add(3, 5, out outValue);
			Assert.AreEqual(8, outValue, "Outvalue was not set correctly.");
		}
	}
}