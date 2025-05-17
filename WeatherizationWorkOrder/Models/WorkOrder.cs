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
		public DateTime? WorkDate { get; set; }
		public string LastModifiedBy { get; set; }
		public DateTime? LastModified { get; set; }

		public List<Material> Materials { get; set; }
		public List<Labor> Labors { get; set; }

		public decimal LaborCost { get; set; }
		public decimal MaterialCost { get; set; }
		public decimal TotalCost { get; set; }
		public bool IsComplete { get; set; }
	}
}