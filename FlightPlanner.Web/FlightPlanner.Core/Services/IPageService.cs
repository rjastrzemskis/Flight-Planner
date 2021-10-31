using FlightPlanner.Core.Models;

namespace FlightPlanner.Core.Services
{
    public interface IPageService : IEntityService<Flight>
    {
        PageResult SearchFlight(FlightSearch search);
    }
}
