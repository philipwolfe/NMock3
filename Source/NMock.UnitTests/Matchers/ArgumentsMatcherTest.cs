#region Using

using System.Reflection;
using NMock;
using NMock.Matchers;
using NMock.Monitoring;
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
	public class ArgumentsMatcherTest
	{
		private readonly object arg1Value = new NamedObject("arg1Value");
		private readonly object arg2Value = new NamedObject("arg2Value");

#if !SILVERLIGHT
		private Invocation InvocationWithArguments(params object[] args)
		{
			ParameterInfo[] paramsInfo = new ParameterInfo[args.Length];

			for (int i = 0; i < paramsInfo.Length; i++)
			{
				paramsInfo[i] = new ParameterInfoStub("arg" + i, ParameterAttributes.In);
			}

			return new Invocation(new NamedObject("receiver"), new MethodInfoStub("method", paramsInfo), args);
		}
#endif

		[TestMethod]
		public void DescribesAsIndexGetterArguments()
		{
			Matcher matcher = new IndexGetterArgumentsMatcher(
				new AlwaysMatcher(true, "arg1-description"),
				new AlwaysMatcher(true, "arg2-description"));

			AssertDescription.IsEqual(matcher, "[arg1-description, arg2-description]");
		}

		[TestMethod]
		public void DescribesAsIndexSetterArguments()
		{
			Matcher matcher = new IndexSetterArgumentsMatcher(
				new AlwaysMatcher(true, "arg1-description"),
				new AlwaysMatcher(true, "arg2-description"));

			AssertDescription.IsEqual(matcher, "[arg1-description] = (arg2-description)");
		}

		[TestMethod]
		public void DoesNotMatchAnObjectThatIsNotAnInvocation()
		{
			Matcher matcher = new ArgumentsMatcher();
			Assert.IsFalse(matcher.Matches("not an invocation"));
		}

#if !SILVERLIGHT
		[TestMethod]
		public void DoesNotMatchAndDoesNotThrowExceptionIfValueSpecifiedForOutputParameter()
		{
			Matcher matcher = new ArgumentsMatcher(Is.EqualTo(arg1Value), Is.EqualTo(arg2Value));

			Invocation invocation = new Invocation(
				new NamedObject("receiver"),
				new MethodInfoStub("method",
				                   new ParameterInfoStub("in", ParameterAttributes.In),
				                   new ParameterInfoStub("out", ParameterAttributes.Out)),
				new[] {arg1Value, null});

			Assert.IsFalse(matcher.Matches(invocation));
		}

		[TestMethod]
		public void DoesNotMatchIfValueMatchersDoNotMatchArgumentValues()
		{
			Matcher matcher = new ArgumentsMatcher(Is.EqualTo(arg1Value), Is.EqualTo(arg2Value));

			Assert.IsFalse(matcher.Matches(InvocationWithArguments("other object", arg2Value)),
			               "different first arg");
			Assert.IsFalse(matcher.Matches(InvocationWithArguments(arg1Value, "other object")),
			               "different second arg");
		}

		[TestMethod]
		public void DoesNotMatchInvocationWithDifferentNumberOfArguments()
		{
			Matcher matcher = new ArgumentsMatcher(Is.Anything, Is.Anything);

			Assert.IsFalse(matcher.Matches(InvocationWithArguments(1)), "fewer arguments");
			Assert.IsFalse(matcher.Matches(InvocationWithArguments(1, 2, 3)), "more arguments");
		}
#endif

		[TestMethod]
		public void FormatsDescriptionToLookSimilarToAnArgumentList()
		{
			Matcher matcher = new ArgumentsMatcher(
				new AlwaysMatcher(true, "arg1-description"),
				new AlwaysMatcher(true, "arg2-description"));

			AssertDescription.IsEqual(matcher, "(arg1-description, arg2-description)");
		}

	#if !SILVERLIGHT
		[TestMethod]
		public void MatchesInvocationWithSameNumberOfArgumentsAsMatcherHasValueMatchersAndValueMatchersMatch()
		{
			Matcher matcher = new ArgumentsMatcher(Is.EqualTo(arg1Value), Is.EqualTo(arg2Value));

			Assert.IsTrue(matcher.Matches(InvocationWithArguments(arg1Value, arg2Value)));
		}

		[TestMethod]
		public void MatchesOutputParametersWithSpecialMatcherClass()
		{
			Matcher matcher = new ArgumentsMatcher(Is.EqualTo(arg1Value), Is.Out);

			Invocation invocation = new Invocation(
				new NamedObject("receiver"),
				new MethodInfoStub("method",
				                   new ParameterInfoStub("in", ParameterAttributes.In),
				                   new ParameterInfoStub("out", ParameterAttributes.Out)),
				new[] {arg1Value, null});

			Assert.IsTrue(matcher.Matches(invocation));
		}
#endif

		[TestMethod]
		public void ShowsOutputParametersInDescription()
		{
			Matcher matcher = new ArgumentsMatcher(new AlwaysMatcher(true, "arg1"), Is.Out);

			AssertDescription.IsEqual(matcher, "(arg1, out)");
		}
	}
}