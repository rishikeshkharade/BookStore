using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Models
{
    public class CartListResponseModel
    {
        public IEnumerable<Cart> Items { get; set; }
        public double CartTotal { get; set; }
    }
}
