namespace NMockTests._TestStructures
{
	public class SampleClassWithObjectOverrides
	{
		public override string ToString()
		{
			return base.ToString();
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}