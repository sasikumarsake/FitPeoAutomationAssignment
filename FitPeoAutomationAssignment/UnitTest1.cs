using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace FitPeoAutomationAssignment
{
    [TestClass]
    public class FitPeoTests
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        [TestInitialize]
        public void SetUp()
        {
            _driver = new ChromeDriver(); // Set up Chrome WebDriver
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10)); // WebDriverWait for synchronization
            _driver.Manage().Window.Maximize(); // Maximize window for test
        }

        [TestMethod]
        public void TestFitPeo()
        {
            // Setup
            string url = "https://www.fitpeo.com/";
            string sliderXpath = "/html/body/div[2]/div[1]/div[1]/div[2]/div/div/span[1]/span[3]";
            int codeSnippets = 560;
            string headerLocator = "//p[text()='Total Recurring Reimbursement for all Patients Per Month:']/child::p";
            string expectedValue = "110700";


            try
            {
                // Navigate to FitPeo homepage
                _driver.Navigate().GoToUrl(url);

                // Navigate to Revenue Calculator page
                IWebElement revenueCalculatorLink = _driver.FindElement(By.XPath("//a[@href='/revenue-calculator']"));
                revenueCalculatorLink.Click();

                // Scroll down to the Slider section
                ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, 200);");
                Thread.Sleep(1000); // Wait for page to settle


                // Adjust the slider
                IWebElement slider = _driver.FindElement(By.XPath("//input[@aria-orientation='horizontal']"));
                for (int i = 0; i < 620; i++) // Adjust slider using ARROW_RIGHT key
                {
                    slider.SendKeys(Keys.ArrowRight);
                }

                // Update the text field with code snippet value
                IWebElement inputField = _driver.FindElement(By.XPath("//input[@id=':r0:']"));
                inputField.SendKeys(Keys.Control + "a"); // Select all
                inputField.SendKeys(Keys.Backspace); // Clear field
                inputField.SendKeys(codeSnippets.ToString()); // Set the value


                // Assert the value entered into the input field
                string enteredValue = inputField.GetAttribute("value");
                Assert.AreEqual(codeSnippets.ToString(), enteredValue, $"Expected value {codeSnippets}, but found {enteredValue}");

                Console.WriteLine($"Value {codeSnippets} entered successfully into the text field.");


                // Scroll further down to ensure all elements are loaded
                ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, 1000);");
                Thread.Sleep(1000);

                // Update the text field again
                inputField.SendKeys(Keys.Control + "a");
                inputField.SendKeys(Keys.Backspace);
                inputField.SendKeys("820");

                // Select CPT codes
                IWebElement cptCode57 = _driver.FindElement(By.XPath("//span[text()='57']"));
                IWebElement cptCode1919 = _driver.FindElement(By.XPath("//span[text()='19.19']"));
                IWebElement cptCode63 = _driver.FindElement(By.XPath("//span[text()='63']"));
                IWebElement cptCode15 = _driver.FindElement(By.XPath("//span[text()='15']"));

                cptCode57.Click();
                cptCode1919.Click();
                cptCode63.Click();
                cptCode15.Click();

                // Validate the Total Recurring Reimbursement
                IWebElement headerElement = _driver.FindElement(By.XPath(headerLocator));
                string headerText = headerElement.Text;
                Assert.IsTrue(headerText.Contains(expectedValue), $"Expected value {expectedValue} not found in header. Found: {headerText}");

                Console.WriteLine($"Total Recurring Reimbursement validated successfully. Found value: {headerText}");
            }
            catch (Exception ex)
            {
                Assert.Fail("Test failed due to an exception: " + ex.Message);
            }

        }




        [TestCleanup]
        public void TearDown()
        {
            // Close the browser after the test
            Thread.Sleep(5000); // Wait for a bit before closing
            _driver.Quit();
        }
    }
}
