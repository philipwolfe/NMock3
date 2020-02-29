#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using NMock;
using NMock.AcceptanceTests;
using NMock.Actions;
using NMock.Matchers;
using NMockTests._TestStructures;
using Is = NMock.Is;

#if NUNIT
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;

#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
#endif

#endregion

namespace NMock.Internal
{
	[TestClass]
	public class StubMockStyleDictionaryTest
	{
		#region Setup/Teardown

		[TestInitialize]
		public void SetUp()
		{
			mockFactory = new MockFactory();
		}

		#endregion

		private MockFactory mockFactory;

		/// <summary>
		/// If a mapping is defined for a mock then it can be requested either for the mock or for a type of that mock.
		/// </summary>
		[TestMethod]
		public void MockStyleForStub()
		{
			StubMockStyleDictionary testee = new StubMockStyleDictionary();
			Mock<IMockObject> mock = mockFactory.CreateMock<IMockObject>();

			testee[mock.MockObject] = MockStyle.Stub;

			Assert.AreEqual(MockStyle.Stub, testee[mock.MockObject]);
			Assert.AreEqual(MockStyle.Stub, testee[mock.MockObject, typeof (IHelloWorld)]);
		}

		/// <summary>
		/// Mappings can be set and requested for a type on a mock.
		/// </summary>
		[TestMethod]
		public void MockStyleForStubAndType()
		{
			StubMockStyleDictionary testee = new StubMockStyleDictionary();
			Mock<IMockObject> mock = mockFactory.CreateMock<IMockObject>();

			testee[mock.MockObject, typeof(IHelloWorld)] = MockStyle.Stub;

			Assert.AreEqual(MockStyle.Stub, testee[mock.MockObject, typeof(IHelloWorld)]);
		}

		/// <summary>
		/// A already defined mapping can be overridden.
		/// </summary>
		[TestMethod]
		public void Override()
		{
			StubMockStyleDictionary testee = new StubMockStyleDictionary();
			Mock<IMockObject> mock = mockFactory.CreateMock<IMockObject>();

			testee[mock.MockObject] = MockStyle.Transparent;
			testee[mock.MockObject, typeof(IHelloWorld)] = MockStyle.Stub;

			testee[mock.MockObject, typeof(IHelloWorld)] = null;

			Assert.AreEqual(MockStyle.Transparent, testee[mock.MockObject, typeof(IHelloWorld)]);

			testee[mock.MockObject] = null;

			Assert.IsNull(testee[mock.MockObject]);
			Assert.IsNull(testee[mock.MockObject, typeof(IHelloWorld)]);
		}

		/// <summary>
		/// IF there is no entry for the requested mock and type then null is returned.
		/// </summary>
		[TestMethod]
		public void RequestNonExistingItem()
		{
			StubMockStyleDictionary testee = new StubMockStyleDictionary();
			Mock<IMockObject> mock = mockFactory.CreateMock<IMockObject>();

			Assert.IsNull(testee[mock.MockObject, typeof(IHelloWorld)]);
		}
	}
}