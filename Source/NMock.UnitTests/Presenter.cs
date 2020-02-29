#region Using

using System;

#endregion

namespace NMockTests
{
	public class Presenter
	{
		private readonly IModel _model;
		private readonly IView _view;

		public Presenter(IView view, IModel model)
		{
			if (view == null)
				throw new ArgumentNullException("view", "view cannot be null.");
			if (model == null)
				throw new ArgumentNullException("model", "model cannot be null.");

			_view = view;
			_model = model;

			Init();
		}

		private void Init()
		{
			_view.Search += _view_Search;
			//_view.Search += (s, e) => { e.ToString(); };
		}

		private void _view_Search(object sender, EventArgs e)
		{
			_view.Count = _model.GetCount(_view.SearchText);
		}
	}
}