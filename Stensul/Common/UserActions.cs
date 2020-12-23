using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Stensul.Common
{
    public class UserActions
    {
        public IWebDriver driver { get; set; }
        public UserActions(IWebDriver d = null)
        {
            driver = d;
        }
        public void Init()
        {
            driver = Browser.Chrome;
        }
        public Actions action()
        {
            return new Actions(driver);
        }

        /// <summary>
        /// This function can be used across all page object files
        /// in order to find elements; it uses By.Css selector as locator strategy.
        /// </summary>
        protected IWebElement element(string locator, int attempts = 30)
        {
            for (int i = 0; i < attempts; i++)
            {
                try
                {
                    return driver.FindElement(By.CssSelector(locator));
                }
                catch (Exception e)
                {
                    if (i < attempts - 1)
                    {
                        sleep();

                    }
                    else
                    {
                        if (e is NoSuchElementException)
                        {
                            throw new Exception("The element \"" + locator + "\" was not found.");
                        }
                        throw e;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Returns an array of web elements. If amountExpected parameter is specified
        /// the function will wait until that amount (or higher) of elements are found before 
        /// returning the list of web elements.
        /// </summary>
        protected IList<IWebElement> elements(string locator, int amountExpected = 0, int attempts = 30)
        {
            for (int i = 0; i < attempts; i++)
            {
                try
                {
                    var webElements = driver.FindElements(By.CssSelector(locator));
                    if (webElements.Count < amountExpected)
                    {
                        throw new Exception("The selector: \"" + locator + "\" returned less than " + amountExpected + " elements.");
                    }
                    return webElements;
                }
                catch (Exception e)
                {
                    if (i < attempts - 1)
                    {
                        sleep();
                    }
                    else
                    {
                        if (e is NoSuchElementException)
                        {
                            throw new Exception("The elements \"" + locator + "\" were not found.");
                        }
                        throw e;
                    }
                }
            }
            return new List<IWebElement>() { };
        }

        /// <summary>
        /// Stops the execution for the amount of seconds specified. By default it waits one second.
        /// </summary>
        public void sleep(int waitTime = 1) => Thread.Sleep(waitTime * 1000);

        /// <summary>
        /// Performs a click over an element
        /// </summary>
        protected void click(IWebElement element)
        {
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    element.Click();
                    break;
                }
                catch (Exception e)
                {
                    if (i < 9)
                    {
                        sleep();
                    }
                    else
                    {
                        if (e is ElementNotVisibleException)
                        {
                            throw new Exception("ElementNotVisibleException: The element is not visible.");
                        }
                        if (e is StaleElementReferenceException)
                        {
                            throw new Exception("StaleElementReferenceException: The DOM has changed before the element has been clicked.");
                        }
                        if (e.Message.Contains("Other element would receive the click"))
                        {
                            throw new Exception("The element could not be clicked because another one is blocking it.");
                        }
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// Executes the element finder function and then erases the text on the returned element, then inserts the new text.
        /// If forceClear is set to true, it will use alternative way to erase the data of
        /// the field.
        /// </summary>
        protected void write(string text, Func<IWebElement> fun, bool forceClear = false, int attempts = 0)
        {
            for (int i = 0; i < 10; i++)
            {
                var element = fun();
                try
                {
                    if (forceClear)
                    {
                        element.Click();
                        element.SendKeys(Keys.Control + "a");
                        element.SendKeys(Keys.Delete);
                    }
                    else
                    {
                        element.Clear();
                    }
                    element.SendKeys(text);
                    return;
                }
                catch (Exception e)
                {
                    if (i < 9)
                    {
                        sleep();
                    }
                    else
                    {
                        throw e;
                    }
                }
            }
        }

        
        
        [SetUp]
        public void Setup()
        {
            driver.Navigate().GoToUrl("http://​immense-hollows-74271.herokuapp.com​"); 
            
        }
        

        [TearDown]
        public void mainAfterEach()
        {
            if (TestContext.CurrentContext.Test.Properties.Get("Description") != null)
            {
                System.Console.WriteLine("Description: " + TestContext.CurrentContext.Test.Properties.Get("Description"));
            }
        }
    }
}
