namespace NMockTests._TestStructures
{
	public abstract class SampleClassWithVirtualAndNonVirtualMethods
	{
		public int Subtract(int x, int y) // non-virtual
		{
			return x - y;
		}

		public virtual int Add(int x, int y) // virtual, overloaded
		{
			return x + y;
		}

		public int Add(int x, int y, int z) // non-virtual, overloaded
		{
			return x + y + z;
		}

		public abstract decimal Add(decimal x, decimal y); // abstract, overloaded
	}
}