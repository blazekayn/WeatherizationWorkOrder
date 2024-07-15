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

        public async Task<int> CreateWorkOrder(WorkOrder workOrder)
        {
            if (workOrder.PreparedDate == DateTime.MinValue)
            {
                workOrder.PreparedDate = DateTime.Now;
            }
            workOrder.LastModified = DateTime.Now;
            workOrder.LastModifiedBy = workOrder.PreparedBy;
            return await _workOrderDataProvider.Create(workOrder);
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
            var wo = await _workOrderDataProvider.Read(id);
            var materials = await _workOrderDataProvider.ReadMaterials(id);
            wo.Materials = materials;
            return wo;
        }

        public async Task UpdateWorkOrder(WorkOrder workOrder)
        {
            workOrder.LastModified = DateTime.Now;
            await _workOrderDataProvider.Update(workOrder);
        }

        public async Task AddLaborToWorkOrder(AddLaborRequest request)
        {
            await _workOrderDataProvider.AddLabor(request.WoId, request.Resource, request.Cost, request.Hours);
        }

        public async Task AddMaterialToWorkOrder(AddMaterialRequest request)
        {
            var remainingUnits = await _inventoryDataProvider.ReadByDesc(request.Description, request.Units);
            if(remainingUnits == null || !remainingUnits.Any())
            {
                return;
            }
            var total = remainingUnits.Sum(item => item.Remaining);
            if(total < request.Used)
            {
                return;
            }
            List<UsedItem> itemsUsed = new List<UsedItem>();
            decimal runningTotal = 0;
            foreach (var item in remainingUnits)
            {
                if (item.Remaining >= request.Used)
                {
                    item.Remaining -= request.Used;
                    itemsUsed.Add(new UsedItem
                    {
                        InventoryItem = item,
                        amount = request.Used
                    });
                    runningTotal = request.Used;
                    break;
                }
                else
                {
                    runningTotal += item.Remaining;
                    item.Remaining = 0;
                    itemsUsed.Add(new UsedItem
                    {
                        InventoryItem = item,
                        amount = item.Remaining
                    });
                }
            }
            if (runningTotal == request.Used)
            {
                //continue successfully
                foreach (var item in itemsUsed)
                {
                    //update the inventory table
                    await _inventoryDataProvider.Update(item.InventoryItem);

                    //add it to the work order
                    await _workOrderDataProvider.AddMaterial(item.InventoryItem.Id, request.WoId, item.amount);

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