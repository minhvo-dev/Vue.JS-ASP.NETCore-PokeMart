/*  Author: Vo, Dinh Tue Minh - INFO3067-ASP.NET
 *  Description: This data access object class provides methods 
 *  that can get 3 closest branches to a given location
 */

using CasestudyAPI.DAL.DomainClasses;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CasestudyAPI.DAL.DAO
{
    public class BranchDAO
    {
        private AppDbContext _db;
        public BranchDAO(AppDbContext ctx) => _db = ctx;
        // Get three closest branches near the input location lat and lng
        public List<Branch> GetThreeClosestBranches(float? lat, float? lng)
        {
            List<Branch> branches = null;
            try
            {
                var latSQLParam = new SqlParameter("@lat", lat);
                var lngSQLParam = new SqlParameter("@lng", lng);
                var query = _db.Branches.FromSqlRaw(
                    "dbo.pGetThreeClosestBranches @lat, @lng", latSQLParam, lngSQLParam);
                branches = query.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return branches;
        }
    }
}
