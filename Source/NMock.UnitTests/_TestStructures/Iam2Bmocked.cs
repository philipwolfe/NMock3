#region Using

using System;
using System.Data;

#endregion

namespace NMockTests._TestStructures
{
	public interface Iam2Bmocked
	{
		string Read { get; }
		string Write { set; }
		string ReadWrite { get; set; }

		DataSet ReadObject { get; }
		DataSet WriteObject { set; }
		DataSet ReadWriteObject { get; set; }

		string this[int index] { get; set; }
		string this[string name] { get; }
		string this[Guid id] { get; }
		string this[Type type] { get; }
		void ReturnVoid(string s);
		bool ReturnBool(string s);
		string ReturnString(string s);
		int ReturnInt(string s);
		TestValues ReturnEnum(string s);
		IamAreturnValue ReturnInterface(string s);
		TestStruct ReturnStruct(string s);
		int[] ReturnArray(string s);
		object ReturnObject(string s);


		//string this[int idx1, int idx2] { get; set; } //evil >:-<
	}
}