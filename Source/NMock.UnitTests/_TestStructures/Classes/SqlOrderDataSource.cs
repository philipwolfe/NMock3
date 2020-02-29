using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMockTests._TestStructures.Interfaces;

namespace NMockTests._TestStructures.Classes
{
	public abstract class SqlOrderDataSource : IOrderDataSource
	{
		public abstract int CombineOrders(int orderId1, int orderId2);

		public abstract int AdjustQuantity(int orderId, int orderDetailId, int newQuantity);

		protected void ProtectedMethod()
		{
		}

		internal void InternalMethod()
		{
		}

		protected internal void ProtectedInternalMethod()
		{
		}

		private void PrivateMethod()
		{
		}

	}
}
