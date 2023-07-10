using GameShop.Data.Repository.IRepository;
using GameShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.DataAccess.Repository.IRepository
{
	public interface IOrderHeaderRepository : IRepository<OrderHeader>
	{
		public void Update(OrderHeader obj);
		public void UpdateStatus(int orderId, string orderStatus, string? paymentStatus = null);
	}
}
