using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMockTests._TestStructures
{
	public interface IPersonalContact : IContactBase
	{
		int Age { get; }
		DateTime Birthdate { get; set; }

	}
}
