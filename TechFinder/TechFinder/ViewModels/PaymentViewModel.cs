using System.ComponentModel.DataAnnotations;

namespace TechFinder.ViewModels
{
    public class PaymentViewModel
    {
        public string PhoneNumber { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }

        // Optional fields for credit card
        public string AccountHolder { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }

}
