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
	public class SetPropertyTests : BasicTestBase
	{
		[TestInitialize]
		public void Init()
		{
			Initalize();
		}

		[TestCleanup]
		public void TestClean()
		{
			Cleanup();
		}

		[TestMethod]
		public void MissingSetTest()
		{
			mocks.ForEach(MissingSetTest);
		}

		private void MissingSetTest(Mock<IParentInterface> mock)
		{
			mock.Expects.One.SetProperty(_ => _.ReadWriteObjectProperty);
			Expect.That(() => mock.MockObject.ReadWriteObjectProperty = new Version(1, 1, 1, 1)).Throws<IncompleteExpectationException>(new StringContainsMatcher(string.Format(@"A property is missing a matcher on the mock: {0}", mock.Name)));
			mock.ClearExpectations();
		}

		[TestMethod]
		public void SetToValueTypeTest()
		{
			mocks.ForEach(SetToValueTypeTest);
		}

		private void SetToValueTypeTest(Mock<IParentInterface> mock)
		{
			mock.Expects.One.SetProperty(_ => _.ReadWriteObjectProperty).To(Is.TypeOf<Version>());
			mock.Expects.One.SetProperty(_ => _.IsReadOnly).To(Is.TypeOf<bool>());

			mock.MockObject.ReadWriteObjectProperty = new Version(1,2,3,4);
			mock.MockObject.IsReadOnly = true;
		}

		[TestMethod]
		public void SetToObjectTest()
		{
			mocks.ForEach(SetToObjectTest);
		}

		private void SetToObjectTest(Mock<IParentInterface> mock)
		{
			Version version = new Version(1,1,1,1);
			mock.Expects.One.SetProperty(_ => _.ReadWriteObjectProperty).To(version);
			mock.MockObject.ReadWriteObjectProperty = version;
		}

		[TestMethod]
		public void SetToAnythingTest()
		{
			mocks.ForEach(SetToAnythingTest);
		}

		private void SetToAnythingTest(Mock<IParentInterface> mock)
		{
			mock.Expects.One.SetProperty(_ => _.ReadWriteObjectProperty).ToAnything();
			mock.MockObject.ReadWriteObjectProperty = new Version(1,1,1,1);
		}

		[TestMethod]
		public void SetToMatcherTest()
		{
			mocks.ForEach(SetToMatcherTest);
		}

		private void SetToMatcherTest(Mock<IParentInterface> mock)
		{
			Version version = new Version(1,1,1,1);
			mock.Expects.One.SetProperty(_ => _.ReadWriteObjectProperty).To(Is.EqualTo(version));
			mock.MockObject.ReadWriteObjectProperty = version;
		}

		[TestMethod]
		public void SetWriteOnlyToTest()
		{
			mocks.ForEach(SetWriteOnlyToTest);
		}

		private void SetWriteOnlyToTest(Mock<IParentInterface> mock)
		{
			Version version = new Version(1,1,1,1);
			mock.Expects.One.SetProperty(_ => _.WriteOnlyObjectProperty = null).To(version);
			mock.MockObject.WriteOnlyObjectProperty = version;
		}

		[TestMethod]
		public void SetReadWriteIndexerTest()
		{
			mocks.ForEach(SetReadWriteIndexerTest);
		}

		private void SetReadWriteIndexerTest(Mock<IParentInterface> mock)
		{
			int i = 54;
			mock.Expects.One.SetProperty(_ => _["A"]).To(i);
			mock.MockObject["A"] = i;
		}

		[TestMethod]
		public void SetReadWriteIndexer2Test()
		{
			mocks.ForEach(SetReadWriteIndexer2Test);
		}

		private void SetReadWriteIndexer2Test(Mock<IParentInterface> mock)
		{
			int i = 54;
			int ignored = 22;
			mock.Expects.One.SetProperty(_ => _["A"] = ignored).To(i);
			mock.MockObject["A"] = i;
		}

		[TestMethod]
		public void SetReadWriteIndexerErrorTest()
		{
			mocks.ForEach(SetReadWriteIndexerErrorTest);
		}

		private void SetReadWriteIndexerErrorTest(Mock<IParentInterface> mock)
		{
			int i = 54;
			mock.Expects.One.SetProperty(_ => _["A"]).To(i);

			Expect.That(() => mock.MockObject["A"] = 53).Throws<UnexpectedInvocationException>(new StringContainsMatcher(string.Format(@"
Unexpected property setter:
  {0}[""A""] = <53>(System.Int32)
MockFactory Expectations:
  {0}[equal to ""A""] = (equal to <54>(System.Int32)) will capture the setter value [EXPECTED: 1 time CALLED: 0 times]", mock.Name)));

			mock.ClearExpectations();
		}

		[TestMethod]
		public void SetReadWriteDoubleIndexerTest()
		{
			mocks.ForEach(SetReadWriteDoubleIndexerTest);
		}

		private void SetReadWriteDoubleIndexerTest(Mock<IParentInterface> mock)
		{
			short s = 5;
			long l = 100000000;
			DateTime d = DateTime.Today;
			mock.Expects.One.SetProperty(_ => _[s, l]).To(d);

			mock.MockObject[s, l] = d;
		}

		[TestMethod]
		public void SetReadWriteDoubleIndexer2Test()
		{
			mocks.ForEach(SetReadWriteDoubleIndexer2Test);
		}

		private void SetReadWriteDoubleIndexer2Test(Mock<IParentInterface> mock)
		{
			short s = 5;
			long l = 100000000;
			DateTime d = DateTime.Today;
			DateTime IGNORED = DateTime.Now;
			mock.Expects.One.SetProperty(_ => _[s, l] = IGNORED).To(d);

			mock.MockObject[s, l] = d;
		}

		[TestMethod]
		public void SetReadWriteDoubleIndexerErrorTest()
		{
			mocks.ForEach(SetReadWriteDoubleIndexerErrorTest);
		}

		private void SetReadWriteDoubleIndexerErrorTest(Mock<IParentInterface> mock)
		{
			//Factory.SuppressUnexpectedAndUnmetExpectations();

			short s = 5;
			long l = 100000000;
			DateTime d = DateTime.Today;
			mock.Expects.One.SetProperty(_ => _[s, l]).To(d);

			try
			{
				mock.MockObject[s, l - 1] = d;
			}
			catch(Exception ex)
			{
				Assert.IsTrue(ex is UnexpectedInvocationException);
			}
		}

		[TestMethod]
		public void SetWriteOnlyIndexerTest()
		{
			mocks.ForEach(SetWriteOnlyIndexerTest);
		}

		private void SetWriteOnlyIndexerTest(Mock<IParentInterface> mock)
		{
			byte b = 0x33;

			mock.Expects.One.SetProperty(_ => _[b] = false).To(true);
			mock.MockObject[b] = true;
		}

		[TestMethod]
		public void SetWriteOnlyDoubleIndexerTest()
		{
			mocks.ForEach(SetWriteOnlyDoubleIndexerTest);
		}

		private void SetWriteOnlyDoubleIndexerTest(Mock<IParentInterface> mock)
		{
			double d = 3;

			mock.Expects.One.SetProperty(_ => _[4, true] = 1).To(d);
			mock.MockObject[4, true] = d;
		}

		[TestMethod]
		public void SetWriteOnlyIndexerErrorTest()
		{
			mocks.ForEach(SetWriteOnlyIndexerErrorTest);
		}

		private void SetWriteOnlyIndexerErrorTest(Mock<IParentInterface> mock)
		{
			//Factory.SuppressUnexpectedAndUnmetExpectations();
			byte b = 0x33;

			mock.Expects.One.SetProperty(_ => _[b] = false).To(true);

			try
			{
				mock.MockObject[b] = false;
			}
			catch(Exception ex)
			{
				Assert.IsTrue(ex is UnexpectedInvocationException);
			}
		}
	}
}