using System;
using WeatherizationWorkOrder.Models.Parents;

namespace WeatherizationWorkOrder.Models
{
	public class WorkOrder
	{
		public int Id { get; set; }
		public string Consumer { get; set; }
		public string PreparedBy { get; set; }
		public string Description { get; set; }
		public DateTime PreparedDate { get; set; }
		public string? LastModifiedBy { get; set; }
		public DateTime? LastModified { get; set; }

		public List<Material> Materials { get; set; }
	}
}