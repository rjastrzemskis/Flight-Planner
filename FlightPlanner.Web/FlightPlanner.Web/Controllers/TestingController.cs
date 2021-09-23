using FlightPlanner.Web.DbContext;
using FlightPlanner.Web.Storage;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Web.Controllers
{
    [Route("testing-api")]
    [ApiController]
    public class TestingController : ControllerBase
    {
        private static FlightPlannerDbContext _context;
        public TestingController(FlightPlannerDbContext context)
        {
            _context = context;
        }

        [Route("clear")]
        [HttpPost]
        public IActionResult Clear()
        {
            FlightStorage.ClearFlights(_context);
            return Ok();
        }
    }
}
