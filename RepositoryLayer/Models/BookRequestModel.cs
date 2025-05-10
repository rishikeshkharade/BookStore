using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Models
{
    public class BookRequestModel
    {
        public string BookName { get; set; }
        public string AuthorName { get; set; }
        public string Description { get; set; }
        public string BookImage { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int DiscountPrice { get; set; }
    }
}
