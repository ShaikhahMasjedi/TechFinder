using TechFinder.Models;

namespace TechFinder.ViewModels
{
    public class DashboardViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Order> Orders { get; set; }

        public decimal TotalEarnings { get; set; }
    }

    public class OrdersViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Order> Orders { get; set; }

    }

    public class OrderDetailsViewModel
    {
        public Order Order { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
