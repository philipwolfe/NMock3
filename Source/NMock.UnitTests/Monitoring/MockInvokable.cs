#region Using

using System;
using System.Reflection;
using NMock.Monitoring;
using NUnit.Framework;

#endregion

namespace NMockTests.Monitoring
{
	public class MockInvokable : IInvokable
	{
		public Invocation Actual;
		public Exception ExceptionSetOnInvocation;
		public Invocation Expected;
		public object[] Outputs;
		public object ResultSetOnInvocation;
		public Exception ThrownException;
		public bool expectNotCalled;

		#region IInvokable Members

		void IInvokable.Invoke(Invocation invocation)
		{
			Assert.IsFalse(expectNotCalled, "MockInvokable should not have been invoked");

			Actual = invocation;
			if (Expected != null) Assert.AreEqual(Expected.Method, Actual.Method, "method");
			if (Outputs != null)
			{
				for (int i = 0; i < Actual.Parameters.Count; i++)
				{
					if ((Actual.Method.GetParameters()[i].Attributes & ParameterAttributes.In) == ParameterAttributes.None)
					{
						Actual.Parameters[i] = Outputs[i];
					}
				}
			}

			if (ThrownException != null) throw ThrownException;

			if (ExceptionSetOnInvocation != null)
			{
				invocation.Exception = ExceptionSetOnInvocation;
			}
			else if (invocation.Method.ReturnType != typeof (void))
			{
				invocation.Result = ResultSetOnInvocation;
			}
		}

		#endregion

		public void ExpectNotCalled()
		{
			expectNotCalled = true;
		}
	}
}