/** Author: Vo, Dinh Tue Minh - INFO3067-ASP.NET
 *  Description: This class contains all information needed for an order detail
 */

namespace CasestudyAPI.Helpers
{
    public class OrderDetailsHelper
    {
        public int OrderId { get; set; }
        public string ProductId { get; set; }
        public int CustomerId { get; set; }
        public string ProductName { get; set; }
        public decimal MSRP { get; set; }
        public int QtyOrdered { get; set; }
        public int QtySold { get; set; }
        public int QtyBackOrdered { get; set; }
        public string OrderDate { get; set; }
    }
}
