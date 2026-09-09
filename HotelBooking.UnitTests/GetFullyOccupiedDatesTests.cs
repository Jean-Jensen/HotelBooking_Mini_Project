using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using HotelBooking.UnitTests.TestData;
using Xunit;

namespace HotelBooking.UnitTests;

public class GetFullyOccupiedDatesTests
{
    
    [Fact]
    public async Task GetFullyOccupiedDates_NoBookings_ReturnsEmptyList()
    {
        //Arrange 
        var rooms = new List<Room>
        {
            new Room { Id = 1, Description = "Room 1" },
            new Room { Id = 2, Description = "Room 2" }
        };
        var bookings = new List<Booking>();
        
        var bookingManager = MoqBookingManager.CreateBookingManager(rooms, bookings);

        var start = DateTime.Today;
        var end = start.AddDays(3);
        
        //Act 
        var result = await bookingManager.GetFullyOccupiedDates(start, end);
        
        //Assert 
        Assert.Empty(result);
    }
    
    [Theory]
    [MemberData(
        nameof(BookingTestData.SomeRoomsOccupiedData),
        MemberType = typeof(BookingTestData))]
    public async Task GetFullyOccupiedDates_SomeRoomsOccupied_DoesNotReturnDate(List<Booking> bookings)
    {
        //Arrange 
        var rooms = BookingTestData.DefaultRooms;
        var bookingManager = MoqBookingManager.CreateBookingManager(rooms, bookings);
        
        //Act 
        var result = await bookingManager.GetFullyOccupiedDates(BookingTestData.StartDate, BookingTestData.EndDate);
        
        //Assert 
        Assert.Empty(result);
    }
}