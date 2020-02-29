#region Using

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NMock;
using NMockTests._TestStructures;

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


#region InternalsVisibleToAttribute declaration

// This will apply to the whole assembly, but as it is only really
// relevant for this fixture, it is being kept alongside it...

// For CastleMockObjectFactory:

[assembly: InternalsVisibleTo(NMock.Constants.InternalsVisibleToDynamicProxy)]

#endregion

namespace NMockTests.MockTests
{
	public abstract class BasicTestBase
	{
		protected List<Mock<IParentInterface>> mocks;
		protected List<IParentInterface> instances; 
		protected readonly MockFactory Factory = new MockFactory();

		protected void Initalize()
		{
			mocks = new List<Mock<IParentInterface>>();
			mocks.Add(Factory.CreateMock<IParentInterface>("parentInterfaceMock"));
			mocks.Add(Factory.CreateMock<IChildInterface>("childInterfaceMock").As<IParentInterface>());
			mocks.Add(Factory.CreateMock<IGenericInterface<System.Version>>("genericInterfaceOfVersionMock").As<IParentInterface>());
			mocks.Add(Factory.CreateMock<ParentClass>("parentClassMock").As<IParentInterface>());
			mocks.Add(Factory.CreateMock<ChildClass>(DefinedAs.Named("classMock").WithArgs("Phil")).As<IParentInterface>());
			mocks.Add(Factory.CreateMock<GenericClass<System.Version>>("genericClassOfVersionMock").As<IParentInterface>());
			mocks.Add(Factory.CreateMock<AbstractParentClass>("abstractParentClassMock").As<IParentInterface>());
			//mocks.Add(Factory.CreateMock<SealedChildClass>("sealedChildClassMock").As<IParentInterface>());
#if !SILVERLIGHT
			mocks.Add(Factory.CreateMock<ExplicitImplementationParentClass>("explicitImplementationParentClassMock").As<IParentInterface>());
			mocks.Add(Factory.CreateMock<InternalParentClass>("internalParentClassMock").As<IParentInterface>());
#endif

			instances = new List<IParentInterface>();
			instances.Add(Factory.CreateInstance<IParentInterface>("parentInterfaceInstance"));
			instances.Add(Factory.CreateInstance<IChildInterface>("childInterfaceInstance"));
			instances.Add(Factory.CreateInstance<IGenericInterface<System.Version>>("genericInterfaceOfVersionInstance"));
			instances.Add(Factory.CreateInstance<ParentClass>("parentClassInstance"));
			instances.Add(Factory.CreateInstance<ChildClass>(DefinedAs.Named("classInstance").WithArgs("Phil")));
			instances.Add(Factory.CreateInstance<GenericClass<System.Version>>("genericClassOfVersionInstance"));
			instances.Add(Factory.CreateInstance<AbstractParentClass>("abstractParentClassInstance"));
#if !SILVERLIGHT
			instances.Add(Factory.CreateInstance<ExplicitImplementationParentClass>("explicitImplementationParentClassInstance"));
			instances.Add(Factory.CreateInstance<InternalParentClass>("internalParentClassInstance"));
#endif
		}

		protected void Cleanup()
		{
			Factory.VerifyAllExpectationsHaveBeenMet();
			Factory.ClearExpectations();
		}
	}
}
