
using TechFinder.Models;

namespace TechFinder.ViewModels
{
    public class InterfaceViewModel
    {
        public List<Category> Categories { get; set; }
        public List<InterfacedealViewModel> Products { get; set; }
        public List<InterfacedealViewModel> LatestProducts { get; set; }  
        public List<InterfacedealViewModel> OldestProducts { get; set; }  
        public List<Review> Reviews { get; set; }
        public List<int> UserFavorites { get; set; }
    }
}
