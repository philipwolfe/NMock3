#region Using

using System;
using System.Collections.Generic;
using System.Diagnostics;
using NMock;
using NMock.Matchers;
using NMockTests.MockTests;
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
namespace NMockTests._MockFactory35
#else
#if SILVERLIGHT
namespace NMockTests._MockFactorySL
#else
namespace NMockTests._MockFactory
#endif
#endif
{
	[TestClass]
	public class OrderedTests : BasicTestBase
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
		public void OrderedTest1()
		{
			mocks.ForEach(OrderedTest1);
		}

		private void OrderedTest1(Mock<IParentInterface> mock)
		{
			mock.Expects.One.DelegateBinding(_ => _.Delegate += null);
			mock.Expects.One.MethodWith(_ => _.Echo("0")).WillReturn("0");

			using (Factory.Ordered())
			{
				mock.Expects.One.MethodWith(_ => _.Echo("1")).WillReturn("1");
				mock.Expects.One.Method(_ => _.Echo(null)).With(Is.EqualTo("2")).WillReturn("2");
				mock.Expects.One.Method(_ => _.Echo(null)).With("3").WillReturn("3");

				using (Factory.Unordered())
				{
					mock.Expects.One.MethodWith(_ => _.Echo("4")).WillReturn("4");
					mock.Expects.One.Method(_ => _.Echo(null)).With(Is.EqualTo("5")).WillReturn("5");
					mock.Expects.One.Method(_ => _.Echo(null)).With("6").WillReturn("6");
				}

				using (Factory.Ordered())
				{
					//do nothing
				}

				mock.Expects.One.MethodWith(_ => _.Echo("7")).WillReturn("7");
				mock.Expects.One.MethodWith(_ => _.Echo("8")).WillReturn("8");

				using (Factory.Unordered())
				{
					mock.Expects.One.MethodWith(_ => _.Echo("9")).WillReturn("9");
					mock.Expects.One.MethodWith(_ => _.Echo("10")).WillReturn("10");

					using (Factory.Ordered())
					{
						// do nothing
					}
					mock.Expects.One.MethodWith(_ => _.Echo("11")).WillReturn("11");
					mock.Expects.One.MethodWith(_ => _.Echo("12")).WillReturn("12");
				}

				using (Factory.Unordered())
				{
					//do nothing
				}
				mock.Expects.One.MethodWith(_ => _.Echo("13")).WillReturn("13");

			}

			mock.MockObject.Delegate += (s) => { };
			mock.MockObject.Echo("0");
			mock.MockObject.Echo("1");
			mock.MockObject.Echo("2");
			mock.MockObject.Echo("3");
			mock.MockObject.Echo("6");
			mock.MockObject.Echo("4");
			mock.MockObject.Echo("5");
			mock.MockObject.Echo("7");
			mock.MockObject.Echo("8");
			mock.MockObject.Echo("12");
			mock.MockObject.Echo("11");
			mock.MockObject.Echo("10");
			mock.MockObject.Echo("9");
			mock.MockObject.Echo("13");
		}


		[TestMethod]
		public void OrderedTest2()
		{
			mocks.ForEach(OrderedTest2);
		}

		private void OrderedTest2(Mock<IParentInterface> mock)
		{
			using (Factory.Ordered())
			{
				//do nothing
			}
		}

		[TestMethod]
		public void OrderedTest3()
		{
			mocks.ForEach(OrderedTest3);
		}

		private void OrderedTest3(Mock<IParentInterface> mock)
		{
			using (Factory.Ordered())
			{
				mock.Expects.One.Method(_ => _.MethodVoid(null as OperatingSystem)).With(new OSMatcher());
				mock.Expects.One.Method(_ => _.MethodVoid(null as OperatingSystem)).With(new OSMatcher());
				mock.Expects.One.MethodWith(_ => _.MethodVoid(new OperatingSystem(PlatformID.Win32NT, new Version(1,1,1,1))));
			}

			try
			{
				mock.MockObject.MethodVoid(new OperatingSystem(PlatformID.Win32Windows, new Version(1, 1, 1, 1)));
				mock.MockObject.MethodVoid(new OperatingSystem(PlatformID.Win32Windows, new Version(2, 1, 1, 1)));
				mock.MockObject.MethodVoid(new OperatingSystem(PlatformID.Win32Windows, new Version(1, 1, 1, 1)));
			}
			catch(UnexpectedInvocationException ex)
			{
				Assert.AreEqual(@"
Unexpected invocation of:
  System.Void parentInterfaceMock.MethodVoid(<Microsoft Windows 95 2.1.1.1>(System.OperatingSystem))
MockFactory Expectations:
  Ordered {
    System.Void parentInterfaceMock.MethodVoid([OSMatcher]) [EXPECTED: 1 time CALLED: 1 time]
    System.Void parentInterfaceMock.MethodVoid([OSMatcher]) [EXPECTED: 1 time CALLED: 0 times]
    System.Void parentInterfaceMock.MethodVoid(equal to <Microsoft Windows NT 1.1.1.1>(System.OperatingSystem)) [EXPECTED: 1 time CALLED: 0 times]
  }
", ex.Message);
			}
		}

		[TestMethod]
		public void OrderedTest4()
		{
			mocks.ForEach(OrderedTest4);
		}

		private void OrderedTest4(Mock<IParentInterface> mock)
		{
			using(Factory.Ordered())
			{
				mock.Expects.No.MethodWith(_ => _.MethodVoid(new Version(1, 2, 3, 4)));
			}
		}

		private class OSMatcher : Matcher
		{
			public override bool Matches(object o)
			{
				var os = o as OperatingSystem;
				return os != null && os.Version.Major == 1;
			}
		}
	}
}