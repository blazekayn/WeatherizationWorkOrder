using WeatherizationWorkOrder.Models.Parents;

namespace WeatherizationWorkOrder.Models
{
    public class InventoryItem : Auditable
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public string Units { get; set; }
        public decimal StartingAmount { get; set; }
        public decimal Remaining { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}