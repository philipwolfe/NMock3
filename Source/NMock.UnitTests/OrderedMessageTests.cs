#region Using
using NMock;
using NMock.Internal;

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

namespace NMockTests
{
	[TestClass]
	public class OrderedMessageTests
	{
		#region Setup/Teardown

		[TestInitialize]
		public void TestInit()
		{
			_factory.ClearExpectations();
			mock = _factory.CreateMock<IOrderedMethods>();
		}

		[TestCleanup]
		public void TestClean()
		{
			_factory.VerifyAllExpectationsHaveBeenMet();
		}

		#endregion

		private readonly MockFactory _factory = new MockFactory();
		private Mock<IOrderedMethods> mock;

		public interface IOrderedMethods
		{
			void Call1();
			void Call2();
			void Call3();
			void Call4();
			void Call5();
			void Call6();
			void Call7();
		}

		[TestMethod]
		public void MessageTest1()
		{
			using (_factory.Ordered())
			{
				mock.Expects.One.Method(_ => _.Call1());
			}
			mock.MockObject.Call1();
		}

		[TestMethod]
		public void MessageTest10()
		{
			using (_factory.Ordered())
			{
				mock.Expects.One.Method(_ => _.Call1());
				mock.Expects.One.Method(_ => _.Call2());

				using (_factory.Unordered())
				{
					mock.Expects.AtMostOne.Method(_ => _.Call3());
					mock.Expects.AtLeastOne.Method(_ => _.Call4());
				}

				mock.Expects.One.Method(_ => _.Call5());
				mock.Expects.One.Method(_ => _.Call6());
			}

			mock.MockObject.Call1();
			mock.MockObject.Call2();
			mock.MockObject.Call4();
			mock.MockObject.Call3();

			try
			{
				mock.MockObject.Call6();
			}
			catch (UnexpectedInvocationException err)
			{
				Assert.AreEqual(@"
Unexpected invocation of:
  System.Void orderedMethods.Call6()
MockFactory Expectations:
  Ordered {
    System.Void orderedMethods.Call1() [EXPECTED: 1 time CALLED: 1 time]
    System.Void orderedMethods.Call2() [EXPECTED: 1 time CALLED: 1 time]
    Unordered {
      System.Void orderedMethods.Call3() [EXPECTED: at most 1 time CALLED: 1 time]
      System.Void orderedMethods.Call4() [EXPECTED: at least 1 time CALLED: 1 time]
    }
    System.Void orderedMethods.Call5() [EXPECTED: 1 time CALLED: 0 times]
    System.Void orderedMethods.Call6() [EXPECTED: 1 time CALLED: 0 times]
  }
", err.Message);
			}

			//_factory.SuppressUnexpectedAndUnmetExpectations();
		}

		[TestMethod]
		public void MessageTest101()
		{
			using (_factory.Ordered())
			{
				mock.Expects.One.Method(_ => _.Call1());
				mock.Expects.One.Method(_ => _.Call2());

				using (_factory.Unordered())
				{
					mock.Expects.One.Method(_ => _.Call3());
					mock.Expects.One.Method(_ => _.Call4());
				}

				mock.Expects.One.Method(_ => _.Call5());
				mock.Expects.One.Method(_ => _.Call6());
			}

			try
			{
				mock.MockObject.Call6();
			}
			catch (UnexpectedInvocationException err)
			{
				Assert.AreEqual(@"
Unexpected invocation of:
  System.Void orderedMethods.Call6()
MockFactory Expectations:
  Ordered {
    System.Void orderedMethods.Call1() [EXPECTED: 1 time CALLED: 0 times]
    System.Void orderedMethods.Call2() [EXPECTED: 1 time CALLED: 0 times]
    Unordered {
      System.Void orderedMethods.Call3() [EXPECTED: 1 time CALLED: 0 times]
      System.Void orderedMethods.Call4() [EXPECTED: 1 time CALLED: 0 times]
    }
    System.Void orderedMethods.Call5() [EXPECTED: 1 time CALLED: 0 times]
    System.Void orderedMethods.Call6() [EXPECTED: 1 time CALLED: 0 times]
  }
", err.Message);
			}

			//_factory.SuppressUnexpectedAndUnmetExpectations();
		}

		[TestMethod]
		public void MessageTest102()
		{
			using (_factory.Ordered())
			{
				mock.Expects.Exactly(2).Method(_ => _.Call1());
				mock.Expects.One.Method(_ => _.Call2());

				using (_factory.Unordered())
				{
					mock.Expects.One.Method(_ => _.Call3());
					mock.Expects.One.Method(_ => _.Call4());
				}

				mock.Expects.One.Method(_ => _.Call5());
				mock.Expects.One.Method(_ => _.Call6());
			}

			mock.MockObject.Call1();

			try
			{
				mock.MockObject.Call6();
			}
			catch (UnexpectedInvocationException err)
			{
				Assert.AreEqual(@"
Unexpected invocation of:
  System.Void orderedMethods.Call6()
MockFactory Expectations:
  Ordered {
    System.Void orderedMethods.Call1() [EXPECTED: 2 times CALLED: 1 time]
    System.Void orderedMethods.Call2() [EXPECTED: 1 time CALLED: 0 times]
    Unordered {
      System.Void orderedMethods.Call3() [EXPECTED: 1 time CALLED: 0 times]
      System.Void orderedMethods.Call4() [EXPECTED: 1 time CALLED: 0 times]
    }
    System.Void orderedMethods.Call5() [EXPECTED: 1 time CALLED: 0 times]
    System.Void orderedMethods.Call6() [EXPECTED: 1 time CALLED: 0 times]
  }
", err.Message);
			}

			//_factory.SuppressUnexpectedAndUnmetExpectations();
		}

		[TestMethod]
		public void MessageTest11()
		{
			using (_factory.Unordered())
			{
				mock.Expects.One.Method(_ => _.Call1());
				mock.Expects.One.Method(_ => _.Call2());

				using (_factory.Ordered())
				{
					mock.Expects.One.Method(_ => _.Call3());
					mock.Expects.One.Method(_ => _.Call4());
				}

				mock.Expects.One.Method(_ => _.Call5());
				mock.Expects.One.Method(_ => _.Call6());
			}

			mock.MockObject.Call2();
			mock.MockObject.Call1();

			try
			{
				mock.MockObject.Call4();
			}
			catch (UnexpectedInvocationException err)
			{
				Assert.AreEqual(@"
Unexpected invocation of:
  System.Void orderedMethods.Call4()
MockFactory Expectations:
  Unordered {
    System.Void orderedMethods.Call1() [EXPECTED: 1 time CALLED: 1 time]
    System.Void orderedMethods.Call2() [EXPECTED: 1 time CALLED: 1 time]
    Ordered {
      System.Void orderedMethods.Call3() [EXPECTED: 1 time CALLED: 0 times]
      System.Void orderedMethods.Call4() [EXPECTED: 1 time CALLED: 0 times]
    }
    System.Void orderedMethods.Call5() [EXPECTED: 1 time CALLED: 0 times]
    System.Void orderedMethods.Call6() [EXPECTED: 1 time CALLED: 0 times]
  }
", err.Message);
			}

			//_factory.SuppressUnexpectedAndUnmetExpectations();
		}

		[TestMethod]
		public void MessageTest12()
		{
			using (_factory.Unordered())
			{
				using (_factory.Ordered())
				{
					mock.Expects.One.Method(_ => _.Call1());
					mock.Expects.One.Method(_ => _.Call2());
				}

				using (_factory.Unordered())
				{
					mock.Expects.One.Method(_ => _.Call3());
					mock.Expects.One.Method(_ => _.Call4());
				}

				using (_factory.Ordered())
				{
					mock.Expects.One.Method(_ => _.Call5());
					mock.Expects.One.Method(_ => _.Call6());
				}

				mock.Expects.One.Method(_ => _.Call7());
			}

			mock.MockObject.Call1();
			mock.MockObject.Call2();
			mock.MockObject.Call4();
			mock.MockObject.Call3();
			mock.MockObject.Call5();
			mock.MockObject.Call6();
			mock.MockObject.Call7();
		}

		[TestMethod]
		public void MessageTest13()
		{
			using (_factory.Unordered())
			{
				using (_factory.Ordered())
				{
					mock.Expects.One.Method(_ => _.Call1());
					mock.Expects.One.Method(_ => _.Call2());
				}

				using (_factory.Unordered())
				{
					mock.Expects.One.Method(_ => _.Call3());
					mock.Expects.One.Method(_ => _.Call4());
				}

				using (_factory.Ordered())
				{
					mock.Expects.One.Method(_ => _.Call5());
					mock.Expects.One.Method(_ => _.Call6());
				}

				mock.Expects.One.Method(_ => _.Call7());
			}

			mock.MockObject.Call5();
			mock.MockObject.Call6();
			mock.MockObject.Call1();
			mock.MockObject.Call2();
			mock.MockObject.Call4();
			mock.MockObject.Call3();
			mock.MockObject.Call7();
		}

		[TestMethod]
		public void MessageTest14()
		{
			using (_factory.Unordered())
			{
				using (_factory.Ordered())
				{
					mock.Expects.One.Method(_ => _.Call1());
					mock.Expects.One.Method(_ => _.Call2());
				}

				using (_factory.Unordered())
				{
					mock.Expects.One.Method(_ => _.Call3());
					mock.Expects.One.Method(_ => _.Call4());
				}

				using (_factory.Ordered())
				{
					mock.Expects.One.Method(_ => _.Call5());
					mock.Expects.One.Method(_ => _.Call6());
				}

				mock.Expects.One.Method(_ => _.Call7());
			}

			mock.MockObject.Call1();
			mock.MockObject.Call2();
			mock.MockObject.Call4();
			mock.MockObject.Call7();
			mock.MockObject.Call3();
			mock.MockObject.Call5();
			mock.MockObject.Call6();
		}

		[TestMethod]
		public void MessageTest2()
		{
			using (_factory.Ordered())
			{
				mock.Expects.One.Method(_ => _.Call2());
				mock.Expects.One.Method(_ => _.Call1());
			}

			try
			{
				mock.MockObject.Call1();
				mock.MockObject.Call2(); //will never hit
			}
			catch (UnexpectedInvocationException err)
			{
				Assert.AreEqual(@"
Unexpected invocation of:
  System.Void orderedMethods.Call1()
MockFactory Expectations:
  Ordered {
    System.Void orderedMethods.Call2() [EXPECTED: 1 time CALLED: 0 times]
    System.Void orderedMethods.Call1() [EXPECTED: 1 time CALLED: 0 times]
  }
", err.Message);
			}

			//_factory.SuppressUnexpectedAndUnmetExpectations();
		}

		[TestMethod]
		public void MessageTest3()
		{
			using (_factory.Ordered())
			{
				mock.Expects.One.Method(_ => _.Call1());
				mock.Expects.One.Method(_ => _.Call2());

				using (_factory.Unordered())
				{
					mock.Expects.One.Method(_ => _.Call3());
					mock.Expects.One.Method(_ => _.Call4());
				}
			}

			mock.MockObject.Call1();
			mock.MockObject.Call2();
			mock.MockObject.Call4();
			mock.MockObject.Call3();
		}

		[TestMethod]
		public void MessageTest4()
		{
			using (_factory.Ordered())
			{
				mock.Expects.One.Method(_ => _.Call1());
				mock.Expects.One.Method(_ => _.Call2());

				using (_factory.Unordered())
				{
					mock.Expects.One.Method(_ => _.Call3());
					mock.Expects.One.Method(_ => _.Call4());

					using (_factory.Ordered())
					{
						mock.Expects.One.Method(_ => _.Call5());
						mock.Expects.One.Method(_ => _.Call6());
					}
				}
			}

			mock.MockObject.Call1();
			mock.MockObject.Call2();
			mock.MockObject.Call4();
			mock.MockObject.Call3();
			try
			{
				mock.MockObject.Call6();
			}
			catch (UnexpectedInvocationException err)
			{
				Assert.AreEqual(@"
Unexpected invocation of:
  System.Void orderedMethods.Call6()
MockFactory Expectations:
  Ordered {
    System.Void orderedMethods.Call1() [EXPECTED: 1 time CALLED: 1 time]
    System.Void orderedMethods.Call2() [EXPECTED: 1 time CALLED: 1 time]
    Unordered {
      System.Void orderedMethods.Call3() [EXPECTED: 1 time CALLED: 1 time]
      System.Void orderedMethods.Call4() [EXPECTED: 1 time CALLED: 1 time]
      Ordered {
        System.Void orderedMethods.Call5() [EXPECTED: 1 time CALLED: 0 times]
        System.Void orderedMethods.Call6() [EXPECTED: 1 time CALLED: 0 times]
      }
    }
  }
", err.Message);
			}

			//_factory.SuppressUnexpectedAndUnmetExpectations();
		}

		[TestMethod]
		public void MessageTest5()
		{
			using (_factory.Ordered())
			{
				mock.Expects.One.Method(_ => _.Call1());
				mock.Expects.One.Method(_ => _.Call2());

				using (_factory.Unordered())
				{
					mock.Expects.One.Method(_ => _.Call3());
					mock.Expects.One.Method(_ => _.Call4());

					using (_factory.Ordered())
					{
						mock.Expects.One.Method(_ => _.Call5());
						mock.Expects.One.Method(_ => _.Call6());
					}
				}
			}

			mock.MockObject.Call1();
			mock.MockObject.Call2();
			mock.MockObject.Call4();
			mock.MockObject.Call3();
			mock.MockObject.Call5();
			mock.MockObject.Call6();
			try
			{
				mock.MockObject.Call7();
			}
			catch (UnexpectedInvocationException err)
			{
				Assert.AreEqual(@"
Unexpected invocation of:
  System.Void orderedMethods.Call7()
MockFactory Expectations:
  Ordered {
    System.Void orderedMethods.Call1() [EXPECTED: 1 time CALLED: 1 time]
    System.Void orderedMethods.Call2() [EXPECTED: 1 time CALLED: 1 time]
    Unordered {
      System.Void orderedMethods.Call3() [EXPECTED: 1 time CALLED: 1 time]
      System.Void orderedMethods.Call4() [EXPECTED: 1 time CALLED: 1 time]
      Ordered {
        System.Void orderedMethods.Call5() [EXPECTED: 1 time CALLED: 1 time]
        System.Void orderedMethods.Call6() [EXPECTED: 1 time CALLED: 1 time]
      }
    }
  }
", err.Message);
			}

			//_factory.SuppressUnexpectedAndUnmetExpectations();
		}

		[TestMethod]
		public void MessageTest6()
		{
			using (_factory.Ordered())
			{
				using (_factory.Unordered())
				{
					mock.Expects.One.Method(_ => _.Call3());
					mock.Expects.One.Method(_ => _.Call4());
				}

				mock.Expects.One.Method(_ => _.Call1());
				mock.Expects.One.Method(_ => _.Call2());
			}

			mock.MockObject.Call4();
			mock.MockObject.Call3();
			mock.MockObject.Call1();
			mock.MockObject.Call2();
			try
			{
				mock.MockObject.Call3();
			}
			catch (UnexpectedInvocationException err)
			{
				Assert.AreEqual(@"
Unexpected invocation of:
  System.Void orderedMethods.Call3()
MockFactory Expectations:
  Ordered {
    Unordered {
      System.Void orderedMethods.Call3() [EXPECTED: 1 time CALLED: 1 time]
      System.Void orderedMethods.Call4() [EXPECTED: 1 time CALLED: 1 time]
    }
    System.Void orderedMethods.Call1() [EXPECTED: 1 time CALLED: 1 time]
    System.Void orderedMethods.Call2() [EXPECTED: 1 time CALLED: 1 time]
  }
", err.Message);
			}
		}

		[TestMethod]
		public void MessageTest7()
		{
			using (_factory.Ordered())
			{
				mock.Expects.One.Method(_ => _.Call1());
				mock.Expects.One.Method(_ => _.Call2());

				using (_factory.Unordered())
				{
					mock.Expects.One.Method(_ => _.Call3());
					mock.Expects.One.Method(_ => _.Call4());
				}

				mock.Expects.One.Method(_ => _.Call5());
				mock.Expects.One.Method(_ => _.Call6());
			}

			mock.MockObject.Call1();
			mock.MockObject.Call2();
			mock.MockObject.Call4();
			mock.MockObject.Call3();
			mock.MockObject.Call5();
			mock.MockObject.Call6();
		}

		[TestMethod]
		public void MessageTest8()
		{
			using (_factory.Unordered())
			{
				mock.Expects.One.Method(_ => _.Call1());
				mock.Expects.One.Method(_ => _.Call2());

				using (_factory.Ordered())
				{
					mock.Expects.One.Method(_ => _.Call3());
					mock.Expects.One.Method(_ => _.Call4());
				}

				mock.Expects.One.Method(_ => _.Call5());
				mock.Expects.One.Method(_ => _.Call6());
			}

			mock.MockObject.Call2();
			mock.MockObject.Call1();
			mock.MockObject.Call3();
			mock.MockObject.Call4();
			mock.MockObject.Call6();
			mock.MockObject.Call5();
		}

		[TestMethod]
		public void MessageTest9()
		{
			using (_factory.Ordered())
			{
				mock.Expects.One.Method(_ => _.Call1());
				mock.Expects.One.Method(_ => _.Call2());

				using (_factory.Unordered())
				{
					mock.Expects.One.Method(_ => _.Call3());
					mock.Expects.One.Method(_ => _.Call4());
				}

				mock.Expects.One.Method(_ => _.Call5());
				mock.Expects.One.Method(_ => _.Call6());
			}

			try
			{
				mock.MockObject.Call2();
			}
			catch (UnexpectedInvocationException err)
			{
				Assert.AreEqual(@"
Unexpected invocation of:
  System.Void orderedMethods.Call2()
MockFactory Expectations:
  Ordered {
    System.Void orderedMethods.Call1() [EXPECTED: 1 time CALLED: 0 times]
    System.Void orderedMethods.Call2() [EXPECTED: 1 time CALLED: 0 times]
    Unordered {
      System.Void orderedMethods.Call3() [EXPECTED: 1 time CALLED: 0 times]
      System.Void orderedMethods.Call4() [EXPECTED: 1 time CALLED: 0 times]
    }
    System.Void orderedMethods.Call5() [EXPECTED: 1 time CALLED: 0 times]
    System.Void orderedMethods.Call6() [EXPECTED: 1 time CALLED: 0 times]
  }
", err.Message);
			}
		}
	}
}