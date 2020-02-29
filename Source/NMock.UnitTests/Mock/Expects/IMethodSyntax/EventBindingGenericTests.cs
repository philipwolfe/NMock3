#region Using

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
	public class EventBindingGenericTests : BindingTestsBase
	{
		private string _message;
		private int _counter1, _counter2;

		private void Listener(object sender, ValueEventArgs e)
		{
			_message = e.Value;
		}
		private void Listener2(object sender, ValueEventArgs e)
		{
			_message = e.Value;
		}
		private void Listener3(object sender, EventArgs e)
		{
			_message = "message 3";
		}
		private void ThrowListener(object sender, ValueEventArgs e)
		{
			throw new InvalidOperationException(e.Value);
		}
		private void ListenerCounter1(object sender, ValueEventArgs e)
		{
			_counter1++;
			_message = e.Value;
		}
		private void ListenerCounter2(object sender, ValueEventArgs e)
		{
			_counter2++;
			_message = e.Value;
		}
		private void Unbinder1(object sender, EventArgs e)
		{
			((IParentInterface)sender).StandardEvent2 -= Unbinder1;
		}
		private void Unbinder2(object sender, ValueEventArgs e)
		{
			((IParentInterface)sender).CustomEvent -= Unbinder2;
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
			mock.Expects.One.EventBinding<ValueEventArgs>(_ => _.CustomEvent += Listener);
			mock.MockObject.CustomEvent += Listener;
		}

		[TestMethod]
		public override void RemoveHandlerTest()
		{
			mocks.ForEach(RemoveHandlerTest);
		}

		private void RemoveHandlerTest(Mock<IParentInterface> mock)
		{
			mock.Expects.One.EventBinding<ValueEventArgs>(_ => _.CustomEvent -= Listener);
			mock.MockObject.CustomEvent -= Listener;
		}

		[TestMethod]
		public override void AddHandlerAndInvokeTest()
		{
			mocks.ForEach(AddHandlerAndInvokeTest);
		}

		private void AddHandlerAndInvokeTest(Mock<IParentInterface> mock)
		{
			_message = null;

			EventInvoker<ValueEventArgs> invoker = mock.Expects.One.EventBinding<ValueEventArgs>(_ => _.CustomEvent += Listener);
			mock.MockObject.CustomEvent += Listener;

			invoker.Invoke(new ValueEventArgs {Value = "message 1"});

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

			EventInvoker<ValueEventArgs> invoker = mock.Expects.One.EventBinding<ValueEventArgs>(_ => _.CustomEvent -= Listener);

			mock.MockObject.CustomEvent -= Listener;

			invoker.Invoke(new ValueEventArgs { Value = "message 2" });

			Assert.IsNull(_message);
		}

		[TestMethod]
		public override void ThrowErrorOnInvokeTest()
		{
			mocks.ForEach(ThrowErrorOnInvokeTest);
		}

		private void ThrowErrorOnInvokeTest(Mock<IParentInterface> mock)
		{
			//Factory.SuppressUnexpectedAndUnmetExpectations();

			EventInvoker<ValueEventArgs> invoker = mock.Expects.One.EventBinding<ValueEventArgs>(_ => _.CustomEvent += ThrowListener);
			mock.MockObject.CustomEvent += ThrowListener;

			try
			{
				invoker.Invoke(new ValueEventArgs { Value = "message 3" });

				Assert.Fail("exception should be thrown.");

			}
			catch (Exception err)
			{
#if SILVERLIGHT
				string message = @"System.Reflection.TargetInvocationException: Exception has been thrown by the target of an invocation. ---> System.Reflection.TargetInvocationException: Exception has been thrown by the target of an invocation. ---> System.InvalidOperationException: message 3
   at NMockTests.MockTests.ExpectsTests.IMethodSyntaxTestsSL.EventBindingGenericTests.ThrowListener(Object sender, ValueEventArgs e)";
#else
#if NetFx35
				string message = @"System.InvalidOperationException: message 3
   at NMockTests.MockTests.ExpectsTests.IMethodSyntaxTests35.EventBindingGenericTests.ThrowListener(Object sender, ValueEventArgs e)";
#else
				string message = @"System.InvalidOperationException: message 3
   at NMockTests.MockTests.ExpectsTests.IMethodSyntaxTests.EventBindingGenericTests.ThrowListener(Object sender, ValueEventArgs e)";
#endif
#endif
				if (!err.ToString().StartsWith(message))
				{
					Assert.Fail("wrong error message. got: " + err);
				}
			}
		}

		[TestMethod]
		public override void ExpectBindingOfSpecificDelegateTest()
		{
			mocks.ForEach(ExpectBindingOfSpecificDelegateTest);
		}

		private void ExpectBindingOfSpecificDelegateTest(Mock<IParentInterface> mock)
		{
			mock.Expects.One.EventBinding<ValueEventArgs>(_ => _.CustomEvent += Listener);

			Expect.That(() => mock.MockObject.CustomEvent += Listener2).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected invocation of:
  {0}.CustomEvent += <Listener2[System.EventHandler`1[NMockTests._TestStructures.ValueEventArgs]]>

If this exception was unexpected, the delegates used in the expectation and invocation may be different.  To bind to *any* delegate, use 'null' in the expectation.

MockFactory Expectations:
  {0}.CustomEvent += <Listener[System.EventHandler`1[NMockTests._TestStructures.ValueEventArgs]]> will bind an event, returning an invoker. [EXPECTED: 1 time CALLED: 0 times]
", mock.Name)));
			
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
			_message = null;

			EventInvoker<ValueEventArgs> invoker = mock.Expects.Exactly(2).EventBinding<ValueEventArgs>(_ => _.CustomEvent += null);

			Assert.AreEqual(null, _message);
			mock.MockObject.CustomEvent += ListenerCounter1;

			invoker.Invoke(new ValueEventArgs { Value = "first" });
			Assert.AreEqual("first", _message);

			mock.MockObject.CustomEvent += ListenerCounter2;

			invoker.Invoke(new ValueEventArgs { Value = "second" });
			Assert.AreEqual("second", _message);

			mock.Expects.One.EventBinding(_ => _.CustomEvent -= null);

			mock.MockObject.CustomEvent -= ListenerCounter1;

			invoker.Invoke(new ValueEventArgs { Value = "third" });
			Assert.AreEqual("third", _message);

			Assert.AreEqual(2, _counter1);
			Assert.AreEqual(2, _counter2);
		}
		
		[TestMethod]
		public override void UnexpectedBindingTest()
		{
			mocks.ForEach(UnexpectedBindingTest);
		}

		private void UnexpectedBindingTest(Mock<IParentInterface> mock)
		{
			mock.Expects.One.EventBinding(_ => _.CustomEvent += null);

			try
			{
				mock.MockObject.StandardEvent1 += null;
				Assert.Fail();
			}
			catch (Exception)
			{
				Assert.IsTrue(true);
			}

			Factory.ClearExpectations();
		}

		[TestMethod]
		public override void AddHandlerAnonymousBindingTest()
		{
			mocks.ForEach(AddHandlerAnonymousBindingTest);
		}

		private void AddHandlerAnonymousBindingTest(Mock<IParentInterface> mock)
		{
			int i = 0;

			EventInvoker<EventArgs> invoker1 = mock.Expects.One.EventBinding<EventArgs>(e => e.StandardEvent2 += (s, args) => {  });
			EventInvoker<ValueEventArgs> invoker2 = mock.Expects.One.EventBinding<ValueEventArgs>(e => e.CustomEvent += (s, args) => {  });

			mock.MockObject.StandardEvent2 += (s, args) => { i++; };
			mock.MockObject.CustomEvent += (s, args) => { i++; };

			invoker1.Invoke(new EventArgs());
			invoker2.Invoke(new ValueEventArgs());

			Assert.AreEqual(2, i);
		}

		[TestMethod]
		public override void AddHandlerEmptyDelegateBindingTest()
		{
			mocks.ForEach(AddHandlerEmptyDelegateBindingTest);
		}

		private void AddHandlerEmptyDelegateBindingTest(Mock<IParentInterface> mock)
		{
			int i = 0;

			EventInvoker<EventArgs> invoker1 = mock.Expects.One.EventBinding<EventArgs>(e => e.StandardEvent2 += delegate { });
			EventInvoker<ValueEventArgs> invoker2 = mock.Expects.One.EventBinding<ValueEventArgs>(e => e.CustomEvent += delegate { });

			mock.MockObject.StandardEvent2 += delegate { i++; };
			mock.MockObject.CustomEvent += delegate { i++; };

			invoker1.Invoke(new EventArgs());
			invoker2.Invoke(new ValueEventArgs());

			Assert.AreEqual(2, i);
		}

		[TestMethod]
		public override void AddHandlerNullBindingTest()
		{
			mocks.ForEach(AddHandlerNullBindingTest);
		}

		private void AddHandlerNullBindingTest(Mock<IParentInterface> mock)
		{
			_message = null;

			EventInvoker<EventArgs> invoker1 = mock.Expects.One.EventBinding<EventArgs>(e => e.StandardEvent2 += null);
			EventInvoker<ValueEventArgs> invoker2 = mock.Expects.One.EventBinding<ValueEventArgs>(e => e.CustomEvent += null);

			mock.MockObject.StandardEvent2 += Listener3;
			mock.MockObject.CustomEvent += Listener;

			invoker1.Invoke(new EventArgs());
			Assert.AreEqual("message 3", _message);

			invoker2.Invoke(new ValueEventArgs{Value = "message 1"});
			Assert.AreEqual("message 1", _message);

		}

		[TestMethod]
		public override void InvokedHandlerUnbindsTest()
		{
			mocks.ForEach(InvokedHandlerUnbindsTest);
		}

		private void InvokedHandlerUnbindsTest(Mock<IParentInterface> mock)
		{
			EventInvoker<EventArgs> invoker1 = mock.Expects.One.EventBinding<EventArgs>(_ => _.StandardEvent2 += null);
			EventInvoker<ValueEventArgs> invoker2 = mock.Expects.One.EventBinding<ValueEventArgs>(_ => _.CustomEvent += null);

			mock.Expects.One.EventBinding(_ => _.StandardEvent2 -= null);
			mock.Expects.One.EventBinding(_ => _.CustomEvent -= null);

			mock.MockObject.StandardEvent2 += Unbinder1;
			mock.MockObject.CustomEvent += Unbinder2;

			invoker1.Invoke(mock.MockObject, new EventArgs());
			invoker2.Invoke(mock.MockObject, new ValueEventArgs());
		}

	}
}
