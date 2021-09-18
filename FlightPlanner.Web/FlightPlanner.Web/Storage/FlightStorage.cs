using System;
using System.Collections.Generic;
using System.Linq;
using FlightPlanner.Web.Models;

namespace FlightPlanner.Web.Storage
{
    public static class FlightStorage
    {
        private static List<Flight> _flights = new List<Flight>();
        private static PageResult _pageResults = new PageResult();
        private static readonly object flightLock = new object();
        private static int _id;

        public static Flight GetById(int id)
        {
            lock (flightLock)
            {
                return _flights.FirstOrDefault(f => f.Id == id);
            }
        }

        public static void ClearFlights()
        {
            _flights.Clear();
        }

        public static Flight AddFlight(Flight flight)
        {
            flight.Id = _id;
            _flights.Add(flight);
            _id++;
            return flight;
        }

        public static bool NotAbleToAddSameFlight(Flight flight)
        {
            lock (flightLock)
            {
                foreach (Flight flightInList in _flights.ToList())
                {
                    return flightInList.From.Country == flight.From.Country &&
                           flightInList.From.City == flight.From.City &&
                           flightInList.From.AirportCode == flight.From.AirportCode &&
                           flightInList.To.Country == flight.To.Country &&
                           flightInList.To.City == flight.To.City &&
                           flightInList.To.AirportCode == flight.To.AirportCode &&
                           flightInList.Carrier == flight.Carrier &&
                           flightInList.DepartureTime == flight.DepartureTime &&
                           flightInList.ArrivalTime == flight.ArrivalTime;
                }

                return false;
            }
        }

        public static bool NotAbleToAcceptWrongValues(Flight flight)
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

        public static bool NotAbleToAcceptSameAirports(Flight flight)
        {
            return flight.From.AirportCode.ToUpper().Trim() == flight.To.AirportCode.ToUpper().Trim();
        }

        public static bool NotAbleToAcceptStrangeDates(Flight flight)
        {
            return Convert.ToDateTime(flight.DepartureTime) >= Convert.ToDateTime(flight.ArrivalTime);
        }

        public static void RemoveFlight(int id)
        {
            lock (flightLock)
            {
                Flight flight = GetById(id);
                if (flight != null)
                    _flights.Remove(flight);
            }
        }

        public static Airport[] SearchForAirports(string phrase)
        {
            phrase = phrase.ToUpper().Trim();
            Flight flight = _flights.SingleOrDefault(f =>
                f.From.AirportCode.ToUpper().StartsWith(phrase) ||
                f.From.City.ToUpper().StartsWith(phrase) ||
                f.From.Country.ToUpper().StartsWith(phrase));
            return new[] { flight.From };
        }

        public static bool NotAbleToSearchSameFlights(FlightSearch search)
        {
            return search.From == search.To;
        }

        public static PageResult SearchFlight(FlightSearch search)
        {
            _pageResults.Items = new List<Flight>();
            Flight flight = _flights.Find(f =>
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
