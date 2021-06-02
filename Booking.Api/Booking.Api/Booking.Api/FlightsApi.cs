using System.Collections.Generic;
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
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace Booking.Api
{
    public static class FlightsApi
    {
        [FunctionName("GetFlights")]
        [OpenApiOperation(operationId: "Get", tags: new []{"Get flights"})]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<FlightModel>), Description = "Returns the flights")]
        public static IActionResult Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "flights")]
            HttpRequest req) =>
            new OkObjectResult(Repository.Flights);

        [FunctionName("GetFlightById")]
        [OpenApiOperation(operationId: "GetById", tags: new[] { "Get flight by id" })]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "flight id")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(FlightModel))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound, contentType:"text/plain", bodyType: typeof(string))]
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
        [OpenApiOperation("add", "Book new flight")]
        [OpenApiRequestBody("application/json", typeof(UpsertFlightModel))]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(FlightModel))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", typeof(FlightModel))]
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
        [OpenApiOperation("edit", "Edit existing flight")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "flight id")]
        [OpenApiRequestBody("application/json", typeof(UpsertFlightModel))]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(FlightModel))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", typeof(object))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound, contentType: "text/plain", bodyType: typeof(string))]
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
        [OpenApiOperation("delete", "Delet flight")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "flight id")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(FlightModel))]
        [OpenApiResponseWithBody(HttpStatusCode.NotFound, "text/plain", typeof(string))]
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