#region Using

using System;
using NMock;

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
	[TestClass]
	public class GetArgumentsTest
	{
		public interface IMessageService
		{
			void GetMessage(int id, Action<string> responseHandler);
		}

		public class MessageController
		{
			private readonly IMessageService service;
			private string message;

			public MessageController(IMessageService service)
			{
				this.service = service;
			}

			public string Message
			{
				get
				{
					return message;
				}
			}

			public void GetMessage(int id)
			{
				service.GetMessage(id, response => message = response);
			}
		}

		/// <summary>
		/// GetArguments can be used to simulate call back from mocked dependance object
		/// </summary>
		[TestMethod]
		public void ShouldGetArgumentsOfInvokedMethod()
		{
			MockFactory mocks = new MockFactory();
			string message = "hello";
			Mock<IMessageService> serviceMock = mocks.CreateMock<IMessageService>();

			MessageController controller = new MessageController(serviceMock.MockObject);

			serviceMock.Expects.One.Method(_ => _.GetMessage(0, null)).With(Is.EqualTo(23), Is.Anything)
				.Will(GetArguments.WhenCalled(arguments => (arguments[1] as Action<string>).Invoke(message)));

			controller.GetMessage(23);

			Assert.AreEqual(message, controller.Message);
		}
	}
}