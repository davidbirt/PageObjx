using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageObjects.PageObjects
{
    public class StackDemoLoginPage : PageObject, IPageObject
    {
        public string Address => "https://bstackdemo.com/signin";
        public IWebElement btn_login => Driver.FindElement(By.Id("login-btn"));
        public IWebElement btn_username => Driver.FindElement(By.CssSelector("div.css-1hwfws3"));
        public IWebElement btn_defaultUser => Driver.FindElement(By.CssSelector("#react-select-2-option-0-0"));

        public IWebElement btn_password => Driver.FindElements(By.CssSelector("div.css-1hwfws3")).Last();
        public IWebElement btn_defaultpw => Driver.FindElement(By.CssSelector("#react-select-3-option-0-0"));

        public override bool AtCorrectLocation(string className = null)
        {
            return true;
        }

        public void Login()
        {
            SetDefaultUser();
            btn_login.Click();
        }

        public void SetDefaultUser() 
        {
            btn_username.Click();
            btn_defaultUser.Click();
            btn_password.Click();
            btn_defaultpw.Click();
        }

    }
}
