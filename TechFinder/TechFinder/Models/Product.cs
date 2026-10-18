using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace TechFinder.Models
{
    public partial class Product
    {
        public Product()
        {
            Favorites = new HashSet<Favorite>();
            OrderDetails = new HashSet<OrderDetail>();
            Reviews = new HashSet<Review>();
            ShoppingCarts = new HashSet<ShoppingCart>();
        }

        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string MainImage { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public decimal? DiscountRate { get; set; }
        public string Extra1 { get; set; } = null!;
        public string Extra2 { get; set; } = null!;
        public int Stock { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }

        [NotMapped]
        public IFormFile ?MainImageFile { get; set; }

        [NotMapped]
        public IFormFile ?Image2File { get; set; }

        [NotMapped]
        public IFormFile ?Image3File { get; set; }

    }
}
