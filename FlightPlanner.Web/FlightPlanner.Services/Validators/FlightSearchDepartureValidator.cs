using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;

namespace FlightPlanner.Services.Validators
{
    public class FlightSearchValidator : ISearchValidator
    {
        public bool IsValid(FlightSearch search)
        {
            return search.From == search.To;
        }
    }
}
