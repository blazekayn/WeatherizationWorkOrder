using WeatherizationWorkOrder.Data;
using WeatherizationWorkOrder.Data.Interfaces;
using WeatherizationWorkOrder.Models;

namespace WeatherizationWorkOrder.Business
{
    public interface IWorkOrderProvider
    {
        Task<List<Labor>> AddLaborToWorkOrder(AddLaborRequest request);
        Task<AddMaterialResponse> AddMaterialToWorkOrder(AddMaterialRequest request);
        Task<int> CreateWorkOrder(WorkOrder workOrder);
        Task<List<Labor>> DeleteLabor(int id);
        Task<List<Material>> DeleteMaterial(int materialId);
        Task<List<Material>> DeleteMaterials(List<int> materialIds);
        Task DeleteWorkOrder(int id);
        Task<List<WorkOrder>> GetAllWorkOrders(bool onlyIncomplete);
        Task<List<WorkOrder>> GetAllWorkOrders(DateTime from, DateTime to);
        Task<WorkOrder> GetWorkOrderByID(int id);
        Task UpdateWorkOrder(WorkOrder workOrder);
    }

    public class WorkOrderProvider(IWorkOrderDataProvider workOrderDataProvider, IInventoryDataProvider inventoryDataProvider) : IWorkOrderProvider
    {
        private readonly IWorkOrderDataProvider _workOrderDataProvider = workOrderDataProvider;
        private readonly IInventoryDataProvider _inventoryDataProvider = inventoryDataProvider;

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
            var material = await _workOrderDataProvider.ReadMaterials(id);
            var labor = await _workOrderDataProvider.ReadLabors(id);
            foreach (Material mat in material)
            {
                await DeleteMaterial(mat.Id);
            }
            foreach (Labor lab in labor)
            {
                await DeleteLabor(lab.Id);
            }
            await _workOrderDataProvider.Delete(id);
        }

        public async Task<List<WorkOrder>> GetAllWorkOrders(bool onlyIncomplete)
        {
            var wos = await _workOrderDataProvider.Read(onlyIncomplete);
            List<WorkOrder> returns = [];
            foreach (var workOrder in (List<WorkOrder>)wos)
            {
                returns.Add(await SetSubProperties(workOrder));
            }
            return returns;
        }
        public async Task<List<WorkOrder>> GetAllWorkOrders(DateTime from, DateTime to)
        {
            if (from == DateTime.MinValue)
            {
                from = DateTime.Parse("01/01/1970");
            }
            if (to == DateTime.MinValue)
            {
                to = DateTime.Now;
            }
            var wos = await _workOrderDataProvider.Read(from, to);
            List<WorkOrder> returns = [];
            foreach (var workOrder in (List<WorkOrder>)wos)
            {
                returns.Add(await SetSubProperties(workOrder));
            }
            return returns;
        }

        private async Task<WorkOrder> SetSubProperties(WorkOrder workOrder)
        {
            var materials = await _workOrderDataProvider.ReadMaterials(workOrder.Id);
            workOrder.Materials = materials;
            var labor = await _workOrderDataProvider.ReadLabors(workOrder.Id);
            workOrder.Labors = labor;
            workOrder.MaterialCost = workOrder.Materials.Sum(item => item.Total);
            workOrder.LaborCost = workOrder.Labors.Sum(item => item.Total);
            workOrder.TotalCost = workOrder.MaterialCost + workOrder.LaborCost;
            return workOrder;
        }

        public async Task<WorkOrder> GetWorkOrderByID(int id)
        {
            var wo = await _workOrderDataProvider.Read(id);
            var materials = await _workOrderDataProvider.ReadMaterials(id);
            wo.Materials = materials;
            var labor = await _workOrderDataProvider.ReadLabors(id);
            wo.Labors = labor;
            return wo;
        }

        public async Task UpdateWorkOrder(WorkOrder workOrder)
        {
            workOrder.LastModified = DateTime.Now;
            await _workOrderDataProvider.Update(workOrder);
        }

        public async Task<List<Labor>> AddLaborToWorkOrder(AddLaborRequest request)
        {
            await _workOrderDataProvider.AddLabor(request.WoId, request.Resource, request.Cost, request.Hours);
            return await _workOrderDataProvider.ReadLabors(request.WoId);
        }

        public async Task<List<Material>> DeleteMaterials(List<int> materialIds)
        {
            var materials = new List<Material>();
            foreach (int i in materialIds)
            {
                materials = await DeleteMaterial(i);
            }
            //Return material list
            return materials;
        }

        public async Task<List<Material>> DeleteMaterial(int materialId)
        {
            //Get the DB Item
            var material = await _workOrderDataProvider.ReadMaterial(materialId);
            if(material == null || material.InventoryItemId == null || material.WorkOrderId == null)
            {
                throw new ArgumentException("Bad material id.");
            }
            var item = await _inventoryDataProvider.Read((int)material.InventoryItemId);
            //Add to Inventory
            item.Remaining += material.AmountUsed;
            await _inventoryDataProvider.Update(item);
            //Remove Item
            await _workOrderDataProvider.DeleteMaterial(materialId);
            //Return material list
            return await _workOrderDataProvider.ReadMaterials((int)material.WorkOrderId);
        }

        public async Task<List<Labor>> DeleteLabor(int id)
        {
            var labor = await _workOrderDataProvider.ReadLabor(id);
            if(labor == null || labor.WorkOrderId == null)
            {
                throw new ArgumentException("Bad labor id.");
            }
            await _workOrderDataProvider.DeleteLabor(id);
            return await _workOrderDataProvider.ReadLabors((int)labor.WorkOrderId);
        }

        public async Task<AddMaterialResponse> AddMaterialToWorkOrder(AddMaterialRequest request)
        {
            var remainingUnits = await _inventoryDataProvider.ReadByDesc(request.Description, request.Units);
            if (remainingUnits == null || remainingUnits.Count == 0)
            {
                return new AddMaterialResponse
                {
                    Success = false,
                    Message = "None of this material in inventory."
                };
            }
            var total = remainingUnits.Sum(item => item.Remaining);
            if (total < request.Used)
            {
                return new AddMaterialResponse
                {
                    Success = false,
                    Message = $"Only {total} units of {request.Used} material in invetory."
                };
            }
            List<UsedItem> itemsUsed = [];
            decimal runningTotal = 0;
            foreach (var item in remainingUnits)
            {
                if (item.Remaining >= (request.Used - runningTotal))
                {
                    item.Remaining -= (request.Used - runningTotal);
                    itemsUsed.Add(new UsedItem
                    {
                        InventoryItem = item,
                        amount = (request.Used - runningTotal)
                    });
                    runningTotal = request.Used;
                    break;
                }
                else
                {
                    runningTotal += item.Remaining;
                    itemsUsed.Add(new UsedItem
                    {
                        InventoryItem = item,
                        amount = item.Remaining
                    });
                    item.Remaining = 0;
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
                return new AddMaterialResponse
                {
                    Success = true,
                    Materials = await _workOrderDataProvider.ReadMaterials(request.WoId)
                };
            }
            return new AddMaterialResponse
            {
                Success = false,
                Message = "Something went wrong adding material to work order"
            };
        }

    }
}