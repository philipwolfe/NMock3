using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMock;
using NMockTests._TestStructures;

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
	public class CustomMatchersTest
	{
		[TestMethod]
		public void TwoMethodsTest()
		{
			var factory = new MockFactory();
			var mock = factory.CreateMock<IParentInterface>();

			mock.Expects.One.Method(_ => _.MethodVoid(null as Version)).With(new VersionMatcher());
			mock.Expects.One.Method(_ => _.MethodVoid(null as OperatingSystem)).With(new OSMatcher());

			mock.MockObject.MethodVoid(new OperatingSystem(PlatformID.Xbox, new Version(1,1,1,1)));
			mock.MockObject.MethodVoid(new Version(1, 2, 2, 2));

			factory.VerifyAllExpectationsHaveBeenMet();
		}

		[TestMethod]
		public void TwoMocksTest()
		{
			var factory = new MockFactory();
			var mock = factory.CreateMock<IParentInterface>();
			var mock2 = factory.CreateMock<IChildInterface>();

			mock.Expects.One.Method(_ => _.MethodVoid(null as Version)).With(new VersionMatcher());
			mock2.Expects.One.Method(_ => _.MethodVoid(null as OperatingSystem)).With(new OSMatcher());

			mock2.MockObject.MethodVoid(new OperatingSystem(PlatformID.Xbox, new Version(1, 1, 1, 1)));
			mock.MockObject.MethodVoid(new Version(1, 2, 2, 2));

			factory.VerifyAllExpectationsHaveBeenMet();
		}
	}

}

namespace NMockTests
{
	public class VersionMatcher : Matcher
	{
		private readonly int _major;

		public VersionMatcher()
		{

		}

		public VersionMatcher(int major)
		{
			_major = major;
		}

		public override bool Matches(object o)
		{
			var version = o as Version;
			var i = version.Build;
			return true;
		}
	}

	public class OSMatcher : Matcher
	{
		public override bool Matches(object o)
		{
			var version = o as OperatingSystem;
			var i = version.Platform;
			return true;
		}
	}

}