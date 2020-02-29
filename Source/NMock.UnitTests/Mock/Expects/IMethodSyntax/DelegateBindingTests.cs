#region Using

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using NMock;
using NMock.Matchers;
using NMockTests._TestStructures;
using Is = NMock.Is;

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

#if NetFx35
namespace NMockTests.MockTests.ExpectsTests.IMethodSyntaxTests35
#else
#if SILVERLIGHT
namespace NMockTests.MockTests.ExpectsTests.IMethodSyntaxTestsSL
#else
namespace NMockTests.MockTests.ExpectsTests.IMethodSyntaxTests
#endif
#endif
{
	[TestClass]
	public class DelegateBindingTests : BindingTestsBase
	{
		private string _message;
		private int _counter1, _counter2;

		private void Listener(string message)
		{
			_message = message;
		}
		private void Listener2(string message)
		{
			_message = message;
		}
		private void ThrowListener(string message)
		{
			throw new InvalidOperationException(message);
		}
		private void ListenerCounter1(string message)
		{
			_counter1++;
			_message = message;
		}
		private void ListenerCounter2(string message)
		{
			_counter2++;
			_message = message;
		}

		[TestInitialize]
		public void Init()
		{
			Initalize();
		}

		[TestCleanup]
		public void Clean()
		{
			Cleanup();
		}

		[TestMethod]
		public override void AddHandlerTest()
		{
			mocks.ForEach(AddHandlerTest);
		}

		private void AddHandlerTest(Mock<IParentInterface> mock)
		{
			mock.Expects.One.DelegateBinding(_ => _.Delegate += Listener);
			mock.MockObject.Delegate += Listener;
		}

		[TestMethod]
		public override void RemoveHandlerTest()
		{
			mocks.ForEach(RemoveHandlerTest);
		}

		private void RemoveHandlerTest(Mock<IParentInterface> mock)
		{
			mock.Expects.One.DelegateBinding(_ => _.Delegate -= Listener);
			mock.MockObject.Delegate -= Listener;
		}

		[TestMethod]
		public override void AddHandlerAndInvokeTest()
		{
			mocks.ForEach(AddHandlerAndInvokeTest);
		}

		private void AddHandlerAndInvokeTest(Mock<IParentInterface> mock)
		{
			_message = null;

			DelegateInvoker invoker = mock.Expects.One.DelegateBinding(_ => _.Delegate += Listener);
			mock.MockObject.Delegate += Listener;

			invoker.Invoke("message 1");

			Assert.AreEqual("message 1", _message);
		}

		[TestMethod]
		public override void RemoveHandlerAndInvokeTest()
		{
			mocks.ForEach(RemoveHandlerAndInvokeTest);
		}

		private void RemoveHandlerAndInvokeTest(Mock<IParentInterface> mock)
		{
			_message = null;

			DelegateInvoker invoker = mock.Expects.One.DelegateBinding(_ => _.Delegate -= Listener);

			mock.MockObject.Delegate -= Listener;

			invoker.Invoke("message 2");

			Assert.IsNull(_message);
		}

		[TestMethod]
		public override void ThrowErrorOnInvokeTest()
		{
			mocks.ForEach(ThrowErrorOnInvokeTest);
		}

		private void ThrowErrorOnInvokeTest(Mock<IParentInterface> mock)
		{
			DelegateInvoker invoker = mock.Expects.One.DelegateBinding(_ => _.Delegate += ThrowListener);
			mock.MockObject.Delegate += ThrowListener; //binding to a listener that throws

#if SILVERLIGHT
			Expect.That(() => invoker.Invoke("message 3")).Throws<TargetInvocationException>(new StringContainsMatcher(@"System.Reflection.TargetInvocationException: Exception has been thrown by the target of an invocation. ---> System.Reflection.TargetInvocationException: Exception has been thrown by the target of an invocation. ---> System.InvalidOperationException: message 3
   at NMockTests.MockTests.ExpectsTests.IMethodSyntaxTestsSL.DelegateBindingTests.ThrowListener(String message)"));
#else
#if NetFx35
				Expect.That(() => invoker.Invoke("message 3")).Throws<InvalidOperationException>(new StringContainsMatcher(@"System.InvalidOperationException: message 3
   at NMockTests.MockTests.ExpectsTests.IMethodSyntaxTests35.DelegateBindingTests.ThrowListener(String message)"));
#else
			Expect.That(() => invoker.Invoke("message 3")).Throws<InvalidOperationException>(new StringContainsMatcher(@"System.InvalidOperationException: message 3
   at NMockTests.MockTests.ExpectsTests.IMethodSyntaxTests.DelegateBindingTests.ThrowListener(String message)"));
#endif
#endif
		}

		[TestMethod]
		public override void ExpectBindingOfSpecificDelegateTest()
		{
			mocks.ForEach(ExpectBindingOfSpecificDelegateTest);
		}

		private void ExpectBindingOfSpecificDelegateTest(Mock<IParentInterface> mock)
		{
			Factory.ClearExpectations();

			mock.Expects.One.DelegateBinding(_ => _.Delegate += Listener);

			Expect.That(() => mock.MockObject.Delegate += Listener2).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected invocation of:
  {0}.Delegate += <Listener2[NMockTests._TestStructures.CustomDelegate]>

If this exception was unexpected, the delegates used in the expectation and invocation may be different.  To bind to *any* delegate, use 'null' in the expectation.

MockFactory Expectations:
  {0}.Delegate += <Listener[NMockTests._TestStructures.CustomDelegate]> will bind a delegate, returning an invoker. [EXPECTED: 1 time CALLED: 0 times]", mock.Name)));

			mock.ClearExpectations();
		}

		[TestMethod]
		public override void MultipleBindingTest()
		{
			mocks.ForEach(MultipleBindingTest);
		}

		private void MultipleBindingTest(Mock<IParentInterface> mock)
		{
			_counter1 = 0;
			_counter2 = 0;

			DelegateInvoker invoker = mock.Expects.Exactly(2).DelegateBinding(_ => _.Delegate += null);
			mock.MockObject.Delegate += ListenerCounter1;
			mock.MockObject.Delegate += ListenerCounter2;

			invoker.Invoke("first");
			Assert.AreEqual("first", _message);

			mock.Expects.One.DelegateBinding(_ => _.Delegate -= null);

			mock.MockObject.Delegate -= ListenerCounter1;

			invoker.Invoke("second");

			Assert.AreEqual("second", _message);
			Assert.AreEqual(1, _counter1);
			Assert.AreEqual(2, _counter2);
		}

		[TestMethod]
		public override void AddHandlerAnonymousBindingTest()
		{
			mocks.ForEach(AddHandlerAnonymousBindingTest);
		}

		private void AddHandlerAnonymousBindingTest(Mock<IParentInterface> mock)
		{
			int i = 0;
			DelegateInvoker invoker1 = mock.Expects.One.DelegateBinding(e => e.Delegate += (msg) => { });

			mock.MockObject.Delegate += (m) => { i++; };

			invoker1.Invoke("message 4");

			Assert.AreEqual(1, i);
		}

		[TestMethod]
		public override void AddHandlerEmptyDelegateBindingTest()
		{
			mocks.ForEach(AddHandlerEmptyDelegateBindingTest);
		}

		private void AddHandlerEmptyDelegateBindingTest(Mock<IParentInterface> mock)
		{
			int i = 0;
			DelegateInvoker invoker1 = mock.Expects.One.DelegateBinding(e => e.Delegate += delegate { });

			mock.MockObject.Delegate += delegate { i++; };

			invoker1.Invoke("message 5");

			Assert.AreEqual(1, i);
		}

		[TestMethod]
		public override void AddHandlerNullBindingTest()
		{
			mocks.ForEach(AddHandlerNullBindingTest);
		}

		private void AddHandlerNullBindingTest(Mock<IParentInterface> mock)
		{
			_message = null;

			DelegateInvoker invoker1 = mock.Expects.One.DelegateBinding(e => e.Delegate += null);

			mock.MockObject.Delegate += Listener;

			invoker1.Invoke("message 1");

			Assert.AreEqual("message 1", _message);
		}

		[TestMethod]
		public override void UnexpectedBindingTest()
		{
			mocks.ForEach(UnexpectedBindingTest);
		}

		private void UnexpectedBindingTest(Mock<IParentInterface> mock)
		{
			mock.Expects.One.DelegateBinding(_ => _.Delegate += null);

			Expect.That(() => mock.MockObject.MethodVoid()).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected invocation of:
  System.Void {0}.MethodVoid()
MockFactory Expectations:
  {0}.Delegate += (anything) will bind a delegate, returning an invoker. [EXPECTED: 1 time CALLED: 0 times]", mock.Name)));
			Factory.ClearExpectations();
		}

		[TestMethod]
		public override void InvokedHandlerUnbindsTest()
		{
			mocks.ForEach(InvokedHandlerUnbindsTest);
		}

		private void InvokedHandlerUnbindsTest(Mock<IParentInterface> mock)
		{
			_message = null;

			var invoker = mock.Expects.One.DelegateBinding(_ => _.Delegate += Listener);

			mock.MockObject.Delegate += Listener;

			invoker.Invoke("message 7");

			Assert.AreEqual("message 7", _message);

			mock.Expects.One.DelegateBinding(_ => _.Delegate -= Listener);

			mock.MockObject.Delegate -= Listener;

			invoker.Invoke("message 8");

			Assert.AreEqual("message 7", _message);
		}

		[TestMethod]
		public void DelegateControllerTest()
		{
			mocks.ForEach(DelegateControllerTest);
		}

		private void DelegateControllerTest(Mock<IParentInterface> mock)
		{
			DelegateInvoker invoker = mock.Expects.One.DelegateBinding(_ => _.Delegate += null);

			var controller = new DelegateController(mock.MockObject);

			invoker.Invoke("Input 1");

			Assert.AreEqual("Input 1", controller.Input);
		}

		internal class DelegateController
		{
			public DelegateController(IParentInterface obj)
			{
				obj.Delegate += SendInput;
			}
			public string Input { get; private set; }

			public void SendInput(string input)
			{
				Input = input;
			}
		}
	}
}
