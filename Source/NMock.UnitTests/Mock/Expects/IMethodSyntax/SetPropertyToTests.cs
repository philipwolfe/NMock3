#region Using

using System;
using NMock.Matchers;
using NMockTests._TestStructures;
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
	public class SetPropertyToToTests : BasicTestBase
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

		[TestMethod]
		public void SetToObjectTest()
		{
			mocks.ForEach(SetToObject1Test);
			mocks.ForEach(SetToObject2Test);
		}

		private void SetToObject1Test(Mock<IParentInterface> mock)
		{
			var v = new Version(1, 2, 3, 4);
			mock.Expects.One.SetPropertyTo(_ => _.ReadWriteObjectProperty = v);
			mock.MockObject.ReadWriteObjectProperty = v;
		}

		private void SetToObject2Test(Mock<IParentInterface> mock)
		{
			var v = new Version(1, 2, 3, 4);
			mock.Arrange(_ => _.ReadWriteObjectProperty = v);
			mock.MockObject.ReadWriteObjectProperty = v;
		}

		[TestMethod]
		public void SetWriteOnlyToTest()
		{
			mocks.ForEach(SetWriteOnlyTo1Test);
			mocks.ForEach(SetWriteOnlyTo2Test);
		}

		private void SetWriteOnlyTo1Test(Mock<IParentInterface> mock)
		{
			var v = new Version(1, 2, 3, 4);
			mock.Expects.One.SetPropertyTo(_ => _.WriteOnlyObjectProperty = v);
			mock.MockObject.WriteOnlyObjectProperty = v;
		}

		private void SetWriteOnlyTo2Test(Mock<IParentInterface> mock)
		{
			var v = new Version(1, 2, 3, 4);
			mock.Arrange(_ => _.WriteOnlyObjectProperty = v);
			mock.MockObject.WriteOnlyObjectProperty = v;
		}

		[TestMethod]
		public void SetReadWriteIndexerTest()
		{
			mocks.ForEach(SetReadWriteIndexer1Test);
			mocks.ForEach(SetReadWriteIndexer2Test);
		}

		private void SetReadWriteIndexer1Test(Mock<IParentInterface> mock)
		{
			var i = 54;
			mock.Expects.One.SetPropertyTo(_ => _["A"] = i);
			mock.MockObject["A"] = i;
		}

		private void SetReadWriteIndexer2Test(Mock<IParentInterface> mock)
		{
			var i = 54;
			mock.Arrange(_ => _["A"] = i);
			mock.MockObject["A"] = i;
		}

		[TestMethod]
		public void UseGetterInSetPropertyToErrorTest()
		{
			mocks.ForEach(UseGetterInSetPropertyToError1Test);
		}

		private void UseGetterInSetPropertyToError1Test(Mock<IParentInterface> mock)
		{
			Expect.That(() => mock.Expects.One.SetPropertyTo(_ => { var a = _["A"]; })).Throws<InvalidOperationException>(new StringContainsMatcher("Using a property as a getter in the SetPropertyTo method is not supported."));
			mock.ClearExpectations();
		}

		[TestMethod]
		public void UnexpectedReadWriteIndexerErrorTest()
		{
			mocks.ForEach(UnexpectedReadWriteIndexerError1Test);
			mocks.ForEach(UnexpectedReadWriteIndexerError2Test);
		}

		private void UnexpectedReadWriteIndexerError1Test(Mock<IParentInterface> mock)
		{
			var i = 54;
			mock.Expects.One.SetPropertyTo(_ => _["A"] = i);

			Expect.That(() => mock.MockObject["A"] = 53).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected property setter:
  {0}[""A""] = <53>(System.Int32)
MockFactory Expectations:
  {0}[equal to ""A""] = (equal to <54>(System.Int32)) [EXPECTED: 1 time CALLED: 0 times]
", mock.Name)));

			mock.ClearExpectations();
		}

		private void UnexpectedReadWriteIndexerError2Test(Mock<IParentInterface> mock)
		{
			var i = 54;
			mock.Arrange(_ => _["A"] = i);

			Expect.That(() => mock.MockObject["A"] = 53).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected property setter:
  {0}[""A""] = <53>(System.Int32)
MockFactory Expectations:
  {0}[equal to ""A""] = (equal to <54>(System.Int32)) [EXPECTED: 1 time CALLED: 0 times]
", mock.Name)));

			mock.ClearExpectations();
		}

		[TestMethod]
		public void SetReadWriteDoubleIndexerTest()
		{
			mocks.ForEach(SetReadWriteDoubleIndexer1Test);
			mocks.ForEach(SetReadWriteDoubleIndexer2Test);
		}

		private void SetReadWriteDoubleIndexer1Test(Mock<IParentInterface> mock)
		{
			short s = 5;
			long l = 100000000;
			var d = DateTime.Today;
			mock.Expects.One.SetPropertyTo(_ => _[s, l] = d);

			mock.MockObject[s, l] = d;
		}

		private void SetReadWriteDoubleIndexer2Test(Mock<IParentInterface> mock)
		{
			short s = 5;
			long l = 100000000;
			var d = DateTime.Today;
			mock.Arrange(_ => _[s, l] = d);

			mock.MockObject[s, l] = d;
		}

		[TestMethod]
		public void SetReadWriteDoubleIndexerErrorTest()
		{
			mocks.ForEach(SetReadWriteDoubleIndexerError1Test);
			mocks.ForEach(SetReadWriteDoubleIndexerError2Test);
		}

		private void SetReadWriteDoubleIndexerError1Test(Mock<IParentInterface> mock)
		{
			short s = 5;
			long l = 100000000;
			DateTime d = DateTime.Today;
			mock.Expects.One.SetPropertyTo(_ => _[s, l] = d);

			Expect.That(() => mock.MockObject[s, l - 1] = d).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected property setter:
  {0}[<5>(System.Int16), <99999999>(System.Int64)] = <{1}>(System.DateTime)
MockFactory Expectations:
  {0}[equal to <5>(System.Int16), equal to <100000000>(System.Int64)] = (equal to <{1}>(System.DateTime)) [EXPECTED: 1 time CALLED: 0 times]", mock.Name, DateTime.Today)));

			mock.ClearExpectations();
		}

		private void SetReadWriteDoubleIndexerError2Test(Mock<IParentInterface> mock)
		{
			short s = 5;
			long l = 100000000;
			DateTime d = DateTime.Today;
			mock.Arrange(_ => _[s, l] = d);

			Expect.That(() => mock.MockObject[s, l - 1] = d).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected property setter:
  {0}[<5>(System.Int16), <99999999>(System.Int64)] = <{1}>(System.DateTime)
MockFactory Expectations:
  {0}[equal to <5>(System.Int16), equal to <100000000>(System.Int64)] = (equal to <{1}>(System.DateTime)) [EXPECTED: 1 time CALLED: 0 times]", mock.Name, DateTime.Today)));

			mock.ClearExpectations();
		}

		[TestMethod]
		public void SetWriteOnlyIndexerTest()
		{
			mocks.ForEach(SetWriteOnlyIndexer1Test);
			mocks.ForEach(SetWriteOnlyIndexer2Test);
		}

		private void SetWriteOnlyIndexer1Test(Mock<IParentInterface> mock)
		{
			byte b = 0x33;

			mock.Expects.One.SetPropertyTo(_ => _[b] = true);
			mock.MockObject[b] = true;
		}

		private void SetWriteOnlyIndexer2Test(Mock<IParentInterface> mock)
		{
			byte b = 0x33;

			mock.Arrange(_ => _[b] = true);
			mock.MockObject[b] = true;
		}

		[TestMethod]
		public void SetWriteOnlyIndexerErrorTest()
		{
			mocks.ForEach(SetWriteOnlyIndexerError1Test);
			mocks.ForEach(SetWriteOnlyIndexerError2Test);
		}

		private void SetWriteOnlyIndexerError1Test(Mock<IParentInterface> mock)
		{
			byte b = 0x33;
			mock.Expects.One.SetPropertyTo(_ => _[b] = true);

			Expect.That(() => mock.MockObject[b] = false).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected property setter:
  {0}[<51>(System.Byte)] = <False>(System.Boolean)
MockFactory Expectations:
  {0}[equal to <51>(System.Byte)] = (equal to <True>(System.Boolean)) [EXPECTED: 1 time CALLED: 0 times]
", mock.Name)));

			mock.ClearExpectations();
		}

		private void SetWriteOnlyIndexerError2Test(Mock<IParentInterface> mock)
		{
			byte b = 0x33;
			mock.Arrange(_ => _[b] = true);

			Expect.That(() => mock.MockObject[b] = false).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected property setter:
  {0}[<51>(System.Byte)] = <False>(System.Boolean)
MockFactory Expectations:
  {0}[equal to <51>(System.Byte)] = (equal to <True>(System.Boolean)) [EXPECTED: 1 time CALLED: 0 times]
", mock.Name)));

			mock.ClearExpectations();
		}
	}
}