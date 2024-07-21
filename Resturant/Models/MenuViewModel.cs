namespace Resturant.Models
{
    public class MenuViewModel
    {
        public List<addfood> Foods { get; set; }
        public string SearchTerm { get; set; }
        public string Category { get; set; }
        public bool IsFoodAvailable { get; set; } 
    }

}
