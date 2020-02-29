#region Using

using System;
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
using NMockTests._TestStructures;
using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
#endif

#endregion

namespace NMockTests
{
	[TestClass]
	public class FakeTests
	{

		[TestMethod]
		public void FakeTest1()
		{
			//arrange
			var fake = new Fake<IParentInterface>();
			var presenter = new TestPresenter(fake.MockObject);

			//act
			presenter.SetPropertyOfMockToInternalValue();
			presenter.SetIndexerPropertyOfMockToInternalValue();
			presenter.BeginInvoke();
			presenter.HookUpStandardEvent1();

			//assert
			fake.PropertyIs(_ => _.ReadWriteObjectProperty = new Version(5, 6, 7, 8));
			fake.Property(_ => _.ReadWriteObjectProperty).Is(new Version(5, 6, 7, 8));

		}
	}
}
