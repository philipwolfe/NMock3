#region Using

using System;

#endregion

namespace NMockTests
{
	public class NamedObject
#if !SILVERLIGHT
		: ICloneable
#endif
	{
		private readonly string id;
		private readonly int cloneId;

		public NamedObject(string id) : this(id, 0)
		{
		}

		public NamedObject(string id, int cloneId)
		{
			this.id = id;
			this.cloneId = cloneId;
		}

		public override string ToString()
		{
			return id + (cloneId > 0 ? " (clone " + cloneId + ")" : "");
		}

		public object Clone()
		{
			return new NamedObject(id, cloneId + 1);
		}
	}
}