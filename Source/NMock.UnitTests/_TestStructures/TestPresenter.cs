using System;
using System.Collections.Generic;
using System.Linq;

namespace NMockTests._TestStructures
{
	public class TestPresenter
	{
		public const string END_INVOKE = "EndInvoke";
		public const string END_INVOKE_COUNT = "EndInvoke Count: {0}";
		public const string SHOW_DIALOG = "Show Dialog";

		public string Status { get; private set; }
		private readonly IParentInterface _mock;

		public TestPresenter(IParentInterface mock)
		{
			_mock = mock;
		}

		public void SetPropertyOfMockToInternalValue()
		{
			_mock.ReadWriteObjectProperty = new Version(5, 6, 7, 8);
		}

		public void SetIndexerPropertyOfMockToInternalValue()
		{
			_mock[4, 5] = new DateTime(2011, 5, 1);
		}

		internal void BeginInvoke()
		{
			_mock.AsyncMethod((Action)EndInvoke);
		}

		private void EndInvoke()
		{
			Status = END_INVOKE;
		}

		internal void BeginIEnumerableInvoke()
		{
			_mock.AsyncMethod(arg => EndInvoke(arg) );
		}

		private void EndInvoke(IEnumerable<Version> versions)
		{
			Status = string.Format(END_INVOKE_COUNT, versions.Count());
		}

		internal void ShowDialog()
		{
			_mock.AsyncMethod(() => { Status = SHOW_DIALOG; });
		}

		internal bool CallMethod(int p)
		{
			return _mock.Method(p);
		}

		internal void CallVoidMethod(Version version)
		{
			_mock.MethodVoid(version);
		}

		internal int GetReadOnlyValueProperty()
		{
			return _mock.ReadOnlyValueProperty;
		}

		internal void AssignWriteOnlyObjectProperty(Version version)
		{
			_mock.WriteOnlyObjectProperty = version;
		}

		internal void HookUpStandardEvent1()
		{
			_mock.StandardEvent1 += (sender, args) => { };
		}
	}
}
