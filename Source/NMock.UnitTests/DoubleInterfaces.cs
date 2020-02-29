#region Using
using System;
using System.Collections;
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

namespace NMockTests
{
	[TestClass]
	public class DoubleInterfaces
	{
		#region Setup/Teardown

		[TestInitialize]
		public void TestInit()
		{
			_mock = _factory.CreateMock<IDisposable>(new[] {typeof (ICloneable), typeof (IDictionary)});
		}

		[TestCleanup]
		public void TestClean()
		{
			_factory.VerifyAllExpectationsHaveBeenMet();
		}

		#endregion

		private readonly MockFactory _factory;
		private Mock<IDisposable> _mock;

		public DoubleInterfaces()
		{
			_factory = new MockFactory();
		}


		[TestMethod]
		public void Test1()
		{
			Mock<ICloneable> clone = _mock.As<ICloneable>();
			clone.Expects.One.Method(c => c.Clone()).WillReturn(new DataSet());

			Assert.IsTrue(clone.MockObject.Clone().GetType() == typeof (DataSet));
		}

		[TestMethod]
		public void Test2()
		{
			Mock<IList> clone = _mock.As<IList>();
			Assert.IsNull(clone);
		}

		[TestMethod]
		public void Test3()
		{
			Mock<IEnumerable> clone = _mock.As<IEnumerable>();
			clone.Expects.One.Method(c => c.GetEnumerator()).WillReturn(new Stack().GetEnumerator());

			Type type1 = clone.MockObject.GetEnumerator().GetType();
			Type type2 = typeof (IEnumerator);

			Assert.IsTrue(type2.IsAssignableFrom(type1));
		}
	}
}