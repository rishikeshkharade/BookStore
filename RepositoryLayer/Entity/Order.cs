using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http.Features;

namespace RepositoryLayer.Entity
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime PurchaseAt { get; set; }

        [NotMapped]
        public double TotalAmount => Items?.Sum(i => i.Quantity * i.Price) ?? 0;
        public virtual ICollection<OrderItem> Items { get; set; }
    }
}
