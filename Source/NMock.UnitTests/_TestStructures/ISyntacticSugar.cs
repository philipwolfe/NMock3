namespace NMockTests._TestStructures
{
	public interface ISyntacticSugar
	{
		string Property { get; set; }
		string this[string s] { get; set; }
		int this[int i, string s] { get; set; }

		event Action1 Actions;
	}
}