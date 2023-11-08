using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using PageObjects.PageObjects;
using PageObjects.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PageObjects.Tests
{
    [TestClass]
    public class StackDemoTransact : UITestBase
    {
        [TestMethod]
        [Description("Should Login.")]
        public async Task LoginTest()
        {
            Driver.Navigate().GoToUrl("https://bstackdemo.com/signin");
            Driver.WaitForScript<bool>("return document.readyState == 'complete'", 2);

            Driver.FindElement(By.CssSelector("div.css-1hwfws3")).Click();
            Driver.FindElement(By.CssSelector("#react-select-2-option-0-0")).Click();

            Driver.FindElements(By.CssSelector("div.css-1hwfws3")).Last().Click();
            Driver.FindElement(By.CssSelector("#react-select-3-option-0-0")).Click();
            Driver.FindElement(By.Id("login-btn")).Click();

            Driver.WaitForElement(By.CssSelector("div.shelf-container"), 5);
            Assert.IsTrue(Driver.FindElement(By.CssSelector("div.shelf-item")).Displayed, "Unable to see products on the shelf");
        }

        [TestMethod]
        [Description("Verify that products view has 25 visible products")]
        public async Task ProductsView()
        {
            Driver.Navigate().GoToUrl("https://bstackdemo.com/signin");
            Driver.WaitForScript<bool>("return document.readyState == 'complete'", 2);

            Driver.FindElement(By.CssSelector("div.css-1hwfws3")).Click();
            Driver.FindElement(By.CssSelector("#react-select-2-option-0-0")).Click();

            Driver.FindElements(By.CssSelector("div.css-1hwfws3")).Last().Click();
            Driver.FindElement(By.CssSelector("#react-select-3-option-0-0")).Click();
            Driver.FindElement(By.Id("login-btn")).Click();

            Driver.WaitForElement(By.CssSelector("div.shelf-container"), 5);
            Assert.IsTrue(Driver.FindElement(By.CssSelector("div.shelf-item")).Displayed, "Unable to see products on the shelf");

            Assert.IsTrue(Driver.FindElements(By.CssSelector("div.shelf-item")).Count == 25);
        }

        [TestMethod]
        [Description("Verify that products can be added to the cart")]
        public async Task CartItems()
        {
            Driver.Navigate().GoToUrl("https://bstackdemo.com/signin");
            Driver.WaitForScript<bool>("return document.readyState == 'complete'", 2);

            Driver.FindElement(By.CssSelector("div.css-1hwfws3")).Click();
            Driver.FindElement(By.CssSelector("#react-select-2-option-0-0")).Click();

            Driver.FindElements(By.CssSelector("div.css-1hwfws3")).Last().Click();
            Driver.FindElement(By.CssSelector("#react-select-3-option-0-0")).Click();
            Driver.FindElement(By.Id("login-btn")).Click();

            

            Driver.WaitForElements(By.CssSelector("div.shelf-container"), 5);

            GiveItASecond(3);

            Assert.IsTrue(Driver.FindElement(By.CssSelector("div.shelf-item")).Displayed, "Unable to see products on the shelf");

            Assert.IsTrue(Driver.FindElements(By.CssSelector("div.shelf-item")).Count == 25);

            Driver.FindElement(By.CssSelector("div.shelf-item")).FindElement(By.CssSelector("div.shelf-item__buy-btn")).Click();

            GiveItASecond(2);

            string itemPrice = Driver.FindElement(By.CssSelector("div.float-cart .shelf-item .shelf-item__price p")).Text;
            string totalPrice = Driver.FindElement(By.CssSelector("div.float-cart__footer .sub-price__val")).Text;

            Trace.WriteLine($"Item Price found to be :{itemPrice}");
            Trace.WriteLine($"Cart Price found to be :{totalPrice}");
            Assert.IsTrue(itemPrice.Replace("$", "").Trim().Equals(totalPrice.Replace("$", "").Trim()), "Cart price not setting propertly");
        }


        [TestMethod]
        [Description("Verify that products can be added to the cart and checkout")]
        public async Task CartCheckout()
        {
            Driver.Navigate().GoToUrl("https://bstackdemo.com/signin");
            Driver.WaitForScript<bool>("return document.readyState == 'complete'", 2);

            Driver.FindElement(By.CssSelector("div.css-1hwfws3")).Click();
            Driver.FindElement(By.CssSelector("#react-select-2-option-0-0")).Click();

            Driver.FindElements(By.CssSelector("div.css-1hwfws3")).Last().Click();
            Driver.FindElement(By.CssSelector("#react-select-3-option-0-0")).Click();
            Driver.FindElement(By.Id("login-btn")).Click();



            Driver.WaitForElements(By.CssSelector("div.shelf-container"), 5);

            GiveItASecond(3);

            Assert.IsTrue(Driver.FindElement(By.CssSelector("div.shelf-item")).Displayed, "Unable to see products on the shelf");

            Assert.IsTrue(Driver.FindElements(By.CssSelector("div.shelf-item")).Count == 25);

            Driver.FindElement(By.CssSelector("div.shelf-item")).FindElement(By.CssSelector("div.shelf-item__buy-btn")).Click();

            GiveItASecond(2);

            string itemPrice = Driver.FindElement(By.CssSelector("div.float-cart .shelf-item .shelf-item__price p")).Text;
            string totalPrice = Driver.FindElement(By.CssSelector("div.float-cart__footer .sub-price__val")).Text;

            Trace.WriteLine($"Item Price found to be :{itemPrice}");
            Trace.WriteLine($"Cart Price found to be :{totalPrice}");
            Assert.IsTrue(itemPrice.Replace("$", "").Trim().Equals(totalPrice.Replace("$", "").Trim()), "Cart price not setting propertly");

            Driver.FindElement(By.CssSelector("div.buy-btn")).Click();
            GiveItASecond(2);

            string orderSumPrice = Driver.FindElement(By.CssSelector("span.cart-priceItem-value span")).Text;
            Assert.IsTrue(orderSumPrice.Replace("$", "").Trim().Equals(totalPrice.Replace("$", "").Trim()), "Order Summary Price not reflecting cart selections total");
        }
    }


    [TestClass]
    public class StackDemoPOM : UITestBase
    {

        [TestMethod]
        [Description("Verifies we can login to the test site.")]
        public async Task LoginTest()
        {
            StackDemoLoginPage login = Navigate<StackDemoLoginPage>();
            login.Login();
        }

        [TestMethod]
        [Description("Verify that products view has 25 visible products")]
        public async Task ProductsView()
        {
            StackDemoLoginPage login = Navigate<StackDemoLoginPage>();
            login.SetDefaultUser();
            StackDemoProductsPage products = Navigate<StackDemoProductsPage>(login.btn_login);
            Assert.IsTrue(products.Products.Count() == 25, "Could not find popkulated products page.");
        }

        [TestMethod]
        [Description("Verify that products can be added to the cart")]
        public async Task CartItems()
        {
            StackDemoLoginPage login = Navigate<StackDemoLoginPage>();
            login.SetDefaultUser();
            StackDemoProductsPage products = Navigate<StackDemoProductsPage>(login.btn_login);
            Assert.IsTrue(products.Products.Count() == 25, "Could not find popkulated products page.");

            products.Products.First(p => { return p.ProductName.Equals("iPhone 11"); }).btn_AddToCart.Click();
            GiveItASecond(3);
            string itemPrice = products.Cart.Products.First().ProductPrice;
            string totalPrice = products.Cart.CartTotalPrice;
            Assert.IsTrue(itemPrice.Replace("$", "").Trim().Equals(totalPrice.Replace("$", "").Trim()), "Cart price not setting propertly");
        }

        [TestMethod]
        [Description("Verify that products can be added to the cart and checkout")]
        public async Task CartCheckout()
        {
            StackDemoLoginPage login = Navigate<StackDemoLoginPage>();
            login.SetDefaultUser();
            StackDemoProductsPage products = Navigate<StackDemoProductsPage>(login.btn_login);
            Assert.IsTrue(products.Products.Count() == 25, "Could not find popkulated products page.");

            products.Products.First(p => { return p.ProductName.Equals("iPhone 11"); }).btn_AddToCart.Click();
            GiveItASecond(3);
            string itemPrice = products.Cart.Products.First().ProductPrice;
            string totalPrice = products.Cart.CartTotalPrice;
            Assert.IsTrue(itemPrice.Replace("$", "").Trim().Equals(totalPrice.Replace("$", "").Trim()), "Cart price not setting propertly");

            StackDemoOrderSummaryPage summary = Navigate<StackDemoOrderSummaryPage>(products.Cart.btn_checkout);  
            Assert.IsTrue(summary.OrderTotalPrice.Replace("$", "").Trim().Equals(totalPrice.Replace("$", "").Trim()), "Order Summary Price not reflecting cart selections total");
        }
    }
}
