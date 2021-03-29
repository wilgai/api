using System.ComponentModel.DataAnnotations;

namespace api.Request
{
    public class CheckoutRequest
    {
        [Required]
        public string orderId { get; set; }
        [Required]
        public string estado { get; set; }
        public decimal TotalPaid { get; set; }
        public string Reference { get; set; }
        public decimal BillPaidWith { get; set; }
        public decimal Change { get; set; }

    }
}
