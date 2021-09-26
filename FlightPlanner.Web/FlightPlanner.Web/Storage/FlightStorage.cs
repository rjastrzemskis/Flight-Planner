using System;
using System.Collections.Generic;
using System.Linq;
using FlightPlanner.Web.DbContext;
using FlightPlanner.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Web.Storage
{
    public class FlightStorage
    {
        private static PageResult _pageResults = new PageResult();
        private static readonly object flightLock = new object();

        public static Flight GetById(int id, FlightPlannerDbContext _context)
        {
            lock (flightLock)
            {
                return _context.Flights.Include(f => f.From).Include(f => f.To).FirstOrDefault(f => f.Id == id);
            }
        }

        public static void ClearFlights(FlightPlannerDbContext _context)
        {
            foreach (Airport airport in _context.Airports)
                _context.Airports.Remove(airport);

            foreach (Flight flight in _context.Flights)
                _context.Flights.Remove(flight);

            _context.SaveChanges();
        }

        public static Flight AddFlight(Flight flight, FlightPlannerDbContext _context)
        {
            lock (flightLock)
            {
                _context.Flights.Add(flight);
                _context.SaveChanges();
                return flight;
            }
        }

        public static bool SameFlightAlreadyExists (Flight flight, FlightPlannerDbContext _context)
        {
            lock (flightLock)
            {
                return _context.Flights.Include(f => f.From)
                    .Include(f => f.To)
                    .Any(f => f.ArrivalTime == flight.ArrivalTime
                              && f.DepartureTime == flight.DepartureTime
                              && f.Carrier == flight.Carrier
                              && f.From.AirportCode == flight.From.AirportCode
                              && f.To.AirportCode == flight.To.AirportCode);
            }
        }

        public static bool FlightHasWrongValues(Flight flight)
        {
            return flight.From == null ||
                   string.IsNullOrEmpty(flight.From.Country) ||
                   string.IsNullOrEmpty(flight.From.City) ||
                   string.IsNullOrEmpty(flight.From.AirportCode) ||
                   flight.To == null ||
                   string.IsNullOrEmpty(flight.To.Country) ||
                   string.IsNullOrEmpty(flight.To.City) ||
                   string.IsNullOrEmpty(flight.To.AirportCode) ||
                   string.IsNullOrEmpty(flight.Carrier) ||
                   flight.DepartureTime == null ||
                   flight.ArrivalTime == null;
        }

        public static bool FlightHasSameAirports(Flight flight)
        {
            return flight.From.AirportCode.ToUpper().Trim() == flight.To.AirportCode.ToUpper().Trim();
        }

        public static bool FlightHasStrangeDates(Flight flight)
        {
            return Convert.ToDateTime(flight.DepartureTime) >= Convert.ToDateTime(flight.ArrivalTime);
        }

        public static void RemoveFlight(int id, FlightPlannerDbContext _context)
        {
            lock (flightLock)
            {
                Flight flight = GetById(id, _context);
                if (flight != null)
                {
                    _context.Airports.Remove(flight.From);
                    _context.Airports.Remove(flight.To);
                    _context.Flights.Remove(flight);
                    _context.SaveChanges();
                }
            }
        }

        public static IQueryable<Airport> SearchForAirports(string phrase, FlightPlannerDbContext _context)
        {
            lock (flightLock)
            {
                phrase = phrase.ToUpper().Trim();
                IQueryable<Airport> airport = _context.Airports.Where(f =>
                    f.AirportCode.ToUpper().StartsWith(phrase) ||
                    f.City.ToUpper().StartsWith(phrase) ||
                    f.Country.ToUpper().StartsWith(phrase));
                return airport;
            }
        }

        public static bool FlightHasSameFlightsFromTo(FlightSearch search)
        {
            return search.From == search.To;
        }

        public static PageResult SearchFlight(FlightSearch search, FlightPlannerDbContext _context)
        {
            _pageResults.Items = new List<Flight>();
            Flight flight = _context.Flights.Include(f => f.From)
                .Include(f => f.To).FirstOrDefault(f =>
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
