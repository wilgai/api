using System;
using System.ComponentModel.DataAnnotations;

namespace api.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public decimal TotalPaid { get; set; }
        public string orderID { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        public DateTime Date { get; set; }
        public string FechaLocal => Date.ToLocalTime().ToString("dd-MM-yyy HH:mm tt");
        public string Reference { get; set; }
        public decimal BillPaidWith { get; set; }
        public decimal Change { get; set; }
        public decimal Total => TotalPaid += TotalPaid;
    }
}
