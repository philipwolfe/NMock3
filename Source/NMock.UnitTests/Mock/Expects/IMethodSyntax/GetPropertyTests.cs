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
namespace NMockTests.MockTests.ExpectsTests.IMethodSyntaxTests35
#else
#if SILVERLIGHT
namespace NMockTests.MockTests.ExpectsTests.IMethodSyntaxTestsSL
#else
namespace NMockTests.MockTests.ExpectsTests.IMethodSyntaxTests
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
			mock.Expects.One.GetProperty(_ => _.ReadWriteObjectProperty);

			Expect.That(() => { var v = mock.MockObject.ReadWriteObjectProperty; }).Throws<IncompleteExpectationException>(new StringContainsMatcher(string.Format(@"A return value for property '{0}' on '{1}' mock must be set.", "ReadWriteObjectProperty", mock.Name)));

			mock.ClearExpectations();
		}
		#endregion

		#region Get ReadWrite
		[TestMethod]
		public void GetPropertyTest()
		{
			mocks.ForEach(GetProperty1Test);
		}

		private void GetProperty1Test(Mock<IParentInterface> mock)
		{
			mock.Expects.One.GetProperty(_ => _.ReadWriteObjectProperty, new Version(1, 2, 3, 4));
			Version v = mock.MockObject.ReadWriteObjectProperty;

			Assert.IsTrue(v.Major == 1 && v.Minor == 2 && v.Build == 3 && v.Revision == 4);
		}

		[TestMethod]
		public void GetPropertyWillReturnTest()
		{
			mocks.ForEach(GetPropertyWillReturn1Test);
			mocks.ForEach(GetPropertyWillReturn2Test);
		}

		private void GetPropertyWillReturn1Test(Mock<IParentInterface> mock)
		{
			mock.Expects.One.GetProperty(_ => _.ReadWriteObjectProperty).WillReturn(new Version(1, 2, 3, 4));
			Version v = mock.MockObject.ReadWriteObjectProperty;

			Assert.IsTrue(v.Major == 1 && v.Minor == 2 && v.Build == 3 && v.Revision == 4);
		}
		private void GetPropertyWillReturn2Test(Mock<IParentInterface> mock)
		{
			mock.Arrange(_ => _.ReadWriteObjectProperty).WillReturn(new Version(1, 2, 3, 4));
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
			mock.Expects.One.GetProperty(_ => _.ReadOnlyObjectProperty, new Version(1, 2, 3, 4));
			Version v = mock.MockObject.ReadOnlyObjectProperty;

			Assert.IsTrue(v.Major == 1 && v.Minor == 2 && v.Build == 3 && v.Revision == 4);
		}

		[TestMethod]
		public void GetPropertyWillReturnReadOnlyTest()
		{
			mocks.ForEach(GetPropertyWillReturnReadOnly1Test);
			mocks.ForEach(GetPropertyWillReturnReadOnly2Test);
		}

		private void GetPropertyWillReturnReadOnly1Test(Mock<IParentInterface> mock)
		{
			mock.Expects.One.GetProperty(_ => _.ReadOnlyObjectProperty).WillReturn(new Version(1, 2, 3, 4));
			Version v = mock.MockObject.ReadOnlyObjectProperty;

			Assert.IsTrue(v.Major == 1 && v.Minor == 2 && v.Build == 3 && v.Revision == 4);
		}
		private void GetPropertyWillReturnReadOnly2Test(Mock<IParentInterface> mock)
		{
			mock.Arrange(_ => _.ReadOnlyObjectProperty).WillReturn(new Version(1, 2, 3, 4));
			Version v = mock.MockObject.ReadOnlyObjectProperty;

			Assert.IsTrue(v.Major == 1 && v.Minor == 2 && v.Build == 3 && v.Revision == 4);
		}
		#endregion

		#region Multiple Expectations
		[TestMethod]
		public void GetPropertyReadOnlyMultipleTest()
		{
			mocks.ForEach(GetPropertyReadOnlyMultiple1Test);
			mocks.ForEach(GetPropertyReadOnlyMultiple2Test);
		}

		private void GetPropertyReadOnlyMultiple1Test(Mock<IParentInterface> mock)
		{
			mock.Expects.One.GetProperty(_ => _.ReadOnlyValueProperty).WillReturn(1);
			mock.Expects.One.GetProperty(_ => _.ReadOnlyValueProperty).Will(Return.Value(2));
			mock.Expects.One.GetProperty(_ => _.ReadOnlyValueProperty, 3);

			int i = mock.MockObject.ReadOnlyValueProperty;
			Assert.AreEqual(1, i);

			i = mock.MockObject.ReadOnlyValueProperty;
			Assert.AreEqual(2, i);

			i = mock.MockObject.ReadOnlyValueProperty;
			Assert.AreEqual(3, i);

		}
		private void GetPropertyReadOnlyMultiple2Test(Mock<IParentInterface> mock)
		{
			mock.Arrange(_ => _.ReadOnlyValueProperty).WillReturn(1);
			mock.Arrange(_ => _.ReadOnlyValueProperty).Will(Return.Value(2));
			mock.Arrange(_ => _.ReadOnlyValueProperty).WillReturn(3);

			int i = mock.MockObject.ReadOnlyValueProperty;
			Assert.AreEqual(1, i);

			i = mock.MockObject.ReadOnlyValueProperty;
			Assert.AreEqual(2, i);

			i = mock.MockObject.ReadOnlyValueProperty;
			Assert.AreEqual(3, i);

		}
		#endregion

		#region Unexpected
		[TestMethod]
		public void GetPropertyUnexpected()
		{
			mocks.ForEach(GetPropertyUnexpected);
		}

		private void GetPropertyUnexpected(Mock<IParentInterface> mock)
		{
			mock.Expects.No.GetProperty(_ => _.ReadWriteObjectProperty);
			Expect.That(() => { mock.MockObject.ReadWriteObjectProperty = new Version(1, 1, 1, 1); }).Throws<UnexpectedInvocationException>();

			mock.ClearExpectations();
		}
		#endregion

		#region Get Indexer
		[TestMethod]
		public void GetPropertyReadWriteIndexerTest()
		{
			mocks.ForEach(GetPropertyReadWriteIndexer1Test);
			mocks.ForEach(GetPropertyReadWriteIndexer2Test);
		}

		private void GetPropertyReadWriteIndexer1Test(Mock<IParentInterface> mock)
		{
			mock.Expects.One.GetProperty(_ => _["name"]).WillReturn(3);
			mock.Expects.One.GetProperty(_ => _["value"], 5);

			Assert.AreEqual(3, mock.MockObject["name"]);
			Assert.AreEqual(5, mock.MockObject["value"]);
		}

		private void GetPropertyReadWriteIndexer2Test(Mock<IParentInterface> mock)
		{
			mock.Arrange(_ => _["name"]).WillReturn(3);
			mock.Arrange(_ => _["value"]).Will(Return.Value(5));

			Assert.AreEqual(3, mock.MockObject["name"]);
			Assert.AreEqual(5, mock.MockObject["value"]);
		}

		[TestMethod]
		public void GetPropertyReadWriteIndexerWillReturnErrorTest()
		{
			mocks.ForEach(GetPropertyReadWriteIndexerWillReturnError1Test);
			mocks.ForEach(GetPropertyReadWriteIndexerWillReturnError2Test);
		}

		private void GetPropertyReadWriteIndexerWillReturnError1Test(Mock<IParentInterface> mock)
		{
			mock.Expects.One.GetProperty(_ => _["name"]).WillReturn(3);
			Expect.That(() => { var i = mock.MockObject["name1"]; }).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected property getter:
  {0}[""name1""]
MockFactory Expectations:
  {0}[equal to ""name""] will return <3>(System.Int32) [EXPECTED: 1 time CALLED: 0 times]
", mock.Name)));

			mock.ClearExpectations();
		}

		private void GetPropertyReadWriteIndexerWillReturnError2Test(Mock<IParentInterface> mock)
		{
			mock.Arrange(_ => _["name"]).WillReturn(3);
			Expect.That(() => { var i = mock.MockObject["name1"]; }).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected property getter:
  {0}[""name1""]
MockFactory Expectations:
  {0}[equal to ""name""] will return <3>(System.Int32) [EXPECTED: 1 time CALLED: 0 times]
", mock.Name)));

			mock.ClearExpectations();
		}

		[TestMethod]
		public void GetPropertyReadWriteIndexerErrorTest()
		{
			mocks.ForEach(GetPropertyReadWriteIndexerError1Test);
			mocks.ForEach(GetPropertyReadWriteIndexerError2Test);
		}

		private void GetPropertyReadWriteIndexerError1Test(Mock<IParentInterface> mock)
		{
			mock.Expects.One.GetProperty(_ => _["value"], 5);
			Expect.That(() => { var i = mock.MockObject["value1"]; }).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected property getter:
  {0}[""value1""]
MockFactory Expectations:
  {0}[equal to ""value""] will return <5>(System.Int32) [EXPECTED: 1 time CALLED: 0 times]
", mock.Name)));

			mock.ClearExpectations();
		}

		private void GetPropertyReadWriteIndexerError2Test(Mock<IParentInterface> mock)
		{
			mock.Expects.One.GetProperty(_ => _["value"], 5);
			Expect.That(() => { var i = mock.MockObject["value1"]; }).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected property getter:
  {0}[""value1""]
MockFactory Expectations:
  {0}[equal to ""value""] will return <5>(System.Int32) [EXPECTED: 1 time CALLED: 0 times]
", mock.Name)));

			mock.ClearExpectations();
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
			mock.Expects.One.GetProperty(_ => _[1, 9L]).WillReturn(d);
			mock.Expects.One.GetProperty(_ => _[2, 8L], e);

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
			mock.Expects.One.GetProperty(_ => _[4L]).WillReturn(g);
			mock.Expects.One.GetProperty(_ => _[5L], h);

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
			mock.Expects.One.GetProperty(_ => _[true, 2]).WillReturn(t);
			mock.Expects.One.GetProperty(_ => _[true, 7], u);

			Assert.AreEqual(t, mock.MockObject[true, 2]);
			Assert.AreEqual(u, mock.MockObject[true, 7]);
		}
		#endregion

		#region Get Internal Value

		[TestMethod]
		public void GetPropertyPreviousSetterTest()
		{
			mocks.ForEach(GetPropertyPreviousSetter1Test);
			mocks.ForEach(GetPropertyPreviousSetter2Test);
		}

		private void GetPropertyPreviousSetter1Test(Mock<IParentInterface> mock)
		{
			var expectations = new Expectations(2);

			expectations[0] = mock.Expects.One.SetProperty(_ => _.ReadWriteObjectProperty).To(Is.TypeOf<Version>());
			expectations[1] = mock.Expects.One.GetProperty(_ => _.ReadWriteObjectProperty).WillReturnSetterValue();

			var presenter = new TestPresenter(mock.MockObject);
			presenter.SetPropertyOfMockToInternalValue();

			Assert.AreEqual(5, mock.MockObject.ReadWriteObjectProperty.Major);

			expectations.ForEach(_=>_.Assert());
		}

		private void GetPropertyPreviousSetter2Test(Mock<IParentInterface> mock)
		{
			var expectations = new Expectations(2);

			expectations[0] = mock.Arrange(_ => _.ReadWriteObjectProperty).To(Is.TypeOf<Version>());
			expectations[1] = mock.Arrange(_ => _.ReadWriteObjectProperty).WillReturnSetterValue();

			var presenter = new TestPresenter(mock.MockObject);
			presenter.SetPropertyOfMockToInternalValue();

			Assert.AreEqual(5, mock.MockObject.ReadWriteObjectProperty.Major);

			expectations.ForEach(_=>_.Assert());
		}

		[TestMethod]
		public void GetPropertyPreviousSetterNonInternal()
		{
			mocks.ForEach(GetPropertyPreviousSetterNonInternal);
		}

		private void GetPropertyPreviousSetterNonInternal(Mock<IParentInterface> mock)
		{
			var version = new Version(2, 3, 4, 5);

			mock.Expects.One.SetProperty(_ => _.ReadWriteObjectProperty).To(version);
			mock.Expects.One.GetProperty(_ => _.ReadWriteObjectProperty).WillReturnSetterValue();

			mock.MockObject.ReadWriteObjectProperty = version;

			Assert.AreEqual(2, mock.MockObject.ReadWriteObjectProperty.Major);
		}

		[TestMethod]
		public void GetPropertyPreviousSetterIndexer()
		{
			mocks.ForEach(GetPropertyPreviousSetterIndexer);
		}

		private void GetPropertyPreviousSetterIndexer(Mock<IParentInterface> mock)
		{
			var presenter = new TestPresenter(mock.MockObject);

			mock.Expects.One.SetProperty(_ => _[4, 5]).To(Is.TypeOf<DateTime>());
			mock.Expects.One.GetProperty(_ => _[4, 5]).WillReturnSetterValue();

			presenter.SetIndexerPropertyOfMockToInternalValue();

			Assert.AreEqual(new DateTime(2011, 5, 1), mock.MockObject[4, 5]);

			//just checking if it can be done twice
			mock.Expects.One.GetProperty(_ => _[4, 5]).WillReturnSetterValue();
			Assert.AreEqual(new DateTime(2011, 5, 1), mock.MockObject[4, 5]);
		}

		#endregion

	}
}