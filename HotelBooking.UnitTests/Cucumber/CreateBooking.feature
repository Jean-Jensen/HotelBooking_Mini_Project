Feature: CreateBooking
    
Creates a booking if start & end date are valid

    #Case 1
@mytag    
Scenario: Start and end date are after room is occupied
    Given start and end date are after room A is occupied
    When I try to book the room
    Then Booking should be created
    
    #Case 2
@mytag
Scenario: Start date is before and end date is after room is occupied
    Given Start date is before room A is occupied
    And end date is after room A is occupied
    When I try to book the room
    Then Booking should not be created

    #Case 3
@mytag
Scenario: Start and end date are before room is occupied
    Given start and end date are before room A is occupied
    When I try to book the room
    Then Booking should be created
    
    #Case 4
@mytag
Scenario: Start date is before occupied, end date is at start of occupied range
    Given Start date is before occupied range
    And End date is at the start of occupied range
    When I try to book the room
    Then Booking should not be created
    
    #Case 5
@mytag
Scenario: Start date is before occupied, end date is at end of occupied range
     Given Start date is before occupied range
     And End date is at the end of occupied range
     When I try to book the room
     Then Booking should not be created
     
    #Case 6
@mytag
Scenario: Start date is at the end of occupied range, end date is after the room is occupied
    Given Start date is at the end of occupied range
    And End date is after the room is occupied
    When I try to book the room
    Then Booking should not be created
    
    #Case 8
@mytag
Scenario: Start date is in the beginning of the occupied range, end date is at the end of occupied range
    Given Start date is in the beginning of occupied range
    And End date is at the end of occupied range
    When I try to book the room
    Then Booking should not be created

    #not a case, to showcase an error
@mytag
Scenario: Start date before today
    Given start date is before today
    When I try to book the room
    Then Booking should throw an error