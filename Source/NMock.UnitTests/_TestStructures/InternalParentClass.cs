using System;
using System.Collections.Generic;

namespace NMockTests._TestStructures
{
	//TODO: should this just inherit ParentClass?
	internal class InternalParentClass : IParentInterface
	{
		public Version ReadWriteObjectProperty { get; set; }
		public bool IsReadOnly { get; set; }
		public Version ReadOnlyObjectProperty { get { return new Version(0, 0, 0, 0); } }
		public Version WriteOnlyObjectProperty { set { } }
		public int ReadOnlyValueProperty { get { return 0; } }
		public int this[string name]
		{
			get
			{
				return int.Parse(name);
			}
			set
			{
			}
		}
		public DateTime this[short s, long l]
		{
			get
			{
				return DateTime.Today.AddDays(s);
			}
			set
			{
			}
		}
		public bool this[byte b]
		{
			set
			{
			}
		}
		public double this[decimal d, bool b]
		{
			set
			{
			}
		}
		public Guid this[long l]
		{
			get
			{
				return Guid.Empty;
			}
		}
		public TimeSpan this[bool b, int i]
		{
			get
			{
				return new TimeSpan(i, i, i, i);
			}
		}

		public event CustomDelegate Delegate;
		public event EventHandler StandardEvent1;
		public event EventHandler<EventArgs> StandardEvent2;
		public event EventHandler<ValueEventArgs> CustomEvent;

		public int Add(int a, int b)
		{
			return DoAdd(a, b);
		}

		protected virtual int DoAdd(int a, int b)
		{
			return a + b;
		}
		public void MethodVoid()
		{}

		public void MethodVoid(List<string> items, int count )
		{}
		
		public void MethodVoid(int r, int g, int b)
		{}

		public void MethodVoid(Version version)
		{ }

		public void MethodVoid(OperatingSystem operatingSystem)
		{ }

		public void MethodVoid<T>(T input)
		{}

		public void MethodVoid<T, U>(T arg1, U arg2)
		{}

		public void MethodVoid<T, U, V>(T arg1, U arg2, V arg3)
			where T : new()
			where U : class
			where V : struct
		{}


		public void AsyncMethod(Action callback)
		{}

		public void AsyncMethod(Action<IEnumerable<Version>> callback)
		{}

		public void AsyncMethod(Version version, Action callback)
		{ }

		public string Echo(string stringToEcho)
		{
			return stringToEcho;
		}

		public bool Method()
		{
			throw new NotImplementedException();
		}

		public bool Method(int arg)
		{
			throw new NotImplementedException();
		}

		public bool Method<T>(T arg)
		{
			throw new NotImplementedException();
		}

		public T Method<T>()
		{
			throw new NotImplementedException();
		}

		public T Method<T, U>()
		{
			throw new NotImplementedException();
		}

		public T Method<T, U>(U arg)
		{
			throw new NotImplementedException();
		}

		public void Set<T>(string arg1, T arg2)
		{
			throw new NotImplementedException();
		}

		public T Get<T>(string arg)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{}
	}
}