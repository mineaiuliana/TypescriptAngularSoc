using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Api.Models
{
    public class UpsertFlightModel
    {
        public int? Id { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
    }
}
