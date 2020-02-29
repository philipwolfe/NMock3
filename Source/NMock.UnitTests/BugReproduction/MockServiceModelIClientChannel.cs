#region Using

using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.ServiceModel;
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
	public class MockServiceModelIClientChannelTest : AcceptanceTestBase
	{
		[TestMethod]
		public void MockServiceModelIClientChannel()
		{
			Mock<IClientChannel> c = Mocks.CreateMock<IClientChannel>();

			c.Expects.One.Method(_ => _.Open());
			c.MockObject.Open();
		}
	}
}