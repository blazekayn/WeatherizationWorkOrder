namespace WeatherizationWorkOrder.Models
{
    public class AddMaterialResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<Material> Materials { get; set; }
    }
}
