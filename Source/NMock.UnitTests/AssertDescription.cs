#region Using

using System;
using System.IO;
using NMock;
using NMock.Internal;
using NMock.Matchers;
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

namespace NMockTests
{
	public abstract class AssertDescription
	{
		public static void IsEqual(ISelfDescribing selfDescribing, string expectedDescription)
		{
			Assert.AreEqual(expectedDescription, DescriptionOf(selfDescribing), "description");
		}

		private static string DescriptionOf(ISelfDescribing selfDescribing)
		{
			TextWriter writer = new DescriptionWriter();

			selfDescribing.DescribeTo(writer);
			return writer.ToString();
		}

		public static void IsComposed(ISelfDescribing selfDescribing, string format, params ISelfDescribing[] components)
		{
			string[] componentDescriptions = new string[components.Length];
			for (int i = 0; i < components.Length; i++)
			{
				componentDescriptions[i] = DescriptionOf(components[i]);
			}

			IsEqual(selfDescribing, String.Format(format, componentDescriptions));
		}
	}
}