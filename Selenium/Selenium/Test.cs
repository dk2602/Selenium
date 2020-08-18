using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium
{

    public class Test
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////////
        //// Najważniejsze zmienne

        int numberOfPictures = 10; // Ilość generowanych obrazków
        int generate_startNumber = 22000; // Od którego elementu ciągu zaczynamy generować (Przydaten jak generujemy na kilka razy)
        string path = @"D:\Uczelnia\MyShit\MyShit\Obrazki\WygenerowaneObrazki"; // Scieżka do folderu do którego mają trafić obrazki
        bool FirefoxChrome = true; // true  = Firefox, false = Chrome
        int sec = 6; // czas maksymalnego oczekiwania na element (warto zmienić w zależności od szybkości internetu)

        ////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////

        IWebDriver driver;

        [SetUp]
        public void startBrowser()
        {
            string s = AppDomain.CurrentDomain.BaseDirectory.ToString();
            if(FirefoxChrome)
                driver = new FirefoxDriver(s + "Drivers");
            else
                driver = new ChromeDriver(s + "Drivers");
        }

        [Test]
        public void Wolfram()
        {
            Generator generator = new Generator(generate_startNumber);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(sec));
            driver.Url = "https://www.wolframalpha.com/widgets/view.jsp?id=f5abe18064d57c2e5a768504a2041036";
            driver.FindElement(By.ClassName("m")).Click();
            for (int i = 0; i < numberOfPictures; i++)
            {
                string name = generator.Generate();
                IWebElement text = driver.FindElement(By.ClassName("wolframAlphaWidgetBody")).FindElement(By.Name("input1"));
                text.Clear();
                text.SendKeys(name);
                driver.FindElement(By.ClassName("wolframAlphaWidgetBody")).FindElement(By.ClassName("m")).Click();
                driver.SwitchTo().Frame("wolframAlphaWidgetResults25165");
                IWebElement Image =  wait.Until<IWebElement>(d => driver.FindElement(By.Id("i_0200_1")));
                System.Threading.Thread.Sleep(1000);
                ((ITakesScreenshot)Image).GetScreenshot().SaveAsFile(path + "\\" + name + ".png", ScreenshotImageFormat.Png);
                driver.SwitchTo().ParentFrame();
            }
        }

        [TearDown]
        public void closeBrowser()
        {
            driver.Close();
            driver.Quit();
        }
    }

    public class Generator
    {
        int current;
        public Generator(int start = 1)
        {
            current = start;
        }

        public string Generate()
        {
            string s = "";
            int temp = current;
            while (temp > 0)
            {
                if (temp % 27 == 0)
                {
                    current++;
                    return Generate();
                }
                s = (char)((temp % 27) + 'a' - 1) + s;
                temp = temp / 27;
            }
            current++;
            return s;
        }
    }
}