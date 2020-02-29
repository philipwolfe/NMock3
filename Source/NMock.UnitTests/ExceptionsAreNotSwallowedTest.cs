#region Using

using System;
using System.Collections;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
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
	/// Tests checking that tested code can not swallow NMock exceptions.
	/// </summary>
	[TestClass]
	public class ExceptionsAreNotSwallowedTest : AcceptanceTestBase
	{
		[TestInitialize]
		public void TestClassSetUp()
		{
			SkipVerificationForThisFixture();
		}

		/// <summary>
		/// The first swallowed exception is thrown.
		/// </summary>
		[TestMethod]
		public void FirstSwallowedUnexpectedInvocationExceptionIsRethrownInVerify()
		{
			Mock<IHelloWorld> mock = Mocks.CreateMock<IHelloWorld>();
			UnexpectedInvocationException firstException = null;

			try
			{
				mock.MockObject.Ahh();
			}
			catch (UnexpectedInvocationException ex)
			{
				// evil code >:-]
				firstException = ex;
			}

			try
			{
				mock.MockObject.Ahh();
			}
			catch (UnexpectedInvocationException)
			{
				// more evil code >:-]
			}

			try
			{
				Mocks.VerifyAllExpectationsHaveBeenMet(true);
			}
			catch (UnexpectedInvocationException rethrownException)
			{
				Assert.AreSame(
					firstException,
					rethrownException,
					"Expected first unexpected invocation exception to be rethrown");

				return;
			}

			Assert.Fail("Expected ExpectationException to be rethrown");
		}

		/// <summary>
		/// Exceptions are rethrown only once.
		/// </summary>
		[TestMethod]
		public void UnexpectedInvocationExceptionIsClearedAfterBeingThrownInVerify()
		{
			Mock<IHelloWorld> mock = Mocks.CreateMock<IHelloWorld>();

			try
			{
				mock.MockObject.Ahh();
			}
			catch (UnexpectedInvocationException)
			{
				// evil code >:-]
			}

			try
			{
				// Exception should be initially rethrown here...
				Mocks.VerifyAllExpectationsHaveBeenMet();
			}
			catch (UnexpectedInvocationException)
			{
			}

			// It should not be rethrown again...
			Mocks.VerifyAllExpectationsHaveBeenMet();
		}

		/// <summary>
		/// <see cref="ExpectationException"/>s are rethrown in <see cref="MockFactory.VerifyAllExpectationsHaveBeenMet()"/>.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof (UnexpectedInvocationException))]
		public void UnexpectedInvocationExceptionsAreRethrownInVerify()
		{
			Mock<IHelloWorld> mock = Mocks.CreateMock<IHelloWorld>();

			try
			{
				mock.MockObject.Ahh();
			}
			catch (UnexpectedInvocationException)
			{
				// evil code >:-]
			}

			Mocks.VerifyAllExpectationsHaveBeenMet(true);
		}
	}
}