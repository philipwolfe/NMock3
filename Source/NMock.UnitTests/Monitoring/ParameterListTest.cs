#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using NMock;
using NMock.AcceptanceTests;
using NMock.Actions;
using NMock.Matchers;
using NMock.Monitoring;
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

namespace NMockTests.Monitoring
{
	[TestClass]
	public class ParameterListTest
	{
		#region Setup/Teardown

		[TestInitialize]
		public void SetUp()
		{
			ParameterInfo inParam = new ParameterInfoStub("inParam", ParameterAttributes.In);
			ParameterInfo refParam = new ParameterInfoStub("refParam", ParameterAttributes.None);
			ParameterInfo outParam = new ParameterInfoStub("outParam", ParameterAttributes.Out);

			method = new MethodInfoStub("method", inParam, refParam, outParam);

			parameterValues = new[] {inValue, refValue, null};

			list = new ParameterList(method, parameterValues);
		}

		#endregion

		private static readonly object inValue = "inValue";
		private static readonly object refValue = "refValue";

		private const int IN_PARAMETER_INDEX = 0;
		private const int REF_PARAMETER_INDEX = 1;
		private const int OUT_PARAMETER_INDEX = 2;

		private MethodInfo method;
		private object[] parameterValues;
		private ParameterList list;

		private void Ignore(object o)
		{
			// The things you have to do to ignore compiler warnings!
			object o2 = o;
			o = o2;
		}

		[TestMethod]
		public void CanSetValuesOfOutAndRefParameters()
		{
			object newRefValue = "newRefValue";
			object outValue = "outValue";

			list[REF_PARAMETER_INDEX] = newRefValue;
			list[OUT_PARAMETER_INDEX] = outValue;

			Assert.AreSame(newRefValue, list[REF_PARAMETER_INDEX], "new ref value");
			Assert.IsTrue(list.IsValueSet(OUT_PARAMETER_INDEX), "out parameter is set");
			Assert.AreSame(outValue, list[OUT_PARAMETER_INDEX], "out value");
		}

		[TestMethod, ExpectedException(typeof (InvalidOperationException))]
		public void DoesNotAllowAccessToValuesOfUnsetOutParameters()
		{
			Assert.IsFalse(list.IsValueSet(OUT_PARAMETER_INDEX), "out parameter is not set");
			Ignore(list[OUT_PARAMETER_INDEX]);
		}

		[TestMethod, ExpectedException(typeof (InvalidOperationException))]
		public void DoesNotAllowValuesOfInputParametersToBeChanged()
		{
			object newValue = "newValue";

			list[IN_PARAMETER_INDEX] = newValue;
		}

		[TestMethod]
		public void ReturnsNumberOfParameters()
		{
			Assert.AreEqual(parameterValues.Length, list.Count, "size");
		}

		[TestMethod]
		public void ReturnsValuesOfInParameters()
		{
			Assert.IsTrue(list.IsValueSet(IN_PARAMETER_INDEX), "in parameter should be set");
			Assert.AreSame(inValue, list[IN_PARAMETER_INDEX], "in value");
			Assert.IsTrue(list.IsValueSet(REF_PARAMETER_INDEX), "ref parameter should be set");
			Assert.AreSame(refValue, list[REF_PARAMETER_INDEX], "ref value");
		}
	}
}