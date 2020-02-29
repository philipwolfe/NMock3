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
	/// <summary>
	/// Tests the <see cref="Is"/> class.
	/// </summary>
	[TestClass]
	public class IsTest
	{
		/// <summary>
		/// Is.TypeOf{T} returns a type matcher on the specified type."/>
		/// </summary>
		[TestMethod]
		public void IsTypeOfGeneric()
		{
			Matcher matcher = Is.TypeOf<Matcher>();

			Assert.IsNotNull(matcher);
			Assert.IsTrue(typeof (TypeMatcher).IsInstanceOfType(matcher));
			Assert.IsTrue(matcher.Matches(matcher));
		}
	}
}