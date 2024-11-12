using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;
using eCommerce.Models.Models;

namespace eCommerce.DataAccess.Repository.IRepository
{
    public interface IProductVariablesRepository : IRepository<ProductVariables>
    {
        public void Update(ProductVariables obj);
        public void Save();
    }
}
