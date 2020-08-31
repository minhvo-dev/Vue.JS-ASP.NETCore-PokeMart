/*  Author: Vo, Dinh Tue Minh
 *  Description: This controller handles user log-in action
 */

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CasestudyAPI.DAL;
using CasestudyAPI.DAL.DAO;
using CasestudyAPI.DAL.DomainClasses;
using CasestudyAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CasestudyAPI.Controllers
{
    [Authorize] // all of the methods need authorized connection (with token) unless there is other instruction
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private AppDbContext _db;
        private IConfiguration _config;

        public LoginController(AppDbContext ctx, IConfiguration config)
        {
            _db = ctx;
            _config = config;
        }

        [AllowAnonymous] // allow unauthorized connection
        [HttpPost]
        [Produces("application/json")]
        public ActionResult<CustomerHelper> Index(CustomerHelper helper)
        {
            CustomerDAO dao = new CustomerDAO(_db);
            Customer customer = dao.GetByEmail(helper.Email);
            if (customer != null && VerifyPassword(helper.Password, customer.Hash, customer.Salt)) // authenticate part
            {
                // authentication succesfull
                helper.Password = ""; // clear customer input password

                // authorization part
                // generate jwt token using MS tools
                var seed = _config.GetSection("AppSettings").GetValue<string>("SeedString");
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(seed); // get the seed from the appsettings
                var tokenDescriptor = new SecurityTokenDescriptor // create the token descriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(ClaimTypes.Name, customer.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1), // expire in 1 day
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor); // create JWT using token descriptor
                string returnToken = tokenHandler.WriteToken(token); // serialize the token into JWT in compact format
                helper.Token = returnToken;
            }
            else
            {
                helper.Token = "customer name or password invalid - login failed";
            }
            return helper;
        }

        // Authenticate customer: verify customer input password 
        // by calculating hash bytes from stored salt string and customer input password
        // then comparing it with the stored hash
        // return true if identical, false otherwise
        private static bool VerifyPassword(string enteredPW, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(enteredPW, saltBytes, RegisterController.NUMBER_ITERATION);
            var calHash = rfc2898DeriveBytes.GetBytes(RegisterController.NUMBER_HASH_BYTE);
            return Convert.ToBase64String(calHash) == storedHash; // compare two strings (do not compare two byte arrays !!!!)
        }
    }
}