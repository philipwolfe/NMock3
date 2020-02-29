#region Using

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NMock;
using NMock.Matchers;
using NMockTests._TestStructures;

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
	public class ExpectTests
	{
		[TestMethod]
		public void TestMethod1()
		{
			var controller = new Controller();

			Expect.That(controller.ThrowsException).Throws();

			Expect.That(() => controller.ThrowsInvalidOperationException()).Throws<InvalidOperationException>();

			Expect.That(() => controller.ThrowsArgumentNullException(null)).Throws<ArgumentNullException>("Expected an ArgumentNullException that contains the string 'argument'.", new StringContainsMatcher("Parameter name: argument"));

			Expect.That(() => controller.ThrowsArgumentNullException(null)).Throws<ArgumentNullException>("Expected an ArgumentNullException that contains the string 'argument'.", new StringContainsMatcher("ArgumentNullException"), new StringContainsMatcher("Parameter name: argument"));

		}

		private class Controller
		{
			public void ThrowsException()
			{
				throw new Exception();
			}

			public string ThrowsInvalidOperationException()
			{
				throw new InvalidOperationException();
			}

			public void ThrowsArgumentNullException(string argument)
			{
				throw new ArgumentNullException("argument");
			}
		}
	}
}
