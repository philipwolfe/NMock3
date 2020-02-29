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
	public class EventBindingTests : BindingTestsBase
	{
		private string _message;
		private int _counter1, _counter2;

		private void Listener1(object sender, EventArgs e)
		{
			_message = "message 1";
		}
		private void Listener2(object sender, EventArgs e)
		{
			_message = "message 2";
		}
		private void ThrowListener(object sender, EventArgs e)
		{
			throw new InvalidOperationException("message 3");
		}
		private void ListenerCounter1(object sender, EventArgs e)
		{
			_counter1++;
			_message = "first";
		}
		private void ListenerCounter2(object sender, EventArgs e)
		{
			_counter2++;
			_message = "second";
		}
		private void Unbinder1(object sender, EventArgs e)
		{
			((IParentInterface)sender).StandardEvent1 -= Unbinder1;
		}
		private void Unbinder2(object sender, EventArgs e)
		{
			((IParentInterface)sender).StandardEvent2 -= Unbinder2;
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
			mock.Expects.One.EventBinding(_ => _.StandardEvent1 += Listener1);
			mock.Expects.One.EventBinding(_ => _.StandardEvent2 += Listener2);
			mock.MockObject.StandardEvent1 += Listener1;
			mock.MockObject.StandardEvent2 += Listener2;
		}

		[TestMethod]
		public override void RemoveHandlerTest()
		{
			mocks.ForEach(RemoveHandlerTest);
		}

		private void RemoveHandlerTest(Mock<IParentInterface> mock)
		{
			mock.Expects.One.EventBinding(_ => _.StandardEvent1 -= Listener1);
			mock.Expects.One.EventBinding(_ => _.StandardEvent2 -= Listener2);
			mock.MockObject.StandardEvent1 -= Listener1;
			mock.MockObject.StandardEvent2 -= Listener2;
		}

		[TestMethod]
		public override void ExpectBindingOfSpecificDelegateTest()
		{
			mocks.ForEach(ExpectBindingOfSpecificDelegateTest);
		}

		private void ExpectBindingOfSpecificDelegateTest(Mock<IParentInterface> mock)
		{
			mock.Expects.One.EventBinding(_ => _.StandardEvent1 += Listener1);

			Expect.That(() => mock.MockObject.StandardEvent1 += Listener2).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected invocation of:
  {0}.StandardEvent1 += <Listener2[System.EventHandler]>

If this exception was unexpected, the delegates used in the expectation and invocation may be different.  To bind to *any* delegate, use 'null' in the expectation.

MockFactory Expectations:
  {0}.StandardEvent1 += <Listener1[System.EventHandler]> will bind an event, returning an invoker. [EXPECTED: 1 time CALLED: 0 times]
", mock.Name)));

			mock.ClearExpectations();
		}

		[TestMethod]
		public override void AddHandlerAndInvokeTest()
		{
			mocks.ForEach(AddHandlerAndInvokeTest);
		}

		private void AddHandlerAndInvokeTest(Mock<IParentInterface> mock)
		{
			_message = null;

			EventInvoker invoker = mock.Expects.One.EventBinding(_ => _.StandardEvent1 += Listener1);
			mock.MockObject.StandardEvent1 += Listener1;

			invoker.Invoke();

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

			EventInvoker invoker = mock.Expects.One.EventBinding(_ => _.StandardEvent1 -= Listener1);

			mock.MockObject.StandardEvent1 -= Listener1;

			invoker.Invoke();

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

			EventInvoker invoker = mock.Expects.One.EventBinding(_ => _.StandardEvent1 += ThrowListener);
			mock.MockObject.StandardEvent1 += ThrowListener;

			try
			{
				invoker.Invoke();

				Assert.Fail("exception should be thrown.");

			}
			catch (Exception err)
			{
#if SILVERLIGHT
				string message = @"System.Reflection.TargetInvocationException: Exception has been thrown by the target of an invocation. ---> System.Reflection.TargetInvocationException: Exception has been thrown by the target of an invocation. ---> System.InvalidOperationException: message 3
   at NMockTests.MockTests.ExpectsTests.IMethodSyntaxTestsSL.EventBindingTests.ThrowListener(Object sender, EventArgs e)";
#else
#if NetFx35
				string message = @"System.InvalidOperationException: message 3
   at NMockTests.MockTests.ExpectsTests.IMethodSyntaxTests35.EventBindingTests.ThrowListener(Object sender, EventArgs e)";
#else
				string message = @"System.InvalidOperationException: message 3
   at NMockTests.MockTests.ExpectsTests.IMethodSyntaxTests.EventBindingTests.ThrowListener(Object sender, EventArgs e)";
#endif
#endif
				if (!err.ToString().StartsWith(message))
				{
					Assert.Fail("wrong error message. got: " + err);
				}
			}
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

			EventInvoker invoker = mock.Expects.Exactly(2).EventBinding(_ => _.StandardEvent1 += null);

			Assert.AreEqual(null, _message);
			mock.MockObject.StandardEvent1 += ListenerCounter1;

			invoker.Invoke();
			Assert.AreEqual("first", _message);

			mock.MockObject.StandardEvent1 += ListenerCounter2;

			invoker.Invoke();
			Assert.AreEqual("second", _message);

			mock.Expects.One.EventBinding(_ => _.StandardEvent1 -= null);

			mock.MockObject.StandardEvent1 -= ListenerCounter1;

			invoker.Invoke();
			Assert.AreEqual("second", _message);

			Assert.AreEqual(2, _counter1);
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
			EventInvoker invoker1 = mock.Expects.One.EventBinding(e => e.StandardEvent1 += (s, args) => { });
			EventInvoker invoker2 = mock.Expects.One.EventBinding(e => e.StandardEvent2 += (s, args) => { });

			mock.MockObject.StandardEvent1 += (s, args) => { i++; };
			mock.MockObject.StandardEvent2 += (s, args) => { i++; };

			invoker1.Invoke();
			invoker2.Invoke();

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
			EventInvoker invoker1 = mock.Expects.One.EventBinding(e => e.StandardEvent1 += delegate { });
			EventInvoker invoker2 = mock.Expects.One.EventBinding(e => e.StandardEvent2 += delegate { });

			mock.MockObject.StandardEvent1 += delegate { i++; };
			mock.MockObject.StandardEvent2 += delegate { i++; };

			invoker1.Invoke();
			invoker2.Invoke();

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

			EventInvoker invoker1 = mock.Expects.One.EventBinding(e => e.StandardEvent1 += null);
			EventInvoker invoker2 = mock.Expects.One.EventBinding(e => e.StandardEvent2 += null);

			mock.MockObject.StandardEvent1 += Listener2;
			mock.MockObject.StandardEvent2 += Listener1;

			invoker1.Invoke();
			Assert.AreEqual("message 2", _message);

			invoker2.Invoke();
			Assert.AreEqual("message 1", _message);

		}

		[TestMethod]
		public override void UnexpectedBindingTest()
		{
			mocks.ForEach(UnexpectedBindingTest);
		}

		private void UnexpectedBindingTest(Mock<IParentInterface> mock)
		{
			mock.Expects.One.EventBinding(_ => _.StandardEvent1 += null);

			Expect.That(() => mock.MockObject.MethodVoid()).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected invocation of:
  System.Void {0}.MethodVoid()
MockFactory Expectations:
  {0}.StandardEvent1 += (anything) will bind an event, returning an invoker. [EXPECTED: 1 time CALLED: 0 times]
", mock.Name)));
			
			mock.ClearExpectations();
		}

		[TestMethod]
		public override void InvokedHandlerUnbindsTest()
		{
			mocks.ForEach(InvokedHandlerUnbindsTest);
		}

		private void InvokedHandlerUnbindsTest(Mock<IParentInterface> mock)
		{
			var invoker = mock.Expects.One.EventBinding(_ => _.StandardEvent1 += null);
			var invoker2 = mock.Expects.One.EventBinding(_ => _.StandardEvent2 += null);

			mock.Expects.One.EventBinding(_ => _.StandardEvent1 -= null);
			mock.Expects.One.EventBinding(_ => _.StandardEvent2 -= null);

			mock.MockObject.StandardEvent1 += Unbinder1;
			mock.MockObject.StandardEvent2 += Unbinder2;

			invoker.Invoke(mock.MockObject, EventArgs.Empty);
			invoker2.Invoke(mock.MockObject, EventArgs.Empty);
		}

		#region Controler Tests
		[TestMethod]
		public void EventInterfaces2Test()
		{
			Mock<DrawingObject> shape = Factory.CreateMock<DrawingObject>(MockStyle.Transparent, typeof(ISupportPreDraw), typeof(ISupportPostDraw));
			Mock<ISupportPreDraw> preDraw = shape.As<ISupportPreDraw>();
			Mock<ISupportPostDraw> postDraw = shape.As<ISupportPostDraw>();

			preDraw.Expects.One.EventBinding(s => s.OnDraw += null);
			postDraw.Expects.One.EventBinding(s => s.OnDraw += null);

			PreDrawSubscriber sub = new PreDrawSubscriber(preDraw.MockObject);
			PostDrawSubscriber sub2 = new PostDrawSubscriber(postDraw.MockObject);
			shape.MockObject.Draw();

			Assert.AreEqual("PreDraw", sub.Message);
			Assert.AreEqual("Drawing", shape.MockObject.Message);
			Assert.AreEqual("PostDraw", sub2.Message);
		}

		[TestMethod]
		public void EventInterfacesTest()
		{
			Mock<DrawingObject> shape = Factory.CreateMock<DrawingObject>(MockStyle.Transparent, typeof(ISupportPostDraw), typeof(ISupportPreDraw));

			shape.As<ISupportPostDraw>().Expects.One.EventBinding(s => s.OnDraw += null);
			shape.As<ISupportPreDraw>().Expects.One.EventBinding(s => s.OnDraw += null);

			PreDrawSubscriber sub = new PreDrawSubscriber(shape.MockObject);
			PostDrawSubscriber sub2 = new PostDrawSubscriber(shape.MockObject);
			shape.MockObject.Draw();

			Assert.AreEqual("PreDraw", sub.Message);
			Assert.AreEqual("PostDraw", sub2.Message);
		}
		#endregion

		#region Private test classes

		//example from http://msdn.microsoft.com/en-us/library/ak9w5846.aspx

		public interface ISupportPreDraw
		{
			// Raise this event before drawing the object.
			event EventHandler OnDraw;
		}

		public interface ISupportPostDraw
		{
			// Raise this event after drawing the object.
			event EventHandler OnDraw;
		}


		// Base class event publisher inherits two
		// interfaces, each with an OnDraw event
		public class DrawingObject : ISupportPreDraw, ISupportPostDraw
		{
			// Create an event for each interface event
			private event EventHandler PreDrawEvent;
			private event EventHandler PostDrawEvent;


			private readonly object objectLock = new Object();
			public string Message { get; private set; }

			#region ISupportPostDraw Members

			// Explicit interface implementation required.
			event EventHandler ISupportPostDraw.OnDraw
			{
				add
				{
					lock (objectLock)
					{
						PostDrawEvent += value;
					}
				}
				remove
				{
					lock (objectLock)
					{
						PostDrawEvent -= value;
					}
				}
			}

			#endregion

			#region ISupportPreDraw Members

			// Explicit interface implementation required.
			event EventHandler ISupportPreDraw.OnDraw
			{
				add
				{
					lock (objectLock)
					{
						PreDrawEvent += value;
					}
				}
				remove
				{
					lock (objectLock)
					{
						PreDrawEvent -= value;
					}
				}
			}

			#endregion

			// For the sake of simplicity this one method
			// implements both interfaces. 
			public void Draw()
			{
				// Raise IDrawingObject's event before the object is drawn.
				EventHandler handler = PreDrawEvent;
				if (handler != null)
				{
					handler(this, new EventArgs());
				}

				Message = "Drawing";

				// RaiseIShape's event after the object is drawn.
				handler = PostDrawEvent;
				if (handler != null)
				{
					handler(this, new EventArgs());
				}
			}
		}

		public class PreDrawSubscriber
		{
			// References the shape object as an ISupportPreDraw
			public PreDrawSubscriber(DrawingObject shape)
				: this((ISupportPreDraw)shape)
			{
			}

			public PreDrawSubscriber(ISupportPreDraw shape)
			{
				shape.OnDraw += d_OnDraw;
			}

			public string Message { get; private set; }

			private void d_OnDraw(object sender, EventArgs e)
			{
				Message = "PreDraw";
			}
		}

		// References the shape object as an ISupportPostDraw
		public class PostDrawSubscriber
		{
			public PostDrawSubscriber(DrawingObject shape)
				: this((ISupportPostDraw)shape)
			{
			}

			public PostDrawSubscriber(ISupportPostDraw shape)
			{
				shape.OnDraw += d_OnDraw;
			}

			public string Message { get; private set; }

			private void d_OnDraw(object sender, EventArgs e)
			{
				Message = "PostDraw";
			}
		}


		#endregion

	}
}