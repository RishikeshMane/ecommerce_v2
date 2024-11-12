using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;
using eCommerce.Models.Models;

namespace eCommerce.DataAccess.Repository.IRepository
{
    public interface IEMailServiceRepository : IRepository<EMailService>
    {
        public void Update(EMailService obj);
        public void Save();
    }
}
