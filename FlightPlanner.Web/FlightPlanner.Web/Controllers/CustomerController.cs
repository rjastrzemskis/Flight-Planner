using System.Linq;
using FlightPlanner.Web.DbContext;
using FlightPlanner.Web.Models;
using FlightPlanner.Web.Storage;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Web.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly FlightPlannerDbContext _context;
        public CustomerController(FlightPlannerDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("airports")]
        public IActionResult SearchAirports(string search)
        {
            IQueryable<Airport> airport = FlightStorage.SearchForAirports(search, _context);
            return Ok(airport);
        }

        [HttpPost]
        [Route("flights/search")]
        public IActionResult SearchFlights(FlightSearch search)
        {
            if (FlightStorage.FlightHasSameFlightsFromTo(search))
                return BadRequest();

            PageResult pageResult = FlightStorage.SearchFlight(search, _context);
            return Ok(pageResult);
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult SearchFlightsById(int id)
        {
            Flight flight = FlightStorage.GetById(id, _context);
            if (flight == null)
                return NotFound();

            return Ok(flight);
        }
    }
}