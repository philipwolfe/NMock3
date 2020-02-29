using System.Collections;

namespace NMockTests._TestStructures
{
	public class SampleClassWithGenericMethods
	{
		public virtual string GetStringValue<T>(T input)
		{
			return input.ToString();
		}

		public virtual int GetCount<T>(T list) where T : IList
		{
			return list.Count;
		}
	}
}