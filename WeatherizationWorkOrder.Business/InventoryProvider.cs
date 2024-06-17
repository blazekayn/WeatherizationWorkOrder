using WeatherizationWorkOrder.DataContracts;

namespace WeatherizationWorkOrder.Business
{
    public class InventoryProvider : InventoryProviderInterface
    {
        public async Task CreateInventoryItem(InventoryItem item)
        {
            
        }

        public async Task DeleteInventoryItem(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<InventoryItem>> GetAllInventoryItems()
        {
            throw new NotImplementedException();
        }

        public async Task<InventoryItem> GetInventoryItemById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateInventoryItem(InventoryItem item)
        {
            throw new NotImplementedException();
        }
    }

    public interface InventoryProviderInterface
    {
        Task CreateInventoryItem(InventoryItem item);
        Task UpdateInventoryItem(InventoryItem item);
        Task<InventoryItem> GetInventoryItemById(int id);
        Task<List<InventoryItem>> GetAllInventoryItems();
        Task DeleteInventoryItem(int id);
    }
}