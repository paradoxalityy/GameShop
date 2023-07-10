using GameShop.Data.Repository;
using GameShop.DataAccess.Data;
using GameShop.DataAccess.Repository.IRepository;
using GameShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.DataAccess.Repository
{
	public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
	{
		private readonly ApplicationDbContext _db;
		public OrderHeaderRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(OrderHeader obj)
		{
			_db.OrderHeaders.Update(obj);
		}

		public void UpdateStatus(int orderId, string orderStatus, string? paymentStatus = null)
		{
			var orderToUpdate = _db.OrderHeaders.FirstOrDefault(o => o.Id == orderId);
			if (orderToUpdate != null)
			{
				orderToUpdate.OrderStatus = orderStatus;
				if (paymentStatus != null) 
				{
					orderToUpdate.PaymentStatus = paymentStatus;
				}
			}
		}
	}
}
