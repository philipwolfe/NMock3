using System;
using System.Collections.Generic;

namespace NMockTests._TestStructures
{
	public abstract class AbstractParentClass : IParentInterface
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

		public abstract int Add(int a, int b);
		public abstract void MethodVoid();
		public abstract void MethodVoid(List<string> items, int count);
		public abstract void MethodVoid(int r, int g, int b);
		public abstract void MethodVoid(Version version);
		public abstract void MethodVoid(OperatingSystem operatingSystem);
		public abstract void MethodVoid<T>(T input);
		public abstract void MethodVoid<T, U>(T arg1, U arg2);

		public abstract void MethodVoid<T, U, V>(T arg1, U arg2, V arg3)
			where T : new()
			where U : class
			where V : struct;

		public abstract void AsyncMethod(Action callback);
		public abstract void AsyncMethod(Action<IEnumerable<Version>> callback);
		public abstract void AsyncMethod(Version version, Action callback);

		public abstract string Echo(string stringToEcho);

		public abstract bool Method();
		public abstract bool Method(int arg);
		public abstract bool Method<T>(T arg);
		public abstract T Method<T>();
		public abstract T Method<T, U>();
		public abstract T Method<T, U>(U arg);

		public abstract void Set<T>(string arg1, T arg2);
		public abstract T Get<T>(string arg);

		public abstract void Dispose();

	}
}