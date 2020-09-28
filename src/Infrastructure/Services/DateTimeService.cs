using PayMeWithRocks.Application.Common.Interfaces;
using System;

namespace PayMeWithRocks.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now.ToUniversalTime();
    }
}