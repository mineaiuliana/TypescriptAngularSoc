using System.Linq;
using Booking.Api.Models;

namespace Booking.Api.Extensions
{
    public static class FlightExtensions
    {
        public static void Update(this FlightModel dbFlight, UpsertFlightModel flight)
        {
            dbFlight.From = flight.From;
            dbFlight.To = flight.To;
            dbFlight.Price = flight.Price;
            dbFlight.Date = flight.Date;
        }

        public static FlightModel Save(this UpsertFlightModel flight)
        {
            var newItem = new FlightModel
                          {
                              Id = Repository.Flights.Max(f => f.Id) + 1,
                              To = flight.To,
                              From = flight.From,
                              Price = flight.Price,
                              Date = flight.Date
                          };

            Repository.Flights.Add(newItem);
            return newItem;
        }
    }
}