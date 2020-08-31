/*  Author: Vo, Dinh Tue Minh - INFO3067-ASP.NET
 *  Description: This controller handles register from users, generates hash code and salt code
 */

using System;
using System.Security.Cryptography;
using CasestudyAPI.DAL;
using CasestudyAPI.DAL.DAO;
using CasestudyAPI.DAL.DomainClasses;
using CasestudyAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CasestudyAPI.Controllers
{
    [Authorize] // all of the methods need authorized connection (with token) unless there is other instruction
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private AppDbContext _db;
        // two static const properties for login controller
        public const int NUMBER_ITERATION = 10000; // always static in C#
        public const int NUMBER_HASH_BYTE = 256; // always static in C#
        public RegisterController(AppDbContext ctx) => _db = ctx;

        [AllowAnonymous] // allow unauthorized connection
        [HttpPost] // use POST
        [Produces("application/json")]
        public ActionResult<CustomerHelper> Index(CustomerHelper helper)
        {
            CustomerDAO dao = new CustomerDAO(_db);
            Customer customer = dao.GetByEmail(helper.Email);
            // check if the customer already exists
            if (customer == null) // new customer
            {
                // generate salt and password for this new customer
                HashSalt hashSalt = GenerateSaltedHash(64, helper.Password);
                helper.Password = ""; // remove the customer input string since we already hashed it
                Customer registeringCustomer = new Customer
                {
                    Firstname = helper.Firstname,
                    Lastname = helper.Lastname,
                    Email = helper.Email,
                    Hash = hashSalt.Hash,
                    Salt = hashSalt.Salt
                };
                registeringCustomer = dao.Register(registeringCustomer); // register customer to db
                // check if the registration succeed
                if (registeringCustomer.Id > 0)
                {
                    helper.Token = "customer registered";
                }
                else
                {
                    helper.Token = "customer registration failed";
                }
            }
            else
            {
                helper.Token = "customer registration failed - email already in use";
            }
            return helper;
        }

        // generate hash and salt string from a given password with a given size
        // it works the way it does :(
        private static HashSalt GenerateSaltedHash(int size, string password)
        {
            var saltBytes = new byte[size];
            var provider = new RNGCryptoServiceProvider();
            // fills an array of bytes with a cryptographically strong sequence 
            // of random nonzero values
            provider.GetNonZeroBytes(saltBytes);
            // convert the salt bytes to base 64 string
            var salt = Convert.ToBase64String(saltBytes);
            // a password, salt, and iteration count, then generates a binary key
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, NUMBER_ITERATION);
            // get 256 byte from the rfc2898DerivedBytes then convert to base 64 string
            var hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(NUMBER_HASH_BYTE));

            // return the result
            HashSalt hashSalt = new HashSalt { Hash = hashPassword, Salt = salt };
            return hashSalt;
        }
    }

    // datatype to store generated hash and salt string
    public class HashSalt
    {
        public string Hash { get; set; }
        public string Salt { get; set; }
    }

}