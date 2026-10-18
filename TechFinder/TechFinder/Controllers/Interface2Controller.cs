using Microsoft.AspNetCore.Mvc;
using TechFinder.Models;
using TechFinder.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class Interface2Controller : Controller
{
    private readonly TechFinder_storeContext _context;

    public Interface2Controller(TechFinder_storeContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var latestProducts = _context.Products
            .OrderByDescending(p => p.CreatedAt)
            .Take(8)
            .ToList();

        var oldestProducts = _context.Products
            .OrderBy(p => p.DiscountRate).ToList();

        var categories = _context.Categories.ToList();
        List<int> userFavorites = new List<int>();
        var user = _context.Users.FirstOrDefault(u => u.Login == 1); // تأكد من أن الشرط صحيح لتحديد المستخدم
        if (user != null)
        {
            userFavorites = _context.Favorites
                .Where(f => f.UserId == user.UserId)
                .Select(f => f.ProductId)
                .ToList();
        }

        var latestProductReviews = latestProducts.Select(product =>
        {
            var reviews = _context.Reviews
                .Where(r => r.ProductId == product.ProductId)
                .ToList();

            double rating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;

            return new InterfacedealViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                MainImage = product.MainImage,
                Price = product.Price,
                DiscountRate = product.DiscountRate,
                Rating = rating,
                CreatedAt = product.CreatedAt,
                Reviews = reviews,
            };
        }).ToList();

        var oldestProductReviews = oldestProducts.Select(product =>
        {
            var reviews = _context.Reviews
                .Where(r => r.ProductId == product.ProductId)
                .ToList();


            double rating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;

            return new InterfacedealViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                MainImage = product.MainImage,
                Price = product.Price,
                DiscountRate = product.DiscountRate,
                Rating = rating,
                CreatedAt = product.CreatedAt,
                Reviews = reviews
            };
        }).ToList();

        var viewModel = new InterfaceViewModel
        {
            Categories = categories,
            LatestProducts = latestProductReviews,
            OldestProducts = oldestProductReviews,
            UserFavorites = userFavorites,
        };

        return View(viewModel);
    }

  
    [HttpGet]

    public IActionResult Results(string searchQuery)
    {
        if (string.IsNullOrWhiteSpace(searchQuery))
        {
            ViewBag.Message = "Please enter a keyword to search.";
            return View("NoResults");
        }

        string loweredQuery = searchQuery.ToLower();

      
        bool isNumeric = decimal.TryParse(searchQuery, out decimal priceValue);

        var productsQuery = _context.Products.AsQueryable();

        if (isNumeric)
        {
          
            productsQuery = productsQuery.Where(p => p.Price <= priceValue);
        }
        else
        {
            productsQuery = productsQuery.Where(p =>
                EF.Functions.Like(p.Name.ToLower(), $"%{loweredQuery}%") ||
                EF.Functions.Like(p.Description.ToLower(), $"%{loweredQuery}%"));
        }

        var products = productsQuery
            .Select(p => new
            {
                Product = p,
                Priority = EF.Functions.Like(p.Name.ToLower(), $"%{loweredQuery}%") ? 1 : 2
            })
            .OrderBy(p => p.Priority)
            .Select(p => p.Product)
            .ToList();

        if (!products.Any())
        {
            ViewBag.Message = $"No results found for \"{searchQuery}\".";
            return View("NoResults");
        }

        return View(products);
    }




    [HttpPost]
    public IActionResult AddToFavorite(int productId)
    {
        var user = _context.Users.FirstOrDefault(u => u.Login == 1);
        if (user == null)
        {
            return RedirectToAction("Login","Account");
        }

       
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

        }
        else
        {
            TempData["ErrorMessage"] = "The product is already in your favorites.";
        }

        return RedirectToAction("Index");
    }



}
















