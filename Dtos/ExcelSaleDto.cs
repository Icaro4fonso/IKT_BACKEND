using IKT_BACKEND.Persistence.Models;

namespace IKT_BACKEND.Dtos
{
    public class ExcelSaleDto
    {
        public DateTime DateTime { get; set; }
        public PaymentType PaymentType { get; set; }
        public decimal Price { get; set; }
        public string? ProductName { get; set; }
        public string? CardNumber { get; set; }
        public int Month { get; set; }

    }
}
