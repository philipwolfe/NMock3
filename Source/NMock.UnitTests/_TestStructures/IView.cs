#region Using

using System;

#endregion

namespace NMockTests
{
	public interface IView
	{
		int Count { get; set; }
		string SearchText { get; }
		event EventHandler Search;
		void OnSearch();
	}
}