namespace NMock.AcceptanceTests
{
	public interface IGenericHelloWorld : IHelloWorld
	{
		T Method<T>();
		T Method<T, U>();
		bool Save<T>(T persistentObject);

		T ReadVariable<T>(string name);
		void SetVariable<T>(string name, T val);

		T Method<U, T>(U value);
	}

	public abstract class GenericHelloWorld : IGenericHelloWorld
	{
		#region IGenericHelloWorld members

		public abstract T Method<T>();
		public abstract T Method<T, U>();
		public abstract bool Save<T>(T persistentObject);

		public abstract T ReadVariable<T>(string name);
		public abstract void SetVariable<T>(string name, T val);

		public abstract T Method<U, T>(U value);

		#endregion

		#region IHelloWorld members

		public abstract void Hello();
		public abstract void Umm();
		public abstract void Err();
		public abstract void Ahh();
		public abstract void Goodbye();
		public abstract string Ask(string question);

		#endregion
	}
}