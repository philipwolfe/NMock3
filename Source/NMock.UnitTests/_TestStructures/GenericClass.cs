namespace NMockTests._TestStructures
{
	public class GenericClass<T> : ParentClass, IGenericInterface<T>
	{
		public virtual T GetT()
		{
			return Val;
		}

		public void SetT(T val)
		{
			Val = val;
		}

		public T Val { get; set; }
	}
}
