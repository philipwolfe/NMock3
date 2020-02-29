#region Using

using System;
using System.Data;
using System.Runtime.CompilerServices;
using NMock;
using NMock.AcceptanceTests;
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

namespace NMock.AcceptanceTests
{
	/// <summary>
	/// Tests the defect that a call on a stub cannot be performed in an empty ordered expectation block.
	/// </summary>
	[TestClass]
	public class InterceptCallOnMockInEmptyOrderedExpectationOrdering
	{
		#region Setup/Teardown

		/// <summary>
		/// Set up tests
		/// </summary>
		[TestInitialize]
		public void SetUp()
		{
			mockFactory = new MockFactory();
		}

		[TestCleanup]
		public void TearDown()
		{
			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		#endregion

		private MockFactory mockFactory;

		/// <summary>
		/// An interface for testing.
		/// </summary>
		public interface IDependency
		{
			/// <summary>
			/// Gets another dependency.
			/// </summary>
			/// <value>Another dependency.</value>
			IAnotherDependency AnotherDependency { get; }
		}

		/// <summary>
		/// Another interface for testing.
		/// </summary>
		public interface IAnotherDependency
		{
			/// <summary>
			/// Does something.
			/// </summary>
			void DoSomething();
		}

		/// <summary>
		/// This test is to check behavior with default mocks.
		/// </summary>
		[TestMethod]
		public void StubDeclaredExplicitly()
		{
			Mock<IDependency> dependency = mockFactory.CreateMock<IDependency>();
			Mock<IAnotherDependency> anotherDependency = mockFactory.CreateMock<IAnotherDependency>();

			dependency.Stub.Out.GetProperty(d=> d.AnotherDependency).Will(Return.Value(anotherDependency.MockObject));

			using (mockFactory.Ordered())
			{
				anotherDependency.Expects.One.Method(_ => _.DoSomething());
			}

			dependency.MockObject.AnotherDependency.DoSomething();
		}

		/// <summary>
		/// This test reproduces the defect.
		/// </summary>
		[TestMethod]
		//[Ignore] //("Fix this when Adding 'Setup' method")]
		public void StubMockStyle()
		{
			Mock<IDependency> dependency = mockFactory.CreateMock<IDependency>(MockStyle.Stub);

			using (mockFactory.Ordered())
			{
				//Expect.Once.On(dependency.AnotherDependency).Method("DoSomething");
			}

			dependency.MockObject.AnotherDependency.DoSomething();
		}
	}
}