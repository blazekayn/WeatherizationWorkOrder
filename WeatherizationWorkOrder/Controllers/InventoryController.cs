using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WeatherizationWorkOrder.Business;
using WeatherizationWorkOrder.Models;

namespace WeatherizationWorkOrder.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly ILogger<InventoryController> _logger;
        private readonly IInventoryProvider _inventoryProvider;

        public InventoryController(ILogger<InventoryController> logger,
            IInventoryProvider inventoryProvider)
        {
            _logger = logger;
            _inventoryProvider = inventoryProvider;
        }

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
        public async Task<List<InventoryItem>> Get([FromQuery(Name = "showOOS")] bool showOutOfStock, [FromQuery(Name = "unique")] bool unique)
        {
            return await _inventoryProvider.GetAllInventoryItems(showOutOfStock, unique);
        }

        [HttpPost]
        public async Task Post([FromBody]InventoryItem item)
        {
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