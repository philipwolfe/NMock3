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

namespace NMockTests
{
	[TestClass]
	public class VerifyTest
	{
		[TestMethod]
		public void CanPrependCustomMessageToDescriptionOfFailure()
		{
			object expected = new NamedObject("expected");
			object actual = new NamedObject("actual");
			Matcher matcher = Is.EqualTo(expected);

			try
			{
				Verify.That(actual, matcher, "message {0} {1}", "0", 1);
			}
			catch (ExpectationException e)
			{
				Assert.AreEqual(
					"message 0 1" + Environment.NewLine +
					"Expected: " + matcher + Environment.NewLine +
					"Actual:   <" + actual + ">(NMockTests.NamedObject)",
					e.Message);
				return;
			}

			Assert.Fail("Verify.That should have failed");
		}

		[TestMethod]
		public void VerifyThatFailsIfMatcherDoesNotMatchValue()
		{
			object expected = new NamedObject("expected");
			object actual = new NamedObject("actual");
			Matcher matcher = Is.EqualTo(expected);

			try
			{
				Verify.That(actual, matcher);
			}
			catch (ExpectationException e)
			{
				Assert.AreEqual(
					Environment.NewLine +
					"Expected: " + matcher + Environment.NewLine +
					"Actual:   <" + actual + ">(NMockTests.NamedObject)",
					e.Message);
				return;
			}

			Assert.Fail("Verify.That should have failed");
		}

		[TestMethod]
		public void VerifyThatPassesIfMatcherMatchesValue()
		{
			object value = new NamedObject("value");

			Verify.That(value, Is.EqualTo(value));
		}
	}
}