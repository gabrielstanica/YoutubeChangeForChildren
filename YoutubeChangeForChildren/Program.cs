using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YoutubeChangeForChildren
{
    class Program
    {
        static void Main(string[] args)
        {
            IWebDriver driver = new ChromeDriver();
            Task.Factory.StartNew(() =>
            {
                ChromeDriverStart(driver);
            });

            Task.Factory.StartNew(() =>
            {
                ConsoleKeyInfo keyInfo;
                do { keyInfo = Console.ReadKey(true); }
                while (keyInfo.Key != ConsoleKey.Enter);
            }).Wait();


            driver.Close();
        }


        static void ChromeDriverStart(IWebDriver drivers)
        {
            IWebDriver driver = drivers;
            driver.Navigate().GoToUrl("https://www.youtube.com");
            SearchVideoChildren(driver, "Cantece copii");
            int countSearcheas = 0;
            while (true)
            {
                try
                { 
                    var g = TimeSpan.FromSeconds(10).TotalMilliseconds;
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    IWebElement repeat = driver.FindElement(By.XPath("//*[@id=\"items\"]/ytd-compact-autoplay-renderer"));
                    var first = repeat;
                    string text = repeat.Text;
                    int counter = 1;
                    try
                    {
                        bool trueee = Compare(text);
                        while (trueee)
                        {
                            repeat = driver.FindElement(By.XPath(String.Format("//*[@id=\"items\"]/ytd-compact-video-renderer[{0}]", counter)));
                            counter++;
                            text = repeat.Text;
                            trueee = Compare(text);
                        }
                    }
                    catch (Exception ex)
                    {
                        repeat = first;
                    }
                    repeat.Click();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    SearchVideoChildren(driver);
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                }
                countSearcheas++;
                if(countSearcheas == 100)
                {
                    SearchVideoChildren(driver);
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    countSearcheas = 0;
                }
            }
        }

        static void SearchVideoChildren(IWebDriver driver, string text="")
        {
            IWebElement element = driver.FindElement(By.Id("search"));
            element.SendKeys(text);
            IWebElement elementSearchButton = driver.FindElement(By.Id("search-icon-legacy"));
            elementSearchButton.Submit();
            IWebElement videotitle = driver.FindElement(By.Id("video-title"));
            videotitle.Click();
        }

        static bool Compare(string text)
        {
            string[] ss = new string[] { "children", "copi", "gradinita" };
            bool trueee = true;
            foreach (var item in ss)
            {
                if (text.ToLower().Contains(item))
                {
                    trueee = false;
                    break;
                }
            }
            return trueee;
        }
    }
}
