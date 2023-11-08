using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Interactions;
using PageObjects.PageObjects;
using PageObjects.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageObjects.Tests
{
    [TestClass]
    public class UITestBase
    {
        private IWebDriver _driver;
        public Autofac.IContainer _container;
        public TestContext TestContext { get; set; }
        public IWebDriver Driver
        {
            get
            {
                try
                {
                    if (_driver == null)
                    {
                        //var options = new ChromeOptions();
                        //options.AddArgument("--start-maximized");
                        //options.AddArgument("--no-sandbox");
                        //options.AddArgument("--disable-dev-shm-usage");
                        //_driver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory, options);

                        _driver = new EdgeDriver(AppDomain.CurrentDomain.BaseDirectory);
                    }
                    return _driver;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        public T Navigate<T>()
            where T : IPageObject
        {
            // Instanciate the page
            //var page = Activator.CreateInstance<T>();
            var page = _container.Resolve<T>();

            // Navigate to page 
            Driver.Navigate().GoToUrl(page.Address);

            // Ensure the view loads completely
            Driver.WaitForScript<bool>("return document.readyState == 'complete'", 2);

            // Give the page a driver to do its thing
            page.Driver = Driver;

            // Ensure the page loads completely
            page.AtCorrectLocation();

            // Give the test this page back to do its thing
            return page;
        }

        public T Navigate<T>(string addressParams)
            where T : IPageObject
        {
            // Instanciate the page
            //var page = Activator.CreateInstance<T>();
            var page = _container.Resolve<T>();

            // Navigate to page 
            Driver.Navigate().GoToUrl($"{page.Address}/{addressParams}");

            // Ensure the view loads completely
            Driver.WaitForScript<bool>("return document.readyState == 'complete'", 30);

            // Give the page a driver to do its thing
            page.Driver = Driver;

            // Ensure the page loads completely
            page.AtCorrectLocation();

            // Give the test this page back to do its thing
            return page;
        }

        public T Navigate<T>(IWebElement navButton)
            where T : IPageObject
        {
            // Navigate to page 
            navButton.Click();
            Thread.Sleep(1000);

            // Instanciate the page
            //var page = Activator.CreateInstance<T>();
            var page = _container.Resolve<T>();
            Driver.SwitchTo().Window(Driver.WindowHandles.Last());
            // Give the page a driver to do its thing
            page.Driver = Driver;

            // Ensure the page loads completely
            page.AtCorrectLocation();

            // Give the test this page back to do its thing
            return page;
        }

        public T NavigateToNewTab<T>(IWebElement navButton)
            where T : IPageObject
        {
            // Navigate to page 
            navButton.Click();
            Thread.Sleep(1000);
            Driver.SwitchTo().Window(Driver.WindowHandles.Last());
            // Instanciate the page
            //var page = Activator.CreateInstance<T>();
            var page = _container.Resolve<T>();

            // Give the page a driver to do its thing
            page.Driver = Driver;

            // Ensure the page loads completely
            page.AtCorrectLocation();

            // Give the test this page back to do its thing
            return page;
        }

        /// <summary>
        /// This call assumes you have multiple window handles
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T NavigateBackToTab<T>()
            where T : IPageObject
        {
            Driver.Close();

            Driver.SwitchTo().Window(Driver.WindowHandles.Last());
            // Instanciate the page
            //var page = Activator.CreateInstance<T>();
            var page = _container.Resolve<T>();

            // Give the page a driver to do its thing
            page.Driver = Driver;

            // Ensure the page loads completely
            page.AtCorrectLocation();

            // Give the test this page back to do its thing
            return page;

        }

        /// <summary>
        /// Sets up the client config and logger with autoMock so you can mock services in your test. if there is a shared resource you are mocking across alot of tests.. please put the init code here.
        /// </summary>
        /// <returns></returns>
        [TestInitialize]
        public async virtual Task BeforeTest()
        {
            try
            {
                Trace.WriteLine("Starting Test - " + TestContext.TestName);
                ContainerBuilder builder = new ContainerBuilder();
                builder.RegisterType<StackDemoLoginPage>().AsSelf();
                builder.RegisterType<StackDemoProductsPage>().AsSelf();
                builder.RegisterType<StackDemoOrderSummaryPage>().AsSelf();
                _container = builder.Build();
                await Task.FromResult(true);
            }
            catch (Exception)
            {    
                throw;
            }
        }

        [TestCleanup]
        public async virtual Task AfterTest()
        {
            try
            {
                if (TestContext.CurrentTestOutcome == UnitTestOutcome.Failed) TakeScreenshot(@$"C:\TestResults\{TestContext.TestName}.png");
                Trace.WriteLine("Test OutCome : " + TestContext.CurrentTestOutcome);
                Trace.WriteLine("Killing the driver");
                _driver.Quit();
            }
            catch (Exception ex)
            {
                _driver.Dispose();
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);
            }
            finally
            {
                Process[] chromeDriverProcesses = Process.GetProcessesByName("chrome");

                foreach (var chromeDriverProcess in chromeDriverProcesses)
                {
                    Trace.WriteLine("Killing individual chrome process");
                    chromeDriverProcess.Kill();
                }
            }
        }

        internal bool GiveItASecond(int secondsToWait)
        {
            new Actions(Driver).Pause(TimeSpan.FromSeconds(secondsToWait)).Perform();
            return true;
        }


        internal void TakeScreenshot(string whereToSave)
        {
            var shot = ((ITakesScreenshot)Driver).GetScreenshot();
            shot.SaveAsFile(whereToSave);
        }
    }
}
