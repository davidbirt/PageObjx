using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageObjects.Utils
{
    public static class WebDriverExtensions
    {
        /// <summary>
        /// Searches the dom for a given element until the timeout(s) has elapsed
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="timeout"> in seconds</param>
        /// <returns>IWebElement</returns>
        public static IWebElement WaitForElement(this IWebDriver driver, By condition, int timeout)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            return wait.Until<IWebElement>((d) => d.FindElement(condition));
        }

        /// <summary>
        /// Searches the dom for a given element until the timeout(s) has elapsed
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="timeout"> in seconds</param>
        /// <returns>IWebElement</returns>
        public static IEnumerable<IWebElement> WaitForElements(this IWebDriver driver, By condition, int timeout)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            return wait.Until<IEnumerable<IWebElement>>((d) => d.FindElements(condition));
        }

        /// <summary>
        /// Waits for a nested element to load. Good for dynamically populated dropdowns!
        /// DOESNT WORK UGH
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="condition"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static IWebElement WaitForDropdownLoad(this IWebDriver driver, By parentCondition, By condition, int timeout, string selector)
        {
            try
            {
                var parentElement = driver.FindElement(parentCondition);
                parentElement.Click();
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                var ddElements = parentElement.FindElements(condition);
                var results = wait.Until<IWebElement>((d) =>
                {
                    return ddElements.Where(el => el.Text.ToLower().Contains(selector)).First();
                });
                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// Runs a piece of Javascript until condition is met or timeout elapses
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="script"></param>
        /// <param name="timeout"> in seconds</param>
        /// <returns>IWebElement</returns>
        public static T WaitForScript<T>(this IWebDriver driver, string script, int timeout)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            return wait.Until((d) => (T)((IJavaScriptExecutor)d).ExecuteScript(script));
        }

        /// <summary>
        /// Runs a piece of Javascript until condition is met or timeout elapses
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="timeout"> in seconds</param>
        /// <returns>IWebElement</returns>
        public static T WaitForCondition<T>(this IWebDriver driver, Func<IWebDriver, T> evaluatorFunc, int timeout)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            return wait.Until(evaluatorFunc);
        }

        //public static T ExecuteSCript(this IWebDriver driver)
    }
}
