using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CasestudyAPI.DAL.DomainClasses
{
    public class Order
    {
        public Order() => OrderLineItems = new HashSet<OrderLineItem>();
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        [Column(TypeName = "money")]
        public decimal TotalAmount { get; set; }
        [Required]
        public int CustomerId { get; set; }
        public virtual ICollection<OrderLineItem> OrderLineItems { get; set; }
        [Column(TypeName ="timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] Timer { get; set; }
    }
}
