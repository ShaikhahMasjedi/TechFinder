
using TechFinder.ViewModels;
using TechFinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;



public class ShoppingCartController : Controller
{
    private readonly TechFinder_storeContext _context;

    public ShoppingCartController(TechFinder_storeContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult AddToCart(int productId, int quantity)
    {
        var userId = GetLoggedInUserId();

        var user = _context.Users.FirstOrDefault(u => u.Login == 1);

        if (user == null)
        {
            TempData["ErrorMessage"] = "يجب تسجيل الدخول أولاً.";
            return RedirectToAction("Login", "Account");
        }

        // التحقق إذا كان المنتج موجودًا في السلة بالفعل
        var existingCartItem = _context.ShoppingCarts
            .FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);

        if (existingCartItem != null)
        {
            // إذا كان المنتج موجودًا في السلة، نقوم بتحديث الكمية
            existingCartItem.Quantity += quantity; // زيادة الكمية
            _context.Update(existingCartItem); // التأكد من أن الكائن يتم تحديثه في الـ Context
        }
        else
        {
            // إذا لم يكن موجودًا في السلة، نقوم بإضافة المنتج
            var cartItem = new ShoppingCart
            {
                UserId = userId.Value,
                ProductId = productId,
                Quantity = quantity
            };
            _context.ShoppingCarts.Add(cartItem);
        }

        // حفظ التغييرات في قاعدة البيانات
        _context.SaveChanges();

        return StatusCode(204);
    }


    // عرض محتويات السلة
    public IActionResult ViewCart()
    {
        var userId = GetLoggedInUserId();

        // إذا لم يوجد مستخدم، نقوم بإرجاعه إلى صفحة تسجيل الدخول
        if (userId == null || userId == 0)
        {
            return RedirectToAction("Login", "Account");
        }

        // استرجاع جميع المنتجات المضافة للسلة للمستخدم
        var cartItems = _context.ShoppingCarts
            .Where(c => c.UserId == userId)
            .Include(c => c.Product)
            .ToList();

        return View(cartItems); // عرض السلة في الـ View
    }

    [HttpPost]
    public IActionResult UpdateCart(Dictionary<int, int> quantities)
    {
        var userId = GetLoggedInUserId();

        if (userId == null || userId == 0)
        {
            return RedirectToAction("Login", "User");
        }

        // تحديث الكميات في السلة بناءً على المدخلات
        foreach (var cartItem in _context.ShoppingCarts.Where(c => c.UserId == userId))
        {
            if (quantities.ContainsKey(cartItem.CartId))
            {
                cartItem.Quantity = quantities[cartItem.CartId]; // تحديث الكمية
            }
        }

        // حفظ التغييرات في قاعدة البيانات
        _context.SaveChanges();

        // إعادة تحميل السلة
        return RedirectToAction("ViewCart");
    }




    // إزالة منتج من السلة
    public IActionResult RemoveFromCart(int id)
    {
        var userId = GetLoggedInUserId();

        // إذا لم يوجد مستخدم، نقوم بإرجاعه إلى صفحة تسجيل الدخول
        if (userId == null || userId == 0)
        {
            return RedirectToAction("Login", "User");
           
        }

        // البحث عن العنصر في السلة باستخدام الـ CartId
        var cartItem = _context.ShoppingCarts.FirstOrDefault(c => c.CartId == id);
        if (cartItem != null)
        {
            _context.ShoppingCarts.Remove(cartItem);
            _context.SaveChanges();
        }

        // إعادة التوجيه إلى صفحة عرض السلة
        return RedirectToAction("ViewCart");
    }

    // دالة مساعدة للحصول على معرف المستخدم الحالي بناءً على حالة Login
    private int? GetLoggedInUserId()
    {
        // البحث عن المستخدم الذي لديه Login == 1 (باعتبار أنه مسجل دخول)
        var user = _context.Users.FirstOrDefault(u => u.Login == 1);

        // إذا تم العثور على مستخدم، نرجع الـ UserId، وإذا لم يكن موجودًا نرجع 0
        if (user != null)
        {
            return user.UserId;
        }
        else
        {
            return 0; // في حالة لم يوجد مستخدم مسجل دخوله
        }
    }
    public IActionResult AddToQuantity(int id)
    {
        ShoppingCart? sh = _context.ShoppingCarts.Where(x => x.CartId == id).FirstOrDefault();
        if (sh == null)
        {
            return NotFound();
        }
        sh.Quantity += 1;
        _context.ShoppingCarts.Update(sh);
        _context.SaveChanges();
        return RedirectToAction("ViewCart");
    }
    public IActionResult subToQuantity(int id)
    {
        ShoppingCart? sh = _context.ShoppingCarts.Where(x => x.CartId == id).FirstOrDefault();
        if (sh == null)
        {
            return NotFound();
        }
        sh.Quantity -= 1;
        _context.ShoppingCarts.Update(sh);
        _context.SaveChanges();
        return RedirectToAction("ViewCart");
    }
}

