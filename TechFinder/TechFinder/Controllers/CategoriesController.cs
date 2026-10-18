
using System.Linq;

using TechFinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechFinder.ViewModels;

public class CategoriesController : Controller
{
    private readonly TechFinder_storeContext _context;


    public CategoriesController(TechFinder_storeContext context)
    {
        _context = context;
    }

    // عرض تفاصيل الفئة
    public async Task<IActionResult> Details(int categoryId)
    {


        // جلب الفئة والمنتجات المرتبطة بها
        var category = await _context.Categories
                .Include(c => c.Products) // تضمين المنتجات المرتبطة بالفئة
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
        List<int> userFavorites = new List<int>();
        var user = _context.Users.FirstOrDefault(u => u.Login == 1); // تأكد من أن الشرط صحيح لتحديد المستخدم
        if (user != null)
        {
            userFavorites = _context.Favorites
                .Where(f => f.UserId == user.UserId)
                .Select(f => f.ProductId)
                .ToList();
        }

        if (category == null)
        {
            return NotFound();
        }
        var viewModel = new categoryfav
        {
            Categories = category,
            UserFavorites = userFavorites // إضافة قائمة المفضلات
        };

        // إرسال الكائن Category مباشرة إلى الـ View
        return View(viewModel); // تأكد من أن الـ View يستقبل Category
    }



    [HttpPost]
    public IActionResult AddToFavorite(int productId)
    {
        // البحث عن المستخدم الذي لديه log = 1
        var user = _context.Users.FirstOrDefault(u => u.Login == 1);
        if (user == null)
        {
            TempData["ErrorMessage"] = "User not found.";
            return RedirectToAction("Index", "Home");
        }

        // التحقق من أن المنتج غير موجود بالفعل في المفضلة
        var existingFavorite = _context.Favorites
            .FirstOrDefault(f => f.UserId == user.UserId && f.ProductId == productId);

        if (existingFavorite == null)
        {
            var favorite = new Favorite
            {
                UserId = user.UserId,
                ProductId = productId
            };

            _context.Favorites.Add(favorite);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "! The product has been successfully added to favorites";
        }
        else
        {
            TempData["ErrorMessage"] = "The product is already in your favorites.";
        }

        // 🔥 نجيب الكاتقوري آي دي الخاص بالمنتج
        var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
        if (product == null)
        {
            return RedirectToAction("Index", "Home"); // منتج غير موجود
        }

        int categoryId = product.CategoryId;

        // ✅ نعيد التوجيه إلى صفحة تفاصيل الفئة الصحيحة
        return RedirectToAction("Details", "Categories", new { categoryId = categoryId });
    }

}

