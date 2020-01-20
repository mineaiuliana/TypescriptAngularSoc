using Booking.Api.Models;
using FluentValidation;

namespace Booking.Api.Validators
{
    public class UpsertFlightModelValidator:AbstractValidator<UpsertFlightModel>
    {
        public UpsertFlightModelValidator()
        {
            RuleFor(m => m.From).NotEmpty();
            RuleFor(m => m.To).NotEmpty();
            RuleFor(m => m.Price).NotNull();
            RuleFor(m => m.Date).NotNull();
        }
    }
}
