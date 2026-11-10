namespace ClaimBack.API.Models
{
    public class Purchase
    {
        public int  Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Retailer { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; } 
        public decimal PurchasePrice { get; set; }
        public int WarrantyDurationMonths { get; set; }

        public bool IsArchived { get; set; } = false;

        public DateTime WarrantyEndDate => PurchaseDate.AddMonths(WarrantyDurationMonths);

    }
}
