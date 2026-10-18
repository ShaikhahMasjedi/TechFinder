using System;
using System.Collections.Generic;

namespace TechFinder.Models
{
    public partial class User
    {
        public User()
        {
            ActivityLogs = new HashSet<ActivityLog>();
            Favorites = new HashSet<Favorite>();
            Orders = new HashSet<Order>();
            Reviews = new HashSet<Review>();
            ShoppingCarts = new HashSet<ShoppingCart>();
        }

        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string UserType { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string FullName { get; set; } = null!;
        public int Login { get; set; }

        public virtual ICollection<ActivityLog> ActivityLogs { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}
