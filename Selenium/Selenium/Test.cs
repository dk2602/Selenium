using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
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
        int generate_startNumber = 0; // Od którego elementu ciągu zaczynamy generować (Przydaten jak generujemy na kilka razy)
        string path = @"D:\Uczelnia\MyShit\MyShit\Obrazki\WygenerowaneObrazki"; // Scieżka do folderu do którego mają trafić obrazki
        bool FirefoxChrome = true; // true  = Firefox, false = Chrome
        int sec = 4; // czas maksymalnego oczekiwania na element (warto zmienić w zależności od szybkości internetu)(w tym momencie jest to w praktyce długość przerwy w sekundach między kolejnymi generowanymi obrazkami)

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
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(sec);
            driver.Url = "https://www.wolframalpha.com/widgets/view.jsp?id=f5abe18064d57c2e5a768504a2041036";
            driver.FindElement(By.ClassName("m")).Click();
            for (int i = 0; i < numberOfPictures; i++)
            {
                string name = generator.Generate();
                var text = driver.FindElement(By.ClassName("wolframAlphaWidgetBody")).FindElement(By.Name("input1"));
                text.Clear();
                text.SendKeys(name);
                driver.FindElement(By.ClassName("wolframAlphaWidgetBody")).FindElement(By.ClassName("m")).Click();
                ((ITakesScreenshot)driver.SwitchTo().Frame("wolframAlphaWidgetResults25165").FindElement(By.Id("i_0200_1"))).GetScreenshot().SaveAsFile(path+"\\"+name+".png",ScreenshotImageFormat.Png);
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
        public Generator(int start = 0)
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