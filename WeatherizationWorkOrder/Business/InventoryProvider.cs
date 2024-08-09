using WeatherizationWorkOrder.Data;
using WeatherizationWorkOrder.Models;

namespace WeatherizationWorkOrder.Business
{
    public class InventoryProvider : IInventoryProvider
    {
        private readonly InventoryDataProvider _inventoryDataProvider;
        public InventoryProvider(InventoryDataProvider inventoryDataProvider) { 
            _inventoryDataProvider = inventoryDataProvider;
        }

        public async Task CreateInventoryItem(InventoryItem item)
        {
            if(item.PurchaseDate == DateTime.MinValue)
            {
                item.PurchaseDate = DateTime.Now;
            }
            item.Created = DateTime.Now;
            item.LastModified = DateTime.Now;
            await _inventoryDataProvider.Create(item);
        }

        public async Task DeleteInventoryItem(int id)
        {
            await _inventoryDataProvider.Delete(id);
        }

        public async Task<List<InventoryItem>> GetAllInventoryItems(bool showOutOfStock, bool unique, bool printed)
        {
            return await _inventoryDataProvider.Read(showOutOfStock, unique, printed);
        }

        public async Task<InventoryItem> GetInventoryItemById(int id)
        {
            return await _inventoryDataProvider.Read(id);
        }

        public async Task UpdateInventoryItem(InventoryItem item)
        {
            await _inventoryDataProvider.Update(item);
        }

        public async Task<List<string>> GetUnits()
        {
            return await _inventoryDataProvider.ReadUnits();
        }
    }

    public interface IInventoryProvider
    {
        Task CreateInventoryItem(InventoryItem item);
        Task UpdateInventoryItem(InventoryItem item);
        Task<InventoryItem> GetInventoryItemById(int id);
        Task<List<InventoryItem>> GetAllInventoryItems(bool showOutOfStock, bool unique, bool printed);
        Task DeleteInventoryItem(int id);
        Task<List<string>> GetUnits();
    }
}