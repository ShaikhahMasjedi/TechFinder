using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechFinder.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TechFinder.Controllers
{
    public class AccountController : Controller
    {
        private readonly TechFinder_storeContext _context;
        private static string? _verificationCode;
        private static string? _userEmail;

        public AccountController(TechFinder_storeContext context)
        {
            _context = context;
        }

       
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View();
            }

          
            var allUsers = await _context.Users.ToListAsync();
            foreach (var u in allUsers)
            {
                u.Login = 0; 
            }
            await _context.SaveChangesAsync(); 
            user.Login = 1;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.UserType)
    };


            if (user.UserType == "admin")
            {
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                return RedirectToAction("Index", "Interface2");
            }
        }


        // Register
        public IActionResult Register() => View();
        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                ModelState.AddModelError("Email", "Email already registered");
                return View(user);
            }

            if (_context.Users.Any(u => u.FullName == user.FullName))
            {
                ModelState.AddModelError("Username", "Username already taken");
                return View(user);
            }

            user.Password = user.Password;
            user.UserType = "Customer";
            user.Login = 0;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }


        // Forgot Password
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            if (!_context.Users.Any(u => u.Email == email))
            {
                ModelState.AddModelError("", "Email not found");
                return View(); 
            }

            _verificationCode = new Random().Next(100000, 999999).ToString();
            _userEmail = email;

            Console.WriteLine($"Verification Code: {_verificationCode}");

            return RedirectToAction("VerifyCode");
        }


        // Verify Code
        public IActionResult VerifyCode() => View();

        [HttpPost]
        public IActionResult VerifyCode(string code)
        {
           if (code != _verificationCode)
            {
                ModelState.AddModelError("", "Invalid code");
                return View();
            }
            return RedirectToAction("ResetPassword");
        }

        // Reset Password
        public IActionResult ResetPassword() => View();
        [HttpPost]
        public async Task<IActionResult> ResetPassword(string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword)
            {
                ModelState.AddModelError("", "Passwords don't match");
                return View();
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == _userEmail);

            if (user == null)
            {
                ModelState.AddModelError("", "User not found");
                return View();
            }

            user.Password = newPassword;
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }
    }
}
