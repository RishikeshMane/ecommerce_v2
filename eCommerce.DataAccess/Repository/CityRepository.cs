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
    public class CityRepository : Repository<City>, ICityRepository
    {
        eCommerceDbContext _db;
        public CityRepository(eCommerceDbContext db) : base(db)
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(City obj)
        {
            _db.City.Update(obj);
        }
    }
}
