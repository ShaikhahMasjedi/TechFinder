
using TechFinder.Models;

namespace TechFinder.ViewModels
{
    public class InterfacedealViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string MainImage { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountRate { get; set; }
        public double Rating { get; set; } // متوسط التقييم

        public int ReviewCount { get; set; }

        public DateTime CreatedAt { get; set; }

        //سبا
        public List<Review> Reviews { get; set; } // التقييمات (تعليقات المستخدمين)
    }

    public class InterfacenewlViewMode
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? MainImage { get; set; }
        public decimal Price { get; set; }
        public double Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Review> Reviews { get; set; } // التقييمات (تعليقات المستخدمين)
    }
}

public class AddToFavoriteViewModel
{
    public int ProductId { get; set; } // معرف المنتج
    public int UserId { get; set; } // معرف المستخدم
}


