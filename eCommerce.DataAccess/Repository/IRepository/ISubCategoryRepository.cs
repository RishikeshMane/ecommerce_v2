using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;
using eCommerce.Models.Models;

namespace eCommerce.DataAccess.Repository.IRepository
{
    public interface ISubCategoryRepository : IRepository<SubCategory>
    {
        public void Update(SubCategory obj);
        public void Save();
    }
}
