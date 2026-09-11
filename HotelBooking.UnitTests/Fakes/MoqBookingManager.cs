
using System;
using System.Collections.Generic;
using HotelBooking.Core;
using Moq;

namespace HotelBooking.UnitTests.Fakes;

public static class MoqBookingManager
{
    
    public static BookingManager CreateBookingManager(
        IEnumerable<Room> rooms,
        IEnumerable<Booking> bookings)
    {
        // Create mocked repositories
        var roomRepository = new Mock<IRepository<Room>>();
        var bookingRepository = new Mock<IRepository<Booking>>();

        // Configure GetAllAsync()
        roomRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(rooms);
        bookingRepository.Setup(b => b.GetAllAsync()).ReturnsAsync(bookings);
        
        // Return BookingManager
        return new BookingManager(bookingRepository.Object, roomRepository.Object);
    }
}