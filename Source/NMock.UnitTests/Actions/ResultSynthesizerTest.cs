#region Using

using System;
using System.Collections;
using System.IO;
using System.Reflection;
using NMock;
using NMock.Actions;
using NMock.Monitoring;
using NMockTests.Monitoring;

#if NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using Is = NMock.Is;

#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
#endif

#endregion

#if NetFx35
namespace NMockTests.Actions35
#else
#if SILVERLIGHT
namespace NMockTests.ActionsSL
#else
namespace NMockTests.Actions
#endif
#endif
{
#if !SILVERLIGHT
	[TestClass]
	public class ResultSynthesizerTest
	{
		[TestMethod]
		public void CreatesDefaultValuesForBasicTypes()
		{
			ResultSynthesizer synth = new ResultSynthesizer();

			AssertReturnsValue(synth, typeof(bool), false);
			AssertReturnsValue(synth, typeof(byte), (byte)0);
			AssertReturnsValue(synth, typeof(sbyte), (sbyte)0);
			AssertReturnsValue(synth, typeof(short), (short)0);
			AssertReturnsValue(synth, typeof(ushort), (ushort)0U);
			AssertReturnsValue(synth, typeof(int), 0);
			AssertReturnsValue(synth, typeof(uint), 0U);
			AssertReturnsValue(synth, typeof(long), 0L);
			AssertReturnsValue(synth, typeof(ulong), 0UL);
			AssertReturnsValue(synth, typeof(char), '\0');
			AssertReturnsValue(synth, typeof(string), "");
		}

		[TestMethod]
		public void DoesNotTryToSetResultForVoidReturnType()
		{
			ResultSynthesizer synth = new ResultSynthesizer();

			AssertReturnsValue(synth, typeof(void), Missing.Value);
		}

		[TestMethod]
		public void CanOverrideDefaultResultForType()
		{
			ResultSynthesizer synth = new ResultSynthesizer();
			string newResult = "new result";
			synth.SetResult(typeof(string), newResult);

			AssertReturnsValue(synth, typeof(string), newResult);
		}

		[TestMethod]
		public void ReturnsZeroLengthArrays()
		{
			ResultSynthesizer synth = new ResultSynthesizer();

			AssertReturnsValue(synth, typeof(int[]), new int[0]);
			AssertReturnsValue(synth, typeof(string[]), new string[0]);
			AssertReturnsValue(synth, typeof(object[]), new object[0]);
		}

		[TestMethod]
		public void ReturnsEmptyCollections()
		{
			ResultSynthesizer synth = new ResultSynthesizer();

			AssertReturnsValue(synth, typeof(ArrayList), IsAnEmpty(typeof(ArrayList)));
			AssertReturnsValue(synth, typeof(SortedList), IsAnEmpty(typeof(SortedList)));
			AssertReturnsValue(synth, typeof(Hashtable), IsAnEmpty(typeof(Hashtable)));
			AssertReturnsValue(synth, typeof(Stack), IsAnEmpty(typeof(Stack)));
			AssertReturnsValue(synth, typeof(Queue), IsAnEmpty(typeof(Queue)));
		}

		[TestMethod]
		public void ReturnsADifferentCollectionOnEachInvocation()
		{
			ResultSynthesizer synth = new ResultSynthesizer();
			ArrayList list = (ArrayList)ValueReturnedForType(synth, typeof(ArrayList));
			list.Add("a new element");

			AssertReturnsValue(synth, typeof(ArrayList), IsAnEmpty(typeof(ArrayList)));
		}

		[TestMethod]
		public void CanSpecifyResultForOtherType()
		{
			var synth = new ResultSynthesizer();
			var value = new NamedObject("value");
			synth.SetResult(typeof(NamedObject), value);

			AssertReturnsValue(synth, typeof(NamedObject), value);
		}

		public struct AValueType
		{
			public int value1, value2;
		}

		[TestMethod]
		public void ReturnsDefaultValueOfValueTypes()
		{
			var synth = new ResultSynthesizer();

			AssertReturnsValue(synth, typeof(DateTime), new DateTime());
			AssertReturnsValue(synth, typeof(AValueType), new AValueType());
		}

		[TestMethod]
		public void ThrowsExceptionIfTriesToReturnValueForUnsupportedResultType()
		{
			var synth = new ResultSynthesizer();

			Expect.That(() => AssertReturnsValue(synth, typeof(NamedObject), Is.Nothing)).Throws<InvalidOperationException>();
		}

		[TestMethod]
		public void HasAReadableDescription()
		{
			AssertDescription.IsEqual(new ResultSynthesizer(),
									  "a synthesized result");
		}

		private Matcher IsAnEmpty(Type type)
		{
			return new IsEmptyCollectionMatcher(type);
		}

		private class IsEmptyCollectionMatcher : Matcher
		{
			private readonly Type collectionType;

			public IsEmptyCollectionMatcher(Type collectionType)
			{
				if (!typeof(ICollection).IsAssignableFrom(collectionType))
				{
					throw new ArgumentException(collectionType.FullName + " is not derived from ICollection");
				}

				this.collectionType = collectionType;
			}

			public override bool Matches(object o)
			{
				return collectionType.IsInstanceOfType(o)
					   && ((ICollection)o).Count == 0;
			}

			public override void DescribeTo(TextWriter writer)
			{
				writer.Write("an empty " + collectionType.Name);
			}
		}

		private static readonly object RECEIVER = new NamedObject("receiver");

		private void AssertReturnsValue(IAction action, Type returnType, object expectedResult)
		{
			AssertReturnsValue(action, returnType, Is.EqualTo(expectedResult));
		}

		private void AssertReturnsValue(IAction action, Type returnType, Matcher resultMatcher)
		{
			Verify.That(ValueReturnedForType(action, returnType), resultMatcher, "result for type " + returnType);
		}

		private static object ValueReturnedForType(IAction action, Type returnType)
		{
			var method = new MethodInfoStub("method", new ParameterInfo[0]);
			method.StubReturnType = returnType;

			var invocation = new Invocation(RECEIVER, method, new object[0]);

			action.Invoke(invocation);

			return invocation.Result;
		}
	}
#endif
}