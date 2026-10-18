using System.Collections.Generic;
using TechFinder.Models;

namespace TechFinder.ViewModels
{
    // ViewModel لعرض تفاصيل المنتج
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; } // المنتج الحالي
        public List<Review> Reviews { get; set; } // قائمة التعليقات
        public List<User> Users { get; set; } // قائمة المستخدمين الذين كتبوا التعليقات
        public List<Product> SimilarProducts { get; set; } // قائمة المنتجات المشابهة
        public Dictionary<int,double> ProductRatings { get; set; }
        public double ProductRating { get; set; }
    }


    // ViewModel لإضافة تعليق
    public class AddReviewViewModel
    {
        public int ProductId { get; set; } // معرف المنتج
        public int Rating { get; set; } // التقييم (من 1 إلى 5)
        public string Comment { get; set; } // نص التعليق
    }

    // ViewModel لإضافة منتج إلى المفضلة
    public class AddToFavoriteViewModel
    {
        public int ProductId { get; set; } // معرف المنتج
        public int UserId { get; set; } // معرف المستخدم
    }

    // ViewModel لإضافة منتج إلى عربة التسوق
    public class AddToCartViewModel
    {
        public int ProductId { get; set; } // معرف المنتج
        public int Quantity { get; set; } // الكمية المطلوبة
        public int UserId { get; set; }
    }
}