using System.ComponentModel.DataAnnotations;

namespace Resturant.Models
{
    public class bookedtables
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Number of persons is required")]
        public string Persons { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }
    }
}
