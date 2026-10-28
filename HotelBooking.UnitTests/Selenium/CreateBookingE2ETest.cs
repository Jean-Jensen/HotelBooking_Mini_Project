using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace HotelBooking.UnitTests.Selenium;

public class CreateBookingE2ETest : IDisposable
{
    private readonly ITestOutputHelper output; //in case we need to output to console
    
    public readonly IWebDriver driver;

    private string createURL = "http://localhost:5000/Bookings/Create";
    private string bookingsURL = "http://localhost:5000/Bookings";
    private string roomsURL = "http://localhost:5000/Rooms";

    private readonly By createBtnSelector = By.XPath("//input[@value='Create']");
    private readonly By bookingRowSelector = By.CssSelector(".other-table tbody tr");
    private readonly By startDateInRowSelector = By.CssSelector(" td:First-Child");

    private WebDriverWait wait;

    public CreateBookingE2ETest(ITestOutputHelper output)
    {
        this.output = output;
        
        ChromeOptions options = new ChromeOptions();
        options.AddArgument("headless");

        driver = new ChromeDriver(Environment.CurrentDirectory, options);
        
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
    }
    
    public void Dispose()
    {
        driver.Quit();
    }
    
    [Fact]
    public void CreateBookingSuccessfully()
    {
        var startDate = DateTime.Today.AddDays(1);
        
        driver.Navigate().GoToUrl(createURL);
        
        //Select from drop box/form
        //technically not necessary but could be useful
        //not even sure if code works rn
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
        var startDateField = driver.FindElement(By.Id("StartDate")); //grab element
        startDateField.Click(); //click on textfield
        string dateToSend = startDate.Day.ToString() + startDate.Month.ToString() + startDate.Year.ToString();
        startDateField.SendKeys("29102026"); //enter date (I could not find how to use the calendar)
        

        //Enter End Date
        //driver.FindElement(By.Id("EndDate")).Click(); //click on calender
        //driver.FindElement(By.LinkText(startDate.AddDays(1).Day.ToString())).Click(); //click on specific date

        var endDateField = driver.FindElement(By.Id("EndDate"));
        endDateField.Click();
        dateToSend = startDate.Day.ToString() + startDate.Month.ToString() + startDate.Year.ToString();
        endDateField.SendKeys("30102026");

        driver.FindElement(createBtnSelector).Click();
        
        driver.Navigate().GoToUrl(bookingsURL);

        var bookingRows = driver.FindElements(bookingRowSelector);
        
        wait.Until(d => d.FindElements(bookingRowSelector).Count > 0);

        string bookingStartDate = "";

        foreach (var row in bookingRows)
        {
            var text = row.FindElement(startDateInRowSelector).Text;

            //var columns = row.FindElements(By.TagName(".other-table td"));
            
            output.WriteLine("StartDate: " + text);
            //output.WriteLine("test: " + columns[0].Text);
            
            if (text == startDate.ToString())
            {
                bookingStartDate = text;
            }
        }
        
        Assert.Equal(startDate.ToString(), bookingStartDate);

    }
    
}