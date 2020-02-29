#region Using

using System.Threading.Tasks;
using NMock;
using NMockTests._TestStructures;

#if NUNIT
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;

#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
#endif

#endregion


namespace NMockTests.StubTests
{
	[TestClass]
	public class ParallelTests
	{

		[TestMethod]
		public void Stub_CallingStubMethodFromDifferentThreads_NoExceptionShouldBeThrown()
		{
			MockFactory factory = new MockFactory();
			Mock<ITestElement> stubElement1 = factory.CreateMock<ITestElement>();
			Mock<ITestElement> stubElement2 = factory.CreateMock<ITestElement>();
			stubElement1.Stub.Out.Method(e => e.Matches(1)).With(Is.In(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })).WillReturn(true);
			stubElement2.Stub.Out.Method(e => e.Matches(1)).With(Is.In(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })).WillReturn(true);
			//stubElement2.Stub.Out.Method(e => e.Matches(1)).WillReturn(true);
			//stubElement1.Stub.Out.Method(e => e.Matches(2)).WillReturn(true);
			//stubElement2.Stub.Out.Method(e => e.Matches(2)).WillReturn(true);
			//stubElement1.Stub.Out.Method(e => e.Matches(3)).WillReturn(true);
			//stubElement2.Stub.Out.Method(e => e.Matches(3)).WillReturn(true);
			//stubElement1.Stub.Out.Method(e => e.Matches(5)).WillReturn(true);
			//stubElement2.Stub.Out.Method(e => e.Matches(5)).WillReturn(true);
			//stubElement1.Stub.Out.Method(e => e.Matches(7)).WillReturn(true);
			//stubElement2.Stub.Out.Method(e => e.Matches(7)).WillReturn(true);
			//stubElement1.Stub.Out.Method(e => e.Matches(8)).WillReturn(true);
			//stubElement2.Stub.Out.Method(e => e.Matches(9)).WillReturn(true);

			Parallel.ForEach(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
				testElement =>
				{
					stubElement1.MockObject.Matches(testElement);
					stubElement2.MockObject.Matches(testElement);
				});

			// No Exception should be thrown
		}
	}

	// Interface to create a stub for
	public interface ITestElement
	{
		bool Matches(int toMatch);
	}

}
