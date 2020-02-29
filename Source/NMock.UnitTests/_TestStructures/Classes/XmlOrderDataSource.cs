using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMockTests._TestStructures.Classes
{
	public abstract class XmlOrderDataSource : SqlOrderDataSource
	{
		protected abstract string Save(object value);
	}
}
