#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
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

namespace NMockTests
{
	public interface IDependencyProvider
	{
		IDependency InterfaceDependency { get; }
		Dependency ClassDependency { get; }
		int Integer { get; }
		string String { get; }
		object GetAndSetProperty { get; set; }

		IDependency GetInterfaceDependency();
		Dependency GetClassDependency();

		int GetInteger();

		List<int> GetList();

		void NoReturnValue();

		int OverloadedMethod(int input);
		string OverloadedMethod(string input);
	}

	public abstract class DependencyProvider : IDependencyProvider
	{
		#region IDependencyProvider Members

		public abstract IDependency InterfaceDependency { get; }
		public abstract Dependency ClassDependency { get; }

		public abstract int Integer { get; }
		public abstract string String { get; }

		public abstract object GetAndSetProperty { get; set; }

		public abstract IDependency GetInterfaceDependency();
		public abstract Dependency GetClassDependency();

		public abstract int GetInteger();

		public abstract List<int> GetList();

		public abstract void NoReturnValue();

		public abstract int OverloadedMethod(int input);
		public abstract string OverloadedMethod(string input);

		#endregion
	}

	public interface IDependency
	{
		Dependency AnotherDependency { get; }
	}

	public abstract class Dependency
	{
		public abstract IDependency AnotherDependency { get; }
	}

	/// <summary>
	/// Generic base class to allow same tests for interfaces and classes and
	/// <see cref="MockStyle.Stub"/> and <see cref="MockStyle.Stub"/>.
	/// </summary>
	/// <typeparam name="TMock">The type of the mock.</typeparam>
	[TestClass]
	public abstract class MockStyleStubTest<TMock> where TMock : class, IDependencyProvider
	{
		/// <summary>
		/// interface used in tests.
		/// </summary>
		private Mock<TMock> dependencyProvider;

		private Mock<TMock> mock;

		/// <summary>
		/// Mock factory.
		/// </summary>
		private MockFactory mockFactory;

		/// <summary>
		/// Gets the dependency provider that is tested.
		/// </summary>
		/// <value>The dependency provider.</value>
		protected Mock<TMock> DependencyProvider
		{
			get
			{
				return dependencyProvider;
			}
		}

		/// <summary>
		/// Sets up the tests.
		/// </summary>
		[TestInitialize]
		public void SetUp()
		{
			mockFactory = new MockFactory();

			dependencyProvider = mockFactory.CreateMock<TMock>(MockStyle.Stub);
			mock = mockFactory.CreateMock<TMock>(MockStyle.Stub);
		}

		/// <summary>
		/// Stubs return newly created mocks with <see cref="MockStyle.Stub"/> as results to method/properties that return interfaces.
		/// </summary>
		[TestMethod]
		public void StubsReturnMocksForInterfaces()
		{
			Assert.IsNotNull(dependencyProvider.MockObject.InterfaceDependency);
			Assert.IsNotNull(dependencyProvider.MockObject.GetInterfaceDependency());
		}

		/// <summary>
		/// Stubs return mocks with <see cref="MockStyle.Stub"/> as results to method/properties that return classes.
		/// </summary>
		[TestMethod]
		public void StubsReturnMocksForClasses()
		{
			Assert.IsNotNull(dependencyProvider.MockObject.ClassDependency);
			Assert.IsNotNull(dependencyProvider.MockObject.GetClassDependency());
		}

		/// <summary>
		/// Stubs return default values for value types.
		/// </summary>
		[TestMethod]
		public void StubsReturnDefaultValuesForValueTypes()
		{
			Assert.AreEqual(default(int), dependencyProvider.MockObject.Integer);
			Assert.AreEqual(default(int), dependencyProvider.MockObject.GetInteger());
		}

		/// <summary>
		/// Stubs return an empty string for string return values.
		/// </summary>
		[TestMethod]
		public void StubsReturnStringEmptyForStrings()
		{
			Assert.AreEqual(string.Empty, dependencyProvider.MockObject.String);
		}

		/// <summary>
		/// Stubs swallow calls to void methods.
		/// </summary>
		[TestMethod]
		public void StubsSwallowCallsToVoidMethods()
		{
			dependencyProvider.MockObject.NoReturnValue();
		}

		/// <summary>
		/// The value returned by a stub can be overruled depending on the requested type.
		/// Missing.Value is used to not override the default behavior.
		/// </summary>
		[TestMethod]
		public void ResolveType()
		{
			mockFactory.SetResolveTypeHandler(delegate(object mock, Type requestedType)
			                              	{
			                              		if (mock == (object) dependencyProvider.MockObject && requestedType == typeof (int))
			                              		{
			                              			return 5;
			                              		}

			                              		return Missing.Value;
			                              	});

			Assert.AreEqual(5, dependencyProvider.MockObject.GetInteger());
		}

		/// <summary>
		/// The <see cref="MockStyle"/> used for a returned mock can be overruled depending on the type of the
		/// return value and the stub that is called.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof (UnexpectedInvocationException))]
		public void ResolveMockStyleWhenMockStyleIsDefinedForType()
		{
			mockFactory.SetStubMockStyle<Dependency>(dependencyProvider.MockObject, MockStyle.Default);

			Dependency dependency = dependencyProvider.MockObject.ClassDependency;
			IDependency anotherDependency = dependency.AnotherDependency;
		}

		/// <summary>
		/// The <see cref="MockStyle"/> used for a returned mock can be overruled depending on the type of the
		/// return value and the stub that is called.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof (UnexpectedInvocationException))]
		public void ResolveMockStyleWhenMockStyleIsDefinedForStub()
		{
			mockFactory.SetStubMockStyle(dependencyProvider.MockObject, MockStyle.Default);

			Dependency dependency = dependencyProvider.MockObject.ClassDependency;
			IDependency anotherDependency = dependency.AnotherDependency;
		}

		/// <summary>
		/// Stubs returns empty objects for enumerables (lists, dictionaries, ...)
		/// </summary>
		[TestMethod]
		public void StubReturnsEmptyObjectForEnumerables()
		{
			Assert.AreEqual(0, dependencyProvider.MockObject.GetList().Count);
		}

		/// <summary>
		/// Makes sure that method overloads are correctly identified
		/// when stub behaviour is applied.
		/// </summary>
		[TestMethod]
		public void StubsRespectOverloadedMethods()
		{
			Assert.AreEqual(0, dependencyProvider.MockObject.OverloadedMethod(1));
			Assert.AreEqual(string.Empty, dependencyProvider.MockObject.OverloadedMethod("A"));
		}

		/// <summary>
		/// The name of auto stubs reflect the path they were accessed/created.
		/// </summary>
		[TestMethod]
		public void Naming()
		{
			IDependency dependency = dependencyProvider.MockObject.InterfaceDependency;

			Assert.AreEqual("dependencyProvider.InterfaceDependency", ((IMockObject) dependency).MockName, "wrong name of direct dependency.");

			Assert.AreEqual(
				"dependencyProvider.InterfaceDependency.AnotherDependency.AnotherDependency.AnotherDependency",
				((IMockObject) dependency.AnotherDependency.AnotherDependency.AnotherDependency).MockName,
				"wrong name of indirect dependency.");
		}

		/// <summary>
		/// Calls on the same property/method return the same value.
		/// </summary>
		[TestMethod]
		public void SameValueIsReturned()
		{
			Assert.AreSame(DependencyProvider.MockObject.ClassDependency, DependencyProvider.MockObject.ClassDependency);
			Assert.AreSame(DependencyProvider.MockObject.GetInterfaceDependency(), DependencyProvider.MockObject.GetInterfaceDependency());
			Assert.AreSame(DependencyProvider.MockObject.GetList(), DependencyProvider.MockObject.GetList());
		}

		/// <summary>
		/// Stubs remember the values set on properties and the value can be accessed with the getter.
		/// </summary>
		[TestMethod]
		public void SetAndGetProperty()
		{
			object value = "hello";

			DependencyProvider.MockObject.GetAndSetProperty = value;
			Assert.AreSame(value, DependencyProvider.MockObject.GetAndSetProperty);

			DependencyProvider.MockObject.GetAndSetProperty = 3;
			Assert.AreEqual(3, DependencyProvider.MockObject.GetAndSetProperty, "wrong value on second set.");
		}
	}

	/// <summary>
	/// Tests the <see cref="MockStyle.Stub"/> option for interfaces.
	/// </summary>
	[TestClass]
	public class InterfaceMockStyleRecursiveStubTest : MockStyleStubTest<IDependencyProvider>
	{
	}

	/// <summary>
	/// Tests the <see cref="MockStyle.Stub"/> option for classes.
	/// </summary>
	[TestClass]
	public class ClassMockStyleRecursiveStubTest : MockStyleStubTest<DependencyProvider>
	{
	}
}