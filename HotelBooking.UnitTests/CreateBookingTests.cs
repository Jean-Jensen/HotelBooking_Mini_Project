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
    public async Task CreateBooking_PeriodIsDuplicateOfExistingBooking_ShouldReturnFalse(Booking booking)
    {
        //Arrange
        List<Room> rooms = [
            new Room { Id = 1, Description = "Room 1" },
        ];
        var bookingManager = MoqBookingManager.CreateBookingManager(rooms, new List<Booking>([booking]));

        Booking newBooking = new Booking
        {
            Id = 44,
            StartDate = BookingTestData.StartDateData,
            EndDate = BookingTestData.EndDateData,
            IsActive = true,
            RoomId = booking.RoomId,
        };
        
        /*
        output.WriteLine(BookingTestData.StartDateData.ToString());
        output.WriteLine(BookingTestData.EndDateData.ToString());
        output.WriteLine(booking.ToString());
        */
        
        //Act
        var result = await bookingManager.CreateBooking(newBooking); 

        //Assert
        Assert.False(result);
    }

    //Case 13
    [Theory]
    [MemberData(
        nameof(BookingTestData.InactiveData),
        MemberType = typeof(BookingTestData))]
    public async Task CreateBooking_PeriodOverlapsPreviousBookingButPreviousBookingIsInactive_ShouldReturnTrue(Booking booking)
    {
        //Arrange
        List<Room> rooms = [
            new Room { Id = 1, Description = "Room 1" },
        ];
        var bookingManager = MoqBookingManager.CreateBookingManager(rooms, new List<Booking>([booking]));

        Booking newBooking = new Booking
        {
            Id = 44,
            StartDate = BookingTestData.StartDateData,
            EndDate = booking.StartDate.AddDays(2),
            IsActive = true,
            RoomId = booking.RoomId,
        };
        
        //Act
        var result = await bookingManager.CreateBooking(newBooking); 

        //Assert
        Assert.True(result);
        
    }


}