using WeatherizationWorkOrder.Daata.Models.Parents;

namespace WeatherizationWorkOrder.Data.Models
{
    public class InventoryItem : Auditable
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public string Units { get; set; }
        public double StartingAmount { get; set; }
        public double Remaining { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}