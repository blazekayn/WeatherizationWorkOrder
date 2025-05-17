using WeatherizationWorkOrder.Models;

namespace WeatherizationWorkOrder.Data.Interfaces
{
    public interface IWorkOrderDataProvider
    {
        Task AddLabor(int woId, string resource, decimal cost, decimal hours);
        Task AddMaterial(int id, int woId, decimal amount);
        Task<int> Create(WorkOrder item);
        Task Delete(int id);
        Task DeleteLabor(int materialId);
        Task DeleteMaterial(int materialId);
        Task<List<WorkOrder>> Read(bool onlyIncomplete);
        Task<List<WorkOrder>> Read(DateTime from, DateTime to);
        Task<WorkOrder> Read(int id);
        Task<Labor> ReadLabor(int id);
        Task<List<Labor>> ReadLabors(int id);
        Task<Material> ReadMaterial(int id);
        Task<List<Material>> ReadMaterials(int id);
        Task Update(WorkOrder workOrder);
    }
}