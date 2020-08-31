/*  Author: Vo, Dinh Tue Minh 
 *  Description: This helper class represents order objects sent from clients
 */

namespace CasestudyAPI.Helpers
{
    public class OrderHelper
    {
        public string Email { get; set; }
        public OrderSelectionHelper[] Selections { get; set; }
    }
}
