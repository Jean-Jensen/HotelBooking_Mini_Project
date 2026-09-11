using System;
using System.Collections.Generic;
using HotelBooking.Core;
using Xunit;

namespace HotelBooking.UnitTests.TestData;

public static class FindAvailableRoomTestData
{
    // All dates used in the tests are relative to today.
    // This keeps the tests valid regardless of which day they are run.
    public static readonly DateTime BaseDate = DateTime.Today.AddDays(30);


    // ---------------------------------------------------------
    // Scenarios 1a, 1b and 2
    // ---------------------------------------------------------

    public static TheoryData<DateTime, DateTime> InvalidDateData =>
    [
        // 1a: Start date is in the past
        (
            DateTime.Today.AddDays(-1),
            DateTime.Today.AddDays(5)
        ),

        // 1b: Start date is today
        (
            DateTime.Today,
            DateTime.Today.AddDays(5)
        ),

        // 2: Start date is after end date
        (
            BaseDate.AddDays(10),
            BaseDate.AddDays(5)
        )
    ];


    // ---------------------------------------------------------
    // Scenarios 3-11
    // ---------------------------------------------------------

    public static TheoryData<
        string,
        List<Room>,
        List<Booking>,
        DateTime,
        DateTime,
        bool
    > AvailabilityData =>
    [
        // 3: No existing bookings
        (
            "Scenario 3 - No existing bookings",
            OneRoom(),
            new List<Booking>(),
            BaseDate,
            BaseDate.AddDays(5),
            true
        ),

        // 4: Requested period is completely BEFORE existing booking
        // Existing: June 5-8
        // Requested: June 2-4
        (
            "Scenario 4 - Requested period before existing booking",
            OneRoom(),
            new List<Booking>
            {
                CreateBooking(
                    1,
                    1,
                    BaseDate.AddDays(5),
                    BaseDate.AddDays(8))
            },
            BaseDate.AddDays(2),
            BaseDate.AddDays(4),
            true
        ),

        // 5: Requested period is completely AFTER existing booking
        // Existing: June 5-9
        // Requested: June 10-12
        (
            "Scenario 5 - Requested period after existing booking",
            OneRoom(),
            new List<Booking>
            {
                CreateBooking(
                    1,
                    1,
                    BaseDate.AddDays(5),
                    BaseDate.AddDays(9))
            },
            BaseDate.AddDays(10),
            BaseDate.AddDays(12),
            true
        ),

        // 6: Requested END touches existing START
        // Existing: June 5-9
        // Requested: June 2-5
        //
        // Because touching counts as a conflict,
        // this room is NOT available.
        (
            "Scenario 6 - Requested end touches existing start",
            OneRoom(),
            new List<Booking>
            {
                CreateBooking(
                    1,
                    1,
                    BaseDate.AddDays(5),
                    BaseDate.AddDays(9))
            },
            BaseDate.AddDays(2),
            BaseDate.AddDays(5),
            false
        ),

        // 7: Requested START touches existing END
        // Existing: June 6-10
        // Requested: June 10-14
        (
            "Scenario 7 - Requested start touches existing end",
            OneRoom(),
            new List<Booking>
            {
                CreateBooking(
                    1,
                    1,
                    BaseDate.AddDays(6),
                    BaseDate.AddDays(10))
            },
            BaseDate.AddDays(10),
            BaseDate.AddDays(14),
            false
        ),

        // 8: Requested period is completely INSIDE existing booking
        // Existing: June 1-20
        // Requested: June 5-10
        (
            "Scenario 8 - Requested period inside existing booking",
            OneRoom(),
            new List<Booking>
            {
                CreateBooking(
                    1,
                    1,
                    BaseDate,
                    BaseDate.AddDays(19))
            },
            BaseDate.AddDays(5),
            BaseDate.AddDays(10),
            false
        ),

        // 9: Requested period completely SURROUNDS existing booking
        // Existing: June 5-10
        // Requested: June 1-20
        (
            "Scenario 9 - Requested period surrounds existing booking",
            OneRoom(),
            new List<Booking>
            {
                CreateBooking(
                    1,
                    1,
                    BaseDate.AddDays(5),
                    BaseDate.AddDays(10))
            },
            BaseDate,
            BaseDate.AddDays(19),
            false
        ),

        // 10: Requested period overlaps ONLY THE START
        // Existing: June 10-20
        // Requested: June 5-12
        (
            "Scenario 10 - Requested period overlaps existing start",
            OneRoom(),
            new List<Booking>
            {
                CreateBooking(
                    1,
                    1,
                    BaseDate.AddDays(10),
                    BaseDate.AddDays(20))
            },
            BaseDate.AddDays(5),
            BaseDate.AddDays(12),
            false
        ),

        // 11: Requested period overlaps ONLY THE END
        // Existing: June 1-10
        // Requested: June 8-15
        (
            "Scenario 11 - Requested period overlaps existing end",
            OneRoom(),
            new List<Booking>
            {
                CreateBooking(
                    1,
                    1,
                    BaseDate,
                    BaseDate.AddDays(10))
            },
            BaseDate.AddDays(8),
            BaseDate.AddDays(15),
            false
        )
    ];


    // ---------------------------------------------------------
    // Helper methods
    // ---------------------------------------------------------

    public static List<Room> OneRoom()
    {
        return new List<Room>
        {
            new Room
            {
                Id = 1,
                Description = "Room 1"
            }
        };
    }


    private static Booking CreateBooking(
        int id,
        int roomId,
        DateTime startDate,
        DateTime endDate,
        bool isActive = true)
    {
        return new Booking
        {
            Id = id,
            StartDate = startDate,
            EndDate = endDate,
            IsActive = isActive,
            RoomId = roomId
        };
    }
}