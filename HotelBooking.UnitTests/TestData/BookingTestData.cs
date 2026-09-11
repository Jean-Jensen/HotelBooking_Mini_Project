using System;
using System.Collections.Generic;
using HotelBooking.Core;
using Xunit;

namespace HotelBooking.UnitTests.TestData;

public static class BookingTestData
{
    public static readonly DateTime StartDateData = DateTime.Today.AddDays(1);
    public static readonly DateTime EndDateData = StartDateData.AddDays(5);

    public static List<Room> DefaultRooms =>
    [
        new Room { Id = 1, Description = "Room 1" },
        new Room { Id = 2, Description = "Room 2" },
        new Room { Id = 3, Description = "Room 3" }
    ];

    public static TheoryData<List<Booking>> SomeRoomsOccupiedData =>
    [

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
    ];
    
    public static TheoryData<List<Booking>> AllRoomsOccupiedForSeveralDaysData =>
    [

        // All rooms occupied with different overlapping dates
        // first theory case
        new List<Booking>
        {
            CreateBooking(1, 1, StartDateData, StartDateData.AddDays(10)),
            CreateBooking(2, 2, StartDateData.AddDays(1), StartDateData.AddDays(15)),
            CreateBooking(3, 3, StartDateData.AddDays(2), StartDateData.AddDays(17))
        },
        // second theory case
        new List<Booking>
        {
            CreateBooking(1, 1, StartDateData, StartDateData.AddDays(8)),
            CreateBooking(2, 2, StartDateData.AddDays(1), StartDateData.AddDays(8)),
            CreateBooking(3, 3, StartDateData.AddDays(2), StartDateData.AddDays(10)),
            
            CreateBooking(1, 1, StartDateData.AddDays(9), StartDateData.AddDays(14)),
            CreateBooking(2, 2, StartDateData.AddDays(9), StartDateData.AddDays(13)),
            CreateBooking(3, 3, StartDateData.AddDays(11), StartDateData.AddDays(17))
        }
    ];
    
    public static TheoryData<Booking> InactiveData =>
    [

        CreateBooking(1, 1, StartDateData.AddDays(2), StartDateData.AddDays(8), false),

        CreateBooking(1, 1, StartDateData.AddDays(10), StartDateData.AddDays(15), false),
        
        CreateBooking(1, 1, StartDateData.AddDays(8), StartDateData.AddDays(12), false),
        
        CreateBooking(1, 1, StartDateData.AddDays(32), StartDateData.AddDays(64), false),
            
    ];
    
    public static TheoryData<Booking> OneRoomBookedData =>
    [

        // One room occupied 
        CreateBooking(1, 1, StartDateData.AddDays(2), StartDateData.AddDays(8)),
        CreateBooking(1, 1, StartDateData.AddDays(12), StartDateData.AddDays(20)),
        CreateBooking(1, 1, StartDateData.AddDays(32), StartDateData.AddDays(64)),
        CreateBooking(1, 1, StartDateData.AddDays(3), StartDateData.AddDays(4)),
        
    ];
    
    public static Booking CreateBooking(int id, int roomId, DateTime? startDate = null, DateTime? endDate = null, bool? isActive = true)
    {
        return new Booking
        {
            Id = id,
            StartDate = startDate ?? StartDateData,
            EndDate = endDate ?? EndDateData,
            IsActive = isActive ?? true,
            RoomId = roomId
        };
    }

}

            
        