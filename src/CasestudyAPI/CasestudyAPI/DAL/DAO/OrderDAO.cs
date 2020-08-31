/*  Author: Vo, Dinh Tue Minh - INFO3067-ASP.NET
 *  Description: This data access object class provides methods that add order to orders table in database
 */

using CasestudyAPI.DAL.DomainClasses;
using CasestudyAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CasestudyAPI.DAL.DAO
{
    public class OrderDAO
    {
        private AppDbContext _db;
        public OrderDAO(AppDbContext ctx) => _db = ctx;
        public int AddOrder(int customerId, OrderSelectionHelper[] selections, out bool isBackOrderred)
        {
            int orderId = -1;
            isBackOrderred = false;
            using (_db) // automatically closes the connection after finishing the operation
            {
                using(var trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        Order order = new Order();
                        order.CustomerId = customerId;
                        order.OrderDate = System.DateTime.Now;
                        order.TotalAmount = 0;

                        _db.Orders.Add(order);
                        _db.SaveChanges(); // get order id from database

                        foreach (var selection in selections)
                        {
                            OrderLineItem lineItem = new OrderLineItem();
                            lineItem.OrderId = order.Id;
                            lineItem.ProductId = selection.Product.Id;
                            if(selection.Qty <= selection.Product.QtyOnHand) // enough supply in stock
                            {
                                lineItem.QtySold = selection.Qty;
                                lineItem.QtyOrdered = selection.Qty;
                                lineItem.QtyBackOrdered = 0;
                                // updates qty on hand in the product table
                                selection.Product.QtyOnHand -= selection.Qty;
                            }
                            else // not enought supply in stock
                            {
                                lineItem.QtySold = selection.Product.QtyOnHand;
                                lineItem.QtyOrdered = selection.Qty;
                                lineItem.QtyBackOrdered = selection.Qty - selection.Product.QtyOnHand;
                                // orders new product into stock
                                selection.Product.QtyOnBackOrder += lineItem.QtyBackOrdered;
                                // updates qty on hand in the product table
                                selection.Product.QtyOnHand = 0;
                                isBackOrderred = true;
                            }
                            lineItem.SellingPrice = selection.Product.MSRP;

                            _db.Products.Update(selection.Product); // update the actual product in the database
                            _db.OrderLineItems.Add(lineItem);
                            _db.SaveChanges(); // get itemLine id from database

                            order.TotalAmount += lineItem.SellingPrice * lineItem.QtySold;
                            order.OrderLineItems.Add(lineItem);
                        }

                        _db.SaveChanges();

                        // commit the transaction if there is nothing wrong
                        trans.Commit();
                        orderId = order.Id;
                    }
                    catch (Exception ex)
                    {
                        // roll back the transaction since there is error
                        Console.WriteLine(ex.Message);
                        trans.Rollback();
                    }
                }
            }
            return orderId;
        }
        // Get all the orders from the customer who has 'customerId'
        public List<Order> GetAll(int customerId)
        {
            return _db.Orders.Where(order => order.CustomerId == customerId).ToList<Order>();
        }
        // Get the order details from an order id and a user's email
        public List<OrderDetailsHelper> GetOrderDetails(int orderId, string email)
        {
            Customer customer = _db.Customers.FirstOrDefault(customer => customer.Email == email);
            // LINQ to query database
            // fancy way (also slower imo) to do this since the OrderLineItem table is enough
            var results = from o in _db.Orders
                          join oli in _db.OrderLineItems on o.Id equals oli.OrderId
                          join p in _db.Products on oli.ProductId equals p.Id
                          where (o.Id == orderId && o.CustomerId == customer.Id)
                          select new OrderDetailsHelper
                          {
                              OrderId = o.Id,
                              ProductId = p.Id,
                              CustomerId = customer.Id,
                              ProductName = p.ProductName,
                              MSRP = p.MSRP,
                              QtyOrdered = oli.QtyOrdered,
                              QtySold = oli.QtySold,
                              QtyBackOrdered = oli.QtyBackOrdered,
                              OrderDate = o.OrderDate.ToString("yyyy/MM/dd - hh:mm tt") // the format that client can process
                          };
            return results.ToList<OrderDetailsHelper>();
        }
    }

}
