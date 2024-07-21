using System.ComponentModel.DataAnnotations;



namespace Resturant.Models
{
    public class addfood
    {

            [Key]
            public int Id { get; set; }

            [Required(ErrorMessage = "food name is required")]
            public string foodname { get; set; }

            [Required(ErrorMessage = "food price is required")]
            public int foodprice { get; set; }

            [Required(ErrorMessage = "food desc is required")]
            public string fooddesc { get; set; }

            [Required(ErrorMessage = "food image is required")]
            public string foodimage { get; set; }

            [Required(ErrorMessage = "food category is required")]
            public string foodcategory { get; set; }


    }
};
