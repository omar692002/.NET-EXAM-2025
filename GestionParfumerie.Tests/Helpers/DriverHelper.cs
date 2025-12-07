using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace GestionParfumerie.Tests.Helpers
{
    public class DriverHelper
    {
        public static IWebDriver GetChromeDriver()
        {
            // Télécharge automatiquement la bonne version de ChromeDriver
            new DriverManager().SetUpDriver(new ChromeConfig());

            var options = new ChromeOptions();

            // Options pour rendre les tests plus stables
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-notifications");
            options.AddArgument("--disable-popup-blocking");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");

            // Décommenter cette ligne pour mode headless (sans interface graphique)
            // options.AddArgument("--headless");

            var driver = new ChromeDriver(options);

            // Timeout implicite pour attendre les éléments
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            return driver;
        }

        public static string GetBaseUrl()
        {
            return "http://localhost:5266";
        }
    }
}