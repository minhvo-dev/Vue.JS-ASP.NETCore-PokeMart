/*  Author: Vo, Dinh Tue Minh
 *  Description: This controller handles branch location requests from users
 */

using System.Collections.Generic;
using CasestudyAPI.DAL;
using CasestudyAPI.DAL.DAO;
using CasestudyAPI.DAL.DomainClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CasestudyAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private AppDbContext _db;
        public BranchController(AppDbContext ctx) => _db = ctx;
        // Get 3 closest branches to the given location
        [HttpGet("{lat}/{lng}")]
        public ActionResult<List<Branch>> Index(float lat, float lng)
        {
            BranchDAO bDao = new BranchDAO(_db);
            return bDao.GetThreeClosestBranches(lat, lng);
        }
    }
}