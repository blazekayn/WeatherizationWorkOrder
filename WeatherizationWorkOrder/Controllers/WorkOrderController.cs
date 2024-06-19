using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WeatherizationWorkOrder.Business;
using WeatherizationWorkOrder.Data;
using WeatherizationWorkOrder.Models;

namespace WeatherizationWorkOrder.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkOrderController : ControllerBase
    {
        private readonly ILogger<InventoryController> _logger;
        private readonly WorkOrderProvider _workOrderProvider;

        public WorkOrderController(ILogger<InventoryController> logger,
            WorkOrderProvider workOrderProvider)
        {
            _logger = logger;
            _workOrderProvider = workOrderProvider;
        }

        //[HttpGet]
        //public async Task<List<User>> Get()
        //{
        //    return await _userProvider.Read();
        //}

        [HttpPut]
        public async Task Put([FromBody] WorkOrder workOrder)
        {
            await _workOrderProvider.CreateWorkOrder(workOrder);
        }

        //[HttpDelete("{id}")]
        //public async Task Delete(int id)
        //{
        //    await _userProvider.Delete(id);
        //}
    }
}