using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace HotelBooking.UnitTests.Selenium;

//fyi: you need to have the program running to do the tests, because selenium tries to go to the URL
//and if the program isn't running, it'll just see a 404 and return an error.
//that means if you change anything in the main program, you have to restart it before you see your changes on the tests.
//Also, the data that gets added here is actually put into the database. since it's an in-memory db, you can restart the program to reset it.
public class CreateBookingE2ETest : IDisposable
{
    private readonly ITestOutputHelper output; //in case we need to output to console
    
    public readonly IWebDriver driver;

    //URLs
    private string createURL = "http://localhost:5000/Bookings/Create";
    private string bookingsURL = "http://localhost:5000/Bookings";
    private string roomsURL = "http://localhost:5000/Rooms";

    //selectors 
    private readonly By startDateSelector = By.Id("StartDate");
    private readonly By endDateSelector = By.Id("EndDate");
    private readonly By createBtnSelector = By.XPath("//input[@value='Create']");
    private readonly By bookingRowSelector = By.CssSelector(".other-table tbody tr"); //I had to change the class of the table to differentiate it from the other table in this view
    private readonly By tableDataSelector = By.CssSelector("td"); //grabs all tabledata objects
    private readonly By errorTextSelector = By.CssSelector("div");
    
    //dates
    DateTime occupiedStartDate = DateTime.Today.AddDays(4);
    DateTime occupiedEndDate = DateTime.Today.AddDays(18);
    
    private WebDriverWait wait;

    //setup
    public CreateBookingE2ETest(ITestOutputHelper output)
    {
        this.output = output; 
        
        ChromeOptions options = new ChromeOptions();
        options.AddArgument("headless"); //so window isnt displayed

        driver = new ChromeDriver(Environment.CurrentDirectory, options);
        
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
    }
    
    //called after tests are done running
    public void Dispose()
    {
        driver.Quit();
    }

    private void CreateBookingOnWeb(DateTime startDate, DateTime endDate)
    {
        //Select customer from drop box/form
        var form = driver.FindElement(By.Id("CustomerId"));
        form.Click();
        foreach (var option in form.FindElements(By.TagName("option")))
        {
            if (option.Text == "Jane Doe")
            {
                option.Click();
            }
        }
        
        //Enter Start Date
        var startDateField = driver.FindElement(startDateSelector);
        startDateField.Click(); //click on textfield
        //this isn't what we want the textfield to look like, it's what we want selenium to type in order.
        //so in the actual program, we would just type the day, month and year, so here we do the same
        string dateToSend = startDate.Day.ToString() + startDate.Month.ToString() + startDate.Year.ToString(); 
        startDateField.SendKeys(dateToSend); //enter date (I could not find how to use the calendar)

        //Enter End Date
        var endDateField = driver.FindElement(endDateSelector);
        endDateField.Click();
        dateToSend = endDate.Day.ToString() + endDate.Month.ToString() + endDate.Year.ToString();
        endDateField.SendKeys(dateToSend); 
        
        driver.FindElement(createBtnSelector).Click(); //click the create button
    }
    
    
    //baseline test, so we know things are working correctly
    //could also be considered case #3
    [Fact]
    public void CreateBookingSuccessfully()
    {
        //Arrange
        var startDate = DateTime.Today.AddDays(1);
        
        //Act
        driver.Navigate().GoToUrl(createURL);
        
        CreateBookingOnWeb(startDate, startDate.AddDays(1));
        
        ////// switch to booking page ///////
        driver.Navigate().GoToUrl(bookingsURL); 

        var bookingRows = driver.FindElements(bookingRowSelector); //get booking rows
        
        wait.Until(d => d.FindElements(bookingRowSelector).Count > 3); //wait until there are enough rows (or throw error)

        string bookingStartDate = "";
        string bookingEndDate = "";
        string customer = "";

        foreach (var row in bookingRows)
        {
            var columns = row.FindElements(tableDataSelector); //grab all td/columns for this specific row
            
            output.WriteLine("StartDate: " + columns[0].Text);
            output.WriteLine("test: " + columns[1].Text);
            
            //if data matches our booking
            if (columns[0].Text == startDate.ToString() 
                && columns[1].Text == startDate.AddDays(1).ToString()
                && columns[3].Text == "Jane Doe")
            {
                bookingStartDate = columns[0].Text;
                bookingEndDate = columns[1].Text;
                customer = columns[3].Text;
            }
        }
        
        //Assert
        Assert.Equal(startDate.ToString(), bookingStartDate);
        Assert.Equal(startDate.AddDays(1).ToString(), bookingEndDate);
        Assert.Equal("Jane Doe", customer);

    }

    [Fact]
    public void CreateBooking_StartDateAtStartOfOccupiedPeriod_ShouldNotCreateBooking()
    {
        //Arrange
        
        
        //Act
        driver.Navigate().GoToUrl(createURL);
        
        CreateBookingOnWeb(occupiedStartDate, occupiedEndDate.AddDays(6));

        Thread.Sleep(1000);        
        
        string errorText = driver.FindElement(errorTextSelector).Text;
        
        output.WriteLine(errorText);
        
        //Assert
        Assert.NotNull(errorText);
        Assert.NotEmpty(errorText);
        Assert.Equal("ArgumentException: The start date cannot be in the past or later than the end date.", errorText);
    }


}