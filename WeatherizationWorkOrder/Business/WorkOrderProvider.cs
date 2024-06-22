using WeatherizationWorkOrder.Data;
using WeatherizationWorkOrder.Models;

namespace WeatherizationWorkOrder.Business
{
    public class WorkOrderProvider
    {
        private readonly WorkOrderDataProvider _workOrderDataProvider;
        public WorkOrderProvider(WorkOrderDataProvider workOrderDataProvider)
        {
            _workOrderDataProvider = workOrderDataProvider;
        }

        public async Task CreateWorkOrder(WorkOrder workOrder)
        {
            if (workOrder.PreparedDate == DateTime.MinValue)
            {
                workOrder.PreparedDate = DateTime.Now;
            }
            workOrder.LastModified = DateTime.Now;
            await _workOrderDataProvider.Create(workOrder);
        }

        public async Task DeleteWorkOrder(int id)
        {
            await _workOrderDataProvider.Delete(id);
        }

        public async Task<List<WorkOrder>> GetAllWorkOrders()
        {
            return await _workOrderDataProvider.Read();
        }

        public async Task<WorkOrder> GetWorkOrderByID(int id)
        {
            return await _workOrderDataProvider.Read(id);
        }

        public async Task UpdateWorkOrder(WorkOrder workOrder)
        {
            await _workOrderDataProvider.Update(workOrder);
        }

        public async Task<List<string>> GetUnits()
        {
            return await _workOrderDataProvider.ReadUnits();
        }
    }
}