using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WeatherizationWorkOrder.Business;
using WeatherizationWorkOrder.Data;
using WeatherizationWorkOrder.Data.Interfaces;
using WeatherizationWorkOrder.Models;

namespace WeatherizationWorkOrder.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(IUserDataProvider userProvider) : ControllerBase
    {
        private readonly IUserDataProvider _userProvider = userProvider;

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