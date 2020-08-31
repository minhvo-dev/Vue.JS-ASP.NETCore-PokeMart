/*  Author: Vo, Dinh Tue Minh
 *  Description: This helper class represents item information in order sent from clients
 */

using CasestudyAPI.DAL.DomainClasses;

namespace CasestudyAPI.Helpers
{
    public class OrderSelectionHelper
    {
        public Product Product { get; set; }
        public int Qty { get; set; }
    }
}
