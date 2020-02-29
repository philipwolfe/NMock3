#region Using
using System.Data;
using NMock;

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

namespace NMockTests.StubTests
{
	[TestClass]
	public class StubTests
	{
		[Ignore]
		//[TestMethod]
		public void StubTest1()
		{
			MockFactory factory = new MockFactory();
			Mock<DataSet> stub = factory.CreateMock<DataSet>();

			stub.Stub.Out.GetProperty(d => d.DataSetName).WillReturn("DS1");

			string name = stub.MockObject.DataSetName;

			Assert.AreEqual("DS1", name);
		}
	}
}