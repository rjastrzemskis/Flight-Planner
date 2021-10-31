﻿using FlightPlanner.Core.Models;

namespace FlightPlanner.Core.Services
{
    public interface ISearchValidator
    {
        bool IsValid(FlightSearch search);
    }
}
