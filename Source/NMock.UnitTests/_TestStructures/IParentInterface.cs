using System;
using System.Collections.Generic;

namespace NMockTests._TestStructures
{
	public interface IParentInterface : IDisposable
	{
		Version ReadWriteObjectProperty { get; set; }
		bool IsReadOnly { get; set; }
		Version ReadOnlyObjectProperty { get; }
		Version WriteOnlyObjectProperty { set; }
		int ReadOnlyValueProperty { get; }
		int this[string name] { get; set; }
		DateTime this[short s, long l] { get; set; }
		bool this[byte b] { set; }
		double this[decimal d, bool b] { set; }
		Guid this[long l] { get; }
		TimeSpan this[bool b, int i] { get; }

		event CustomDelegate Delegate;
		event EventHandler StandardEvent1;
		event EventHandler<EventArgs> StandardEvent2;
		event EventHandler<ValueEventArgs> CustomEvent;

		int Add(int a, int b);

		void MethodVoid();
		void MethodVoid(List<string> items, int count);
		void MethodVoid(int r, int g, int b);
		void MethodVoid(Version version);
		void MethodVoid(OperatingSystem operatingSystem);
		void MethodVoid<T>(T input);
		void MethodVoid<T, U>(T arg1, U arg2);

		void MethodVoid<T, U, V>(T arg1, U arg2, V arg3)
			where T : new()
			where U : class
			where V : struct;

		void AsyncMethod(Action callback);
		void AsyncMethod(Action<IEnumerable<Version>> callback);
		void AsyncMethod(Version version, Action callback);

		string Echo(string stringToEcho);

		bool Method();
		bool Method(int arg);
		bool Method<T>(T arg);
		T Method<T>();
		T Method<T, U>();
		T Method<T, U>(U arg);


		void Set<T>(string arg1, T arg2);
		T Get<T>(string arg);

		/*
		 * 		object Complex1(int i, DateTime d, string s, bool b, Guid g, long? l, object o, DataSet ds);
		object Complex2(string s, ref DataSet ds);
		object Complex3(string s, out DataSet ds);
		object Complex4(int i, string s, params Exception[] errors);
		 * */
	}

	public interface IParentInterface<T> : IParentInterface
	{
		T MethodTypeSpecific();
	}

	public delegate void CustomDelegate(string message);

	public class ValueEventArgs : EventArgs
	{
		public string Value { get; set; }
	}
}