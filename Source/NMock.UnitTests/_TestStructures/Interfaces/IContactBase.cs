using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMockTests._TestStructures
{
	public interface IContactBase
	{
		#region Properties

		int Id { get; } //ReadOnly

		bool Active { get; set; }
		ContactType ContactType { get; set; }
		Name Name { get; set; } //ReadWrite
		List<string> Phones { get; set; } //ReadWrite

		string Password { set; } //WriteOnly

		//Indexers
		int this[string key] { get; set; }
		decimal this[int key] { get; }
		bool this[byte key] { set; }
		DateTime this[short s, long l] { get; set; }
		//string Item { get; set; }//I am so glad this is a compiler error!!

		#endregion
		//Methods
	}
}
