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

namespace NMock.AcceptanceTests
{
	/// <summary>
	/// Fixture that provides tests for a stub that enumerates a few data items.
	/// </summary>
	[TestClass]
	public class MockIEnumerableAcceptanceTest : AcceptanceTestBase
	{
		#region Setup/Teardown

		/// <summary>
		/// Initializes the fixture before each testwill run.
		/// </summary>
		/// <remarks>
		/// This method creates a stub thatimplements the <see cref="IEnumerable.GetEnumerator()"/> method.
		/// The <see cref="IEnumerator"/> the stubreturns should really enumerate a few strings in an array.
		/// </remarks>
		[TestInitialize]
		public override void Setup()
		{
			base.Setup();

			myEnumerable = Mocks.CreateMock<IMyEnumerable>();

			data = new[] {"a", "b", "c", "d", "e"};

			myEnumerable.Stub.Out.Method(_ => _.GetEnumerator()).Will(new CallGetEnumeratorAction(data));
		}

		#endregion

		/// <summary>
		/// A simple interface that stub should provide.
		/// Really we are only interested in the <see cref="IEnumerable.GetEnumerator()"/>
		/// method, that is provided by the <see cref="IEnumerable"/> interface.
		/// </summary>
		public interface IMyEnumerable : IEnumerable
		{
			string Name { get; }
		}

		private string[] data;

		private Mock<IMyEnumerable> myEnumerable;

		public class CallGetEnumeratorAction : IReturnAction
		{
			private readonly string[] data;

			public CallGetEnumeratorAction(string[] data)
			{
				this.data = data;
			}

			#region IAction Members

			void IAction.Invoke(Invocation invocation)
			{
				invocation.Result = data.GetEnumerator();
			}

			void ISelfDescribing.DescribeTo(TextWriter writer)
			{
				writer.Write("Test");
			}

			public Type ReturnType
			{
				get { return typeof(IEnumerator); }
			}

			#endregion
		}

		/// <summary>
		/// Verifies that the each of the enumerated strings match the corresponding original string
		/// and number of the enumerated strings equal to length of the string array.
		/// </summary>
		private void ShouldEnumerateData()
		{
			int dataIdx = 0;

			foreach (string s in myEnumerable.MockObject)
			{
				Assert.AreEqual(data[dataIdx++], s);
			}
			Assert.AreEqual(data.Length, dataIdx);
		}

		/// <summary>
		/// Verifies that the string successfully enumerated once.
		/// </summary>
		[TestMethod]
		public void ShouldEnumerateDataOnce()
		{
			ShouldEnumerateData();
		}

		/// <summary>
		/// Verifies that the string successfully enumerated twice.
		/// </summary>
		[TestMethod]
		public void ShouldEnumerateDataTwice()
		{
			ShouldEnumerateData();

			ShouldEnumerateData();
		}
	}
}