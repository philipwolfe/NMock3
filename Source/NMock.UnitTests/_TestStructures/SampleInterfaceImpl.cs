namespace NMockTests._TestStructures
{
	public class SampleInterfaceImpl : SampleInterfaceImplSuperClass, ISampleInterface
	{
		// non-virtual impl of interface method

		#region ISampleInterface Members

		public int SomeMethod()
		{
			return 7;
		}

		// non-virtual impl of interface generic method
		public int SomeGenericMethod<T>(T input)
		{
			return 1;
		}

		#endregion
	}
}