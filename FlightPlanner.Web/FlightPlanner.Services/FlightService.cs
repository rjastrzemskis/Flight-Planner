using System.Linq;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Services
{
    public class FlightService : EntityService<Flight>, IFlightService
    {
        public FlightService(FlightPlannerDbContext context) : base(context)
        {
        }

        public Flight GetFullFlightById(int id)
        {
            return _context.Flights
                .Include(f => f.From)
                .Include(f => f.To)
                .SingleOrDefault(f => f.Id == id);
        }

        public bool Exists(Flight flight)
        {
            return Query().Any(f => f.ArrivalTime == flight.ArrivalTime &&
                                    f.DepartureTime == flight.DepartureTime &&
                                    f.Carrier == flight.Carrier &&
                                    f.From.AirportCode == flight.From.AirportCode &&
                                    f.To.AirportCode == flight.To.AirportCode);
        }

        public void RemoveFlight(int id)
        {
            Flight flight = GetFullFlightById(id);
            if (flight != null)
            {
                _context.Airports.Remove(flight.From);
                _context.Airports.Remove(flight.To);
                _context.Flights.Remove(flight);
                _context.SaveChanges();
            }
        }
    }
}
