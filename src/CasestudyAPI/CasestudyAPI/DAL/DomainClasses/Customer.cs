using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CasestudyAPI.DAL.DomainClasses
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string Email { get; set; }
        // We do not store an actual password in our database for security
        // instead, we store hash string of it and a salt string
        [Required]
        public string Hash { get; set; }
        [Required]
        public string Salt { get; set; }
    }
}
