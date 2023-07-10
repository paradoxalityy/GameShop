using GameShop.Data.Repository.IRepository;
using GameShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.DataAccess.Repository.IRepository
{
	public interface IOrderDetailRepository : IRepository<OrderDetail>
	{
		public void Update(OrderDetail obj);
	}
}
