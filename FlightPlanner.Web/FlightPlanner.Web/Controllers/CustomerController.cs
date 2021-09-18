using FlightPlanner.Web.Models;
using FlightPlanner.Web.Storage;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Web.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        [HttpGet]
        [Route("airports")]
        public IActionResult SearchAirports(string search)
        {
            Airport[] airport = FlightStorage.SearchForAirports(search);
            return Ok(airport);
        }

        [HttpPost]
        [Route("flights/search")]
        public IActionResult SearchFlights(FlightSearch search)
        {
            if (FlightStorage.NotAbleToSearchSameFlights(search))
                return BadRequest();

            PageResult pageResult = FlightStorage.SearchFlight(search);
            return Ok(pageResult);
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult SearchFlightsById(int id)
        {
            Flight flight = FlightStorage.GetById(id);
            if (flight == null)
                return NotFound();

            return Ok(flight);
        }
    }
}