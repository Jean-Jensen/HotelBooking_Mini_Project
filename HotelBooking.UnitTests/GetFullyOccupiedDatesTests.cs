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
        var rooms = BookingTestData.DefaultRooms;
        var bookings = new List<Booking>();
        var bookingManager = MoqBookingManager.CreateBookingManager(rooms, bookings);
        
        //Act 
        var result = await bookingManager
            .GetFullyOccupiedDates(BookingTestData.StartDateData, BookingTestData.EndDateData);
        
        //Assert 
        Assert.Empty(result);
        Assert.NotNull(result);
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
        var result = await bookingManager
            .GetFullyOccupiedDates(BookingTestData.StartDateData, BookingTestData.EndDateData);
        
        //Assert 
        Assert.Empty(result);
    }
    
    [Fact]
    public async Task GetFullyOccupiedDates_AllRoomsOccupied_ReturnsDate()
    {
        //Arrange 
        var rooms = BookingTestData.DefaultRooms;
        var bookings = new List<Booking>
        {
            BookingTestData.CreateBooking(1, rooms[0].Id),
            BookingTestData.CreateBooking(2, rooms[1].Id),
            BookingTestData.CreateBooking(3, rooms[2].Id)
        };
        
        var bookingManager = MoqBookingManager.CreateBookingManager(rooms, bookings);
        
        //Act 
        var result = await bookingManager
            .GetFullyOccupiedDates(BookingTestData.StartDateData, BookingTestData.EndDateData);
        
        //Assert 
        Assert.NotEmpty(result);
        Assert.NotNull(result);
        Assert.Contains(BookingTestData.StartDateData, result);
        Assert.Contains(BookingTestData.EndDateData, result);
    }
    
    [Theory]
    [MemberData(
        nameof(BookingTestData.AllRoomsOccupiedForSeveralDaysData),
        MemberType = typeof(BookingTestData))]
    public async Task GetFullyOccupiedDates_AllRoomsOccupiedForSeveralDays_ReturnsAllDates(List<Booking> bookings)
    {
        //Arrange 
        var rooms = BookingTestData.DefaultRooms;
        var bookingManager = MoqBookingManager.CreateBookingManager(rooms, bookings);
        
        //Act 
        var result = await bookingManager
            .GetFullyOccupiedDates(BookingTestData.StartDateData, BookingTestData.StartDateData.AddDays(30));
        
        //Assert 
        //TODO: Add assertions to check that the result contains all the expected fully occupied dates based on the provided bookings.
        Assert.NotEmpty(result);
        Assert.Contains(BookingTestData.StartDateData.AddDays(2), result);
        Assert.Contains(BookingTestData.StartDateData.AddDays(8), result);
    }
}