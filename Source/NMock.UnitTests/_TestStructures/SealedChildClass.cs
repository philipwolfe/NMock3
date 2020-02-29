using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMockTests._TestStructures
{
	public sealed class SealedChildClass : AbstractParentClass
	{
		public override int Add(int a, int b)
		{
			return DoAdd(a, b);
		}

		private int DoAdd(int a, int b)
		{
			return a + b;
		}
		public override void MethodVoid()
		{}

		public override void MethodVoid(List<string> items, int count)
		{ }

		public override void MethodVoid(int r, int g, int b)
		{}

		public override void MethodVoid(Version version)
		{ }

		public override void MethodVoid(OperatingSystem operatingSystem)
		{ }

		public override void MethodVoid<T>(T input)
		{}

		public override void MethodVoid<T, U>(T arg1, U arg2)
		{}

		public override void MethodVoid<T, U, V>(T arg1, U arg2, V arg3)
		{}


		public override void AsyncMethod(Action callback)
		{}

		public override void AsyncMethod(Action<IEnumerable<Version>> callback)
		{}

		public override void AsyncMethod(Version version, Action callback)
		{ }

		public override string Echo(string stringToEcho)
		{
			return stringToEcho;
		}

		public override bool Method()
		{
			throw new NotImplementedException();
		}

		public override bool Method(int arg)
		{
			throw new NotImplementedException();
		}

		public override bool Method<T>(T arg)
		{
			throw new NotImplementedException();
		}

		public override T Method<T>()
		{
			throw new NotImplementedException();
		}

		public override T Method<T, U>()
		{
			throw new NotImplementedException();
		}

		public override T Method<T, U>(U arg)
		{
			throw new NotImplementedException();
		}

		public override void Set<T>(string arg1, T arg2)
		{
			throw new NotImplementedException();
		}

		public override T Get<T>(string arg)
		{
			throw new NotImplementedException();
		}

		public override void Dispose()
		{
			
		}
	}
}
