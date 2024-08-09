namespace WeatherizationWorkOrder.Models
{
    public class Labor
    {
        public int Id { get; set; }
        public int? WorkOrderId { get; set; }
        public string Resource { get; set; }
        public decimal Cost { get; set; }
        public decimal Hours { get; set; }
        public decimal Total { get; set; }
    }
}
