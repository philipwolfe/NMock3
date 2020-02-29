using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMockTests._TestStructures
{
	public class VisibilityTestClass
	{
		internal interface ISomeInternalInterface
		{
			void DoWork();
		}

		internal class SomeInternalClass
		{
			public virtual void DoWork()
			{
			}
		}

		public class SomeClassWithInternalMembers
		{
			internal virtual void DoWork()
			{
			}
		}

		protected internal interface ISomeProtectedInternalInterface
		{
			void DoWork();
		}

		protected internal class SomeProtectedInternalClass
		{
			public virtual void DoWork()
			{
			}
		}

		public class SomeClassWithProtectedInternalMembers
		{
			protected internal virtual void DoWork()
			{
			}
		}
	}
}
