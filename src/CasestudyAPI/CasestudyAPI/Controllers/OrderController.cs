/*  Author: Vo, Dinh Tue Minh - INFO3067-ASP.NET
 *  Description: This controller handles order requests from users
 */

using System;
using System.Collections.Generic;
using CasestudyAPI.DAL;
using CasestudyAPI.DAL.DAO;
using CasestudyAPI.DAL.DomainClasses;
using CasestudyAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CasestudyAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private AppDbContext _db;
        public OrderController(AppDbContext ctx) => _db = ctx;
        [HttpPost]
        [Produces("application/json")]
        public ActionResult<string> Index(OrderHelper helper)
        {
            string retVal;
            try
            {
                CustomerDAO custDAO = new CustomerDAO(_db);
                Customer customer = custDAO.GetByEmail(helper.Email);
                OrderDAO orderDAO = new OrderDAO(_db);
                bool isBackOrderred = false;
                int orderId = orderDAO.AddOrder(customer.Id, helper.Selections, out isBackOrderred);
                
                if(orderId > 0)
                {
                    retVal = $"Order ID: {orderId} created!";
                    if (isBackOrderred) retVal += " Goods backorderred!";
                }
                else
                {
                    retVal = "Order not created!";
                }
            }
            catch(Exception ex)
            {
                retVal = "Order not created! Error: " + ex.Message;
            }
         
            return retVal;
        }
        [HttpGet("{email}")]
        public ActionResult<List<Order>> List(string email)
        {
            CustomerDAO cDao = new CustomerDAO(_db);
            OrderDAO oDao = new OrderDAO(_db);
            var customerId = cDao.GetByEmail(email).Id;
            return oDao.GetAll(customerId);
        }
        [HttpGet("{orderId}/{email}")]
        public ActionResult<List<OrderDetailsHelper>> GetOrderDetails(int orderId, string email)
        {
            OrderDAO oDao = new OrderDAO(_db);
            return oDao.GetOrderDetails(orderId, email);
        }
    }
}