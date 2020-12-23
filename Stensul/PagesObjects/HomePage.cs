using Stensul.Common;
using OpenQA.Selenium;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Stensul.PageObjects
{
    public class HomeLocators : UserActions
    {
        public HomeLocators(IWebDriver d) : base(d) { }
        public IWebElement ChosenFile() => element("#inputImage");
        public IWebElement TextField() => element("textarea[name='text']");
        public IWebElement CreateItem() => element("button[class='btn pull-right btn-success']");
        public IList<IWebElement> ListOfItem() => elements("ul[class='media-list ng-pristine ng-untouched ng-valid ng-isolate-scope ui-sortable'] li");
        public String ItemImage(IWebElement item) => item.FindElement(By.CssSelector("img[class='media-object img-responsive img-rounded']")).GetAttribute("src");
        public IWebElement EditItem(IWebElement item) => item.FindElements(By.CssSelector("button[class='btn btn-default']"))[0];
        public IWebElement DeleteItem(IWebElement item) => item.FindElements(By.CssSelector("button[class='btn btn-default']"))[1];
        public IWebElement UpdateItem() => element("button[class='btn pull-right btn-primary']");
        private int ButtonIndex() => elements("button[class='btn btn-default']").Count();
        public IWebElement Cancel() => elements("button[class='btn btn-default']")[ButtonIndex()-1];
        public IWebElement YesDeleteIt() => element("button[class='btn btn-primary']");
    }

    public class HomePage : HomeLocators
    {
        public HomePage(IWebDriver d) : base(d) { }

        /// <summary>
        /// Select the image, which is passed as a parameter
        /// </summary>
        /// <param name="imageName"></param>
        public void SelectImage(string imageName)
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            var image_path = dir.Substring(0, dir.LastIndexOf("Stensul")) + "Image\\"+imageName;
            this.ChosenFile().SendKeys(image_path);
        }
        /// <summary>
        /// Gives us the created item
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public IWebElement CreatedItem(string description)
        {
            var newItem = this.ListOfItem().Where(item => item.Text.Contains(description)).FirstOrDefault();
            return newItem;
        }

        

    }
}
