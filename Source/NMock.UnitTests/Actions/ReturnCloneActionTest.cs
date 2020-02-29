#region Using

using System;
using NMock;
using NMock.Actions;
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
#if !SILVERLIGHT
	
	[TestClass]
	public class ReturnCloneActionTest
	{
		private ICloneable ACloneableObject()
		{
			return new NamedObject("a cloneable object");
		}

		[TestMethod]
		public void HasAReadableDescription()
		{
			ICloneable prototype = ACloneableObject();
			AssertDescription.IsEqual(new ReturnCloneAction(prototype),
									  "a clone of <" + prototype + ">(NMockTests.NamedObject)");
		}

		[TestMethod]
		public void ReturnsCloneOfPrototypeObject()
		{
			ICloneable prototype = ACloneableObject();
			IAction action = new ReturnCloneAction(prototype);

			object result = ReturnActionTest.ResultOfAction(action);
			Verify.That(result, !Is.EqualTo(prototype));
		}
	}
#endif
}