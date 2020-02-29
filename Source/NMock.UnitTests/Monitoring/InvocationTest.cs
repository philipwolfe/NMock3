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
	public class InvocationTest
	{
		#region Setup/Teardown

		[TestInitialize]
		public void SetUp()
		{
			receiver = "receiver";
			result = "result";
			method = new MethodInfoStub("method");
			invocation = new Invocation(receiver, method, new object[0]);
			exception = new Exception();
		}

		#endregion

		private object receiver;
		private MethodInfoStub method;
		private Invocation invocation;
		private object result;
		private Exception exception;

		public class A
		{
		}

		public class B : A
		{
		}

		public class C
		{
		}

		public interface IFoo
		{
			string Foo(string input, out string output);
		}

		public static readonly MethodInfo FOO_METHOD = typeof (IFoo).GetMethod("Foo");

		public class MockFoo : IFoo
		{
			public bool FooWasInvoked;
			public Exception Foo_Exception;
			public string Foo_ExpectedInput;
			public string Foo_Output = "output";
			public string Foo_Result;

			#region IFoo Members

			public string Foo(string input, out string output)
			{
				FooWasInvoked = true;

				Assert.AreEqual(Foo_ExpectedInput, input, "input");
				if (Foo_Exception != null)
				{
					throw Foo_Exception;
				}
				else
				{
					output = Foo_Output;
					return Foo_Result;
				}
			}

			#endregion
		}

		public interface SugarMethods
		{
			int Property { get; set; }
			int this[string s, int i] { get; set; }
			event EventHandler Event;
		}

		private void HandleEvent(object sender, EventArgs args)
		{
		}

		[TestMethod]
		public void AllowsNullResultForMethodThatReturnsANullableType()
		{
			method.StubReturnType = typeof (int?);
			invocation.Result = null;
		}

		[TestMethod]
		public void AllowsSettingNullResultOfVoidMethod()
		{
			method.StubReturnType = typeof (void);
			invocation.Result = null;
		}

		[TestMethod]
		public void CanBeInvokedOnAnotherObject()
		{
			string input = "INPUT";
			string output = "OUTPUT";
			string result = "RESULT";

			invocation = new Invocation(receiver, FOO_METHOD, new object[] {input, null});

			MockFoo mockFoo = new MockFoo();
			mockFoo.Foo_ExpectedInput = input;
			mockFoo.Foo_Output = output;
			mockFoo.Foo_Result = result;

			invocation.InvokeOn(mockFoo);

			Assert.IsTrue(mockFoo.FooWasInvoked, "Foo should have been invoked");
			Assert.AreEqual(invocation.Result, result, "result");
			Assert.AreEqual(invocation.Parameters[1], output, "output");
		}

		[TestMethod]
		public void DescriptionOfEventAdderDoesNotShowSugaredMethod()
		{
			MethodInfo adder = typeof (SugarMethods).GetMethod(
				"add_Event", new[] {typeof (EventHandler)});
			Delegate handler = new EventHandler(HandleEvent);

			invocation = new Invocation(receiver, adder, new object[] {handler});

			AssertDescription.IsEqual(invocation, "receiver.Event += <HandleEvent[System.EventHandler]>\r\n");
		}

		[TestMethod]
		public void DescriptionOfEventRemoverDoesNotShowSugaredMethod()
		{
			MethodInfo adder = typeof (SugarMethods).GetMethod(
				"remove_Event", new[] {typeof (EventHandler)});
			Delegate handler = new EventHandler(HandleEvent);

			invocation = new Invocation(receiver, adder, new object[] {handler});

			AssertDescription.IsEqual(invocation, "receiver.Event -= <HandleEvent[System.EventHandler]>\r\n");
		}

		[TestMethod]
		public void DescriptionOfInvocationOfIndexerGetterDoesNotShowSugaredMethod()
		{
			MethodInfo getter = typeof (SugarMethods).GetMethod(
				"get_Item", new[] {typeof (string), typeof (int)});
			invocation = new Invocation(receiver, getter, new object[] {"hello", 10});

			AssertDescription.IsEqual(invocation, "receiver[\"hello\", <10>(System.Int32)]\r\n");
		}

		[TestMethod]
		public void DescriptionOfInvocationOfIndexerSetterDoesNotShowSugaredMethod()
		{
			MethodInfo getter = typeof (SugarMethods).GetMethod(
				"set_Item", new[] {typeof (string), typeof (int), typeof (int)});
			invocation = new Invocation(receiver, getter, new object[] {"hello", 10, 11});

			AssertDescription.IsEqual(invocation, "receiver[\"hello\", <10>(System.Int32)] = <11>(System.Int32)\r\n");
		}

		[TestMethod]
		public void DescriptionOfInvocationOfPropertyGetterDoesNotShowSugaredMethod()
		{
			MethodInfo getter = typeof (SugarMethods).GetMethod("get_Property", new Type[0]);
			invocation = new Invocation(receiver, getter, new object[0]);

			AssertDescription.IsEqual(invocation, "receiver.Property\r\n");
		}

		[TestMethod]
		public void DescriptionOfInvocationOfPropertySetterDoesNotShowSugaredMethod()
		{
			MethodInfo setter = typeof (SugarMethods).GetMethod("set_Property", new[] {typeof (int)});
			invocation = new Invocation(receiver, setter, new object[] {10});

			AssertDescription.IsEqual(invocation, "receiver.Property = <10>(System.Int32)\r\n");
		}

		[TestMethod, ExpectedException(typeof (ArgumentNullException))]
		public void DoesNotAllowNullException()
		{
			invocation.Exception = null;
		}

		[TestMethod, ExpectedException(typeof (ArgumentException))]
		public void DoesNotAllowNullResultForMethodThatReturnsAValueType()
		{
			method.StubReturnType = typeof (int);
			invocation.Result = null;
		}

		[TestMethod]
		public void DoesNotAllowSettingIncompatibleResultType()
		{
			method.StubReturnType = typeof (A);

			invocation.Result = new B();

			try
			{
				invocation.Result = new C();
				Assert.Fail("expected ArgumentException");
			}
			catch (ArgumentException)
			{
				//expected
			}
		}

		[TestMethod, ExpectedException(typeof (ArgumentException))]
		public void DoesNotAllowSettingNonNullResultOfVoidMethod()
		{
			method.StubReturnType = typeof (void);
			invocation.Result = "some value";
		}

		[TestMethod]
		public void SettingExceptionClearsResult()
		{
			invocation.Result = result;
			invocation.Exception = exception;

			Assert.AreSame(exception, invocation.Exception, "should store exception");
			Assert.IsTrue(invocation.IsThrowing, "should be throwing");
			Assert.IsNull(invocation.Result, "should not store a result");
		}

		[TestMethod]
		public void SettingResultClearsException()
		{
			invocation.Exception = exception;
			invocation.Result = result;

			Assert.AreSame(result, invocation.Result, "should store result");
			Assert.IsFalse(invocation.IsThrowing, "should not be throwing");
			Assert.IsNull(invocation.Exception, "should not store an exception");
		}

		[TestMethod]
		public void StoresExceptionToThrow()
		{
			invocation.Exception = exception;

			Assert.AreSame(exception, invocation.Exception, "should store exception");
			Assert.IsTrue(invocation.IsThrowing, "should be throwing");
			Assert.IsNull(invocation.Result, "should not store a result");
		}

		[TestMethod]
		public void StoresResultToReturn()
		{
			invocation.Result = result;

			Assert.AreSame(result, invocation.Result, "should store result");
			Assert.IsFalse(invocation.IsThrowing, "should not be throwing");
			Assert.IsNull(invocation.Exception, "should not store an exception");
		}

		[TestMethod]
		public void TrapsExceptionsWhenInvokedOnAnotherObject()
		{
			Exception exception = new Exception("thrown from Foo");

			invocation = new Invocation(receiver, FOO_METHOD, new object[] {"input", null});

			MockFoo mockFoo = new MockFoo();
			mockFoo.Foo_ExpectedInput = "input";
			mockFoo.Foo_Exception = exception;

			invocation.InvokeOn(mockFoo);

			Assert.IsTrue(mockFoo.FooWasInvoked, "Foo should have been invoked");
			Assert.AreSame(exception, invocation.Exception, "exception");
		}
	}
}