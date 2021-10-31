using System.Collections.Generic;
using System.Linq;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;

namespace FlightPlanner.Services
{
    public class PageResultService : EntityService<Flight>, IPageResultService
    {
        private static PageResult _pageResults = new PageResult();
        public PageResultService(FlightPlannerDbContext context) : base(context)
        {
        }

        public PageResult SearchFlight(FlightSearch search)
        {
            _pageResults.Items = new List<Flight>();
            Flight flight = _context.Flights.FirstOrDefault(f =>
                    f.From.AirportCode == search.From &&
                    f.To.AirportCode == search.To &&
                    f.DepartureTime.Substring(0, 10) == search.DepartureDate);

            if (flight != null)
                _pageResults.Items.Add(flight);

            _pageResults.TotalItems = _pageResults.Items.Count;
            _pageResults.Page = _pageResults.TotalItems;
            return _pageResults;
        }
    }
}
