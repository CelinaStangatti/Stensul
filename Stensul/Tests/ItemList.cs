using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using Stensul.Common;
using Stensul.PageObjects;



namespace Stensul.Tests
{
    public class ItemList : UserActions
    {
        HomePage onHomePage;
        string pictureDescription;

        [OneTimeSetUp]
        public new void Setup()
        {
            Init();
            onHomePage = new HomePage(driver);
            pictureDescription = "Bicicleteando hacia poblet";
        }

        [Test,Order(1)]
        [Description("Test Case 1: Clicking the 'Create Item button' the new item is added on the list of items")]
        public void _TestCase1()
        {
            string pictureName = "bicicleteando.jpg";
            
            onHomePage.SelectImage(pictureName);
            write(pictureDescription, onHomePage.TextField);
            click(onHomePage.CreateItem());
            sleep(2);
            //Check if the image and description are found in the items list
            var item = onHomePage.CreatedItem(pictureDescription);
            Assert.IsNotNull(item,"The picture description is not displayed in the List Of Itmes");
            Assert.IsTrue(onHomePage.ItemImage(item).Contains(pictureName),"The picture  is not displayed in the List Of Itmes");
        }
        [Test]
        [Description("Test Case 2: Edit an existing item")]
        public void _TestCase2()
        {
            sleep(5);
            string searchedDescription = "Mike plays the guitar";
            string newDescription = "Mike tries to play the guitar.";
            var item = onHomePage.CreatedItem(searchedDescription);
            sleep(3);
            click(onHomePage.EditItem(item));
            //Check that the old description appears in the text area
            Assert.AreEqual(onHomePage.TextField().GetAttribute("value"), searchedDescription, "Description does not match");
            //Check the cancel button
            click(onHomePage.Cancel());
            Assert.IsEmpty(onHomePage.TextField().GetAttribute("value"));
            click(onHomePage.EditItem(item));
            write(newDescription, onHomePage.TextField);
            click(onHomePage.UpdateItem());
            sleep();
            //Check if the new description is in the items list
            Assert.IsNotNull(onHomePage.CreatedItem(newDescription), "The new description is not displayed in the List Of Itmes");
        }

        [Test,Order(2)]
        [Description("Test Case 3: Delete the created Item in test case 1")]
        public void _TestCase3()
        {
            sleep(5);
            var item = onHomePage.CreatedItem(pictureDescription);
            sleep(2);
            click(onHomePage.DeleteItem(item));
            sleep();
            click(onHomePage.YesDeleteIt());
            sleep(2);
            //Verify on the List of Itmes that the item is not displayed
            Assert.IsNull(onHomePage.CreatedItem(pictureDescription), "The item is displayed in the List Of Itmes");

        }

        [Test]
        [Description("Test Case 4: Check max long in description")]
        public void _TestCase4()
        {
            string pictureName = "bicicleteando.jpg";
            string aboveMaxDescription = "Este text contiene 300 caracteres, para la prueba del test case 4, se agregaran caracteres aleatoriamente para completar el maximo de caracteres posibles. Los caracteres que se agregaran, conformaran plabras independientes entre si sin ningun tipo de valor significativo.Este escrito contiene 301.";
            
            onHomePage.SelectImage(pictureName);
            write(aboveMaxDescription, onHomePage.TextField);
            click(onHomePage.CreateItem());
            sleep(2);
            //Check if the description is not displayed in the items list
            var item = onHomePage.CreatedItem(aboveMaxDescription);
            Assert.IsNull(item, "The picture description is displayed in the List Of Itmes");
        }


        [Test]
        [Description("Test Case 5: Check if exist in the list the item with text: 'Creators: Matt Duffer, Ross Duffer'")]
        public void _TestCase5()
        {
            string itemDescription = "Creators: Matt Duffer, Ross Duffer";
            var item = onHomePage.CreatedItem(itemDescription);
            sleep(2);
            //Verify on the List of Itmes that the item is displayed
            Assert.IsNotNull(onHomePage.CreatedItem(itemDescription), "The item is not displayed in the List Of Itmes");

        }

        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
