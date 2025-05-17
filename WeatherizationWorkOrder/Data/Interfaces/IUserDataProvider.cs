using WeatherizationWorkOrder.Models;

namespace WeatherizationWorkOrder.Data.Interfaces
{
    public interface IUserDataProvider
    {
        Task Create(string name);
        Task Delete(int id);
        Task<List<User>> Read();
        Task Update(int id, string name);
    }
}