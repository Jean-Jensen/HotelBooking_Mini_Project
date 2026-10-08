Feature: CreateBooking
    
Creates a booking if start & end date are valid

    
@mytag    
Scenario: Start and end date are after room is occupied
    Given start and end date are after room A is occupied
    When I try to book the room
    Then Booking should be created

@mytag
Scenario: Start and end date are before room is occupied
    Given start and end date are before room A is occupied
    When I try to book the room
    Then Booking should be created
    
@mytag
Scenario: Start date before today
    Given start date is before today
    When I try to book the room
    Then Booking should throw an error