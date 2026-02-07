namespace IKT_BACKEND.Persistence.Models
{
    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
