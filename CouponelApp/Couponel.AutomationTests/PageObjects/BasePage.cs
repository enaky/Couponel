﻿using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Couponel.AutomationTests.PageObjects
{
    public class BasePage
    {
        public IWebDriver driver;

        public void WaitForPageToLoad(string selector)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(selector)));
        }
    }
}
