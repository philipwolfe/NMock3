#region Using

using NMock;
using NMock.Actions;

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

#if NetFx35
namespace NMockTests.Actions35
#else
#if SILVERLIGHT
namespace NMockTests.ActionsSL
#else
namespace NMockTests.Actions
#endif
#endif
{
	[TestClass]
	public class FireActionTest
	{
		public delegate void BellListener(string who);

		private void Salivate(string who)
		{
			dog = who;
		}

		private string dog;

		public interface IBell
		{
			void Ring();
			event BellListener Listeners;
		}

		[TestMethod]
		public void FiresEventOnInvocationReceiver()
		{
			MockFactory factory = new MockFactory();
			Mock<IBell> mock = factory.CreateMock<IBell>();

			DelegateInvoker invoker = mock.Expects.One.DelegateBinding(_ => _.Listeners += Salivate);

			mock.MockObject.Listeners += Salivate;

			invoker.Invoke("Rover");

			Assert.AreEqual("Rover", dog);
			factory.VerifyAllExpectationsHaveBeenMet();
		}

		[TestMethod]
		public void HasAReadableDescription()
		{
			IAction action = new FireAction("MyEvent", 123);

			AssertDescription.IsEqual(action, "fire MyEvent");
		}
	}
}