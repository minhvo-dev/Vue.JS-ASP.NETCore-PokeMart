/*  Author: Vo, Dinh Tue Minh
 *  Description: This controller handles brand requests from users
 */

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CasestudyAPI.DAL;
using CasestudyAPI.DAL.DAO;
using CasestudyAPI.DAL.DomainClasses;
using Microsoft.AspNetCore.Authorization;

namespace CasestudyAPI.Controllers
{
    [Authorize] // all of the methods need authorized connection (with token) unless there is other instruction
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private AppDbContext _db;
        public BrandController(AppDbContext ctx) => _db = ctx;
        // return all the brands in the database
        public ActionResult<List<Brand>> Index()
        {
            BrandDAO dao = new BrandDAO(_db);
            return dao.GetAll();
        }
    }
}