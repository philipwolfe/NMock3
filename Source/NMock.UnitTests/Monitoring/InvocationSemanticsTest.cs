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

namespace NMockTests.Monitoring
{
	[TestClass]
	public class InvocationSemanticsTest
	{
		private const int REF_PARAM_VALUE = 1;
		private const int OUT_PARAM_VALUE = 2;

		public void SetAndThrow(ref int refParam, out int outParam)
		{
			refParam = REF_PARAM_VALUE;
			outParam = OUT_PARAM_VALUE;
			throw new TestException();
		}

		[TestMethod]
		public void OutParametersAreSetAfterExceptionThrown()
		{
			int refParam = 0;
			int outParam = 0;

			try
			{
				SetAndThrow(ref refParam, out outParam);
			}
			catch (TestException)
			{
			}

			Assert.AreEqual(REF_PARAM_VALUE, refParam);
			Assert.AreEqual(OUT_PARAM_VALUE, outParam);
		}
	}

	internal class TestException : Exception
	{
	}
}