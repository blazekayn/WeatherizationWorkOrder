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

        //        public async Task DeleteInventoryItem(int id)
        //        {
        //            await _inventoryDataProvider.Delete(id);
        //        }

        //        public async Task<List<InventoryItem>> GetAllInventoryItems(bool showOutOfStock)
        //        {
        //            return await _inventoryDataProvider.Read(showOutOfStock);
        //        }

        //        public async Task<InventoryItem> GetInventoryItemById(int id)
        //        {
        //            return await _inventoryDataProvider.Read(id);
        //        }

        //        public async Task UpdateInventoryItem(InventoryItem item)
        //        {
        //            await _inventoryDataProvider.Update(item);
        //        }

        //        public async Task<List<string>> GetUnits()
        //        {
        //            return await _inventoryDataProvider.ReadUnits();
    // }
}
}