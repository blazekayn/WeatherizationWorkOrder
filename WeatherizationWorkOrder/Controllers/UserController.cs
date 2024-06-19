using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WeatherizationWorkOrder.Business;
using WeatherizationWorkOrder.Data;
using WeatherizationWorkOrder.Models;

namespace WeatherizationWorkOrder.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<InventoryController> _logger;
        private readonly UserDataProvider _userProvider;

        public UserController(ILogger<InventoryController> logger,
            UserDataProvider userProvider)
        {
            _logger = logger;
            _userProvider = userProvider;
        }

        [HttpGet]
        public async Task<List<User>> Get()
        {
            return await _userProvider.Read();
        }

        [HttpPut]
        public async Task Put([FromBody] string name)
        {
            await _userProvider.Create(name);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _userProvider.Delete(id);
        }

        [HttpPost("{id}")]
        public async Task Update(int id, [FromBody] string name)
        {
            await _userProvider.Update(id, name);
        }
    }
}