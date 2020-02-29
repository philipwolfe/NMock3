#region Using
#if NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestContext = System.Object;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
#endif

using System;
using NMock;
#endregion

namespace NMock3.Tutorial.FAQ
{
	[TestClass]
	public class PropertyExamples : ExamplesBase
	{
		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void HowToThrowAnExceptionOnAPropertySetter()
		{
			Mock<IContactManagementView> view = Factory.CreateMock<IContactManagementView>();

			view.Expects.One.SetProperty(v => v.Name).ToAnything().Will(Throw.Exception(new InvalidOperationException()));

			view.MockObject.Name = new Name();
		}

		[TestMethod]
		public void HowToUseAMatcherOnAPropertySetter()
		{
			//Mock<IContactManagementView> view = Factory.CreateMock<IContactManagementView>();

			//view.Expects.One.SetProperty(v => v.Name).ToAnything().Will(Throw.Exception(new InvalidOperationException()));

			//view.MockObject.Name = new Name();
		}
	}
}
