using System;
using System.Collections.Generic;

namespace TechFinder.Models
{
    public partial class SalesAnalytic
    {
        public int AnalyticsId { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
