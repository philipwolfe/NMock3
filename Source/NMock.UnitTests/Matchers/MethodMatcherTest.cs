#region Using

using System;
using NMock;
using NMockTests.MockTests;
using NMockTests._TestStructures;
using Is = NMock.Is;

#if NUNIT
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

#if NetFx35
namespace NMockTests.Matchers35
#else
#if SILVERLIGHT
namespace NMockTests.MatchersSL
#else
namespace NMockTests.Matchers
#endif
#endif
{
	[TestClass]
	public class MethodMatcherTest : BasicTestBase
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
		public void GenericMethodsTest()
		{
			mocks.ForEach(GenericMethodsTest);
		}

		private void GenericMethodsTest(Mock<IParentInterface> mock)
		{
			var obj = new object();
			var guid = Guid.NewGuid();

			mock.Expects.One.Method(_ => _.MethodVoid(null as object)).With(obj);
			mock.Expects.One.Method(_ => _.MethodVoid(null as object)).With(null);
			mock.Expects.One.Method(_ => _.MethodVoid<object>(null)).With(null);
			mock.Expects.One.Method(_ => _.MethodVoid(3, "")).With(3, "");
			mock.Expects.One.Method(_ => _.MethodVoid(3, "")).With(3, null);
			mock.Expects.One.Method(_ => _.MethodVoid());
			mock.Expects.One.Method(_ => _.MethodVoid(3, "")).WithAnyArguments();
			mock.Expects.One.Method(_ => _.MethodVoid<int?, string>(3, "")).With(null, null);
			mock.Expects.One.Method(_ => _.MethodVoid(null as object, null as string, Guid.Empty)).With(null, null, guid);

			mock.Expects.One.MethodWith(_ => _.MethodVoid(obj));
			mock.Expects.One.MethodWith(_ => _.MethodVoid(null as object));
			mock.Expects.One.MethodWith(_ => _.MethodVoid<object>(null));
			mock.Expects.One.MethodWith(_ => _.MethodVoid(3, ""));
			mock.Expects.One.MethodWith(_ => _.MethodVoid<int, string>(3, null));
			mock.Expects.One.MethodWith(_ => _.MethodVoid());
			mock.Expects.One.MethodWith(_ => _.MethodVoid<int?, string>(null, null));
			mock.Expects.One.MethodWith(_ => _.MethodVoid(null as object, null as string, guid));

			mock.MockObject.MethodVoid(obj);
			mock.MockObject.MethodVoid(null as object);
			mock.MockObject.MethodVoid(null as object);
			mock.MockObject.MethodVoid(3, "");
			mock.MockObject.MethodVoid<int, string>(3, null);
			mock.MockObject.MethodVoid();
			mock.MockObject.MethodVoid(4, "Hello");
			mock.MockObject.MethodVoid<int?, string>(null, null);
			mock.MockObject.MethodVoid(null as object, null as string, guid);

			mock.MockObject.MethodVoid(obj);
			mock.MockObject.MethodVoid(null as object);
			mock.MockObject.MethodVoid(null as object);
			mock.MockObject.MethodVoid(3, "");
			mock.MockObject.MethodVoid<int, string>(3, null);
			mock.MockObject.MethodVoid();
			mock.MockObject.MethodVoid<int?, string>(null, null);
			mock.MockObject.MethodVoid(null as object, null as string, guid);
		}
	}
}