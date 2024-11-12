using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;
using eCommerce.Models.Models;

namespace eCommerce.DataAccess.Repository.IRepository
{
    public interface IFAST2SMSRepository : IRepository<FAST2SMS>
    {
        public void Update(FAST2SMS obj);
        public void Save();
    }
}
