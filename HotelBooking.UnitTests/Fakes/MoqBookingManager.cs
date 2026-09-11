
using System;
using System.Collections.Generic;
using HotelBooking.Core;
using Moq;

namespace HotelBooking.UnitTests.Fakes;

public static class MoqBookingManager
{
    
    // Create mocked repositories
    private static Mock<IRepository<Room>> roomRepository;
    private static Mock<IRepository<Booking>> bookingRepository;
    
    public static BookingManager CreateBookingManager(
        IEnumerable<Room> rooms,
        IEnumerable<Booking> bookings)
    {
        // Create mocked repositories
        roomRepository = new Mock<IRepository<Room>>();
        bookingRepository = new Mock<IRepository<Booking>>();

        // Configure GetAllAsync()
        roomRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(rooms);
        bookingRepository.Setup(b => b.GetAllAsync()).ReturnsAsync(bookings);
        
        // Return BookingManager
        return new BookingManager(bookingRepository.Object, roomRepository.Object);
    }

    public static Mock<IRepository<Booking>> GetBookingRepo()
    {
        return bookingRepository;
    }
    
}