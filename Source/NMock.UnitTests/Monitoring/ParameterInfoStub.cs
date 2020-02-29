#region Using

using System.Reflection;

#endregion

namespace NMockTests.Monitoring
{
	public class ParameterInfoStub : ParameterInfo
	{
		public ParameterInfoStub(string name, ParameterAttributes attributes)
		{
			NameImpl = name;
			AttrsImpl = attributes;
		}

#if SILVERLIGHT
		public string NameImpl{get;set;}
		public ParameterAttributes AttrsImpl {get;set;}
#endif
	}
}