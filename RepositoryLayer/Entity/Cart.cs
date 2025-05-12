using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartId { get; set; }

        public int UserId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }

        // unit price at time of adding
        public double Price { get; set; }

        // not stored in the DB—calculated on serialization
        [NotMapped]
        public double TotalPrice => Price * Quantity;
    }
}
