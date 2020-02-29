#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NMock;
using NMock.Actions;
using NMock.Matchers;
using NMock.Monitoring;
using NMockTests._TestStructures;
using NMockTests.MockTests;

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
namespace NMockTests.MockTests35
#else
#if SILVERLIGHT
namespace NMockTests.MockTestsSL
#else
namespace NMockTests.MockTests
#endif
#endif
{
	[TestClass]
	public class IgnoreUnexpectedInvocationsTests : BasicTestBase
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
		public void IgnoreUnexpectedInvocationSetTest()
		{
			mocks.ForEach(IgnoreUnexpectedInvocationSetTest);
		}

		private void IgnoreUnexpectedInvocationSetTest(Mock<IParentInterface> mock)
		{
			mock.IgnoreUnexpectedInvocations = true;

			//unexpected set
			mock.MockObject.ReadWriteObjectProperty = new Version(1, 2, 3, 4);

			Assert.IsTrue(true);
		}

		[TestMethod]
		public void DontIgnoreUnexpectedInvocationSetTest()
		{
			mocks.ForEach(DontIgnoreUnexpectedInvocationSetTest);
		}

		private void DontIgnoreUnexpectedInvocationSetTest(Mock<IParentInterface> mock)
		{
			try
			{
				//unexpected set
				mock.MockObject.ReadWriteObjectProperty = new Version(1, 2, 3, 4);
			}
			catch (Exception ex)
			{
				Assert.IsTrue(ex is UnexpectedInvocationException);
			}
		}

		[TestMethod]
		public void IgnoreUnexpectedInvocationGetTest()
		{
			mocks.ForEach(IgnoreUnexpectedInvocationGetTest);
		}

		private void IgnoreUnexpectedInvocationGetTest(Mock<IParentInterface> mock)
		{
			mock.IgnoreUnexpectedInvocations = true;

			mock.Stub.Out.GetProperty(_ => _.ReadOnlyValueProperty).WillReturn(3);

			//unexpected get
			var v = mock.MockObject.ReadOnlyValueProperty;

			Assert.AreEqual(3, v);
		}

		[TestMethod]
		public void DontIgnoreUnexpectedInvocationGetTest()
		{
			mocks.ForEach(DontIgnoreUnexpectedInvocationGetTest);
		}

		private void DontIgnoreUnexpectedInvocationGetTest(Mock<IParentInterface> mock)
		{
			try
			{
				//unexpected get
				var v = mock.MockObject.ReadOnlyValueProperty;
			}
			catch (Exception ex)
			{
				Assert.IsTrue(ex is UnexpectedInvocationException);
			}
		}

		[TestMethod]
		public void IgnoreUnexpectedInvocationIndexerTest()
		{
			mocks.ForEach(IgnoreUnexpectedInvocationIndexerTest);
		}

		private void IgnoreUnexpectedInvocationIndexerTest(Mock<IParentInterface> mock)
		{
			mock.IgnoreUnexpectedInvocations = true;

			Guid e = Guid.NewGuid();

			mock.Stub.Out.GetProperty(_ => _[33L]).WillReturn(e);

			//unexpected get
			var g = mock.MockObject[33L];

			Assert.AreEqual(e, g);
		}

		[TestMethod]
		public void DontIgnoreUnexpectedInvocationIndexerTest()
		{
			mocks.ForEach(DontIgnoreUnexpectedInvocationIndexerTest);
		}

		private void DontIgnoreUnexpectedInvocationIndexerTest(Mock<IParentInterface> mock)
		{
			try
			{
				//unexpected get
				var g = mock.MockObject[33L];
			}
			catch (Exception ex)
			{
				Assert.IsTrue(ex is UnexpectedInvocationException);
			}
		}

		[TestMethod]
		public void IgnoreUnexpectedInvocationMethodTest()
		{
			mocks.ForEach(IgnoreUnexpectedInvocationMethodTest);
		}

		private void IgnoreUnexpectedInvocationMethodTest(Mock<IParentInterface> mock)
		{
			mock.IgnoreUnexpectedInvocations = true;

			Expect.That(() => mock.MockObject.Method(3)).Throws<ExpectationException>(new StringContainsMatcher("requires a return value and can not be ignored using the IgnoreUnexpectedInvocations setting."));
		}

		[TestMethod]
		public void IgnoreUnexpectedInvocationMethodVoidTest()
		{
			mocks.ForEach(IgnoreUnexpectedInvocationMethodVoidTest);
		}

		private void IgnoreUnexpectedInvocationMethodVoidTest(Mock<IParentInterface> mock)
		{
			mock.IgnoreUnexpectedInvocations = true;

			mock.MockObject.MethodVoid(3);
			mock.MockObject.MethodVoid(3);

			Assert.IsNull(Factory.FirstIncompleteExpectationException);

			mock.Expects.One.MethodWith(_ => _.Method<DateTime>(DateTime.MinValue)).WillReturn(true);
			mock.MockObject.Method<DateTime>(DateTime.MinValue);

			Assert.IsTrue(true);
		}

		[TestMethod]
		public void DontIgnoreUnexpectedInvocationMethodVoidTest()
		{
			mocks.ForEach(DontIgnoreUnexpectedInvocationMethodVoidTest);
		}

		private void DontIgnoreUnexpectedInvocationMethodVoidTest(Mock<IParentInterface> mock)
		{
			try
			{
				mock.MockObject.MethodVoid(3);
			}
			catch (Exception e)
			{
				Assert.IsTrue(e is UnexpectedInvocationException);
			}
		}

		//TODO: events


		[TestMethod]
		public void ThrowTheRightExceptionTest()
		{
			var factory = new MockFactory();
			var ignoredMock = factory.CreateMock<IParentInterface>();
			var observedMock = factory.CreateMock<IChildInterface>();

			ignoredMock.IgnoreUnexpectedInvocations = true;

			ignoredMock.MockObject.MethodVoid();

			Expect.That(()=>observedMock.MockObject.MethodVoid(new Version(1,2,3,4))).Throws<UnexpectedInvocationException>(new StringContainsMatcher(@"
Unexpected invocation of:
  System.Void childInterface.MethodVoid(<1.2.3.4>(System.Version))
MockFactory Expectations:
  No expectations have been defined.
"));
		}

		[TestMethod]
		public void TestIgnoredMethodInOrdered1()
		{
			mocks.ForEach(TestIgnoredMethodInOrdered1);
		}

		private void TestIgnoredMethodInOrdered1(Mock<IParentInterface> mock)
		{
			var localMock = Factory.CreateMock<IParentInterface>();
			localMock.IgnoreUnexpectedInvocations = true;

			using (Factory.Ordered())
			{
				mock.Expects.One.Method(_ => _.Method(null as Version)).With(new VersionMatcher(1)).WillReturn(true);

				localMock.MockObject.MethodVoid();
				Assert.IsTrue(mock.MockObject.Method(new Version(1, 2, 3, 4)));
			}
		}

		[TestMethod]
		public void TestIgnoredMethodInOrdered2()
		{
			mocks.ForEach(TestIgnoredMethodInOrdered2);
		}

		private void TestIgnoredMethodInOrdered2(Mock<IParentInterface> mock)
		{
			var localMock = Factory.CreateMock<IParentInterface>();
			localMock.IgnoreUnexpectedInvocations = true;

			using (Factory.Ordered())
			{
				mock.Expects.One.Method(_ => _.Method(null as Version)).With(new VersionMatcher(1)).WillReturn(true);

				Assert.IsTrue(mock.MockObject.Method(new Version(1, 2, 3, 4)));
				localMock.MockObject.MethodVoid();
			}
		}

		[TestMethod]
		public void TestIgnoredMethodInOrdered3()
		{
			mocks.ForEach(TestIgnoredMethodInOrdered3);
		}

		private void TestIgnoredMethodInOrdered3(Mock<IParentInterface> mock)
		{
			var localMock = Factory.CreateMock<IParentInterface>();
			localMock.IgnoreUnexpectedInvocations = true;

			using (Factory.Ordered())
			{
				mock.Expects.One.Method(_ => _.Method(null as Version)).With(new VersionMatcher(1)).WillReturn(true);

				Assert.IsTrue(mock.MockObject.Method(new Version(1, 2, 3, 4)));
			}
			localMock.MockObject.MethodVoid();
		}

		[TestMethod]
		public void TestIgnoredMethodInOrdered4()
		{
			mocks.ForEach(TestIgnoredMethodInOrdered4);
		}

		private void TestIgnoredMethodInOrdered4(Mock<IParentInterface> mock)
		{
			var localMock = Factory.CreateMock<IParentInterface>();
			localMock.IgnoreUnexpectedInvocations = true;

			localMock.MockObject.MethodVoid();
			using (Factory.Ordered())
			{
				mock.Expects.One.Method(_ => _.Method(null as Version)).With(new VersionMatcher(1)).WillReturn(true);

				Assert.IsTrue(mock.MockObject.Method(new Version(1, 2, 3, 4)));
			}
		}

		[TestMethod]
		public void TestIgnoredMethodInOrdered5()
		{
			mocks.ForEach(TestIgnoredMethodInOrdered5);
		}

		private void TestIgnoredMethodInOrdered5(Mock<IParentInterface> mock)
		{
			var localMock = Factory.CreateMock<IParentInterface>();
			localMock.IgnoreUnexpectedInvocations = true;

			localMock.MockObject.MethodVoid();
			using (Factory.Ordered())
			{
				mock.Expects.One.Method(_ => _.Method(null as Version)).With(new VersionMatcher(1)).WillReturn(true);

				localMock.MockObject.MethodVoid();
				Assert.IsTrue(mock.MockObject.Method(new Version(1, 2, 3, 4)));
				localMock.MockObject.MethodVoid();
			}
			localMock.MockObject.MethodVoid();
		}


	}

}