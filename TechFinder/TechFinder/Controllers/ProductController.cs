using Microsoft.AspNetCore.Mvc;
using TechFinder.Models;
using TechFinder.ViewModels;
using System.Security.Claims;
using System.Linq;

namespace TechFinder.Controllers
{
    public class ProductController : Controller
    {
        private readonly TechFinder_storeContext _context;

        public ProductController(TechFinder_storeContext context)
        {
            _context = context;
        }

        // عرض تفاصيل المنتج
        public IActionResult Details(int id)
        {
            // البحث عن المنتج باستخدام ProductId
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound(); // إذا لم يتم العثور على المنتج، إرجاع صفحة 404
            }

            // جلب التعليقات المرتبطة بالمنتج
            var reviews = _context.Reviews
                .Where(r => r.ProductId == id)
                .ToList();

            // جلب المستخدمين الذين كتبوا التعليقات
            var userIds = reviews.Select(r => r.UserId).Distinct();
            var users = _context.Users
                .Where(u => userIds.Contains(u.UserId))
                .ToList();

            //حساب متوسط التقييم للمنتج الرئسي
            double productRating = 0;
            if (reviews.Any())
            {
                productRating = reviews.Average(r => r.Rating);
            }
            // جلب المنتجات المشابهة (من نفس الفئة)
            var similarProducts = _context.Products
                .Where(p => p.CategoryId == product.CategoryId && p.ProductId != id)
                .Take(3) // عرض 3 منتجات مشابهة
                .ToList();

            //حساب متوسط التقييم لكل منتج مشابه
            var productRatings=new Dictionary<int, double>();
            foreach (var similarProduct in similarProducts)
            {
                var productReviews = _context.Reviews.Where(r => r.ProductId == similarProduct.ProductId).ToList();
                if (productReviews.Any())
                {
                    productRatings[similarProduct.ProductId]=productReviews.Average(r => r.Rating);
                }
                else
                {
                    productRatings[similarProduct.ProductId] = 0;
                }
                    
            
            }
            // إنشاء ViewModel
            var viewModel = new ProductDetailsViewModel
            {
                Product = product,
                Reviews = reviews,
                Users = users,
                SimilarProducts = similarProducts,
                ProductRatings = productRatings,
                ProductRating = productRating,

            };


            return View(viewModel);
        }

        // POST: إضافة المنتج إلى عربة التسوق والبقاء في الصفحة
        [HttpPost]
        public IActionResult AddToCartAndStay(int productId, int quantity = 1)
        {
            // البحث عن المستخدم الذي لديه log = 1

            var user = _context.Users.FirstOrDefault(u => u.Login == 1);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // البحث عن المنتج في عربة التسوق
            var existingCartItem = _context.ShoppingCarts
                .FirstOrDefault(c => c.UserId == user.UserId && c.ProductId == productId);

            if (existingCartItem != null)
            {
                // إذا كان المنتج موجودًا في السلة، قم بتحديث الكمية
                existingCartItem.Quantity += quantity;
                TempData["SuccessMessage"] = "! The product quantity in the shopping cart has been updated";
            }
            else
            {
                // إذا لم يكن المنتج موجودًا في السلة، قم بإضافته
                var cartItem = new ShoppingCart
                {
                    UserId = user.UserId,
                    ProductId = productId,
                    Quantity = quantity
                };

                _context.ShoppingCarts.Add(cartItem);
                TempData["SuccessMessage"] = "!The product has been successfully added to the shopping cart";
            }

            _context.SaveChanges();
            return RedirectToAction("Details", new { id = productId });
        }

        // POST: إضافة المنتج إلى عربة التسوق والانتقال إلى صفحة العربة
        [HttpPost]
        public IActionResult BuyNow(int productId, int quantity = 1)
        {
            // البحث عن المستخدم الذي لديه log = 1
            var user = _context.Users.FirstOrDefault(u => u.Login == 1);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // البحث عن المنتج في عربة التسوق
            var existingCartItem = _context.ShoppingCarts
                .FirstOrDefault(c => c.UserId == user.UserId && c.ProductId == productId);

            if (existingCartItem != null)
            {
                // إذا كان المنتج موجودًا في السلة، قم بتحديث الكمية
                existingCartItem.Quantity += quantity;
            }
            else
            {
                // إذا لم يكن المنتج موجودًا في السلة، قم بإضافته
                var cartItem = new ShoppingCart
                {
                    UserId = user.UserId,
                    ProductId = productId,
                    Quantity = quantity
                };

                _context.ShoppingCarts.Add(cartItem);
            }

            _context.SaveChanges();

            return RedirectToAction("ViewCart", "ShoppingCart");
        }

        // POST: إضافة المنتج إلى المفضلة
        [HttpPost]
        public IActionResult AddToFavorite(int productId)
        {
            // البحث عن المستخدم الذي لديه log = 1

            var user = _context.Users.FirstOrDefault(u => u.Login == 1);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // التحقق من أن المنتج غير موجود بالفعل في المفضلة
            var existingFavorite = _context.Favorites
                .FirstOrDefault(f => f.UserId == user.UserId && f.ProductId == productId);

            if (existingFavorite == null)
            {
                // إضافة المنتج إلى المفضلة
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
                TempData["ErrorMessage"] = "The product is already in your favoriteshas ";
            }

            return RedirectToAction("Details", new { id = productId });
        }

        // POST: إرسال تعليق وتقييم
        [HttpPost]
        public IActionResult AddReview(int productId, int rating, string comment = null)
        {
            // البحث عن المستخدم الذي لديه log = 1
            var user = _context.Users.FirstOrDefault(u => u.Login == 1);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }


            // إضافة التقييم والتعليق
            var review = new Review
            {
                UserId = user.UserId,
                ProductId = productId,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.Now
            };

            _context.Reviews.Add(review);
            _context.SaveChanges();

            TempData["ReviewSuccessMessage"] = "! The rating has been added successfully";
            return RedirectToAction("Details", new { id = productId });
        }






        ///

        //Favotite
        // عرض قائمة المفضلة
        public IActionResult Favorite()
        {
            // البحث عن المستخدم الذي لديه log = 1
            var user = _context.Users.FirstOrDefault(u => u.Login == 1);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }


            // جلب المنتجات المفضلة للمستخدم
            var favoriteProducts = _context.Favorites
                .Where(f => f.UserId == user.UserId) // تصفية حسب UserId
                .Select(f => f.Product) // اختيار المنتجات المرتبطة
                .ToList();

            //حساب متوسط التقييم لكل منتج
            var productRatings = new Dictionary<int, double>();
            foreach (var product in favoriteProducts)
            {
                var reviews = _context.Reviews.Where(r => r.ProductId == product.ProductId).ToList();
                if (reviews.Any())
                {
                    productRatings[product.ProductId] = reviews.Average(r => r.Rating);
                }
                else
                {
                    productRatings[product.ProductId] = 0;
                }
            }

            // إنشاء ViewModel
            var viewModel = new FavoriteViewModel
            {
                FavoriteProducts = favoriteProducts,
                IsEmpty = !favoriteProducts.Any() ,// تحديد إذا كانت القائمة فارغة
                ProductRatings = productRatings
            };

            //ViewBag.Header = "interfaceheader";
            return View(viewModel); // إرسال ViewModel إلى الـ View
        }

        // POST: حذف منتج من المفضلة
        [HttpPost]
        public IActionResult RemoveFromFavorite(int productId)
        {
            // البحث عن المستخدم الذي لديه log = 1
            var user = _context.Users.FirstOrDefault(u => u.Login == 1);

            // البحث عن المنتج في المفضلة
            var favorite = _context.Favorites
                .FirstOrDefault(f => f.UserId == user.UserId && f.ProductId == productId);

            if (favorite != null)
            {
                // حذف المنتج من المفضلة
                _context.Favorites.Remove(favorite);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "! The product has been successfully deleted ";
            }
            

            return RedirectToAction("Favorite");
        }

        // POST: إضافة منتج إلى سلة التسوق
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            // البحث عن المستخدم الذي لديه log = 1
            var user = _context.Users.FirstOrDefault(u => u.Login == 1);

            

            // البحث عن المنتج في سلة التسوق
            var existingCartItem = _context.ShoppingCarts
                .FirstOrDefault(c => c.UserId == user.UserId && c.ProductId == productId);

            if (existingCartItem != null)
            {
                // إذا كان المنتج موجودًا في السلة، قم بتحديث الكمية
                existingCartItem.Quantity += quantity;
                TempData["SuccessMessage"] = " ! The product quantity in the shopping cart has been updated ";
            }
            else
            {
                // إذا لم يكن المنتج موجودًا في السلة، قم بإضافته
                var cartItem = new ShoppingCart
                {
                    UserId = user.UserId,
                    ProductId = productId,
                    Quantity = quantity
                };
                
                _context.ShoppingCarts.Add(cartItem);
                TempData["SuccessMessage"] = " !The product has been successfully added to the shopping cart";
            }

            _context.SaveChanges();
            return RedirectToAction("Favorite");
        }


        //User Protofile
        // عرض معلومات المستخدم وتفاصيل الطلبات
        public IActionResult UserProfile()
        {
            // البحث عن المستخدم الذي لديه log = 1
            var user = _context.Users.FirstOrDefault(u => u.Login == 1);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            // جلب الطلبات الخاصة بالمستخدم
            var orders = _context.Orders
                .Where(o => o.UserId == user.UserId)
                .Select(o => new OrderViewModel
                {
                    OrderId = o.OrderId,
                    TotalAmount = o.TotalAmount,
                    CreatedAt = o.CreatedAt,
                    PaymentMethod = o.Payments.First().PaymentMethod,
                    /*.Any() ? o.Payments.First().PaymentMethod : "Not Paid",*/ // طريقة الدفع*/
                    OrderDetails = o.OrderDetails.Select(od => new OrderDetailViewModel
                    {
                        ProductName = od.Product.Name,
                        UnitPrice = od.Price,
                        Quantity = od.Quantity,
                        TotalPrice = od.Price * od.Quantity
                    }).ToList()
                })
                .ToList();

            // إنشاء ViewModel
            var viewModel = new UserProfileViewModel
            {
                User = user,
                Orders = orders
            };

            ViewBag.Header = "interfaceheader";

            // إرسال ViewModel إلى الـ View
            return View(viewModel);
        }


        // Edit user profile
        // GET: عرض نموذج تعديل بيانات المستخدم
        public IActionResult EditUserProfile()
        {
            ViewBag.Header = "interfaceheader";
            // البحث عن المستخدم الذي لديه log = 1
            var user = _context.Users.FirstOrDefault(u => u.Login == 1);
            ViewBag.Header = "interfaceheader";
            return View(user);
        }

        // POST: حفظ التعديلات على بيانات المستخدم
        [HttpPost]
        public IActionResult EditUserProfile(User user)
        {
            
                // البحث عن المستخدم الذي لديه log = 1
            var existingUser = _context.Users.FirstOrDefault(u => u.Login == 1);

            if (existingUser == null)
            {
                return RedirectToAction("Login", "Account");
            }
            // تحديث بيانات المستخدم
            existingUser.FullName = user.FullName;
                existingUser.Email = user.Email;
                
                existingUser.Password = user.Password;               
                

                // حفظ التعديلات في قاعدة البيانات
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Your profile has been updated successfully!";
                return RedirectToAction("UserProfile"); // توجيه المستخدم إلى صفحة البروفايل بعد التعديل
        }






    }
}