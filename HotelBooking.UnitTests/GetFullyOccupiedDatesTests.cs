using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using HotelBooking.UnitTests.TestData;
using Xunit;

namespace HotelBooking.UnitTests;

public class GetFullyOccupiedDatesTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper output = output; // output to console

    // Case 1
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
    }
    
    // Case 2
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
    
    // Case 3
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
        var expectedDates = Enumerable.Range(
                0,
                (BookingTestData.EndDateData - BookingTestData.StartDateData).Days + 1)
            .Select(offset => BookingTestData.StartDateData.AddDays(offset))
            .ToArray();

        Assert.Equal(expectedDates, result.OrderBy(date => date).ToArray());
    }
    
    // Case 4 and 5
    [Theory]
    [MemberData(
        nameof(BookingTestData.AllRoomsOccupiedForSeveralDaysData),
        MemberType = typeof(BookingTestData))]
    public async Task GetFullyOccupiedDates_PartOfPeriodFullyOccupied_ReturnsOnlyFullyOccupiedDates(
        List<Booking> bookings,
        int[] expectedDayOffsets)    
    {
        //Arrange 
        var rooms = BookingTestData.DefaultRooms;
        var bookingManager = MoqBookingManager.CreateBookingManager(rooms, bookings);
        
        //Act 
        var result = await bookingManager
            .GetFullyOccupiedDates(BookingTestData.StartDateData, BookingTestData.StartDateData.AddDays(30));
        
        //Assert 
        output.WriteLine($"Fully occupied dates: {string.Join(", ", result)}");
        
        var expectedDates = expectedDayOffsets
            .Select(offset => BookingTestData.StartDateData.AddDays(offset))
            .ToArray();

        Assert.Equal(expectedDates, result.OrderBy(date => date).ToArray());
       
    }
    
    // Case 6
    [Fact]
    public async Task GetFullyOccupiedDates_StartAndEndDatesFullyOccupied_IncludesBoundaries()
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
        output.WriteLine($"Fully occupied dates: {string.Join(", ", result)}");
        
        Assert.Contains(BookingTestData.StartDateData, result);
        Assert.Contains(BookingTestData.EndDateData, result);
    }
    
    // Case 7
    [Fact]
    public async Task GetFullyOccupiedDates_InactiveBooking_IgnoresBooking()
    {
        //Arrange 
        var rooms = BookingTestData.DefaultRooms;
        var bookings = new List<Booking>
        {
            BookingTestData.CreateBooking(1, rooms[0].Id),
            BookingTestData.CreateBooking(2, rooms[1].Id),
            BookingTestData.CreateBooking(3, rooms[2].Id, isActive: false)
        };
        
        var bookingManager = MoqBookingManager.CreateBookingManager(rooms, bookings);
        
        //Act 
        var result = await bookingManager
            .GetFullyOccupiedDates(BookingTestData.StartDateData, BookingTestData.EndDateData);
        
        //Assert 
        output.WriteLine($"Fully occupied dates: {string.Join(", ", result)}");
        
        Assert.Empty(result);
    }
}