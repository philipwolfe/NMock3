#region Using

using System;
using System.Collections;
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
	public class CommentTest : AcceptanceTestBase
	{
		#region Setup/Teardown
		[TestInitialize]
		public override void Setup()
		{
			base.Setup();

			mock = Mocks.CreateMock<IComment>();
		}

		[TestCleanup]
		public override void Teardown()
		{
			VerifyException();
		}

		#endregion

		private const string comment = "Should be called because it is a test.";

		private Mock<IComment> mock;

		private void VerifyException()
		{
			try
			{
				Mocks.VerifyAllExpectationsHaveBeenMet();

				Assert.Fail("An ExpectationException should occur.");
			}
			catch (UnmetExpectationException e)
			{
				Assert.IsTrue(e.Message.Contains(comment), e.Message + " does not contain comment: " + comment);
			}
		}

		[TestMethod]
		public void EventAdd()
		{
			mock.Expects.One.EventBinding(_ => _.Event += null).Comment(comment);
		}

		[TestMethod]
		public void EventRemove()
		{
			mock.Expects.One.EventBinding(_ => _.Event -= null).Comment(comment);
		}

		[TestMethod]
		public void GetProperty()
		{
			mock.Expects.One.GetProperty(m =>m.Property).Will(Return.Value("test")).Comment(comment);
		}

		[TestMethod]
		public void IndexerGet()
		{
			mock.Expects.One.GetProperty(_ => _[0]).Comment(comment);
		}

		[TestMethod]
		public void Method()
		{
			mock.Expects.One.Method(_ => _.Method()).Comment(comment);
		}

		[TestMethod]
		public void MethodWithActions()
		{
			mock.Expects.One.Method(_ => _.Method()).Will(Throw.Exception(new Exception())).Comment(comment);
		}
	}

	public interface IComment
	{
		string Property { get; set; }

		string this[int index] { get; set; }
		void Method();

		event EventHandler Event;
	}
}