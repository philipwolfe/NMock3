#region Using

using System;
using System.Data;
using System.Runtime.CompilerServices;
using NMock;
using NMock.AcceptanceTests;
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

namespace NMock.AcceptanceTests
{
	[TestClass]
	public class MockThrowingExceptionInEventHandler : AcceptanceTestBase
	{
		public interface IEventProvider
		{
			event EventHandler Event;
			event EventHandler Event2;
		}

		public interface IExceptionThrower
		{
			void ThrowException();
			void ThrowException(bool b);
			object ThrowException(string s);
			void DoSomethingWith(Exception e);
		}

		public class Presenter
		{
			private readonly IEventProvider _provider;
			private readonly IExceptionThrower _thrower;

			public Presenter(IEventProvider provider, IExceptionThrower thrower)
			{
				_provider = provider;
				_thrower = thrower;
				_provider.Event += Presenter_Go;
				_provider.Event2 += Presenter_Go2;
			}

			void Presenter_Go2(object sender, EventArgs e)
			{
				try
				{
					_thrower.ThrowException("");
				}
				catch(Exception ex)
				{
					_thrower.DoSomethingWith(ex);
					throw;
				}
			}

			void Presenter_Go(object sender, EventArgs e)
			{
				_thrower.ThrowException();
			}
		}

		[TestMethod]
		public void ExceptionsInEventHandlersAreNotWrappedInInternalExceptions()
		{
			Mock<IEventProvider> eventProvider = Mocks.CreateMock<IEventProvider>();
			Mock<IExceptionThrower> exceptionThrower = Mocks.CreateMock<IExceptionThrower>();

			EventInvoker invoker = eventProvider.Expects.One.EventBinding(e => e.Event += null);
			exceptionThrower.Expects.One.Method(e => e.ThrowException()).Will(Throw.Exception(new ApplicationException("got you!")));

			eventProvider.MockObject.Event += delegate { exceptionThrower.MockObject.ThrowException(); };

			Expect.That(invoker.Invoke).Throws<ApplicationException>();
		}

		[TestMethod]
		public void ExceptionFromEventIsTheRightType()
		{
			Mock<IEventProvider> eventProvider = Mocks.CreateMock<IEventProvider>();
			Mock<IExceptionThrower> exceptionThrower = Mocks.CreateMock<IExceptionThrower>();

			EventInvoker invoker = eventProvider.Expects.One.EventBinding(e => e.Event += null);
			EventInvoker invoker2 = eventProvider.Expects.One.EventBinding(e => e.Event2 += null);
			exceptionThrower.Expects.One.MethodWith(_ => _.ThrowException()).Will(Throw.Exception(new TimeoutException()));

			Presenter p = new Presenter(eventProvider.MockObject, exceptionThrower.MockObject);

			Expect.That(invoker.Invoke).Throws<TimeoutException>();
		}

		[TestMethod]
		public void ExceptionFromEventIsTheRightType2()
		{
			Mock<IEventProvider> eventProvider = Mocks.CreateMock<IEventProvider>();
			Mock<IExceptionThrower> exceptionThrower = Mocks.CreateMock<IExceptionThrower>();
			TimeoutException ex = new TimeoutException();

			EventInvoker invoker = eventProvider.Expects.One.EventBinding(e => e.Event += null);
			EventInvoker invoker2 = eventProvider.Expects.One.EventBinding(e => e.Event2 += null);
			exceptionThrower.Expects.One.MethodWith(_ => _.ThrowException("")).Will(Throw.Exception(ex));
			exceptionThrower.Expects.One.MethodWith(_ => _.DoSomethingWith(ex));

			Presenter p = new Presenter(eventProvider.MockObject, exceptionThrower.MockObject);

			Expect.That(invoker2.Invoke).Throws<TimeoutException>();
		}

		[TestMethod]
		public void ExceptionFromEventIsTheRightType3()
		{
			Mock<IEventProvider> eventProvider = Mocks.CreateMock<IEventProvider>();
			Mock<IExceptionThrower> exceptionThrower = Mocks.CreateMock<IExceptionThrower>();
			TimeoutException ex = new TimeoutException();

			EventInvoker invoker = eventProvider.Expects.One.EventBinding(e => e.Event += null);
			EventInvoker invoker2 = eventProvider.Expects.One.EventBinding(e => e.Event2 += null);
			exceptionThrower.Expects.One.MethodWith(_ => _.ThrowException("")).Will(Throw.Exception(ex));
			exceptionThrower.Expects.One.MethodWith(_ => _.DoSomethingWith(ex));

			Presenter p = new Presenter(eventProvider.MockObject, exceptionThrower.MockObject);

			Expect.That(()=>invoker2.Invoke(null)).Throws<TimeoutException>();
		}
	}
}