using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WeatherizationWorkOrder.Business;
using WeatherizationWorkOrder.Models;

namespace WeatherizationWorkOrder.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController(IInventoryProvider inventoryProvider) : ControllerBase
    {
        private readonly IInventoryProvider _inventoryProvider = inventoryProvider;

        [HttpGet("{id}")]
        public async Task<InventoryItem> Get(int id)
        {
            return await _inventoryProvider.GetInventoryItemById(id);
        }

        [HttpGet("units")]
        public async Task<List<string>> GetUnits()
        {
            return await _inventoryProvider.GetUnits();
        }


        [HttpGet]
        public async Task<List<InventoryItem>> Get([FromQuery(Name = "showOOS")] bool showOutOfStock, [FromQuery(Name = "unique")] bool unique, [FromQuery(Name = "printed")] bool printed)
        {
            return await _inventoryProvider.GetAllInventoryItems(showOutOfStock, unique, printed);
        }

        [HttpPost]
        public async Task Post([FromBody]InventoryItem item)
        {
            item.LastModified = DateTime.Now;
            await _inventoryProvider.UpdateInventoryItem(item);
        }

        [HttpPut]
        public async Task Put([FromBody] InventoryItem item)
        {
            await _inventoryProvider.CreateInventoryItem(item);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _inventoryProvider.DeleteInventoryItem(id);
        }
    }
}