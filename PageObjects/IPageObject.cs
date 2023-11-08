using OpenQA.Selenium.Remote;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PageObjects.Utils;

namespace PageObjects.PageObjects
{
    public interface IDialogObject
    {
        IWebElement DialogElement { get; set; }
        IWebDriver Driver { get; set; }
    }

    /// <summary>
	/// That Pageeeee object
	/// </summary>
	public interface IPageObject
    {
        string Address { get; }
        IWebDriver Driver { get; set; }
        bool AtCorrectLocation(string className = null);
    }

    // Fetches elements from the page and builds components for the test
    public abstract class PageObject
    {
        public PageObject()
        {
        }

        public PageObject(IWebElement element)
        {
        }

        public PageObject(IWebDriver driver)
        {
            Driver = driver;
        }

        public IWebDriver Driver { get; set; }

        //Methods
        public bool PageInErrorState()
        {
            bool loaded = Driver.WaitForCondition<bool>((d) =>
            {
                return d.FindElements(By.ClassName("alert-danger")).Any(e => e.Displayed);
            }, 10);
            return loaded;
        }

     
        public virtual bool AtCorrectLocation(string className = null)
        {
            Trace.WriteLine("Waiting for page to load");
            bool loaded = Driver.WaitForCondition<bool>((d) =>
            {
                return d.FindElements(By.ClassName(className)).Any(e => e.Displayed);
            }, 30);
            return loaded;
        }

        public virtual T ExecuteScript<T>(string script)
        {
            try
            {
                return (T)((RemoteWebDriver)Driver).ExecuteScript(script);
            }
            catch (System.Exception ex)
            {
                Trace.WriteLine("ERROR: " + ex.Message);
                return default(T);
            }
        }

        /// <summary>
        /// Waits up to 8 seconds for a question to load and its text to be dislpayed. 
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public virtual bool TextDisplayed(string searchText)
        {
            var selection = Driver.FindElement(By.TagName("body"));
            bool loaded = Driver.WaitForCondition<bool>((d) =>
            {
                return selection.Text.Contains(searchText);
            }, 8);
            return loaded;
        }

        public virtual bool ErrorText()
        {
            //this method specifically checks for Toastr error messages in the application.
            bool _errortext = Driver.WaitForCondition<bool>((d) =>
            {
                return d.FindElements(By.ClassName("toast-error")).Any(e => e.Displayed);
            }, 5);
            return _errortext;
        }

        public bool SuccessText()
        {
            //this method specifically checks for Toastr success messages in the application.
            bool _successtext = Driver.WaitForCondition<bool>((d) =>
            {
                return d.FindElements(By.ClassName("toast-success")).Any(e => e.Displayed);
            }, 5);
            return _successtext;
        }
    }
}
