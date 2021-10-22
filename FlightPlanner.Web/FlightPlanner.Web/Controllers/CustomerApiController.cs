using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FlightPlanner.Core.Dto;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Web.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CustomerApiController : ControllerBase
    {

        private readonly IFlightService _flightService;
        private readonly IMapper _mapper;
        private readonly IFlightSearchService _flightSearchService;
        private readonly IPageResultService _pageResultService;
        private readonly ISearchValidator _searchValidators;

        public CustomerApiController(IFlightService flightService, IMapper mapper, IFlightSearchService flightSearchService, IPageResultService pageResultService, ISearchValidator searchValidators)
        {
            _flightService = flightService;
            _mapper = mapper;
            _flightSearchService = flightSearchService;
            _pageResultService = pageResultService;
            _searchValidators = searchValidators;
        }

        [HttpGet]
        [Route("airports")]
        public IActionResult GetByTag(string search)
        {
            Airport airport = _flightSearchService.SearchForAirports(search);
            return Ok(new[] { _mapper.Map<AirportResponse>(airport)});
        }

        [HttpPost]
        [Route("flights/search")]
        public IActionResult SearchFlights(FlightSearch search)
        {
            if (_searchValidators.IsValid(search))
                return BadRequest();

            PageResult pageResult = _pageResultService.SearchFlight(search);
            return Ok(pageResult);
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult SearchFlightsById(int id)
        {
            Flight flight = _flightService.GetFullFlightById(id);
            if (flight == null)
                return NotFound();

            return Ok(_mapper.Map<FlightResponse>(flight));
        }
    }
}
