namespace WeatherizationWorkOrder.Models
{
    public class Material
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal CostPer { get; set; }
        public decimal AmountUsed { get; set; }
        public decimal Total { get; set; }
        public string? Units { get; set; }
        public int? InventoryItemId { get; set; }
        public int? WorkOrderId { get; set; }
    }
}
