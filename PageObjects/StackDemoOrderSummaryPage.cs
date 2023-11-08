using OpenQA.Selenium;
using PageObjects.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageObjects.PageObjects
{
    public class StackDemoOrderSummaryPage : PageObject, IPageObject
    {
        public string Address => throw new NotImplementedException();
        public IWebElement txt_FirstName => Driver.FindElement(By.Id("firstNameInput"));
        public IWebElement txt_LastName => Driver.FindElement(By.Id("lastNameInput"));
        public IWebElement txt_Address => Driver.FindElement(By.Id("addressLine1Input"));
        public IWebElement txt_StateProvince => Driver.FindElement(By.Id("provinceInput"));
        public IWebElement txt_ZpCode => Driver.FindElement(By.Id("postCodeInput"));
        public IWebElement btn_SubmitOrder => Driver.FindElement(By.Id("checkout-shipping-continue"));

        public string OrderTotalPrice => Driver.FindElement(By.CssSelector("span.cart-priceItem-value span")).Text;

        public override bool AtCorrectLocation(string className = null)
        {
            return Driver.WaitForCondition<bool>((d) =>
            {
                return d.FindElements(By.CssSelector("h3.optimizedCheckout-headingSecondary")).Any();
            }, 4);
        }
    }
}
