#region Using

using System;
using NMock;
using NMock.Internal;
using NMock.Monitoring;
using NMock.Proxy;

#endregion

namespace NMockTests._TestStructures
{
	public class TestingMockObjectFactoryA : IMockObjectFactory
	{
		#region IMockObjectFactory Members

		public object CreateMock(MockFactory mockFactory, CompositeType mockedTypes, string name, MockStyle mockStyle, object[] constructorArgs)
		{
			return new Named(GetType().Name);
		}

		public string SaveAssembly()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}