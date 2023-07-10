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
	public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
	{
		private readonly ApplicationDbContext _db;
		public OrderDetailRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(OrderDetail obj)
		{
			_db.OrderDetails.Update(obj);
		}
	}
}
