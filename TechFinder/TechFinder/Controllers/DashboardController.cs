using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechFinder.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
using TechFinder.ViewModels;
using Microsoft.IdentityModel.Tokens;

namespace TechFinder.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly TechFinder_storeContext _techService;

        public DashboardController(IWebHostEnvironment env,TechFinder_storeContext techService)
        {
            _env = env;
            _techService = techService;
        }

        public IActionResult Index()
        {
            var user = _techService.Users.FirstOrDefault(u => u.Login == 1 && u.UserType == "admin");

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            try
            {
                var viewModel = new DashboardViewModel
                {
                    Categories = _techService.Categories.ToList(),
                    Products = _techService.Products.Include(p => p.Category).ToList(),
                    Users = _techService.Users.ToList(),
                    Orders = _techService.Orders.ToList(),

                    TotalEarnings = _techService.Orders.Sum(o => o.TotalAmount)

                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
                return RedirectToAction("Error", "Home");
            }
                
        }

        public IActionResult employees()
        {
            var user = _techService.Users.FirstOrDefault(u => u.Login == 1 && u.UserType == "admin");

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            try
            {
                var employees = _techService.Users.ToList();
                return View(employees);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
                return RedirectToAction("Error", "Home");
            }

        }

        [HttpGet]
        public IActionResult employeesadd()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult employeesadd(User user)
        {
            try
            {
                user.UserType = "admin";
                user.CreatedAt = DateTime.Now;
                user.Login = 0;
                _techService.Users.Add(user);
                _techService.SaveChanges();
                return RedirectToAction("employees");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult employeeDetails(int id)
        {
            try
            {
                var employee = _techService.Users.FirstOrDefault(u => u.UserId == id);

                if (employee == null)
                {
                    return NotFound();
                }

                return View(employee);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public IActionResult employeeUpdate(int id)
        {
            try
            {
                var employee = _techService.Users.FirstOrDefault(u => u.UserId == id);

                if (employee == null)
                {
                    return NotFound();
                }

                return View(employee);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult employeeUpdate(User user)
        {
            try
            {
                _techService.Users.Update(user);
                _techService.SaveChanges();
                return RedirectToAction("employees");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteEmployee(int userId)
        {
            try
            {
                var employee = _techService.Users.FirstOrDefault(u => u.UserId == userId);
                if (employee != null)
                {
                    _techService.Users.Remove(employee);
                    _techService.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
            }

            return RedirectToAction(nameof(employees));
        }

        public IActionResult customer()
        {
            var user = _techService.Users.FirstOrDefault(u => u.Login == 1 && u.UserType == "admin");

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            try
            {
                var customers = _techService.Users.ToList();
                return View(customers);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public IActionResult customeradd()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult customeradd(User user)
        {
            try
            {
                user.UserType = "Customer";
                user.CreatedAt = DateTime.Now;
                user.Login = 0;
                _techService.Users.Add(user);
                _techService.SaveChanges();
                return RedirectToAction("customer");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult customerDetails(int id)
        {
            try
            {
                var employee = _techService.Users.FirstOrDefault(u => u.UserId == id);

                if (employee == null)
                {
                    return NotFound();
                }

                return View(employee);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public IActionResult customerUpdate(int id)
        {
            try
            {
                var customer = _techService.Users.FirstOrDefault(u => u.UserId == id);

                if (customer == null)
                {
                    return NotFound();
                }

                return View(customer);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult customerUpdate(User user)
        {
            try
            {
                _techService.Users.Update(user);
                _techService.SaveChanges();
                return RedirectToAction("customer");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCustomer(int userId)
        {
            try
            {
                var customer = _techService.Users.FirstOrDefault(u => u.UserId == userId);
                if (customer != null)
                {
                    _techService.Users.Remove(customer);
                    _techService.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
            }
            return RedirectToAction(nameof(customer));
        }

        public IActionResult categories()
        {
            var user = _techService.Users.FirstOrDefault(u => u.Login == 1 && u.UserType == "admin");

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            try
            {
                var categories = _techService.Categories.ToList();
                return View(categories);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public IActionResult categoriesadd()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> categoriesadd(Category category)
        {
            try
            {
                if (category.ImageFile != null && category.ImageFile.Length > 0)
                {
                    string fileName = Path.GetFileName(category.ImageFile.FileName);
                    string filePath = Path.Combine(
                        _env.WebRootPath,
                        "image",
                        fileName
                    );
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await category.ImageFile.CopyToAsync(stream);
                    }
                    category.ImageUrl = fileName;
                }
                _techService.Categories.Add(category);
                await _techService.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
            }
            return RedirectToAction(nameof(categories));
        }

        [HttpGet]
        public IActionResult CategoryUpdate(int id)
        {
            try
            {
                var category = _techService.Categories.FirstOrDefault(c => c.CategoryId == id);

                if (category == null)
                {
                    return NotFound();
                }

                return View(category);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
                return RedirectToAction("Error", "Home");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CategoryUpdate(Category category)
        {
            try
            {
                var existingCategory = await _techService.Categories
                    .FirstOrDefaultAsync(c => c.CategoryId == category.CategoryId);

                if (existingCategory == null)
                {
                    return NotFound();
                }

                if (category.ImageFile == null || category.ImageFile.Length == 0)
                {
                    category.ImageUrl = existingCategory.ImageUrl;
                }
                else
                {
                    string fileName = Path.GetFileName(category.ImageFile.FileName);
                    string filePath = Path.Combine(_env.WebRootPath, "image", fileName);

                    if (!string.IsNullOrEmpty(existingCategory.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_env.WebRootPath, "image", existingCategory.ImageUrl);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await category.ImageFile.CopyToAsync(stream);
                    }

                    category.ImageUrl = fileName;
                }

                existingCategory.Name = category.Name;
                existingCategory.ImageUrl = category.ImageUrl;

                _techService.Categories.Update(existingCategory);
                await _techService.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
            }
            return RedirectToAction(nameof(categories));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult categoryDelete(int id)
        {
            try
            {
                var category = _techService.Categories.FirstOrDefault(c => c.CategoryId == id);
                if (category == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                if (!string.IsNullOrEmpty(category.ImageUrl))
                {
                    var imagePath = Path.Combine(
                        _env.WebRootPath,
                        "image",
                        category.ImageUrl
                    );

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _techService.Categories.Remove(category);
                _techService.SaveChanges();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
            }
            return RedirectToAction(nameof(categories));
        }


        public IActionResult Orders()
        {
            var user = _techService.Users.FirstOrDefault(u => u.Login == 1 && u.UserType == "admin");

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            try
            {
                var viewModel = new OrdersViewModel
                {
                    Users = _techService.Users.ToList(),
                    Orders = _techService.Orders.ToList()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpGet]
        public IActionResult OrderDetails(int id)
        {
            try
            {
                var order = _techService.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                    .FirstOrDefault(o => o.OrderId == id);

                if (order == null)
                {
                    return NotFound();
                }

                var viewModel = new OrderDetailsViewModel
                {
                    Order = order,
                    OrderDetails = order.OrderDetails.ToList()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult products()
        {
            var user = _techService.Users.FirstOrDefault(u => u.Login == 1 && u.UserType == "admin");

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            try
            {
                var products = _techService.Products
                    .Include(p => p.Category)
                    .ToList();

                return View(products);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public IActionResult productadd()
        {
            try
            {
                ViewBag.Categories = _techService.Categories.ToList();
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> productadd(Product product)
        {
            try
            {
                if (product.MainImageFile != null && product.MainImageFile.Length > 0)
                {
                    string originalFileName = Path.GetFileName(product.MainImageFile.FileName);
                    string filePath = Path.Combine(_env.WebRootPath, "image", originalFileName);

                    int counter = 1;
                    while (System.IO.File.Exists(filePath))
                    {
                        string newFileName = $"{Path.GetFileNameWithoutExtension(originalFileName)}_{counter}{Path.GetExtension(originalFileName)}";
                        filePath = Path.Combine(_env.WebRootPath, "image", newFileName);
                        counter++;
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await product.MainImageFile.CopyToAsync(stream);
                    }

                    product.MainImage = originalFileName;
                }
                if (product.Image2File != null && product.Image2File.Length > 0)
                {
                    string originalFileName = Path.GetFileName(product.Image2File.FileName);
                    string filePath = Path.Combine(_env.WebRootPath, "image", originalFileName);

                    int counter = 1;
                    while (System.IO.File.Exists(filePath))
                    {
                        string newFileName = $"{Path.GetFileNameWithoutExtension(originalFileName)}_{counter}{Path.GetExtension(originalFileName)}";
                        filePath = Path.Combine(_env.WebRootPath, "image", newFileName);
                        counter++;
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await product.Image2File.CopyToAsync(stream);
                    }

                    product.Extra1 = originalFileName;
                }
                if (product.Image3File != null && product.Image3File.Length > 0)
                {
                    string originalFileName = Path.GetFileName(product.Image3File.FileName);
                    string filePath = Path.Combine(_env.WebRootPath, "image", originalFileName);

                    int counter = 1;
                    while (System.IO.File.Exists(filePath))
                    {
                        string newFileName = $"{Path.GetFileNameWithoutExtension(originalFileName)}_{counter}{Path.GetExtension(originalFileName)}";
                        filePath = Path.Combine(_env.WebRootPath, "image", newFileName);
                        counter++;
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await product.Image3File.CopyToAsync(stream);
                    }

                    product.Extra2 = originalFileName;
                }
                product.CreatedAt = DateTime.Now;
                _techService.Products.Add(product);
                await _techService.SaveChangesAsync();

                return RedirectToAction(nameof(products));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public IActionResult ProductDetails(int id)
        {
            try
            {
                var product = _techService.Products
                    .Include(p => p.Category)
                    .FirstOrDefault(p => p.ProductId == id);

                if (product == null)
                {
                    return NotFound();
                }

                return View(product);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public IActionResult ProductUpdate(int id)
        {
            try
            {
                var product = _techService.Products
                    .Include(p => p.Category)
                    .FirstOrDefault(p => p.ProductId == id);

                if (product == null)
                {
                    return NotFound();
                }

                ViewBag.Categories = _techService.Categories.ToList();
                return View(product);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductUpdate(Product product)
        {
            try
            {
                var existingproduct = await _techService.Products
        .FirstOrDefaultAsync(c => c.ProductId == product.ProductId);

                if (existingproduct == null)
                {
                    return NotFound();
                }

                if (product.MainImageFile == null || product.MainImageFile.Length == 0)
                {
                    product.MainImage = existingproduct.MainImage;
                }
                else
                {
                    string fileName = Path.GetFileName(product.MainImageFile.FileName);
                    string filePath = Path.Combine(_env.WebRootPath, "image", fileName);

                    if (!string.IsNullOrEmpty(existingproduct.MainImage))
                    {
                        var oldImagePath = Path.Combine(_env.WebRootPath, "image", existingproduct.MainImage);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await product.MainImageFile.CopyToAsync(stream);
                    }

                    product.MainImage = fileName;
                }

                if (product.Image2File == null || product.Image2File.Length == 0)
                {
                    product.Extra1 = existingproduct.Extra1;
                }
                else
                {
                    string fileName = Path.GetFileName(product.Image2File.FileName);
                    string filePath = Path.Combine(_env.WebRootPath, "image", fileName);

                    if (!string.IsNullOrEmpty(existingproduct.Extra1))
                    {
                        var oldImagePath = Path.Combine(_env.WebRootPath, "image", existingproduct.Extra1);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await product.Image2File.CopyToAsync(stream);
                    }

                    product.Extra1 = fileName;
                }

                if (product.Image3File == null || product.Image3File.Length == 0)
                {
                    product.Extra2 = existingproduct.Extra2;
                }
                else
                {
                    string fileName = Path.GetFileName(product.Image3File.FileName);
                    string filePath = Path.Combine(_env.WebRootPath, "image", fileName);

                    if (!string.IsNullOrEmpty(existingproduct.Extra2))
                    {
                        var oldImagePath = Path.Combine(_env.WebRootPath, "image", existingproduct.Extra2);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await product.Image3File.CopyToAsync(stream);
                    }

                    product.Extra2 = fileName;
                }

                existingproduct.Name = product.Name;
                existingproduct.Description = product.Description;
                existingproduct.Price = product.Price;
                existingproduct.Stock = product.Stock;
                existingproduct.DiscountRate = product.DiscountRate;
                existingproduct.CategoryId = product.CategoryId;
                existingproduct.MainImage = product.MainImage;
                existingproduct.Extra1 = product.Extra1;
                existingproduct.Extra2 = product.Extra2;


                _techService.Products.Update(existingproduct);
                await _techService.SaveChangesAsync();

                return RedirectToAction(nameof(products));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProductDelete(int id)
        {
            try
            {
                var product = _techService.Products.FirstOrDefault(c => c.ProductId == id);

                if (product == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                if (!string.IsNullOrEmpty(product.MainImage))
                {
                    var imagePath = Path.Combine(
                        _env.WebRootPath,
                        "image",
                        product.MainImage
                    );

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }


                if (!string.IsNullOrEmpty(product.Extra1))
                {
                    var imagePath = Path.Combine(
                        _env.WebRootPath,
                        "image",
                        product.Extra1
                    );

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }


                if (!string.IsNullOrEmpty(product.Extra2))
                {
                    var imagePath = Path.Combine(
                        _env.WebRootPath,
                        "image",
                        product.Extra2
                    );

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                _techService.Products.Remove(product);
                _techService.SaveChanges();

                return RedirectToAction(nameof(products));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

    }
}
