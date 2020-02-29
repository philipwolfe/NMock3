namespace NMockTests._TestStructures
{
	public interface ISampleInterface
	{
		int SomeMethod();
		int SomeOtherMethod();
		int SomeGenericMethod<T>(T input);
	}
}