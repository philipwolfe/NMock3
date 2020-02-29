#region Using

using System;
using NMockTests._TestStructures;
using NMock;

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

namespace NMockTests.MockTests
{
	/*
	[TestClass]
	public class StaticMethodsTests
	{
		[TestMethod]
		[Ignore]
		public void ExpectsTest1()
		{
			Person p = NMock.Mock.Create<Person>();

			NMock.Mock.Expects.One.Method(() => p.Speak()).WillReturn("Goodbye!");
			//NMock.Mock.Expects<Person>(q => q.BirthDate = DateTime.Parse("10/10/08"));
			//NMock.Mock.Expects(() => (p.Age));

			//p.BirthDate = DateTime.Parse("10/10/08");
			Assert.AreEqual("GoodBye!", p.Speak());
			//Assert.AreEqual(1, p.Age);
		}
	}

	public sealed class Person
	{
		public string Speak()
		{
			return "Hello, World!";
		}

		public int Age
		{
			get
			{
				return BirthDate.Age();
			}
		}

		public DateTime BirthDate { get; set; }

	}

	public static class Extensions
	{
		public static int Age(this DateTime birthDate)
		{
			DateTime now = DateTime.Today;
			int age = now.Year - birthDate.Year;
			if (birthDate > now.AddYears(-age)) age--;
			return age;
		}
	}
	*/
}
