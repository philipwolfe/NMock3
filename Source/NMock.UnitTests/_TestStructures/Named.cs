namespace NMockTests._TestStructures
{
	public class Named : INamed
	{
		private readonly string name;

		public Named(string name)
		{
			this.name = name;
		}

		#region INamed Members

		public string GetName()
		{
			return name;
		}

		#endregion
	}
}