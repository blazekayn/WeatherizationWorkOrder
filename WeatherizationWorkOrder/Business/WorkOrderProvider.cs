using WeatherizationWorkOrder.Data;
using WeatherizationWorkOrder.Models;

namespace WeatherizationWorkOrder.Business
{
    public class WorkOrderProvider
    {
        private readonly WorkOrderDataProvider _workOrderDataProvider;
        private readonly InventoryDataProvider _inventoryDataProvider;
        public WorkOrderProvider(WorkOrderDataProvider workOrderDataProvider, InventoryDataProvider inventoryDataProvider)
        {
            _workOrderDataProvider = workOrderDataProvider;
            _inventoryDataProvider = inventoryDataProvider;
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

        public async Task AddMaterialToWorkOrder(AddMaterialRequest request)
        {
            var remainingUnits = await _inventoryDataProvider.ReadByDesc(request.Description);
            if(remainingUnits == null || !remainingUnits.Any())
            {
                return;
            }
            var total = remainingUnits.Sum(item => item.Remaining);
            if(total < request.Used)
            {
                return;
            }
            List<InventoryItem> itemsUsed = new List<InventoryItem>();
            decimal runningTotal = 0;
            foreach (var item in remainingUnits)
            {
                if (item.Remaining >= request.Used)
                {
                    item.Remaining -= request.Used;
                    itemsUsed.Add(item);
                    break;
                }
                else
                {
                    runningTotal += item.Remaining;
                    item.Remaining = 0;
                    itemsUsed.Add(item);
                }
            }
            if (runningTotal == request.Used)
            {
                //continue successfully
                foreach (var item in itemsUsed)
                {
                    //update the inventory table
                    
                    //add it to the work order

                    //add it to the return object to be displayed on the front end?
                }
            }
            else
            {
                //Show the user that they don't have enough

            }
        }

    }
}