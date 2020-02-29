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
	[TestClass]
	public abstract class AcceptanceTestBase
	{
		private bool doVerificationForCurrentTest = true;
		private bool doVerificationForEveryTestInFixture = true;
		private MockFactory mocks;

		/// <summary>
		/// A default MockFactory instance created for each test.
		/// </summary>
		protected MockFactory Mocks
		{
			get
			{
				return mocks;
			}
		}

		[TestInitialize]
		public virtual void Setup()
		{
			mocks = new MockFactory();
		}

		[TestCleanup]
		public virtual void Teardown()
		{
			if (doVerificationForCurrentTest && doVerificationForEveryTestInFixture)
			{
				mocks.VerifyAllExpectationsHaveBeenMet();
			}

			doVerificationForCurrentTest = true;
		}

		/// <summary>
		/// Prevents MockFactory.VerifyAllExpectationsHaveBeenMet() being called after the current test.
		/// </summary>
		protected void SkipVerificationForThisTest()
		{
			doVerificationForCurrentTest = false;
		}

		/// <summary>
		/// Prevents MockFactory.VerifyAllExpectationsHaveBeenMet() being called after every test in the current fixture.
		/// </summary>
		protected void SkipVerificationForThisFixture()
		{
			doVerificationForEveryTestInFixture = false;
		}
	}
}