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
    public class PlatformRepository : Repository<Platform>, IPlatformRepository
    {
        private readonly ApplicationDbContext _db;
        public PlatformRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Platform obj)
        {
            _db.Update(obj);
        }
    }
}
