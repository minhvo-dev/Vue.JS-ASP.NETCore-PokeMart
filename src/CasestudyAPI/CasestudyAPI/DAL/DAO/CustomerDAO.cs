/*  Author: Vo, Dinh Tue Minh - INFO3067-ASP.NET
 *  Description: Data access object class corresponds to Customer table.
 */

using System.Linq;
using CasestudyAPI.DAL.DomainClasses;

namespace CasestudyAPI.DAL.DAO
{
    public class CustomerDAO
    {
        private AppDbContext _db;
        public CustomerDAO(AppDbContext ctx) => _db = ctx;

        // Register a new customer to the database
        public Customer Register(Customer cust)
        {
            _db.Customers.Add(cust);
            _db.SaveChanges();
            return cust;
        }

        // Query the database using email
        // return customer object if exist, null if not exist
        public Customer GetByEmail(string email)
        {
            return _db.Customers.FirstOrDefault(cust => cust.Email == email);
        }
    }
}
