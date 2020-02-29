namespace NMockTests._TestStructures
{
	public class ChildClass : ParentClass, IChildInterface
	{
		public ChildClass(string name) : base(name)
		{
			
		}

		public int AddThenDouble(int a, int b)
		{
			return DoAdd(a, b) * 2;
		}

	}
}
