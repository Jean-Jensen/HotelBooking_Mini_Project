using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using HotelBooking.UnitTests.TestData;
using Xunit;

namespace HotelBooking.UnitTests;

public class CreateBookingTests
{

    private readonly ITestOutputHelper output;

    public CreateBookingTests(ITestOutputHelper output)
    {
        this.output = output;
    }
    
    
    //Case 12
    [Theory]
    [MemberData(
        nameof(BookingTestData.OneRoomBookedData), 
        MemberType = typeof(BookingTestData))]
    public async Task CreateBooking_PeriodIsDuplicateOfExistingBooking_ShouldReturnFalse(Booking bookings)
    {
        //Arrange
        List<Room> rooms = [
            new Room { Id = 1, Description = "Room 1" },
        ];
        var bookingManager = MoqBookingManager.CreateBookingManager(rooms, new List<Booking>([bookings]));

        Booking newBooking = new Booking
        {
            Id = 44,
            StartDate = BookingTestData.StartDateData,
            EndDate = BookingTestData.EndDateData,
            IsActive = true,
            RoomId = bookings.RoomId,
        };
        
        output.WriteLine(BookingTestData.StartDateData.ToString());
        output.WriteLine(BookingTestData.EndDateData.ToString());
        
        output.WriteLine(bookings.ToString());
        
        //Act
        var result = await bookingManager.CreateBooking(newBooking); 

        //Assert
        Assert.False(result);
    }
    
    
}