namespace ClaimBack.API.Models
{
    public class Purchase
    {
        public int  Id { get; set; }
        public string? UserId { get; set; }

        public ApplicationUser? User { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Retailer { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; } 
        public decimal PurchasePrice { get; set; }
        public DateTime? ReturnDeadline { get; set; }
        public ICollection<Claim> Claims { get; set; } = new List<Claim>();
        public ICollection<PurchaseDocument> Documents { get; set; } = new List<PurchaseDocument>();
        public int WarrantyDurationMonths { get; set; }
        public bool IsArchived { get; set; } = false;
        
        public DateTime WarrantyEndDate => PurchaseDate.AddMonths(WarrantyDurationMonths);

    }
}
