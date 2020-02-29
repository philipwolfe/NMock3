#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using NMock;
using NMock.AcceptanceTests;
using NMock.Actions;
using NMock.Matchers;
using NMockTests._TestStructures;
using Is = NMock.Is;

#if NUNIT
using NUnit.Framework;
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

namespace NMock.AcceptanceTests
{
	[TestClass]
	public class OrderedCallsAcceptanceTest : AcceptanceTestBase
	{
		#region Setup/Teardown

		[TestInitialize]
		public void SetUp()
		{
			base.Setup();

			helloWorld = Mocks.CreateMock<IHelloWorld>();
		}

		#endregion

		private Mock<IHelloWorld> helloWorld;

		[TestMethod]
		public void AllowsCallsIfCalledInSameOrderAsExpectedWithinAnInOrderBlock()
		{
			using (Mocks.Ordered())
			{
				helloWorld.Expects.One.Method(_ => _.Hello());
				helloWorld.Expects.One.Method(_ => _.Goodbye());
			}

			helloWorld.MockObject.Hello();
			helloWorld.MockObject.Goodbye();
		}

		[TestMethod]
		[ExpectedException(typeof (UnmetExpectationException))]
		public void CallsInOrderedBlocksThatAreNotMatchedFailVerification()
		{
			SkipVerificationForThisTest();

			using (Mocks.Ordered())
			{
				helloWorld.Expects.One.Method(_ => _.Hello());
				helloWorld.Expects.One.Method(_ => _.Goodbye());
			}

			helloWorld.MockObject.Hello();

			Mocks.VerifyAllExpectationsHaveBeenMet();
		}

		[TestMethod]
		public void CallsWithinAnInOrderedBlockCanBeExpectedMoreThanOnce()
		{
			using (Mocks.Ordered())
			{
				helloWorld.Expects.One.Method(_ => _.Hello());
				helloWorld.Expects.AtLeastOne.Method(_ => _.Err());
				helloWorld.Expects.One.Method(_ => _.Goodbye());
			}

			helloWorld.MockObject.Hello();
			helloWorld.MockObject.Err();
			helloWorld.MockObject.Err();
			helloWorld.MockObject.Goodbye();
		}

		[TestMethod]
		public void CanExpectUnorderedCallsWithinAnOrderedSequence()
		{
			using (Mocks.Ordered())
			{
				helloWorld.Expects.One.Method(_ => _.Hello());
				using (Mocks.Unordered())
				{
					helloWorld.Expects.One.Method(_ => _.Umm());
					helloWorld.Expects.One.Method(_ => _.Err());
				}
				helloWorld.Expects.One.Method(_ => _.Goodbye());
			}

			helloWorld.MockObject.Hello();
			helloWorld.MockObject.Err();
			helloWorld.MockObject.Umm();
			helloWorld.MockObject.Goodbye();
		}

		[TestMethod]
		public void DoesNotEnforceTheOrderOfCallsByDefault()
		{
			helloWorld.Expects.One.Method(_ => _.Hello());
			helloWorld.Expects.One.Method(_ => _.Goodbye());

			helloWorld.MockObject.Goodbye();
			helloWorld.MockObject.Hello();
		}

		[TestMethod, ExpectedException(typeof (UnexpectedInvocationException))]
		public void EnforcesTheOrderOfCallsWithinAnInOrderBlock()
		{
			SkipVerificationForThisTest();

			using (Mocks.Ordered())
			{
				helloWorld.Expects.One.Method(_ => _.Hello());
				helloWorld.Expects.One.Method(_ => _.Goodbye());
			}

			helloWorld.MockObject.Goodbye();
			helloWorld.MockObject.Hello();
		}

		[TestMethod, ExpectedException(typeof (UnexpectedInvocationException))]
		public void UnorderedCallsWithinAnInOrderedBlockCannotBeCalledAfterTheEndOfTheUnorderedExpectations()
		{
			SkipVerificationForThisTest();

			using (Mocks.Ordered())
			{
				helloWorld.Expects.One.Method(_ => _.Hello());
				using (Mocks.Unordered())
				{
					helloWorld.Expects.One.Method(_ => _.Umm());
					helloWorld.Expects.One.Method(_ => _.Err());
				}
				helloWorld.Expects.One.Method(_ => _.Goodbye());
			}

			helloWorld.MockObject.Hello();
			helloWorld.MockObject.Err();
			helloWorld.MockObject.Goodbye();
			helloWorld.MockObject.Umm();
		}

		[TestMethod, ExpectedException(typeof (UnexpectedInvocationException))]
		public void UnorderedCallsWithinAnInOrderedBlockCannotBeCalledBeforeTheStartOfTheUnorderedExpectations()
		{
			SkipVerificationForThisTest();

			using (Mocks.Ordered())
			{
				helloWorld.Expects.One.Method(_ => _.Hello());
				using (Mocks.Unordered())
				{
					helloWorld.Expects.One.Method(_ => _.Umm());
					helloWorld.Expects.One.Method(_ => _.Err());
				}
				helloWorld.Expects.One.Method(_ => _.Goodbye());
			}

			helloWorld.MockObject.Err();
			helloWorld.MockObject.Hello();
			helloWorld.MockObject.Umm();
			helloWorld.MockObject.Goodbye();
		}

		[TestMethod]
		public void UnorderedExpectationsMatchInOrderOfSpecification()
		{
			helloWorld.Expects.One.Method(_ => _.Ask(null)).With(Is.Anything).Will(Return.Value("1"));
			helloWorld.Expects.One.Method(_ => _.Ask(null)).With(Is.Anything).Will(Return.Value("2"));

			Assert.AreEqual("1", helloWorld.MockObject.Ask("ignored"), "first call");
			Assert.AreEqual("2", helloWorld.MockObject.Ask("ignored"), "second call");
		}
	}
}