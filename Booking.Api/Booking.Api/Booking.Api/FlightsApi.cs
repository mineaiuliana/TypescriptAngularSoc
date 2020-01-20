using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Booking.Api.Models;
using Booking.Api.Validators;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Booking.Api
{
    public static class FlightsApi
    {
        [FunctionName("GetFlights")]
        public static IActionResult Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "flights")] HttpRequest req)
        {
            return new OkObjectResult(Repository.Flights);
        }

        [FunctionName("GetFlightById")]
        public static IActionResult GetById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "flights/{id}")] HttpRequest req,
            int id)
        {
            var item = Repository.Flights.FirstOrDefault(f => f.Id == id);
            if (item == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(item);
        }

        [FunctionName("CreateFlight")]
        public static async Task<IActionResult> Post(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "flights")]HttpRequest req)
        {
            var json = await req.ReadAsStringAsync();
            var flight = JsonConvert.DeserializeObject<UpsertFlightModel>(json);

            var validationResult = new UpsertFlightModelValidator().Validate(flight);
            if (!validationResult.IsValid)
            {
                return CreateBadRequestResponse(validationResult);
            }

            var item  =  SaveFlight(flight);
            return new ObjectResult(item) {StatusCode = (int) HttpStatusCode.Created, };
        }


        [FunctionName("EditFlight")]
        public static async Task<IActionResult> Put(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "flights/{id}")]HttpRequest req,
            int id)
        {
            var json = await req.ReadAsStringAsync();
            var flight = JsonConvert.DeserializeObject<UpsertFlightModel>(json);

            var validationResult = new UpsertFlightModelValidator().Validate(flight);
            if (!validationResult.IsValid)
            {
                return CreateBadRequestResponse(validationResult);
            }

            var dbFlight = Repository.Flights.FirstOrDefault(t => t.Id == id);
            if (dbFlight == null)
            {
                return new NotFoundResult();
            }

            UpdateFlight(dbFlight, flight);
            return new OkObjectResult(dbFlight);
        }

        [FunctionName("DeleteFlight")]
        public static IActionResult Delete(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "flights/{id}")]HttpRequest req,
            int id)
        {
            var flight = Repository.Flights.FirstOrDefault(t => t.Id == id);
            if (flight == null)
            {
                return new NotFoundResult();
            }
            Repository.Flights.Remove(flight);
            return new OkResult();
        }

        private static BadRequestObjectResult CreateBadRequestResponse(ValidationResult validationResult)
        {
            return new BadRequestObjectResult(validationResult.Errors.Select(e => new
                                                                                  {
                                                                                      Field = e.PropertyName,
                                                                                      Error = e.ErrorMessage
                                                                                  }));
        }

        private static void UpdateFlight(FlightModel dbFlight, UpsertFlightModel flight)
        {
            dbFlight.From = flight.From;
            dbFlight.To = flight.To;
            dbFlight.Price = flight.Price;
            dbFlight.Date = flight.Date;
        }

        private static FlightModel SaveFlight(UpsertFlightModel flight)
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
