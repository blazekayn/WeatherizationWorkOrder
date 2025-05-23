﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WeatherizationWorkOrder.Business;
using WeatherizationWorkOrder.Data;
using WeatherizationWorkOrder.Models;

namespace WeatherizationWorkOrder.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkOrderController(IWorkOrderProvider workOrderProvider) : ControllerBase
    {
        private readonly IWorkOrderProvider _workOrderProvider = workOrderProvider;

        [HttpGet]
        public async Task<List<WorkOrder>> Get(bool onlyIncomplete)
        {
            return await _workOrderProvider.GetAllWorkOrders(onlyIncomplete);
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
        [Route("materials")]
        public async Task<List<Material>> DeleteMaterials([FromBody] List<int> materialIds)
        {
            return await _workOrderProvider.DeleteMaterials(materialIds);
        }

        [HttpDelete]
        [Route("labor/{id}")]
        public async Task<List<Labor>> DeleteLabor(int id)
        {
            return await _workOrderProvider.DeleteLabor(id);
        }
    }
}