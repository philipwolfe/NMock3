#region Using

using System;
using NMock;
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

#if NetFx35
namespace NMockTests.MockTests.StubTests.IMethodSyntaxTests35
#else
#if SILVERLIGHT
namespace NMockTests.MockTests.StubTests.IMethodSyntaxTestsSL
#else
namespace NMockTests.MockTests.StubTests.IMethodSyntaxTests
#endif
#endif
{
	[TestClass]
	public class GetPropertyTests : BasicTestBase
	{
		[TestInitialize]
		public void Init()
		{
			Initalize();
		}

		[TestCleanup]
		public void Clean()
		{
			Cleanup();
		}

		#region Missing
		[TestMethod]
		public void MissingGetTest()
		{
			mocks.ForEach(MissingGetTest);
		}

		private void MissingGetTest(Mock<IParentInterface> mock)
		{
			//Factory.SuppressUnexpectedAndUnmetExpectations();
			mock.Stub.Out.GetProperty(_ => _.ReadWriteObjectProperty);

			Expect.That(() => { var v = mock.MockObject.ReadWriteObjectProperty; }).Throws<IncompleteExpectationException>();
		}

		[TestMethod]
		public void MissingGetMessageTest()
		{
			mocks.ForEach(MissingGetMessageTest);
		}

		private void MissingGetMessageTest(Mock<IParentInterface> mock)
		{
			mock.Expects.One.GetProperty(_ => _.ReadWriteObjectProperty);

			Expect.That(() => { var v = mock.MockObject.ReadWriteObjectProperty; }).Throws<IncompleteExpectationException>(new StringContainsMatcher(
				string.Format(@"An expectation match was found but the expectation was incomplete.  A return value for property '{0}' on '{1}' mock must be set.", "ReadWriteObjectProperty", mock.Name)));

		}
		#endregion

		#region Get ReadWrite
		[TestMethod]
		public void GetPropertyTest()
		{
			mocks.ForEach(GetPropertyTest);
		}

		private void GetPropertyTest(Mock<IParentInterface> mock)
		{
			mock.Stub.Out.GetProperty(_ => _.ReadWriteObjectProperty, new Version(1, 2, 3, 4));
			Version v = mock.MockObject.ReadWriteObjectProperty;
			Version v2 = mock.MockObject.ReadWriteObjectProperty;

			Assert.IsTrue(v.Major == 1 && v.Minor == 2 && v.Build == 3 && v.Revision == 4);
			Assert.IsTrue(v2.Major == 1 && v2.Minor == 2 && v2.Build == 3 && v2.Revision == 4);
		}

		[TestMethod]
		public void GetPropertyWillReturnTest()
		{
			mocks.ForEach(GetPropertyWillReturnTest);
		}

		private void GetPropertyWillReturnTest(Mock<IParentInterface> mock)
		{
			mock.Stub.Out.GetProperty(_ => _.ReadWriteObjectProperty).WillReturn(new Version(1, 2, 3, 4));
			Version v = mock.MockObject.ReadWriteObjectProperty;

			Assert.IsTrue(v.Major == 1 && v.Minor == 2 && v.Build == 3 && v.Revision == 4);
		}
		#endregion

		#region Get ReadOnly
		[TestMethod]
		public void GetPropertyReadOnlyTest()
		{
			mocks.ForEach(GetPropertyReadOnlyTest);
		}

		private void GetPropertyReadOnlyTest(Mock<IParentInterface> mock)
		{
			mock.Stub.Out.GetProperty(_ => _.ReadOnlyObjectProperty, new Version(1, 2, 3, 4));
			Version v = mock.MockObject.ReadOnlyObjectProperty;

			Assert.IsTrue(v.Major == 1 && v.Minor == 2 && v.Build == 3 && v.Revision == 4);
		}

		[TestMethod]
		public void GetPropertyWillReturnReadOnlyTest()
		{
			mocks.ForEach(GetPropertyWillReturnReadOnlyTest);
		}

		private void GetPropertyWillReturnReadOnlyTest(Mock<IParentInterface> mock)
		{
			mock.Stub.Out.GetProperty(_ => _.ReadOnlyObjectProperty).WillReturn(new Version(1, 2, 3, 4));
			Version v = mock.MockObject.ReadOnlyObjectProperty;

			Assert.IsTrue(v.Major == 1 && v.Minor == 2 && v.Build == 3 && v.Revision == 4);
		}
		#endregion

		#region Multiple Expectations
		[TestMethod]
		public void GetPropertyReadOnlyMultipleTest()
		{
			mocks.ForEach(GetPropertyReadOnlyMultipleTest);
		}

		private void GetPropertyReadOnlyMultipleTest(Mock<IParentInterface> mock)
		{
			mock.Stub.Out.GetProperty(_ => _.ReadOnlyValueProperty).WillReturn(1);
			mock.Stub.Out.GetProperty(_ => _.ReadOnlyValueProperty).Will(Return.Value(2));
			mock.Stub.Out.GetProperty(_ => _.ReadOnlyValueProperty, 3);

			int i = mock.MockObject.ReadOnlyValueProperty;
			Assert.AreEqual(1, i);

			i = mock.MockObject.ReadOnlyValueProperty;
			Assert.AreEqual(1, i);

			i = mock.MockObject.ReadOnlyValueProperty;
			Assert.AreEqual(1, i);

		}
		#endregion

		#region Unexpected
		[TestMethod]
		public void GetPropertyUnexpected()
		{
			//Factory.SuppressUnexpectedAndUnmetExpectations();
			mocks.ForEach(GetPropertyUnexpected);
		}

		private void GetPropertyUnexpected(Mock<IParentInterface> mock)
		{
			mock.Expects.No.GetProperty(_ => _.ReadWriteObjectProperty);

			Expect.That(() => { mock.MockObject.ReadWriteObjectProperty = new Version(1, 1, 1, 1); }).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"Unexpected property setter:
  {0}.ReadWriteObjectProperty = <1.1.1.1>(System.Version)
MockFactory Expectations:
  {0}.ReadWriteObjectProperty [EXPECTED: never CALLED: 0 times]
", mock.Name)));
			mock.ClearExpectations();
		}
		#endregion

		#region Get Indexer
		[TestMethod]
		public void GetPropertyReadWriteIndexerTest()
		{
			mocks.ForEach(GetPropertyReadWriteIndexerTest);
		}

		private void GetPropertyReadWriteIndexerTest(Mock<IParentInterface> mock)
		{
			mock.Stub.Out.GetProperty(_ => _["name"]).WillReturn(3);
			mock.Stub.Out.GetProperty(_ => _["value"], 5);

			Assert.AreEqual(3, mock.MockObject["name"]);
			Assert.AreEqual(5, mock.MockObject["value"]);
		}

		[TestMethod]
		public void GetPropertyReadWriteIndexerWillReturnErrorTest()
		{
			//Factory.SuppressUnexpectedAndUnmetExpectations();
			mocks.ForEach(GetPropertyReadWriteIndexerWillReturnErrorTest);
		}

		private void GetPropertyReadWriteIndexerWillReturnErrorTest(Mock<IParentInterface> mock)
		{
			mock.Stub.Out.GetProperty(_ => _["name"]).WillReturn(3);
			Expect.That(() => { var v = mock.MockObject["name1"]; }).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected property getter:
  {0}[""name1""]
MockFactory Expectations:
  {0}[equal to ""name""] will return <3>(System.Int32) [EXPECTED: Stub CALLED: 0 times]
", mock.Name)));

			Factory.ClearExpectations();
		}

		[TestMethod]
		public void GetPropertyReadWriteIndexerErrorTest()
		{
			//Factory.SuppressUnexpectedAndUnmetExpectations();
			mocks.ForEach(GetPropertyReadWriteIndexerErrorTest);
		}

		private void GetPropertyReadWriteIndexerErrorTest(Mock<IParentInterface> mock)
		{
			mock.Stub.Out.GetProperty(_ => _["value"], 5);
			Expect.That(() => { var v = mock.MockObject["value1"]; }).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected property getter:
  {0}[""value1""]
MockFactory Expectations:
  {0}[equal to ""value""] will return <5>(System.Int32) [EXPECTED: Stub CALLED: 0 times]
", mock.Name)));

			//because this is being stubbed and multiple exceptions are being thrown because of the foreach
			//clear the exceptions and expectatoins
			Factory.ClearExpectations();
		}

		[TestMethod]
		public void GetPropertyReadWriteDoubleIndexerTest()
		{
			mocks.ForEach(GetPropertyReadWriteDoubleIndexerTest);
		}

		private void GetPropertyReadWriteDoubleIndexerTest(Mock<IParentInterface> mock)
		{
			DateTime d = DateTime.Now.AddDays(1);
			DateTime e = DateTime.Now.AddDays(2);
			mock.Stub.Out.GetProperty(_ => _[1, 9L]).WillReturn(d);
			mock.Stub.Out.GetProperty(_ => _[2, 8L], e);

			Assert.AreEqual(d, mock.MockObject[1, 9L]);
			Assert.AreEqual(e, mock.MockObject[2, 8L]);
		}

		[TestMethod]
		public void GetPropertyReadOnlyIndexerTest()
		{
			mocks.ForEach(GetPropertyReadOnlyIndexerTest);
		}

		private void GetPropertyReadOnlyIndexerTest(Mock<IParentInterface> mock)
		{
			Guid g = Guid.NewGuid();
			Guid h = Guid.NewGuid();
			mock.Stub.Out.GetProperty(_ => _[4L]).WillReturn(g);
			mock.Stub.Out.GetProperty(_ => _[5L], h);

			Assert.AreEqual(g, mock.MockObject[4L]);
			Assert.AreEqual(h, mock.MockObject[5L]);
		}

		[TestMethod]
		public void GetPropertyReadOnlyDoubleIndexerTest()
		{
			mocks.ForEach(GetPropertyReadOnlyDoubleIndexerTest);
		}

		private void GetPropertyReadOnlyDoubleIndexerTest(Mock<IParentInterface> mock)
		{
			TimeSpan t = new TimeSpan(1, 2, 3);
			TimeSpan u = new TimeSpan(4, 5, 6);
			mock.Stub.Out.GetProperty(_ => _[true, 2]).WillReturn(t);
			mock.Stub.Out.GetProperty(_ => _[true, 7], u);

			Assert.AreEqual(t, mock.MockObject[true, 2]);
			Assert.AreEqual(u, mock.MockObject[true, 7]);
		}
		#endregion

	}
}
