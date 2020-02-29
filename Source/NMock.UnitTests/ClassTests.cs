#region Using
using System;
using System.Linq.Expressions;
using NMock;
using NMock.Syntax;

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
	public class ClassTests
	{
		private readonly MockFactory _factory = new MockFactory();

		public class TesterClass
		{
			public virtual string MyMethod()
			{
				return "Me";
			}

			public override string ToString()
			{
				return base.ToString();
			}
		}

		[TestMethod]
		//[Ignore] //Testing class stuff
		public void ToStringTest()
		{
			Mock<TesterClass> mock = _factory.CreateMock<TesterClass>();

			string expected = "my string";

			mock.Expects.One.Method(_ => _.ToString()).WillReturn(expected);

			string s = mock.MockObject.ToString();

			Assert.AreEqual(expected, s);
		}

		[TestMethod]
		public void VirtualMethodTest()
		{
			Mock<TesterClass> mock = _factory.CreateMock<TesterClass>();

			string expected = "my string";

			mock.Expects.One.Method(_ => _.MyMethod()).WillReturn(expected);

			string s = mock.MockObject.MyMethod();

			Assert.AreEqual(expected, s);
		}
	}

	public class Foo<TInterface>
	{
		public IArgumentSyntax Method(Expression<Action<TInterface>> action)
		{
			throw new NotImplementedException();
		}

		public IAutoArgumentSyntax<TResult> Method<TResult>(Expression<Func<TInterface, TResult>> function)
		{
			throw new NotImplementedException();
		}

		public ICommentSyntax MethodWith(Expression<Action<TInterface>> action)
		{
			throw new NotImplementedException();
		}

		public IAutoActionSyntax<TResult> MethodWith<TResult>(Expression<Func<TInterface, TResult>> function)
		{
			throw new NotImplementedException();
		}

		public ICommentSyntax MethodWith<TProperty>(Expression<Func<TInterface, TProperty>> function, TProperty actualValue)
		{
			throw new NotImplementedException();
		}

		public void test1()
		{
			Foo<Bar> bar = new Foo<Bar>();

			bar.Method(b => b.M1()); //action
			bar.Method(b => b.M2("")); //action
			bar.Method(b => b.M3()); //func
			bar.Method(b => b.M4("")); //func

			bar.MethodWith(b => b.M1()); //action
			bar.MethodWith(b => b.M2("")); //action
			bar.MethodWith(b => b.M3()); //func
			bar.MethodWith(b => b.M4("")); //func

			bar.MethodWith(b => b.M3(), "");
			bar.MethodWith(b => b.M4(""), "");
		}

		#region Nested type: Bar

		private interface Bar
		{
			void M1();
			void M2(string s);
			string M3();
			string M4(string s);
		}

		#endregion
	}
}