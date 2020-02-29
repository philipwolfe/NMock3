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

namespace NMockTests
{
	[TestClass]
	public class MockMethodsFromObjectTest
	{
		#region Setup/Teardown

		[TestInitialize]
		public void SetUp()
		{
			mockFactory = new MockFactory();
		}

		[TestCleanup]
		public void TearDown()
		{
			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		#endregion

		private MockFactory mockFactory;

		public class TestClass1
		{
		}

		public interface ITestInterface
		{
			string ToString();
		}

		[TestMethod]
		[Ignore]
		public void MockToStringOnClass()
		{
			Mock<TestClass1> mock = mockFactory.CreateMock<TestClass1>();

			mock.Expects.One.Method(_ => _.ToString()).Will(Return.Value("whatYouWant"));

			string s = mock.MockObject.ToString();
			Assert.AreEqual("whatYouWant", s);
		}

		[TestMethod]
		public void MockToStringOnInterface()
		{
			Mock<ITestInterface> mock = mockFactory.CreateMock<ITestInterface>();

			mock.Expects.One.Method(_ => _.ToString()).Will(Return.Value("whatYouWant"));

			string s = mock.MockObject.ToString();
			Assert.AreEqual("whatYouWant", s);
		}
	}
}