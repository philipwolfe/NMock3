using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMockTests._TestStructures
{
	public interface IChildInterface : IParentInterface
	{
		int AddThenDouble(int a, int b);
	}
}
