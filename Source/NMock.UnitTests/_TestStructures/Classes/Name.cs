using System.Collections.Generic;
using System.Linq;

namespace NMockTests._TestStructures
{
	public struct Name
	{
		public static Name Empty = new Name();

		public Name(string title, string first, string middle, string last, string suffix)
		{
			Title = title;
			First = first;
			Middle = middle;
			Last = last;
			Suffix = suffix;
		}

		public string Title;
		public string First;
		public string Middle;
		public string Last;
		public string Suffix;

		public string FullName
		{
			get
			{
				List<string> parts = new List<string>(new []{Title, First, Middle, Last, Suffix});

				var name = parts.Where(p => !string.IsNullOrEmpty(p));

				return string.Join(" ", name.ToArray());
			}
		}
	}
}
