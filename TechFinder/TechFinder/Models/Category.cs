using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechFinder.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }
        [NotMapped]
        public IFormFile ?ImageFile { get; set; }

        public int CategoryId { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string Name { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
