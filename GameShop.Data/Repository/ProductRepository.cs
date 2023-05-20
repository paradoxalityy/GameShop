using GameShop.Data.Repository.IRepository;
using GameShop.DataAccess.Data;
using GameShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.Data.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product obj)
        {
            // Using other way of updating 
            var product = _db.Products.FirstOrDefault(p => p.Id == obj.Id);
            if(product != null)
            {
                product.Name = obj.Name;
                product.Description = obj.Description;
                product.Developer = obj.Developer;
                product.Publisher = obj.Publisher;
                product.ReleaseDate = obj.ReleaseDate;
                product.ListPrice = obj.ListPrice;
                product.Price = obj.Price;
                product.Price50 = obj.Price50;
                product.Price100 = obj.Price100;
                product.CategoryId = obj.CategoryId;
                product.PlatformId = obj.PlatformId;
                if (obj.ImageUrl != null)
                {
                    product.ImageUrl = obj.ImageUrl;
                }
            }
        }
    }
}
