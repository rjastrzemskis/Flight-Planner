using FlightPlanner.Web.Models;
using FlightPlanner.Web.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Web.Controllers
{
    [Authorize]
    [Route("admin-api")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlight(int id)
        {
            Flight flight = FlightStorage.GetById(id);
            if (flight == null)
                return NotFound();

            return Ok(flight);
        }

        [HttpPut]
        [Route("flights")]
        public IActionResult AddFlightRequest(Flight flight)
        {
            if (FlightStorage.NotAbleToAcceptWrongValues(flight) ||
                FlightStorage.NotAbleToAcceptSameAirports(flight) ||
                FlightStorage.NotAbleToAcceptStrangeDates(flight))
                return BadRequest();

            if (FlightStorage.NotAbleToAddSameFlight(flight))
                return Conflict();

            return Created(string.Empty, FlightStorage.AddFlight(flight));
        }

        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult RemoveFlightRequest(int id)
        {
            FlightStorage.RemoveFlight(id);
            return Ok();
        }
    }
}
