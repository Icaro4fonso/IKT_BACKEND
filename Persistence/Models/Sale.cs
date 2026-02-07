namespace IKT_BACKEND.Persistence.Models
{
    public class Sale
    {
        public DateTime DateTime { get; set; }
        public PaymentType PaymentType { get; set; }
        public decimal Price { get; set; }
        public long ProductId { get; set; }
        public Product Product { get; set; }
    }
}
