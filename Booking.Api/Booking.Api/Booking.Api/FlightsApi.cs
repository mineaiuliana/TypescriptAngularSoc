using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Booking.Api.Extensions;
using Booking.Api.Models;
using Booking.Api.Validators;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;

namespace Booking.Api
{
    public static class FlightsApi
    {
        [FunctionName("GetFlights")]
        public static IActionResult Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "flights")]
            HttpRequest req) =>
            new OkObjectResult(Repository.Flights);

        [FunctionName("GetFlightById")]
        public static IActionResult GetById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "flights/{id}")]
            HttpRequest req,
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "flights")]
            HttpRequest req)
        {
            var json = await req.ReadAsStringAsync();
            var flight = JsonConvert.DeserializeObject<UpsertFlightModel>(json);

            var validationResult = await new UpsertFlightModelValidator().ValidateAsync(flight);
            if (!validationResult.IsValid)
            {
                return CreateBadRequestResponse(validationResult);
            }

            var item = flight.Save();
            return new ObjectResult(item) {StatusCode = (int) HttpStatusCode.Created,};
        }

        [FunctionName("EditFlight")]
        public static async Task<IActionResult> Put(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "flights/{id}")]
            HttpRequest req,
            int id)
        {
            var json = await req.ReadAsStringAsync();
            var flight = JsonConvert.DeserializeObject<UpsertFlightModel>(json);

            var validationResult = await new UpsertFlightModelValidator().ValidateAsync(flight);
            if (!validationResult.IsValid)
            {
                return CreateBadRequestResponse(validationResult);
            }

            var dbFlight = Repository.Flights.FirstOrDefault(t => t.Id == id);
            if (dbFlight == null)
            {
                return new NotFoundResult();
            }

            dbFlight.Update(flight);
            return new OkObjectResult(dbFlight);
        }

        [FunctionName("DeleteFlight")]
        public static IActionResult Delete(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "flights/{id}")]
            HttpRequest req,
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
            var errors = validationResult.Errors.Select(e => new
                                                             {
                                                                 Field = e.PropertyName,
                                                                 Error = e.ErrorMessage
                                                             });
            return new BadRequestObjectResult(errors);
        }
    }
}