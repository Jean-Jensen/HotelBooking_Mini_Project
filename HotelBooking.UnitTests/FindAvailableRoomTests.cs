using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using HotelBooking.UnitTests.TestData;
using Xunit;

namespace HotelBooking.UnitTests;

public class FindAvailableRoomTests
{
    // ---------------------------------------------------------
    // Scenarios 1a, 1b and 2
    // ---------------------------------------------------------

    [Theory]
    [MemberData(
        nameof(FindAvailableRoomTestData.InvalidDateData),
        MemberType = typeof(FindAvailableRoomTestData))]
    public async Task FindAvailableRoom_InvalidDates_ShouldThrowArgumentException(
        DateTime startDate,
        DateTime endDate)
    {
        // Arrange
        var rooms = FindAvailableRoomTestData.OneRoom();

        var bookingManager = MoqBookingManager.CreateBookingManager(
            rooms,
            new List<Booking>());

        // Act
        Task result() =>
            bookingManager.FindAvailableRoom(startDate, endDate);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(result);
    }


    // ---------------------------------------------------------
    // Scenarios 3-11
    // ---------------------------------------------------------

    [Theory]
    [MemberData(
        nameof(FindAvailableRoomTestData.AvailabilityData),
        MemberType = typeof(FindAvailableRoomTestData))]
    public async Task FindAvailableRoom_BookingPeriod_ShouldReturnExpectedAvailability(
        string scenario,
        List<Room> rooms,
        List<Booking> bookings,
        DateTime startDate,
        DateTime endDate,
        bool expectedAvailable)
    {
        // Arrange
        var bookingManager = MoqBookingManager.CreateBookingManager(
            rooms,
            bookings);

        // Act
        int roomId = await bookingManager.FindAvailableRoom(
            startDate,
            endDate);

        // Assert
        if (expectedAvailable)
        {
            Assert.NotEqual(-1, roomId);
        }
        else
        {
            Assert.Equal(-1, roomId);
        }
    }


    // ---------------------------------------------------------
    // Scenarios 3-11 through CreateBooking
    // ---------------------------------------------------------

    [Theory]
    [MemberData(
        nameof(FindAvailableRoomTestData.AvailabilityData),
        MemberType = typeof(FindAvailableRoomTestData))]
    public async Task CreateBooking_BookingPeriod_ShouldReturnExpectedResult(
        string scenario,
        List<Room> rooms,
        List<Booking> bookings,
        DateTime startDate,
        DateTime endDate,
        bool expectedAvailable)
    {
        // Arrange
        var bookingManager = MoqBookingManager.CreateBookingManager(
            rooms,
            bookings);

        Booking newBooking = new Booking
        {
            Id = 100,
            StartDate = startDate,
            EndDate = endDate,
            IsActive = true
        };

        // Act
        bool result = await bookingManager.CreateBooking(newBooking);

        // Assert
        Assert.Equal(expectedAvailable, result);
    }
}