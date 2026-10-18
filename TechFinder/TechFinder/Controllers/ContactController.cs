using Microsoft.AspNetCore.Mvc;
using TechFinder.Models;
using TechFinder.ViewModels;

namespace TechFinder.Controllers
{
    public class ContactController : Controller
  
       
    {
        
        public IActionResult Contactus()
        {
            return View();
        }

      
        [HttpPost]
        public IActionResult Contactus(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
               
              
                ViewBag.Message = "   Thank you for contacting us! We will reply to you soon.";
                return View();
            }

         
            return View(model);
        }
    }
}



