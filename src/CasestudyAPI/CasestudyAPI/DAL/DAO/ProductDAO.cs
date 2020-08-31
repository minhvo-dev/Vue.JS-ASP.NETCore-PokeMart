/*  Author: Vo, Dinh Tue Minh - INFO3067-ASP.NET
 *  Description: Data access object class corresponds to Products table.
 */

using System.Collections.Generic;
using System.Linq;
using CasestudyAPI.DAL.DomainClasses;

namespace CasestudyAPI.DAL.DAO
{
    public class ProductDAO
    {
        private AppDbContext _db;
        public ProductDAO(AppDbContext ctx) => _db = ctx;
        public List<Product> GetAllByBrand(int id)
        {
            return _db.Products.Where(product => product.BrandId == id).ToList();
        }
    }
}
