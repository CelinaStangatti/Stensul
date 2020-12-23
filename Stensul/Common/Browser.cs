using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Stensul.Common {
    class Browser {
        public static IWebDriver Chrome {
            get {
                ChromeOptions options = new ChromeOptions();
                options.AddArgument("--start-maximized");
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                var testResultPath = baseDir.Substring(0, baseDir.LastIndexOf("Stensul")) + "Image\\";
                options.AddUserProfilePreference("download.default_directory", testResultPath);
                
                return new ChromeDriver(options);
            }
        }
    }
}