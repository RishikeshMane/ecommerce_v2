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
    public class FAST2SMSRepository : Repository<FAST2SMS>, IFAST2SMSRepository
    {
        eCommerceDbContext _db;
        public FAST2SMSRepository(eCommerceDbContext db) : base(db)
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(FAST2SMS obj)
        {
            _db.FAST2SMS.Update(obj);
        }
    }
}
