using System.Collections.Generic;
using TechFinder.Models;

namespace TechFinder.ViewModels
{
    public class FavoriteViewModel
    {
        public List<Product> FavoriteProducts { get; set; }
        public bool IsEmpty { get; set; } // لتحديد إذا كانت القائمة فارغة
        public Dictionary<int,double> ProductRatings { get; set; }
    }
   
}