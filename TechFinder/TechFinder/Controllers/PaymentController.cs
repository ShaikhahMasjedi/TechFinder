using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TechFinder.Models;
using TechFinder.ViewModels;
using Microsoft.EntityFrameworkCore;

public class PaymentController : Controller
{
    private readonly TechFinder_storeContext _context;

    public PaymentController(TechFinder_storeContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Payment()
    {
        var model = new PaymentViewModel();
        return View(model);
    }

    [HttpPost]
    public IActionResult Payment(PaymentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = _context.Users.FirstOrDefault(u => u.Login == 1);

        if (user == null)
        {
            ViewBag.Message = "You must be logged in to complete the payment.";
            return RedirectToAction("login", "Account");
        }

        int userId = user.UserId;

        var cartItems = _context.ShoppingCarts
            .Include(c => c.Product)
            .Where(c => c.UserId == userId)
            .ToList();

        if (!cartItems.Any())
        {
            TempData["Warning"] = "Your cart is empty. Please add items before proceeding to payment.";
            return RedirectToAction("Index", "Interface2");
        }

        foreach (var item in cartItems)
        {
            if (item.Product.Stock < item.Quantity)
            {
                TempData["Warning"] = $"The product '{item.Product.Name}' does not have enough stock. Available: {item.Product.Stock}, Requested: {item.Quantity}.";
                return RedirectToAction("ViewCart", "ShoppingCart");
            }
        }

     
        var groupedCartItems = cartItems
            .GroupBy(c => c.ProductId)
            .Select(g => new
            {
                Product = g.First().Product,
                TotalQuantity = g.Sum(c => c.Quantity)
            })
            .ToList();

        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
               
                var order = new Order
                {
                    UserId = userId,
                    ShippingAddress = model.ShippingAddress,
                    Phone = model.PhoneNumber,
                    CreatedAt = DateTime.Now,
                    Status = "Pending",
                    TotalAmount = 0
                };
                _context.Orders.Add(order);
                _context.SaveChanges();

                decimal totalAmount = 0;

               
                foreach (var item in groupedCartItems)
                {
                    var detail = new OrderDetail
                    {
                        OrderId = order.OrderId,
                        ProductId = item.Product.ProductId,
                        Quantity = item.TotalQuantity,
                        Price = item.Product.Price
                    };

                    totalAmount += item.TotalQuantity * item.Product.Price;
                    _context.OrderDetails.Add(detail);

                    var product = item.Product;
                    if (product != null)
                    {
                        product.Stock -= item.TotalQuantity;
                        if (product.Stock < 0)
                            product.Stock = 0;
                    }
                }

              
                order.TotalAmount = totalAmount;
                _context.SaveChanges();

                
                var payment = new Payment
                {
                    OrderId = order.OrderId,
                    PaymentMethod = model.PaymentMethod
                };
                _context.Payments.Add(payment);
                _context.SaveChanges();

             
                _context.ShoppingCarts.RemoveRange(cartItems);
                _context.SaveChanges();

                transaction.Commit(); 
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                TempData["ErrorMessage"] = "An error occurred during payment. Please try again.";
                return RedirectToAction("payment");
            }
        }

      
        return RedirectToAction("Success");
    }


    public IActionResult Success()
    {
        return View();
    }
}





