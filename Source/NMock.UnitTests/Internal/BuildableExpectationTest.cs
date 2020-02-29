#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using NMock;
using NMock.AcceptanceTests;
using NMock.Actions;
using NMock.Matchers;
using NMock.Monitoring;
using NMock.Proxy.Reflective;
using NMockTests.Monitoring;
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

namespace NMock.Internal.Test
{
	[TestClass]
	public class BuildableExpectationTest
	{
		#region Setup/Teardown

		[TestInitialize]
		public void SetUp()
		{
			MockFactory mockFactory = new MockFactory();
			receiver = mockFactory.CreateMock<IMockObject>(DefinedAs.Named("receiver"));
			invocation = new Invocation(receiver.MockObject, new MethodInfoStub("method"), new object[] {"arg"});
		}

		#endregion

		private Invocation invocation;
		private Mock<IMockObject> receiver;

		private static object BuildExpectation(
			bool matchRequiredCallCount,
			bool matchMatchingCallCount,
			IMockObject receiver,
			bool matchMethod,
			bool matchArguments,
			params bool[] matchExtraMatchers)
		{
			Matcher[] extraMatchers = new Matcher[matchExtraMatchers.Length];
			for (int i = 0; i < extraMatchers.Length; i++)
			{
				extraMatchers[i] = new AlwaysMatcher(matchExtraMatchers[i], "extra matcher " + (i + 1));
			}

			return BuildExpectation(
				"description",
				new AlwaysMatcher(matchRequiredCallCount, "required call count"),
				new AlwaysMatcher(matchMatchingCallCount, "matching call count"),
				receiver,
				new AlwaysMatcher(matchMethod, "method"),
				new AlwaysMatcher(matchArguments, "argument"),
				extraMatchers);
		}

		private static BuildableExpectation BuildExpectation(
			string expectationDescription,
			string matchRequiredCallCountDescription,
			string matchMatchingCallCountDescription,
			IMockObject receiver,
			string matchMethodDescription,
			string matchArgumentsDescription,
			params string[] extraMatcherDescriptions)
		{
			var extraMatchers = new Matcher[extraMatcherDescriptions.Length];
			for (int i = 0; i < extraMatchers.Length; i++)
			{
				extraMatchers[i] = new AlwaysMatcher(true, extraMatcherDescriptions[i]);
			}

			return BuildExpectation(
				expectationDescription,
				new AlwaysMatcher(true, matchRequiredCallCountDescription),
				new AlwaysMatcher(true, matchMatchingCallCountDescription),
				receiver,
				new AlwaysMatcher(true, matchMethodDescription),
				new AlwaysMatcher(true, matchArgumentsDescription),
				extraMatchers);
		}

		private static BuildableExpectation BuildExpectation(
			string description,
			Matcher requiredCallCountMatcher,
			Matcher matchingCallCountMatcher,
			IMockObject receiver,
			Matcher methodMatcher,
			Matcher argumentsMatcher,
			params Matcher[] extraMatchers)
		{
			var e = new BuildableExpectation(description, requiredCallCountMatcher, matchingCallCountMatcher);
			e.ArgumentsMatcher = argumentsMatcher;
			e.MethodMatcher = methodMatcher;
			e.AddAction(Return.Value(new object()));
			e.Receiver = receiver;
			foreach (Matcher extraMatcher in extraMatchers) e.AddInvocationMatcher(extraMatcher);
			return e;
		}

		private void AssertIsActive(IExpectation expectation, string message)
		{
			Assert.IsTrue(expectation.IsActive, message);
		}

		private void AssertHasBeenMet(IExpectation expectation, string message)
		{
			Assert.IsTrue(expectation.HasBeenMet, message);
		}

		private void AssertHasNotBeenMet(IExpectation expectation, string message)
		{
			Assert.IsFalse(expectation.HasBeenMet, message);
		}

		private void AssertIsNotActive(IExpectation expectation, string message)
		{
			Assert.IsFalse(expectation.IsActive, message);
		}

		private void AssertDescriptionIsEqual(IExpectation expectation, string expected)
		{
			DescriptionWriter writer = new DescriptionWriter();
			expectation.DescribeActiveExpectationsTo(writer);

			Assert.AreEqual(expected, writer.ToString());
		}

		[TestMethod]
		public void ChecksCallCountToAssertThatItHasBeenMet()
		{
			var irrelevant = Is.Anything;
			var mock = new ReflectiveInterceptor(null, null, "name", MockStyle.Default);

			var expectation = BuildExpectation(
				"description",
				Is.AtLeast(2),
				Is.AtMost(4),
				mock,
				irrelevant,
				irrelevant,
				irrelevant,
				irrelevant);

			AssertHasNotBeenMet(expectation, "should not have been met after no invocations");

			expectation.Perform(invocation);
			AssertHasNotBeenMet(expectation, "should not have been met after 1 invocation");

			expectation.Perform(invocation);
			AssertHasBeenMet(expectation, "should have been met after 2 invocations");

			expectation.Perform(invocation);
			AssertHasBeenMet(expectation, "should have been met after 3 invocations");

			expectation.Perform(invocation);
			AssertHasBeenMet(expectation, "should have been met after 4 invocations");
		}

		[TestMethod]
		public void DoesNotMatchIfAnyMatcherDoesNotMatch()
		{
			const bool ignoreRequiredCallCount = true;

			for (int i = 1; i < 64; i++)
			{
				var e = (BuildableExpectation) BuildExpectation(
					ignoreRequiredCallCount,
					(i & 1) == 0,
					(i & 2) == 0 ? receiver.MockObject : null,
					(i & 4) == 0,
					(i & 8) == 0,
					(i & 16) == 0,
					(i & 32) == 0);

				Assert.IsFalse(e.Matches(invocation), "should not match (iteration " + i + ")");
			}
		}

		[TestMethod]
		public void HasReadableDescription()
		{
			var expectation = BuildExpectation(
				"1 time",
				"required call count description is ignored",
				"matching call count description is ignored",
				receiver.MockObject,
				"method",
				"(arguments)",
				"extra matcher 1", "extra matcher 2"
			                                                          	);

			expectation.AddAction(new MockAction("action 1"));
			expectation.AddAction(new MockAction("action 2"));

			AssertDescriptionIsEqual(expectation,
			           "receiver.method(arguments) Matching[extra matcher 1, extra matcher 2] will return <System.Object>(System.Object), action 1, action 2 [EXPECTED: 1 time CALLED: 0 times]\r\n");
		}

		[TestMethod]
		public void InvokesAListOfActionsToPerformAnInvocation()
		{
			BuildableExpectation e = (BuildableExpectation) BuildExpectation(true, true, receiver.MockObject, true, true, true, true);
			MockAction action1 = new MockAction();
			MockAction action2 = new MockAction();

			e.AddAction(action1);
			e.AddAction(action2);

			e.Perform(invocation);

			Assert.AreSame(invocation, action1.Received, "action1 received");
			Assert.AreSame(invocation, action2.Received, "action1 received");
		}

		[TestMethod]
		public void MatchesCallCountWhenMatchingInvocation()
		{
			Matcher irrelevant = Is.Anything;

			BuildableExpectation expectation = (BuildableExpectation) BuildExpectation(
				"description",
				Is.AtLeast(0),
				Is.AtMost(4),
				receiver.MockObject,
				irrelevant,
				irrelevant,
				irrelevant,
				irrelevant);

			AssertIsActive(expectation, "should be active before any invocation");
			Assert.IsTrue(expectation.Matches(invocation), "should match 1st invocation");
			expectation.Perform(invocation);

			AssertIsActive(expectation, "should be active before 2nd invocation");
			Assert.IsTrue(expectation.Matches(invocation), "should match 2nd invocation");
			expectation.Perform(invocation);

			AssertIsActive(expectation, "should be active before 3rd invocation");
			Assert.IsTrue(expectation.Matches(invocation), "should match 3rd invocation");
			expectation.Perform(invocation);

			AssertIsActive(expectation, "should be active before 4th invocation");
			Assert.IsTrue(expectation.Matches(invocation), "should match 4th invocation");
			expectation.Perform(invocation);

			AssertIsNotActive(expectation, "should not be active after 4th invocation");
			Assert.IsFalse(expectation.Matches(invocation), "should not match 5th invocation");
		}

		[TestMethod]
		public void MatchesIfAllMatchersMatch()
		{
			BuildableExpectation e = (BuildableExpectation) BuildExpectation(true, true, receiver.MockObject, true, true, true, true);
			Assert.IsTrue(e.Matches(invocation), "should match");
		}

		[TestMethod]
		public void WillNotPrintAPeriodBetweenReceiverAndMethodIfToldToDescribeItselfAsAnIndexer()
		{
			BuildableExpectation expectation = (BuildableExpectation) BuildExpectation(
				"1 time",
				"required call count description is ignored",
				"matching call count description is ignored",
				receiver.MockObject,
				"",
				"[arguments]",
				"extra matcher 1", "extra matcher 2"
			                                                          	);

			expectation.AddAction(new MockAction("action 1"));
			expectation.AddAction(new MockAction("action 2"));
			expectation.DescribeAsIndexer();

			AssertDescriptionIsEqual(expectation,
			                         "receiver[arguments] Matching[extra matcher 1, extra matcher 2] will return <System.Object>(System.Object), action 1, action 2 [EXPECTED: 1 time CALLED: 0 times]\r\n");
		}
	}

	internal class MockAction : IAction
	{
		public string Description;
		public MockAction Previous;
		public Invocation Received;

		public MockAction() : this("MockAction")
		{
		}

		public MockAction(string description)
		{
			Description = description;
		}

		#region IAction Members

		void IAction.Invoke(Invocation invocation)
		{
			if (Previous != null) Assert.IsNotNull(Previous.Received, "called out of order");
			Received = invocation;
		}

		void ISelfDescribing.DescribeTo(TextWriter writer)
		{
			writer.Write(Description);
		}

		#endregion
	}
}