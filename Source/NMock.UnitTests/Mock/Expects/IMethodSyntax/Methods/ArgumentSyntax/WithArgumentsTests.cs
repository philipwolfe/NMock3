#region Using

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
namespace NMockTests.MockTests.ExpectsTests.IMethodSyntaxTests.MethodsTest.ArgumentSyntaxTests35
#else
#if SILVERLIGHT
namespace NMockTests.MockTests.ExpectsTests.IMethodSyntaxTests.MethodsTest.ArgumentSyntaxTestsSL
#else
namespace NMockTests.MockTests.ExpectsTests.IMethodSyntaxTests.MethodsTest.ArgumentSyntaxTests
#endif
#endif
{
	[TestClass]
	public class WithArgumentsTests : BasicTestBase
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
		public void TestMethod1()
		{
			// This seems to be 2 versions of the same thing... 

			//
			// version 1

			var predicateMatcher1 = new PredicateMatcher<string>(key => key == "b");
			var predicateMatcher2 = new PredicateMatcher<DateTime>(data => data > DateTime.Now.AddSeconds(-5));

			var persistenceMock = Factory.CreateMock<IPersistence>();
			persistenceMock.Expects.One.Method(_ => _.Save(null, null)).With(predicateMatcher1, predicateMatcher2);

			persistenceMock.MockObject.Save("b", DateTime.Now);
		}

		[TestMethod]
		public void TestMethod3()
		{
			//
			// version 2

			var persistenceMock = Factory.CreateMock<IPersistence>();
			persistenceMock.Expects.One.Method(_ => _.Save(null, null))
				.With(
					Is.Match<string>(key => key == "b"),
					Is.Match<DateTime>(d => d > DateTime.Now.AddSeconds(-5))
					);

			persistenceMock.MockObject.Save("b", DateTime.Now);
		}

		[TestMethod]
		public void WithArgumentsTest()
		{
			mocks.ForEach(WithArgumentsTest1);
			mocks.ForEach(WithArgumentsTest2);
			instances.ForEach(WithArgumentsTest);
		}

		private void WithArgumentsTest1(Mock<IParentInterface> mock)
		{
			var expectations = new Expectations(4);
			using (Factory.Ordered())
			{
				expectations[0] = mock.Expects.One.Method(_ => _.MethodVoid(0, 0, 0)).With(1, 2, 3);
				expectations[1] = mock.Expects.One.Method(_ => _.MethodVoid(0, 0, 0)).With(Is.EqualTo(4), Is.AtLeast(5), Is.AtMost(6));
				expectations[2] = mock.Expects.One.Method(_ => _.MethodVoid(0, 0, 0)).With(7, Is.AtLeast(8), 9);
				expectations[3] = mock.Expects.One.Method(_ => _.MethodVoid(0, 0, 0)).With(
					new PredicateMatcher<int>(parameter => parameter == 10),
					Is.Match<int>(parameter => parameter == 11),
					Is.Anything);
			}

			mock.MockObject.MethodVoid(1, 2, 3);
			mock.MockObject.MethodVoid(4, 5, 6);
			mock.MockObject.MethodVoid(7, 8, 9);
			mock.MockObject.MethodVoid(10, 11, 12);

			expectations.ForEach(_ => _.Assert());
		}

		private void WithArgumentsTest2(Mock<IParentInterface> mock)
		{
			var expectations = new Expectations(4);
			using (Factory.Ordered())
			{
				expectations[0] = mock.Arrange(_ => _.MethodVoid(0, 0, 0)).With(1, 2, 3);
				expectations[1] = mock.Arrange(_ => _.MethodVoid(0, 0, 0)).With(Is.EqualTo(4), Is.AtLeast(5), Is.AtMost(6));
				expectations[2] = mock.Arrange(_ => _.MethodVoid(0, 0, 0)).With(7, Is.AtLeast(8), 9);
				expectations[3] = mock.Arrange(_ => _.MethodVoid(0, 0, 0)).With(
					new PredicateMatcher<int>(parameter => parameter == 10),
					Is.Match<int>(parameter => parameter == 11),
					Is.Anything);
			}

			mock.MockObject.MethodVoid(1, 2, 3);
			mock.MockObject.MethodVoid(4, 5, 6);
			mock.MockObject.MethodVoid(7, 8, 9);
			mock.MockObject.MethodVoid(10, 11, 12);

			expectations.ForEach(_ => _.Assert());
		}

		private void WithArgumentsTest(IParentInterface instance)
		{
			using (Factory.Ordered())
			{
				Expect.On(instance).One.Method(_ => _.MethodVoid(0, 0, 0)).With(1, 2, 3);
				Expect.On(instance).One.Method(_ => _.MethodVoid(0, 0, 0)).With(Is.EqualTo(4), Is.AtLeast(5), Is.AtMost(6));
				Expect.On(instance).One.Method(_ => _.MethodVoid(0, 0, 0)).With(7, Is.AtLeast(8), 9);
				Expect.On(instance).One.Method(_ => _.MethodVoid(0, 0, 0)).With(
					new PredicateMatcher<int>(parameter => parameter == 10),
					Is.Match<int>(parameter => parameter == 11),
					Is.Anything);
			}

			instance.MethodVoid(1, 2, 3);
			instance.MethodVoid(4, 5, 6);
			instance.MethodVoid(7, 8, 9);
			instance.MethodVoid(10, 11, 12);
		}

	}

	public interface IPersistence
	{
		void Save(string key, object data);
	}
}
