#region Using
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
	public class EqualTests
	{
		private readonly MockFactory _factory = new MockFactory();
		private Mock<ISaveStuff> _mock;

		[TestMethod]
		public void DifferentObjectTest()
		{
			_mock = _factory.CreateMock<ISaveStuff>();

			DoesStuff o = new DoesStuff(_mock.MockObject);

			Customer c = new Customer();

			_mock.Expects.One.MethodWith(_ => _.SaveCustomer(c));

			Expect.That(o.Button_Click).Throws<UnexpectedInvocationException>();
		}
	}

	public interface ISaveStuff
	{
		void SaveCustomer(Customer customer);
	}

	public class Customer
	{
	}

	public class DoesStuff
	{
		private readonly ISaveStuff _saver;

		public DoesStuff(ISaveStuff saver)
		{
			_saver = saver;
		}

		public void Button_Click()
		{
			Customer c = new Customer();
			_saver.SaveCustomer(c);
		}
	}
}