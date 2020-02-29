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
	public class UsingBlockAcceptanceTest
	{
		public interface IUsingBlock
		{
			int IntReturnValue();
		}

		[TestMethod]
		[ExpectedException(typeof (UnmetExpectationException))]
		public void AssertsExpectationsAreMetAtEndOfUsingBlock()
		{
			using (MockFactory mocks = new MockFactory())
			{
				Mock<IHelloWorld> helloWorld = mocks.CreateMock<IHelloWorld>();

				helloWorld.Expects.One.Method(_ => _.Hello());
			}
		}

		/// <summary>
		/// That is a test where the InvalidOperationException will be hidden by the ExpectationException
		/// which is thrown at the Dispose() while executing the using's end block.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof (IncompleteExpectationException))]
		public void ReturnValueForgottenWithUsingBlock()
		{
			using (MockFactory mocks = new MockFactory())
			{
				Mock<IUsingBlock> usingBlock = mocks.CreateMock<IUsingBlock>();

				usingBlock.Expects.One.Method(_ => _.IntReturnValue());
				usingBlock.MockObject.IntReturnValue();
			}
		}

		/// <summary>
		/// That is a test with the expected behaviour. An InvalidOperationException will be
		/// thrown, if the developer forgot to set the return value for a Value Type.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof (IncompleteExpectationException))]
		public void ReturnValueForgottenWithVerify()
		{
			Mock<IUsingBlock> usingBlock = new MockFactory().CreateMock<IUsingBlock>();

			usingBlock.Expects.One.Method(_ => _.IntReturnValue()); // Developer forgot to set: .Will(Return.Value(12));
			usingBlock.MockObject.IntReturnValue();
		}
	}
}