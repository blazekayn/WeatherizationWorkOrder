namespace WeatherizationWorkOrder.Models
{
    public class AddLaborRequest
    {
        public int WoId { get; set; }
        public string Resource { get; set; }
        public decimal Cost { get; set; }
        public decimal Hours { get; set; }
    }
}
