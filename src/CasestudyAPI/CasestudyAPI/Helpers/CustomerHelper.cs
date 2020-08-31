/*  Author: Vo, Dinh Tue Minh - INFO3067-ASP.NET
 *  Description: This helper class represents customer objects sent from clients
 */

namespace CasestudyAPI.Helpers
{
    public class CustomerHelper
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Token { get; set; }
    }
}
