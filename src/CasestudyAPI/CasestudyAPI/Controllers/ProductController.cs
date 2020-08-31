/*  Author: Vo, Dinh Tue Minh
 *  Description: This controller handles product request
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
    public class ProductController : ControllerBase
    {
        private AppDbContext _db;
        public ProductController(AppDbContext ctx) => _db = ctx;
        [Route("{brandId}")]
        public ActionResult<List<Product>> Index(int brandId)
        {
            ProductDAO dao = new ProductDAO(_db);
            return dao.GetAllByBrand(brandId);
        }
    }
}