using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;
using eCommerce.Data;
using eCommerce.DataAccess.Repository.IRepository;
using eCommerce.Models.Models;

namespace BulkyBook.DataAccess.Repository
{
    public class SizeRepository : Repository<Size>, ISizeRepository
    {
        eCommerceDbContext _db;
        public SizeRepository(eCommerceDbContext db) : base(db)
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Size obj)
        {
            _db.Size.Update(obj);
        }
    }
}
