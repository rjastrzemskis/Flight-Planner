using FlightPlanner.Core.Models;

namespace FlightPlanner.Core.Services
{
    public interface IDbServiceExtended : IDbService
    {
        void DeleteAll<T>() where T : Entity;
    }
}
