using FlightPlanner.Core.Models;

namespace FlightPlanner.Core.Services
{
    public interface IPageResultService : IEntityService<Flight>
    {
        PageResult SearchFlight(FlightSearch search);
    }
}
