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
using NMock.Proxy;
using NMockTests._TestStructures;
using NMockTests._TestStructures.Classes;
using NMockTests._TestStructures.Interfaces;
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
using CollectionAssert = NUnit.Framework.CollectionAssert;
#endif

#endregion

namespace NMock.Internal.Test
{
	[TestClass]
	public class CompositeTypeTest
	{
		private readonly string IEnumerableName = typeof (IEnumerable).Name;
		private readonly string IDisposableName = typeof (IDisposable).Name;
		private readonly string IEqualityComparerName = typeof (IEqualityComparer).Name;
		private readonly string SomeClassName = typeof (ParentClass).Name;

		private void AssertMethodWasLocated(MethodInfo expected, Matcher matcher, params Type[] typesToBuildCompositeTypeFrom)
		{
			IList<MethodInfo> matches = new CompositeType(typesToBuildCompositeTypeFrom)
				.GetMatchingMethods(matcher, false);

			CollectionAssert.Contains(matches, expected);
		}

		private void AssertMethodWasLocated(string expectedMethodName, Matcher matcher, params Type[] typesToBuildCompositeTypeFrom)
		{
			IList<MethodInfo> matches = new CompositeType(typesToBuildCompositeTypeFrom)
				.GetMatchingMethods(matcher, false);

			foreach (MethodInfo method in matches)
			{
				if (method.Name == expectedMethodName)
					return;
			}

			Assert.Fail("Could not match method '" + expectedMethodName + "'");
		}

		private void AssertAllMethodsWereLocated(MethodInfo[] expected, Matcher matcher, params Type[] typesToBuildCompositeTypeFrom)
		{
			IList<MethodInfo> matches = new CompositeType(typesToBuildCompositeTypeFrom)
				.GetMatchingMethods(matcher, false);

			CollectionAssert.IsSubsetOf(expected, matches);
		}


		[TestMethod]
		public void AllConstructorsProduceEquivalentInstances()
		{
			Assert.AreEqual(
				new CompositeType(new[] { typeof(ParentClass), typeof(IEnumerable), typeof(IDisposable) }),
				new CompositeType(typeof(ParentClass), new[] { typeof(IEnumerable), typeof(IDisposable) }));
		}

		[TestMethod, ExpectedException(typeof (ArgumentException))]
		public void ConstructorThrowsArgumentExceptionWhenDelegateTypeSupplied()
		{
			new CompositeType(new[] {typeof (IList), typeof (EventHandler)});
		}

		[TestMethod, ExpectedException(typeof (ArgumentException))]
		public void ConstructorThrowsArgumentExceptionWhenEmptyTypeArraySupplied()
		{
			new CompositeType(new Type[0]);
		}

		[TestMethod, ExpectedException(typeof (ArgumentException))]
		public void ConstructorThrowsArgumentExceptionWhenMoreThanOneClassTypeSupplied()
		{
			new CompositeType(new[] { typeof(ParentClass), typeof(IList), typeof(ChildClass) });
		}

		[TestMethod, ExpectedException(typeof (ArgumentException))]
		public void ConstructorThrowsArgumentExceptionWhenValueTypeSupplied()
		{
			new CompositeType(new[] {typeof (IList), typeof (int)});
		}

		[TestMethod, ExpectedException(typeof (ArgumentNullException))]
		public void ConstructorThrowsArgumentNullExceptionWhenNullTypeSupplied()
		{
			new CompositeType(null, new Type[0]);
		}

		[TestMethod]
		public void EqualsReturnsFalseForDifferingTypeCounts()
		{
			Assert.IsFalse(
				new CompositeType(new[] {typeof (IEnumerable), typeof (IDisposable)})
					.Equals(new CompositeType(new[] {typeof (IEnumerable), typeof (IDisposable), typeof (ICollection)})));
		}

		[TestMethod]
		public void EqualsReturnsFalseForNullInstance()
		{
			Assert.IsFalse(new CompositeType(new[] {typeof (IEnumerable)}).Equals((object) null)); // Exercises Equals(object) overload
			Assert.IsFalse(new CompositeType(new[] {typeof (IEnumerable)}).Equals(null)); // Exercises Equals(CompositeType) overload
		}

		[TestMethod]
		public void GetHashCodeAndEqualsAreConsistentForIdenticalTypes()
		{
			Type[] types = new[] { typeof(IEnumerable), typeof(IDisposable), typeof(ParentClass) };

			Assert.AreEqual(new CompositeType(types).GetHashCode(), new CompositeType(types).GetHashCode(), "Hash codes differ");
			Assert.AreEqual(new CompositeType(types), new CompositeType(types), "Expected to be equal");
		}

		[TestMethod]
		public void GetHashCodeAndEqualsAreConsistentForNonMatchingTypes()
		{
			Type[] types = new[] { typeof(ParentClass), typeof(IEnumerable) };
			Type[] otherTypes = new[] { typeof(ParentClass), typeof(ICollection) };

			Assert.AreNotEqual(new CompositeType(types).GetHashCode(), new CompositeType(otherTypes).GetHashCode(), "Hash codes are the same");
			Assert.AreNotEqual(new CompositeType(types), new CompositeType(otherTypes), "Expected to not be equal");
		}

		[TestMethod]
		public void GetHashCodeAndEqualsAreConsistentForSameTypesButDifferentOrder()
		{
			Type[] types = new[] { typeof(IEnumerable), typeof(IDisposable), typeof(ParentClass) };
			Type[] otherTypes = new[] { typeof(ParentClass), typeof(IEnumerable), typeof(IDisposable) };

			Assert.AreEqual(new CompositeType(types).GetHashCode(), new CompositeType(otherTypes).GetHashCode(), "Hash codes differ");
			Assert.AreEqual(new CompositeType(types), new CompositeType(otherTypes), "Expected to be equal");
		}

		[TestMethod]
		public void GetMatchingMethodsCanLocateInternalMethods()
		{
			AssertMethodWasLocated("InternalMethod", new MethodNameMatcher("InternalMethod"), typeof (SqlOrderDataSource));
		}


		[TestMethod]
		public void GetMatchingMethodsCanLocateMethodOnClass()
		{
			MethodInfo expected = typeof(SqlOrderDataSource).GetMethod("CombineOrders");

			AssertMethodWasLocated(expected, new EqualMatcher(expected), typeof(SqlOrderDataSource));
		}

		[TestMethod]
		public void GetMatchingMethodsCanLocateMethodOnInheritedInterface()
		{
			MethodInfo expected = typeof(IOrderDataSource).GetMethod("CombineOrders");

			AssertMethodWasLocated(expected, new EqualMatcher(expected), typeof(ISpecialOrderDataSource));
		}

		[TestMethod]
		public void GetMatchingMethodsCanLocateMethodOnInterface()
		{
			MethodInfo expected = typeof(IOrderDataSource).GetMethod("CombineOrders");

			AssertMethodWasLocated(expected, new EqualMatcher(expected), typeof(IOrderDataSource));
		}

		[TestMethod]
		public void GetMatchingMethodsCanLocateMethodOnSuperClass()
		{
			MethodInfo expected = typeof(XmlOrderDataSource).GetMethod("CombineOrders");

			AssertMethodWasLocated(expected, new EqualMatcher(expected), typeof(XmlOrderDataSource));
		}

		[TestMethod]
		public void GetMatchingMethodsCanLocateMultipleMethodsAcrossDifferentTypes()
		{
			MethodInfo[] expected = new[]
			                        	{
			                        		typeof (SqlOrderDataSource).GetMethod("CombineOrders"),
			                        		typeof (ISpecialOrderDataSource).GetMethod("CombineOrders", new[] {typeof (decimal), typeof (decimal)})
			                        	};

			AssertAllMethodsWereLocated(expected, new MethodNameMatcher("CombineOrders"), typeof(SqlOrderDataSource), typeof(ISpecialOrderDataSource));
		}

		[TestMethod]
		public void GetMatchingMethodsCanLocateProtectedInternalMethods()
		{
			AssertMethodWasLocated("ProtectedInternalMethod", new MethodNameMatcher("ProtectedInternalMethod"), typeof (SqlOrderDataSource));
		}

		[TestMethod]
		public void GetMatchingMethodsCanLocateProtectedMethods()
		{
			AssertMethodWasLocated("ProtectedMethod", new MethodNameMatcher("ProtectedMethod"), typeof(SqlOrderDataSource));
		}

		[TestMethod]
		public void GetMatchingMethodsCanReturnJustFirstMatch()
		{
			// For interfaces
			Assert.AreEqual(1, new CompositeType(new[] {typeof (ISpecialOrderDataSource)})
			                   	.GetMatchingMethods(Is.Anything, true).Count);

			// For classes
			Assert.AreEqual(1, new CompositeType(new[] {typeof (SqlOrderDataSource)})
			                   	.GetMatchingMethods(Is.Anything, true).Count);
		}

		[TestMethod]
		public void GetMatchingMethodsIgnoresPrivateMethods()
		{
			CollectionAssert.IsEmpty(new CompositeType(new[] { typeof(SqlOrderDataSource) })
			                         	.GetMatchingMethods(new MethodNameMatcher("PrivateMethod"), true));
		}

		[TestMethod]
		public void PrimaryTypeAndAdditionalInterfaceTypesCorrectWhenOnlyOneTypeSpecified()
		{
			CompositeType type = new CompositeType(new[] {typeof (IDisposable)});

			Assert.AreEqual(typeof (IDisposable), type.PrimaryType, "Incorrect PrimaryType");
			CollectionAssert.IsEmpty(type.AdditionalInterfaceTypes, "Incorrect AdditionalInterfaceTypes");
		}

		[TestMethod]
		public void PrimaryTypeAndAdditionalInterfaceTypesIdentifiedAndOrderedCorrectly()
		{
			CompositeType type = new CompositeType(new[] {typeof (IDisposable), typeof (IList), typeof (IEnumerable)});

			Assert.AreEqual(typeof (IEnumerable), type.PrimaryType, "Incorrect PrimaryType");
			CollectionAssert.AreEqual(new[] {typeof (IList), typeof (IDisposable)}, type.AdditionalInterfaceTypes, "Incorrect AdditionalInterfaceTypes");
		}

		[TestMethod]
		public void PrimaryTypeAndAdditionalInterfaceTypesIdentifiedAndOrderedCorrectlyWhenClassTypeSpecified()
		{
			CompositeType type = new CompositeType(new[] { typeof(IDisposable), typeof(ParentClass), typeof(IEnumerable) });

			Assert.AreEqual(typeof(ParentClass), type.PrimaryType, "Incorrect PrimaryType");
			CollectionAssert.AreEqual(new[] {typeof (IEnumerable), typeof (IDisposable)}, type.AdditionalInterfaceTypes, "Incorrect AdditionalInterfaceTypes");
		}

		[TestMethod]
		public void ToStringListsSingleTypeCorrectly()
		{
			CompositeType type = new CompositeType(new[] {typeof (IEnumerable)});

			Assert.AreEqual("{" + IEnumerableName + "}", type.ToString());
		}

		[TestMethod]
		public void ToStringListsTypesInExpectedOrder()
		{
			CompositeType type = new CompositeType(new[]
			                                       	{
			                                       		typeof (IEnumerable),
			                                       		typeof (IDisposable),
			                                       		typeof (ParentClass),
			                                       		typeof (IEqualityComparer)
			                                       	});

			// Should be ordered by namespace-qualified name

			Assert.AreEqual(
				"{" +
				SomeClassName + ", " +
				IEnumerableName + ", " +
				IEqualityComparerName + ", " +
				IDisposableName + "}",
				type.ToString());
		}
	}
}