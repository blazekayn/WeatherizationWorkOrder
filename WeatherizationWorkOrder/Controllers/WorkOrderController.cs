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

        [HttpGet]
        [Route("WorkOrderByDate")]
        public async Task<List<WorkOrder>> GetByDate(DateTime from, DateTime to)
        {
            return await _workOrderProvider.GetAllWorkOrders(from, to);
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
            workOrder.LastModified = DateTime.Now;
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
        public async Task<List<Labor>> AddLabor([FromBody] AddLaborRequest addLaborRequest)
        {
            return await _workOrderProvider.AddLaborToWorkOrder(addLaborRequest);
        }

        [HttpDelete]
        [Route("materials/{materialId}")]
        public async Task<List<Material>> DeleteMaterial(int materialId)
        {
            return await _workOrderProvider.DeleteMaterial(materialId);
        }

        [HttpDelete]
        [Route("labor/{id}")]
        public async Task<List<Labor>> DeleteLabor(int id)
        {
            return await _workOrderProvider.DeleteLabor(id);
        }
    }
}