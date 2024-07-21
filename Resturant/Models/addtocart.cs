using System;

using System.ComponentModel.DataAnnotations;

namespace Resturant.Models
{
    public class addtocart
    {
        [Key]
        public int Id { get; set; }

        public int CartId { get; set; } 
        public string Name { get; set; }   

        public string Image { get; set; }

        public int Qty { get; set; }
        public int Price { get; set; }

       
    }
}
