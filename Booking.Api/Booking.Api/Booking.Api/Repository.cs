using System;
using System.Collections.Generic;
using System.Text;
using Booking.Api.Models;

namespace Booking.Api
{
    public static class Repository
    {
       public static List<FlightModel> Flights = new List<FlightModel>
                                                 {
                                                     new FlightModel
                                                     {
                                                         Id = 1,
                                                         From= "Iasi",
                                                         To ="Rome",
                                                         Price= 23.54m,
                                                         Date= new DateTime(2020, 1, 30, 12, 10, 0, 0)
                                                     },
                                                     new FlightModel
                                                     {
                                                         Id = 2,
                                                         From= "Rome",
                                                         To ="Iasi",
                                                         Price= 123.54m,
                                                         Date= new DateTime(2020, 1, 30, 17, 50, 0, 0)
                                                     }
                                                 };
    }
}
