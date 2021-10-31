using System.Linq;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;

namespace FlightPlanner.Services
{
    public class FlightSearchService : EntityService<Airport>, IFlightSearchService
    {
        public FlightSearchService(FlightPlannerDbContext context) : base(context)
        {
        }

        public Airport SearchForAirports(string phrase)
        {
            phrase = phrase.ToUpper().Trim();
            var airport = _context.Airports.SingleOrDefault(f =>
                f.AirportCode.ToUpper().StartsWith(phrase) ||
                f.City.ToUpper().StartsWith(phrase) ||
                f.Country.ToUpper().StartsWith(phrase));
            return airport;
        }
    }
}
