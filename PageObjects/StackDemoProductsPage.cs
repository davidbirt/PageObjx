using OpenQA.Selenium;
using PageObjects.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageObjects.PageObjects
{
    public class StackDemoProductsPage : PageObject, IPageObject
    {
        public string Address => throw new NotImplementedException();

        public List<StackDemoShelfItem> Products
        {
            get
            {
                var _listo = new List<StackDemoShelfItem>();
                var rows = Driver.FindElements(By.CssSelector(".shelf-container .shelf-item"));
                foreach (var row in rows)
                {
                    var rowToAdd = new StackDemoShelfItem(row);
                    _listo.Add(rowToAdd);
                }
                return _listo;
            }
        }

        StackDemoShoppingCart _cartToRtn;
        public StackDemoShoppingCart Cart
        { 
            get
            {
                if(_cartToRtn == null) _cartToRtn = new StackDemoShoppingCart(Driver.FindElement(By.CssSelector("div.float-cart")));
                return _cartToRtn;
            }
        }

        public override bool AtCorrectLocation(string className = null)
        {
            return Driver.WaitForCondition<bool>((d) =>
            {
                return d.FindElements(By.CssSelector(".shelf-container .shelf-item")).Any();
            }, 4);
        }
    }

    public class StackDemoShelfItem
    {
        IWebElement _parentElement;
        public StackDemoShelfItem(IWebElement parentElement)
        {
            _parentElement = parentElement;
        }

        public string ProductName => _parentElement.FindElement(By.CssSelector("p.shelf-item__title")).Text;
        public string ProductPrice => _parentElement.FindElement(By.CssSelector("div.shelf-item__price .val")).Text;
        public IWebElement btn_AddToCart => _parentElement.FindElement(By.CssSelector(".shelf-item__buy-btn"));
    }

    public class StackDemoShoppingCart
    {
        IWebElement _parentElement;
        public StackDemoShoppingCart(IWebElement parentElement)
        {
            _parentElement = parentElement;
        }

        public IWebElement btn_checkout => _parentElement.FindElement(By.CssSelector("div.buy-btn"));
        public string CartTotalPrice => _parentElement.FindElement(By.CssSelector(".float-cart .float-cart__footer .sub-price__val")).Text;

        public List<StackDemoShoppingCartItem> Products
        {
            get
            {
                var _listo = new List<StackDemoShoppingCartItem>();
                var rows = _parentElement.FindElements(By.CssSelector("div.shelf-item"));
                foreach (var row in rows)
                {
                    var rowToAdd = new StackDemoShoppingCartItem(row);
                    _listo.Add(rowToAdd);
                }
                return _listo;
            }
        }
    }

    public class StackDemoShoppingCartItem
    {
        IWebElement _parentElement;
        public StackDemoShoppingCartItem(IWebElement parentElement)
        {
            _parentElement = parentElement;
        }

        public string ProductName => _parentElement.FindElement(By.CssSelector(".shelf-item__details p.title")).Text;
        public string ProductPrice => _parentElement.FindElement(By.CssSelector(".shelf-item__price p")).Text;

    }
}
