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
    public class RazorpayRepository : Repository<Razorpay>, IRazorpayRepository
    {
        eCommerceDbContext _db;
        public RazorpayRepository(eCommerceDbContext db) : base(db)
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Razorpay obj)
        {
            _db.Razorpay.Update(obj);
        }
    }
}
