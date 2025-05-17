using WeatherizationWorkOrder.Models;

namespace WeatherizationWorkOrder.Data.Interfaces
{
    public interface IInventoryDataProvider
    {
        Task Create(InventoryItem item);
        Task Delete(int id);
        Task<List<InventoryItem>> Read(bool showOutOfStock, bool unique, bool printed);
        Task<InventoryItem> Read(int id);
        Task<List<InventoryItem>> ReadByDesc(string desc, string units);
        Task<List<string>> ReadUnits();
        Task Update(InventoryItem item);
    }
}