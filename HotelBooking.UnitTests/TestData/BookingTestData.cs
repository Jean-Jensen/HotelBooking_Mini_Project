using System;
using System.Collections.Generic;
using HotelBooking.Core;
using Xunit;

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

    public static TheoryData<List<Booking>> SomeRoomsOccupiedData =>
        new()
        {
            // One room occupied (first theory case)
            new List<Booking>
            {
                CreateBooking(1, 1)
            },

            // Two rooms occupied (second theory case)
            new List<Booking>
            {
                CreateBooking(1, 1),
                CreateBooking(2, 2)
            }
        };
    
    private static Booking CreateBooking(int id, int roomId)
    {
        return new Booking
        {
            Id = id,
            StartDate = StartDate,
            EndDate = EndDate,
            IsActive = true,
            RoomId = roomId
        };
    }

}

            
        