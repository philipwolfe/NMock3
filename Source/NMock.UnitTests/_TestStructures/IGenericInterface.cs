namespace NMockTests._TestStructures
{
	public interface IGenericInterface<T> : IParentInterface
	{
		T GetT();
		void SetT(T val);

		T Val { get; set; }
	}
}
