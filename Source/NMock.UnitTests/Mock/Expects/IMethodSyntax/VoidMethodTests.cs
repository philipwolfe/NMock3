#region Using

using System;
using NMock;
using NMock.Actions;
using NMock.Syntax;
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
	public class VoidMethodTests : BasicTestBase
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
		public void InvokeActionTest1()
		{
			mocks.ForEach(InvokeAction1Test1);
			mocks.ForEach(InvokeAction2Test1);
		}

		private void InvokeAction1Test1(Mock<IParentInterface> mock)
		{
			var mockName = string.Empty;
			mock.Expects.One.Method(_ => _.MethodVoid()).Will(Invoke.Action(() => mockName = mock.Name));

			mock.MockObject.MethodVoid();

			Assert.AreEqual(mock.Name, mockName);
		}

		private void InvokeAction2Test1(Mock<IParentInterface> mock)
		{
			var mockName = string.Empty;
			mock.Arrange(_ => _.MethodVoid()).Will(Invoke.Action(() => mockName = mock.Name));

			mock.MockObject.MethodVoid();

			Assert.AreEqual(mock.Name, mockName);
		}

		[TestMethod]
		public void InvokeActionTest2()
		{
			mocks.ForEach(InvokeAction1Test2);
			mocks.ForEach(InvokeAction2Test2);
		}

		private void InvokeAction1Test2(Mock<IParentInterface> mock)
		{
			var mockName = string.Empty;
			mock.Expects.One.Method(_ => _.MethodVoid()).Will(Invoke.Action(delegate { mockName = mock.Name; }));

			mock.MockObject.MethodVoid();

			Assert.AreEqual(mock.Name, mockName);
		}

		private void InvokeAction2Test2(Mock<IParentInterface> mock)
		{
			var mockName = string.Empty;
			mock.Arrange(_ => _.MethodVoid()).Will(Invoke.Action(delegate { mockName = mock.Name; }));

			mock.MockObject.MethodVoid();

			Assert.AreEqual(mock.Name, mockName);
		}

		private int _expectedCallCount;
		[TestMethod]
		public void InvokeActionTest3()
		{
			mocks.ForEach(InvokeAction1Test3);
			mocks.ForEach(InvokeAction2Test3);
		}

		private void InvokeAction1Test3(Mock<IParentInterface> mock)
		{
			mock.Expects.One.Method(_ => _.MethodVoid()).Will(Invoke.Action(InvokeActionTest3Method));

			mock.MockObject.MethodVoid();

			Assert.AreEqual(++_expectedCallCount, _callCount);
		}

		private void InvokeAction2Test3(Mock<IParentInterface> mock)
		{
			mock.Arrange(_ => _.MethodVoid()).Will(Invoke.Action(InvokeActionTest3Method));

			mock.MockObject.MethodVoid();

			Assert.AreEqual(++_expectedCallCount, _callCount);
		}

		private int _callCount;
		private void InvokeActionTest3Method()
		{
			_callCount++;
		}
	}
}