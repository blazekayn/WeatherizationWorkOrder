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
        private readonly ILogger<WorkOrderController> _logger;
        private readonly WorkOrderProvider _workOrderProvider;

        public WorkOrderController(ILogger<WorkOrderController> logger,
            WorkOrderProvider workOrderProvider)
        {
            _logger = logger;
            _workOrderProvider = workOrderProvider;
        }

        [HttpGet]
        public async Task<List<WorkOrder>> Get()
        {
            return await _workOrderProvider.GetAllWorkOrders();
        }

        [HttpGet("{id}")]
        public async Task<WorkOrder> Get(int id)
        {
            return await _workOrderProvider.GetWorkOrderByID(id);
        }

        [HttpPut]
        public async Task<int> Put([FromBody] WorkOrder workOrder)
        {
            return await _workOrderProvider.CreateWorkOrder(workOrder);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _workOrderProvider.DeleteWorkOrder(id);
        }
        [HttpPost]
        public async Task Update([FromBody] WorkOrder workOrder)
        {
            await _workOrderProvider.UpdateWorkOrder(workOrder);
        }   

        [HttpPost]
        [Route("AddMaterial")]
        public async Task<AddMaterialResponse> AddMaterial([FromBody] AddMaterialRequest addMaterialRequest)
        {
            return await _workOrderProvider.AddMaterialToWorkOrder(addMaterialRequest);
        }

        [HttpPost]
        [Route("AddLabor")]
        public async Task AddLabor([FromBody] AddLaborRequest addLaborRequest)
        {
            await _workOrderProvider.AddLaborToWorkOrder(addLaborRequest);
        }
    }
}