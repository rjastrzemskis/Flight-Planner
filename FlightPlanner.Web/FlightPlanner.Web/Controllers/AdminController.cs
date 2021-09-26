using FlightPlanner.Web.DbContext;
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
        private readonly FlightPlannerDbContext _context;
        public AdminController(FlightPlannerDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlight(int id)
        {
            Flight flight = FlightStorage.GetById(id,_context);
            if (flight == null)
                return NotFound();

            return Ok(flight);
        }

        [HttpPut]
        [Route("flights")]
        public IActionResult AddFlightRequest(Flight flight)
        {
            if (FlightStorage.FlightHasWrongValues(flight) ||
                FlightStorage.FlightHasSameAirports(flight) ||
                FlightStorage.FlightHasStrangeDates(flight))
                return BadRequest();

            if (FlightStorage.SameFlightAlreadyExists(flight, _context))
                return Conflict();

            FlightStorage.AddFlight(flight, _context);
            return Created(string.Empty, flight);
        }

        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult RemoveFlightRequest(int id)
        {
            FlightStorage.RemoveFlight(id, _context);
            return Ok();
        }
    }
}
