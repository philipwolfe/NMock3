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
	public class MessageGrabberTests
	{
		#region Setup/Teardown

		[TestInitialize]
		public void TestInit()
		{
			_mock = factory.CreateMock<Iam2Bmocked>();
			_driver = new Driver(_mock.MockObject);
		}

		[TestCleanup]
		public void TestClean()
		{
			factory.VerifyAllExpectationsHaveBeenMet();
		}

		#endregion

		private readonly MockFactory factory = new MockFactory();
		private Mock<Iam2Bmocked> _mock;
		private Driver _driver;

		[TestMethod]
		public void ReturnArrayTest()
		{
			_mock.Expects.One.MethodWith(_ => _.ReturnArray("value")).Will(Return.Value(new int[] {}));
			_mock.Expects.One.MethodWith(_ => _.ReturnArray("value")).WillReturn(new int[] {});

			_driver.Go("array", "value");
			_driver.Go("array", "value");

			Assert.IsTrue(true);
		}

		[TestMethod]
		public void ReturnBoolTest()
		{
			_mock.Expects.One.MethodWith(_ => _.ReturnBool("value")).Will(Return.Value(true));
			_mock.Expects.One.MethodWith(_ => _.ReturnBool("value")).WillReturn(true);

			_driver.Go("bool", "value");
			_driver.Go("bool", "value");

			Assert.IsTrue(true);
		}

		[TestMethod]
		public void ReturnEnumTest()
		{
			_mock.Expects.One.MethodWith(_ => _.ReturnEnum("value")).Will(Return.Value(TestValues.Second));
			_mock.Expects.One.MethodWith(_ => _.ReturnEnum("value")).WillReturn(TestValues.Second);

			_driver.Go("enum", "value");
			_driver.Go("enum", "value");

			Assert.IsTrue(true);
		}

		[TestMethod]
		public void ReturnIntTest()
		{
			_mock.Expects.One.MethodWith(_ => _.ReturnInt("value")).Will(Return.Value(3));
			_mock.Expects.One.MethodWith(_ => _.ReturnInt("value")).WillReturn(3);

			_driver.Go("int", "value");
			_driver.Go("int", "value");

			Assert.IsTrue(true);
		}

		[TestMethod]
		public void ReturnInterfaceTest()
		{
			_mock.Expects.One.MethodWith(_ => _.ReturnInterface("value")).Will(Return.Value(new ReturnValue()));
			_mock.Expects.One.MethodWith(_ => _.ReturnInterface("value")).WillReturn(new ReturnValue());

			_driver.Go("interface", "value");
			_driver.Go("interface", "value");

			Assert.IsTrue(true);
		}

		[TestMethod]
		public void ReturnObjectTest()
		{
			_mock.Expects.One.MethodWith(_ => _.ReturnObject("value")).Will(Return.Value(new object()));
			_mock.Expects.One.MethodWith(_ => _.ReturnObject("value")).WillReturn(new object());

			_driver.Go("object", "value");
			_driver.Go("object", "value");

			Assert.IsTrue(true);
		}

		[TestMethod]
		public void ReturnStringTest()
		{
			_mock.Expects.One.MethodWith(_ => _.ReturnString("value")).Will(Return.Value("hello"));
			_mock.Expects.One.MethodWith(_ => _.ReturnString("value")).WillReturn("hello");

			_driver.Go("string", "value");
			_driver.Go("string", "value");

			Assert.IsTrue(true);
		}

		[TestMethod]
		public void ReturnStructTest()
		{
			_mock.Expects.One.MethodWith(_ => _.ReturnStruct("value")).Will(Return.Value(new TestStruct()));
			_mock.Expects.One.MethodWith(_ => _.ReturnStruct("value")).WillReturn(new TestStruct());

			_driver.Go("struct", "value");
			_driver.Go("struct", "value");

			Assert.IsTrue(true);
		}

		[TestMethod]
		public void ReturnVoidTest()
		{
			_mock.Expects.One.MethodWith(_ => _.ReturnVoid("value"));

			_driver.Go("void", "value");

			Assert.IsTrue(true);
		}
	}
}