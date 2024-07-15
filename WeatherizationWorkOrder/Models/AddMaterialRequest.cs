﻿namespace WeatherizationWorkOrder.Models
{
    public class AddMaterialRequest
    {
        public string Description { get; set; } = "";
        public string Units { get; set; }
        public int WoId { get; set; }
        public decimal Used { get; set; }
    }
}
