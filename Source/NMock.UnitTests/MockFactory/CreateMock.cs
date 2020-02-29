#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using NMock;
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


#if NetFx35
namespace NMockTests._MockFactory35
#else
#if SILVERLIGHT
namespace NMockTests._MockFactorySL
#else
namespace NMockTests._MockFactory
#endif
#endif
{
	[TestClass]
	public class CreateMockTests
	{
			MockFactory factory = new MockFactory();
		[TestMethod]
		public void CreateGenericInterface()
		{
			var mock = factory.CreateMock<IParentInterface<IClassStub>>();
		}

		[TestMethod]
		public void CreateTransparentMockTest()
		{
			var mock = factory.CreateMock<ParentClass>(MockStyle.Transparent);

			mock.Stub.Out.Method(_ => _.Echo(null)).WithAnyArguments().WillReturn("Hello, World!");

			Assert.AreEqual("Hello, World!", mock.MockObject.Echo("something"));
		}

		[TestMethod]
		public void CreateMockWithDuplicateInterfacesTest()
		{
			var mock = factory.CreateMock<IParentInterface>(typeof(IParentInterface), typeof(IParentInterface));
			var otherMock = factory.CreateMock<IParentInterface>(DefinedAs.Implementing<IParentInterface>());

			Assert.AreEqual(mock.MockObject.GetType(), otherMock.MockObject.GetType(), "Mocks were not of the same runtime type");
		}

		[TestMethod, ExpectedException(typeof(ArgumentException))]
		public void MockingAClassAsAnAdditionalTypeWhenAlreadyMockingAClassThrowsArgumentException()
		{
			var mock = factory.CreateMock<ParentClass>(DefinedAs.Implementing(typeof(IEnumerable), typeof(ChildClass)));
		}

		[TestMethod, ExpectedException(typeof(ArgumentException))]
		public void MockingMultipleClassesAsAdditionalTypesThrowsArgumentException()
		{
			Mock<IEnumerable> mock = factory.CreateMock<IEnumerable>(DefinedAs.Implementing(typeof(ParentClass), typeof(ChildClass)));
		}

		[TestMethod]
		public void OrderOfInterfacesIsIgnored()
		{
			Mock<IEnumerable> mock = factory.CreateMock<IEnumerable>(DefinedAs.Implementing(typeof(IDisposable), typeof(IParentInterface)));
			Mock<IDisposable> otherMock = factory.CreateMock<IDisposable>(DefinedAs.Implementing(typeof(IParentInterface), typeof(IEnumerable)));

			Assert.AreEqual(mock.MockObject.GetType(), otherMock.MockObject.GetType());
		}

		[TestMethod]
		public void TransparentMocksAllowImplementationOfClassTypeToBeCalled()
		{
			var mock = factory.CreateMock<IEnumerable>(DefinedAs.OfStyle(MockStyle.Transparent).Implementing<ParentClass>()).As<ParentClass>();

			Assert.AreEqual(9, mock.MockObject.ReadOnlyObjectProperty.Major);
		}
	}

	public interface IGenericInterface<TGenericType> where TGenericType : IClassStub
	{
		void Method<TGenericSubType>() where TGenericSubType : TGenericType;
	}

	public interface IClassStub { }

}
