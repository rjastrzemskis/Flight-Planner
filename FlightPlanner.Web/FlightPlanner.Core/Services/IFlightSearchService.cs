using System.Linq;
using FlightPlanner.Core.Models;

namespace FlightPlanner.Core.Services
{
    public interface IFlightSearchService : IEntityService<Airport>
    {
        Airport SearchForAirports(string phrase);
    }
}
