namespace IKT_BACKEND.Dtos
{
    public class ProductResumeDto
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
