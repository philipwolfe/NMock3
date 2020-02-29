using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMockTests._TestStructures.Interfaces
{
	public interface IOrderDataSource
	{
		int CombineOrders(int orderId1, int orderId2);
		int AdjustQuantity(int orderId, int orderDetailId, int newQuantity);
	}
}
