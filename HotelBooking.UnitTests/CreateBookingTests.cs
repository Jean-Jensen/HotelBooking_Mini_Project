using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using HotelBooking.UnitTests.TestData;
using Moq;
using Xunit;

namespace HotelBooking.UnitTests;

public class CreateBookingTests
{

    private readonly ITestOutputHelper output; //in case we need to output to console

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
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            IsActive = true,
            RoomId = booking.RoomId,
        };
        
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

    //Case 14
    [Theory]
    [MemberData(
        nameof(BookingTestData.OneRoomBookedData),
        MemberType = typeof(BookingTestData))]
    public async Task CreateBooking_PeriodIsBookedButOneRoomIsStillFree_ShouldReturnTrue(Booking booking)
    {
        //Arrange
        List<Room> rooms = [
            new Room { Id = 1, Description = "Room 1" },
            new Room { Id = 2, Description = "Room 2" },
        ];
        var bookingManager = MoqBookingManager.CreateBookingManager(rooms, new List<Booking>([booking]));

        Booking newBooking = new Booking
        {
            Id = 44,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            IsActive = true,
            RoomId = booking.RoomId,
        };
        
        //Act
        var result = await bookingManager.CreateBooking(newBooking); 

        //Assert
        Assert.True(result);
    }

    //Case 15
    [Theory]
    [MemberData(
        nameof(BookingTestData.AllRoomsOccupiedForSeveralDaysData),
        MemberType = typeof(BookingTestData))]
    public async Task CreateBooking_AllRoomsBookedForPeriod_ShouldReturnFalse(List<Booking> bookings)
    {
        //Arrange
        var rooms = BookingTestData.DefaultRooms;
        var bookingManager = MoqBookingManager.CreateBookingManager(rooms, bookings);
        
        Booking newBooking = new Booking
        {
            Id = 44,
            StartDate = BookingTestData.StartDateData.AddDays(3),
            EndDate = BookingTestData.StartDateData.AddDays(7),
            IsActive = true,
        };
        
        //Act
        var result = await bookingManager.CreateBooking(newBooking); 

        //Assert
        Assert.False(result);
        
    }

    //Case 16
    [Theory]
    [MemberData(
        nameof(BookingTestData.OneRoomBookedData),
        MemberType = typeof(BookingTestData))]
    public async Task CreateBooking_BookingARoomThatDoesntExist_ShouldReturnTrueAnyways(Booking booking)
    {
        List<Room> rooms = [
            new Room { Id = 1, Description = "Room 1" },
            new Room { Id = 2, Description = "Room 2" },
        ];
        var bookingManager = MoqBookingManager.CreateBookingManager(rooms, new List<Booking>([booking]));

        Booking newBooking = new Booking
        {
            Id = 44,
            StartDate = BookingTestData.StartDateData,
            EndDate = BookingTestData.EndDateData,
            IsActive = true,
            RoomId = 29803489,
        };
        
        //Act
        var result = await bookingManager.CreateBooking(newBooking); 

        //Assert
        Assert.True(result);
    }
    
    //Case 17
    [Theory]
    [MemberData(
        nameof(BookingTestData.OneRoomBookedData),
        MemberType = typeof(BookingTestData))]
    public async Task CreateBooking_StartDateIsExactlyOneMinuteAfterPreviousBookingEndDate_ShouldReturnTrue(Booking booking)
    {
        List<Room> rooms = [
            new Room { Id = 1, Description = "Room 1" },
        ];
        var bookingManager = MoqBookingManager.CreateBookingManager(rooms, new List<Booking>([booking]));

        Booking newBooking = new Booking
        {
            Id = 44,
            StartDate = booking.EndDate.AddMinutes(1),
            EndDate = booking.EndDate.AddDays(3),
            IsActive = true,
            RoomId = booking.RoomId,
        };
        
        //Act
        var result = await bookingManager.CreateBooking(newBooking); 

        //Assert
        Assert.True(result);
    }
    
    //Case 18
    [Theory]
    [MemberData(
        nameof(BookingTestData.OneRoomBookedData),
        MemberType = typeof(BookingTestData))]
    public async Task CreateBooking_EndDateIsExactlyOneMinuteBeforePreviousBookingStartDate_ShouldReturnTrue(Booking booking)
    {
        List<Room> rooms = [
            new Room { Id = 1, Description = "Room 1" },
        ];
        var bookingManager = MoqBookingManager.CreateBookingManager(rooms, new List<Booking>([booking]));

        Booking newBooking = new Booking
        {
            Id = 44,
            StartDate = booking.StartDate.AddHours(-12),
            EndDate = booking.StartDate.AddMinutes(-1),
            IsActive = true,
            RoomId = booking.RoomId,
        };
        
        //Act
        var result = await bookingManager.CreateBooking(newBooking); 

        //Assert
        Assert.True(result);
    }
    
    //Case 19
    [Fact]
    public async Task CreateBooking_OneDayBooking_ShouldReturnTrue()
    {
        List<Room> rooms = [
            new Room { Id = 1, Description = "Room 1" },
        ];
        var bookingManager = MoqBookingManager.CreateBookingManager(rooms, new List<Booking>());

        Booking newBooking = new Booking
        {
            Id = 44,
            StartDate = DateTime.Today.AddDays(2),
            EndDate = DateTime.Today.AddDays(2),
            IsActive = true,
            RoomId = 1,
        };
        
        //Act
        var result = await bookingManager.CreateBooking(newBooking); 

        //Assert
        Assert.True(result);
    }
    
    //Case 20
    [Fact]
    public async Task CreateBooking_ValidBooking_ShouldReturnTrue()
    {
        List<Room> rooms = [
            new Room { Id = 1, Description = "Room 1" },
        ];
        var bookingManager = MoqBookingManager.CreateBookingManager(rooms, new List<Booking>());
        

        Booking newBooking = new Booking
        {
            Id = 44,
            StartDate = DateTime.Today.AddDays(2),
            EndDate = DateTime.Today.AddDays(2),
            IsActive = true,
            RoomId = 1,
        };
        
        //Act
        var result = await bookingManager.CreateBooking(newBooking); 

        //Assert
        Assert.True(result);
        MoqBookingManager.GetBookingRepo().Verify(x => x.AddAsync(newBooking), Times.Once); //verify add was called
    }
    
    //Case 21
    [Theory]
    [MemberData(
        nameof(BookingTestData.OneRoomBookedData),
        MemberType = typeof(BookingTestData))]
    public async Task CreateBooking_InvalidBooking_ShouldReturnFalseAndAddShouldNotBeCalled(Booking booking)
    {
        List<Room> rooms = [
            new Room { Id = 1, Description = "Room 1" },
        ];
        var bookingManager = MoqBookingManager.CreateBookingManager(rooms, new List<Booking>([booking]));
        

        Booking newBooking = new Booking
        {
            Id = 44,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            IsActive = true,
            RoomId = booking.RoomId,
        };
        
        //Act
        var result = await bookingManager.CreateBooking(newBooking); 

        //Assert
        Assert.False(result);
        MoqBookingManager.GetBookingRepo().Verify(x => x.AddAsync(newBooking), Times.Never); //verify add was called
    }
}