using System;
using NMock;

namespace NMockTests._TestStructures
{
	public class SampleClassWithIMockObjectMembers
	{
		public string MockName
		{
			get
			{
				throw new Exception("The method or operation is not implemented.");
			}
		}

		public virtual int Multiply(int a, int b)
		{
			return a * b;
		}

		public bool HasMethodMatching(Matcher methodMatcher)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void AddEventHandler(string eventName, Delegate handler)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void RemoveEventHandler(string eventName, Delegate handler)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void RaiseEvent(string eventName, params object[] args)
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}