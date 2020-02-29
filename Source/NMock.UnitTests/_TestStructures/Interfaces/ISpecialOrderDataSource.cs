using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMockTests._TestStructures.Interfaces
{
	public interface ISpecialOrderDataSource : IOrderDataSource
	{
		decimal CombineOrders(decimal orderId1, decimal orderId2);
		decimal AdjustQuantity(int orderId, int orderDetailId, decimal newQuantity);
	}
}
