#region Using

using System;
using NMock;
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

#if NetFx35
namespace NMockTests.MockTests.ExpectsTests.IMethodSyntaxTests35
#else
#if SILVERLIGHT
namespace NMockTests.MockTests.ExpectsTests.IMethodSyntaxTestsSL
#else
namespace NMockTests.MockTests.ExpectsTests.IMethodSyntaxTests
#endif
#endif
{
	//[TestClass]
	public class NonVoidMethodTests : BasicTestBase
	{
		//[TestInitialize]
		public void Init()
		{
			Initalize();
		}

		//[TestCleanup]
		public void Clean()
		{
			Cleanup();
		}
	}
}