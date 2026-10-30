using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Reqnroll;
using Xunit;

namespace HotelBooking.UnitTests.Cucumber.StepDefinitions;

[Binding]
public class CreateBookingStepDefinitions
{
    private DateTime startDate;
    private DateTime endDate;
    private int targetRoomId;
    private BookingManager manager = MoqBookingManager.CreateBookingManager(new List<Room>(), new List<Booking>());
    private Task<bool> _act { get; set; }

    private void SetupManager()
    {
        Room room1 = new Room()
        {
            Id = 1,
            Description = "very fancy"
        };
        Booking booking1 = new Booking
        {
            Id = 1,
            StartDate = DateTime.Today.AddDays(7),
            EndDate = DateTime.Today.AddDays(10),
            IsActive = true,
            CustomerId = 0,
            RoomId = 1,
        };
        manager = MoqBookingManager.CreateBookingManager(new List<Room>{room1}, new List<Booking>{booking1});
    }
    
    [Given("start and end date are before room A is occupied")]
    public void GivenStartAndEndDateAreBeforeRoomAIsOccupied()
    {
        SetupManager();
        
        startDate = DateTime.Today.AddDays(2);
        endDate = DateTime.Today.AddDays(5);
        targetRoomId = 1;
    }


    [Given("start and end date are after room A is occupied")]
    public void GivenStartAndEndDateAreAfterRoomAIsOccupied()
    {
        Room room1 = new Room()
        {
            Id = 1,
            Description = "very fancy"
        };
        Booking booking1 = new Booking
        {
            Id = 1,
            StartDate = DateTime.Today.AddDays(1),
            EndDate = DateTime.Today.AddDays(3),
            IsActive = true,
            CustomerId = 0,
            RoomId = 1,
        };
        manager = MoqBookingManager.CreateBookingManager(new List<Room>{room1}, new List<Booking>{booking1});

        startDate = DateTime.Today.AddDays(4);
        endDate = DateTime.Today.AddDays(6);
        targetRoomId = 1;

    }
    
    [When("I try to book the room")]
    public async Task WhenITryToBookTheRoom()
    {
        Booking newBooking = new Booking
        {
            Id = 38,
            StartDate = startDate,
            EndDate = endDate,
            IsActive = true,
            CustomerId = 0,
            RoomId = targetRoomId
        };

        //CreateBooking returns a bool, so we could just get the result in this method and check it in the next.
        //but this doesn't account for if it throws an error, which we want the next step to know about
        _act = manager.CreateBooking(newBooking);
    }

    [Then("Booking should be created")]
    public async Task ThenBookingShouldBeCreated()
    {
        bool result = await _act;
        
        Assert.True(result);
    }

    [Given("start date is before today")]
    public void GivenStartDateIsBeforeToday()
    {
        Room room1 = new Room()
        {
            Id = 1,
            Description = "very fancy"
        };
        manager = MoqBookingManager.CreateBookingManager(new List<Room>{room1}, new List<Booking>());

        startDate = DateTime.Today.AddDays(-1);
        endDate = DateTime.Today.AddDays(6);
        targetRoomId = 1;
    }

    [Then("Booking should throw an error")]
    public async Task ThenBookingShouldThrowAnError()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _act);
    }

    [Given("Start date is before occupied range")]
    public void GivenStartDateIsBeforeOccupiedRange()
    {
        SetupManager();
        
        startDate = DateTime.Today.AddDays(1);
        targetRoomId = 1;
    }

    [Given("End date is at the start of occupied range")]
    public void GivenEndDateIsAtTheStartOfOccupiedRange()
    {
        endDate = DateTime.Today.AddDays(7);
    }

    [Then("Booking should not be created")]
    public async Task ThenBookingShouldNotBeCreated()
    {
        bool result = await _act;
        
        Assert.False(result);
    }

    [Given("End date is at the end of occupied range")]
    public void GivenEndDateIsAtTheEndOfOccupiedRange()
    {
        endDate = DateTime.Today.AddDays(10);
    }

    [Given("Start date is before room A is occupied")]
    public void GivenStartDateIsBeforeRoomAIsOccupied()
    {
        SetupManager();
        
        startDate = DateTime.Today.AddDays(2);
        targetRoomId = 1;
    }

    [Given("end date is after room A is occupied")]
    public void GivenEndDateIsAfterRoomAIsOccupied()
    {
        endDate = DateTime.Today.AddDays(13);
    }

    [Given("{int} is at the start or end of occupied range")]
    public void GivenIsAtTheStartOrEndOfOccupiedRange(int startDates)
    {
        startDate = DateTime.Today.AddDays(startDates);
    }

    [Given("{int} is after the room is occupied")]
    public void GivenIsAfterTheRoomIsOccupied(int endDates)
    {
        endDate = DateTime.Today.AddDays(endDates);
    }

    [Given("{int} is at the end of occupied range")]
    public void GivenIsAtTheEndOfOccupiedRange(int endDates)
    {
        endDate = DateTime.Today.AddDays(endDates);
    }
}