using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class WishlistEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WishlistId { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }

        [ForeignKey("BookId")]
        public virtual Books Book { get; set; }
    }
}
