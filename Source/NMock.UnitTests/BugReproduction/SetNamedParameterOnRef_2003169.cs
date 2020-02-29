#region Using

using System;
using System.Data;
using System.Runtime.CompilerServices;
using NMock;
using NMock.AcceptanceTests;
using NMock.Matchers;
using NMockTests._TestStructures;
using Is = NMock.Is;

#if NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;

#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
#endif

#endregion

namespace NMock.AcceptanceTests
{
	[TestClass]
	public class SetNamedParameterOnRef_2003169 : AcceptanceTestBase
	{
		[TestMethod]
		[Ignore]
		public void Test1()
		{
			// _cSelect is a COM wrapper generated interface
			// We need to know the COM object
			// No solution yet.
			// 12-JUL-2008 Thomas

			//MockFactory mocks = new MockFactory();
			//_cSelect mock = Mocks.CreateMock<_cSelect>();
			//int a = 0;
			//Array b = Array.CreateInstance(typeof(Int32), 0);
			//Array c = Array.CreateInstance(typeof(String), 0);
			//Expect.Once.On(mock).Method("GetSelected").Will(
			//new SetNamedParameterAction("NumberItems", a),
			//new SetNamedParameterAction("ObjectType", b),
			//new SetNamedParameterAction("ObjectName", c),
			//Return.Value(0));

			//mock.GetSelected(ref a, ref b, ref c);
			//mocks.VerifyAllExpectationsHaveBeenMet();
		}
	}
}