using System;
using System.Collections.Generic;

namespace NMockTests._TestStructures
{
	public class ExplicitImplementationParentClass : IParentInterface
	{
		Version IParentInterface.ReadWriteObjectProperty { get; set; }
		bool IParentInterface.IsReadOnly { get; set; }
		Version IParentInterface.ReadOnlyObjectProperty { get { return new Version(0, 0, 0, 0); } }
		Version IParentInterface.WriteOnlyObjectProperty { set { } }
		int IParentInterface.ReadOnlyValueProperty { get { return 0; } }

		int IParentInterface.this[string name]
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		DateTime IParentInterface.this[short s, long l]
		{
			get
			{
				return DateTime.Today.AddDays(s);
			}
			set
			{
			}
		}

		bool IParentInterface.this[byte b]
		{
			set
			{
			}
		}

		double IParentInterface.this[decimal d, bool b]
		{
			set
			{
			}
		}

		Guid IParentInterface.this[long l]
		{
			get
			{
				return Guid.Empty;
			}
		}

		TimeSpan IParentInterface.this[bool b, int i]
		{
			get
			{
				return new TimeSpan(i, i, i, i);
			}
		}

		event CustomDelegate IParentInterface.Delegate
		{
			add
			{
			}
			remove
			{
			}
		}
		event EventHandler IParentInterface.StandardEvent1
		{
			add
			{
			}
			remove
			{
			}
		}
		event EventHandler<EventArgs> IParentInterface.StandardEvent2
		{
			add
			{
			}
			remove
			{
			}
		}
		event EventHandler<ValueEventArgs> IParentInterface.CustomEvent
		{
			add
			{
			}
			remove
			{
			}
		}

		int IParentInterface.Add(int a, int b)
		{
			return DoAdd(a, b);
		}

		protected virtual int DoAdd(int a, int b)
		{
			return a + b;
		}
		void IParentInterface.MethodVoid()
		{ }

		void IParentInterface.MethodVoid(List<string> items, int count)
		{ }

		void IParentInterface.MethodVoid(int r, int g, int b)
		{}

		void IParentInterface.MethodVoid(Version version)
		{}

		void IParentInterface.MethodVoid(OperatingSystem operatingSystem)
		{}

		void IParentInterface.MethodVoid<T>(T input)
		{}

		void IParentInterface.MethodVoid<T, U>(T arg1, U arg2)
		{}

		void IParentInterface.MethodVoid<T, U, V>(T arg1, U arg2, V arg3)
		{}


		void IParentInterface.AsyncMethod(Action action)
		{ }
		
		void IParentInterface.AsyncMethod(Action<IEnumerable<Version>> callback)
		{ }

		void IParentInterface.AsyncMethod(Version version, Action action)
		{ }

		string IParentInterface.Echo(string stringToEcho)
		{
			return stringToEcho;
		}

		bool IParentInterface.Method()
		{
			throw new NotImplementedException();
		}

		bool IParentInterface.Method(int arg)
		{
			throw new NotImplementedException();
		}

		bool IParentInterface.Method<T>(T arg)
		{
			throw new NotImplementedException();
		}

		T IParentInterface.Method<T>()
		{
			throw new NotImplementedException();
		}

		T IParentInterface.Method<T, U>()
		{
			throw new NotImplementedException();
		}

		T IParentInterface.Method<T, U>(U arg)
		{
			throw new NotImplementedException();
		}

		void IParentInterface.Set<T>(string arg1, T arg2)
		{
			throw new NotImplementedException();
		}

		T IParentInterface.Get<T>(string arg)
		{
			throw new NotImplementedException();
		}

		void IDisposable.Dispose()
		{
			
		}
	}
}