using Microsoft.AspNetCore.Mvc;
using TechFinder.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace TechFinder.ViewComponents
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly TechFinder_storeContext _context;

        public CategoryMenuViewComponent(TechFinder_storeContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }
    }
}
