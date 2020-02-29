#region Using

using System;
using System.Data;
using NMock;
using NMock.Matchers;
using NMockTests._TestStructures;

#if NUNIT
using NUnit.Framework;
using Is = NMock.Is;
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
	public class MethodTests
	{
		#region Setup/Teardown

		[TestInitialize]
		public void TestInit()
		{
			_mock = _factory.CreateMock<IParentInterface>();
		}

		[TestCleanup]
		public void TestClean()
		{
			_factory.VerifyAllExpectationsHaveBeenMet();
		}

		#endregion

		private readonly MockFactory _factory;
		private Mock<IParentInterface> _mock;

		public MethodTests()
		{
			_factory = new MockFactory();
		}

		[TestMethod]
		[ExpectedException(typeof (IncompleteExpectationException))]
		public void MissingReturnTest()
		{
			_mock.Expects.One.Method(_ => _.Method<string>());

			string s = _mock.MockObject.Method<string>();

			Assert.AreEqual(string.Empty, s);

			//_factory.SuppressUnexpectedAndUnmetExpectations();
		}

		[TestMethod]
		public void StringMethodWiths()
		{
			Outer outer = new Outer();
			outer.Inner = new Inner();
			outer.Inner.Prop = string.Empty;

			_mock.Expects.One.MethodWith(_ => _.Method<string>(), "1014").Comment("");
			_mock.Expects.One.MethodWith(_ => _.Get<string>("140"), "1015").Comment("");
			_mock.Expects.One.MethodWith(_ => _.MethodVoid<string>(string.Empty));
			_mock.Expects.One.MethodWith(_ => _.MethodVoid<string>(outer.Inner.Prop));
			outer.Inner.Prop = "testing";
			_mock.Expects.One.MethodWith(_ => _.MethodVoid<string>(outer.Inner.Prop));
			_mock.Expects.One.MethodWith(_ => _.MethodVoid<string>(Outer.CONST));
			_mock.Expects.One.MethodWith(_ => _.MethodVoid<string>(outer.Inner.CONST));
			_mock.Expects.One.MethodWith(_ => _.MethodVoid<string>(outer.Inner.GetString()));
			_mock.Expects.One.MethodWith(_ => _.MethodVoid<string>(new Outer {Inner = new Inner {Prop = "dynamic"}}.Inner.Prop));
			//_mock.Expects.One.MethodWith(_ => _.MethodVoid<string>(() => { return string.Empty; }));

			Assert.AreEqual("1014", _mock.MockObject.Method<string>());
			Assert.AreEqual("1015", _mock.MockObject.Get<string>("140"));
			_mock.MockObject.MethodVoid<string>(string.Empty);
			_mock.MockObject.MethodVoid<string>(string.Empty);
			_mock.MockObject.MethodVoid<string>("testing");
			_mock.MockObject.MethodVoid<string>("Constant");
			_mock.MockObject.MethodVoid<string>("Constant1");
			_mock.MockObject.MethodVoid<string>("Method");
			_mock.MockObject.MethodVoid<string>("dynamic");
		}

		[TestMethod]
		public void StringMethodWithsWill()
		{
			_mock.Expects.One.MethodWith(_ => _.Method<string>()).Will(Return.Value("1010")).Comment("");
			_mock.Expects.One.MethodWith(_ => _.Get<string>("120")).Will(Return.Value("1011")).Comment("");

			Assert.AreEqual("1010", _mock.MockObject.Method<string>());
			Assert.AreEqual("1011", _mock.MockObject.Get<string>("120"));
		}

		[TestMethod]
		public void StringMethodWithsWillReturn()
		{
			_mock.Expects.One.MethodWith(_ => _.Method<string>()).WillReturn("1012").Comment("");
			_mock.Expects.One.MethodWith(_ => _.Get<string>("130")).WillReturn("1013").Comment("");

			Assert.AreEqual("1012", _mock.MockObject.Method<string>());
			Assert.AreEqual("1013", _mock.MockObject.Get<string>("130"));
		}

		[TestMethod]
		public void StringMethodsMatchingWillReturn()
		{
			//_mock.Expects.One.Method(_ => _.Method<string>()).Matching().WillReturn("1002").Comment("");
			_mock.Expects.One.Method(_ => _.Get<string>("80")).With(new TypeMatcher(typeof (string))).WillReturn("1003").Comment("");

			//Assert.AreEqual("1002", _mock.MockObject.Method<string>());
			Assert.AreEqual("1003", _mock.MockObject.Get<string>("80"));
		}

		[TestMethod]
		public void StringMethodsWillReturn()
		{
			_mock.Expects.One.Method(_ => _.Method<string>()).WillReturn("1000").Comment("");
			_mock.Expects.One.Method(_ => _.Get<string>("70")).WithAnyArguments().WillReturn("1001").Comment("");

			Assert.AreEqual("1000", _mock.MockObject.Method<string>());
			Assert.AreEqual("1001", _mock.MockObject.Get<string>("70"));
		}

		[TestMethod]
		public void StringMethodsWithAnyWillReturn()
		{
			_mock.Expects.One.Method(_ => _.Method<string>()).WillReturn("1004").Comment("");
			_mock.Expects.One.Method(_ => _.Get<string>("90")).WithAnyArguments().WillReturn("1005").Comment("");

			Assert.AreEqual("1004", _mock.MockObject.Method<string>());
			Assert.AreEqual("1005", _mock.MockObject.Get<string>("90"));
		}

		[TestMethod]
		public void StringMethodsWithNoWillReturn()
		{
			_mock.Expects.One.Method(_ => _.Method<string>()).WillReturn("1008").Comment("");
			//_mock.Expects.One.Method(_ => _.Get<string>("110")).WithNoArguments().WillReturn("1009").Comment("");

			Assert.AreEqual("1008", _mock.MockObject.Method<string>());
			//Assert.AreEqual("1009", _mock.MockObject.Get<string>("110"));
		}

		[TestMethod]
		public void StringMethodsWithWillReturn()
		{
			_mock.Expects.One.Method(_ => _.Method<string>()).WillReturn("1006").Comment("");
			_mock.Expects.One.Method(_ => _.Get<string>("100")).With("100").WillReturn("1007").Comment("");

			Assert.AreEqual("1006", _mock.MockObject.Method<string>());
			Assert.AreEqual("1007", _mock.MockObject.Get<string>("100"));
		}

		[TestMethod]
		public void VoidMethodWiths()
		{
			_mock.Expects.One.MethodWith(_ => _.MethodVoid()).Comment("");
			_mock.Expects.One.MethodWith(_ => _.MethodVoid<string>("60")).Comment("");

			_mock.MockObject.MethodVoid();
			_mock.MockObject.MethodVoid<string>("60");
		}

		[TestMethod]
		public void VoidMethods()
		{
			_mock.Expects.One.Method(_ => _.MethodVoid()).Comment("");
			_mock.Expects.One.Method(_ => _.MethodVoid<string>("10")).With(Is.StringContaining("10")).Comment("");

			_mock.MockObject.MethodVoid();
			_mock.MockObject.MethodVoid<string>("10");
		}

		[TestMethod]
		[ExpectedException(typeof (Exception))]
		public void VoidMethodsWill1()
		{
			_mock.Expects.One.Method(_ => _.MethodVoid()).Will(Throw.Exception(new Exception()));

			_mock.MockObject.MethodVoid();

			//_factory.SuppressUnexpectedAndUnmetExpectations();
		}

		[TestMethod]
		[ExpectedException(typeof (Exception))]
		public void VoidMethodsWill2()
		{
			_mock.Expects.One.Method(_ => _.MethodVoid<string>("55")).WithAnyArguments().Will(Throw.Exception(new Exception()));

			_mock.MockObject.MethodVoid<string>("55");
			//_factory.SuppressUnexpectedAndUnmetExpectations();

		}

		[TestMethod]
		public void VoidMethodsWithMatchers()
		{
			//_mock.Expects.One.Method(_ => _.MethodVoid()).Matching().Comment("");
			_mock.Expects.One.Method(_ => _.MethodVoid<string>("20")).With(new TypeMatcher(typeof (string))).Comment("");

			//_mock.MockObject.MethodVoid();
			_mock.MockObject.MethodVoid<string>("20");
		}

		[TestMethod]
		public void VoidMethodsWithWith()
		{
			//_mock.Expects.One.Method(_ => _.MethodVoid()).With().Comment("");
			_mock.Expects.One.Method(_ => _.MethodVoid<string>("30")).With("30").Comment("");

			//_mock.MockObject.MethodVoid();
			_mock.MockObject.MethodVoid<string>("30");
		}

		[TestMethod]
		public void VoidMethodsWithWithAny()
		{
			_mock.Expects.One.Method(_ => _.MethodVoid()).Comment("");
			_mock.Expects.One.Method(_ => _.MethodVoid<string>("40")).WithAnyArguments().Comment("");

			_mock.MockObject.MethodVoid();
			_mock.MockObject.MethodVoid<string>("40");
		}

		[TestMethod]
		public void VoidMethodsWithWithNo()
		{
			_mock.Expects.One.Method(_ => _.MethodVoid()).Comment("");
			//_mock.Expects.One.Method(_ => _.MethodVoid<string>("50")).WithNoArguments().Comment("");

			_mock.MockObject.MethodVoid();
			//_mock.MockObject.MethodVoid<string>("50");
		}
	}
}