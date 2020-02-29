namespace NMockTests._TestStructures
{
	public class Driver
	{
		private readonly Iam2Bmocked _mock;

		public Driver(Iam2Bmocked mock)
		{
			_mock = mock;
		}

		public void Go(string method, string parameter)
		{
			switch (method)
			{
				case "void":
					_mock.ReturnVoid(parameter);
					break;
				case "bool":
					_mock.ReturnBool(parameter);
					break;
				case "string":
					_mock.ReturnString(parameter);
					break;
				case "int":
					_mock.ReturnInt(parameter);
					break;
				case "enum":
					_mock.ReturnEnum(parameter);
					break;
				case "interface":
					_mock.ReturnInterface(parameter);
					break;
				case "struct":
					_mock.ReturnStruct(parameter);
					break;
				case "array":
					_mock.ReturnArray(parameter);
					break;
				case "object":
					_mock.ReturnObject(parameter);
					break;
			}
		}

		public void SetWrite(string value)
		{
			_mock.Write = value;
		}

		public string GetRead()
		{
			return _mock.Read;
		}

		public void SetReadWrite(string value)
		{
			_mock.ReadWrite = value;
		}

		public string GetReadWrite()
		{
			return _mock.ReadWrite;
		}
	}

	public delegate void Action1();
}