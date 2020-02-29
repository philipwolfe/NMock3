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

namespace NMock.Internal.Test
{
	[TestClass]
	public class DescriptionWriterTest
	{
		[TestMethod]
		public void FormatsNullAsNull()
		{
			DescriptionWriter writer = new DescriptionWriter();
			writer.Write((object) null);
			Assert.AreEqual("null", writer.ToString());
		}

		[TestMethod]
		public void FormatsStringsInCSharpSyntaxWhenWrittenAsObject()
		{
			DescriptionWriter writer = new DescriptionWriter();
			object aString = "hello \"world\"";
			writer.Write(aString);

			Assert.AreEqual("\"hello \\\"world\\\"\"", writer.ToString());
		}

		[TestMethod]
		public void HighlightsOtherValuesWhenWrittenAsObject()
		{
			DescriptionWriter writer = new DescriptionWriter();
			object anInt = 1;
			writer.Write(anInt);

			Assert.AreEqual("<1>(System.Int32)", writer.ToString());
		}
	}
}