﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Models
{
    public class WishlistItemModel
    {
        public int WishlistId { get; set; }
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public int Price { get; set; }
        public int DiscountPrice { get; set; }
    }
}
