using System;
using System.Collections.Generic;
using HotelBooking.Core;

namespace HotelBooking.UnitTests.TestData;

public static class BookingTestData
{
    public static readonly DateTime StartDate = DateTime.Today;
    public static readonly DateTime EndDate = StartDate.AddDays(4);

    public static List<Room> DefaultRooms =>
        new()
        {
            new Room { Id = 1, Description = "Room 1" },
            new Room { Id = 2, Description = "Room 2" },
            new Room { Id = 3, Description = "Room 3" }
        };
    
    public static IEnumerable<object[]> SomeRoomsOccupiedData =>
        new List<object[]>
        {
            // One room occupied
            new object[]
            {
                new List<Booking>
                {
                    new Booking
                    {
                        Id = 1,
                        StartDate = StartDate,
                        EndDate = EndDate,
                        IsActive = true,
                        RoomId = 1
                    }
                }
            },

            // Two rooms occupied
            new object[]
            {
                new List<Booking>
                {
                    new Booking
                    {
                        Id = 1,
                        StartDate = StartDate,
                        EndDate = EndDate,
                        IsActive = true,
                        RoomId = 1
                    },
                    new Booking
                    {
                        Id = 2,
                        StartDate = StartDate,
                        EndDate = EndDate,
                        IsActive = true,
                        RoomId = 2
                    }
                }
            }
        };
}