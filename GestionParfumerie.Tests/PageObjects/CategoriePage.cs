using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace GestionParfumerie.Tests.PageObjects
{
    public class CategoriePage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public CategoriePage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        private By BtnAjouterCategorie => By.CssSelector("button.btn.btn-primary.btn-sm");
        private By InputNom => By.XPath("//label[text()='Nom']/following-sibling::input");
        private By InputDescription => By.XPath("//label[text()='Description']/following-sibling::textarea");
        private By BtnEnregistrer => By.XPath("//div[@class='modal-footer']//button[contains(text(),'Enregistrer')]");

        public void GoToPage()
        {
            _driver.Navigate().GoToUrl($"{Helpers.DriverHelper.GetBaseUrl()}/admin/categories");
        }

        public void ClickAddButton()
        {
            _wait.Until(d => d.FindElement(BtnAjouterCategorie)).Click();
        }

        public void SetNom(string nom)
        {
            var element = _wait.Until(d => d.FindElement(InputNom));
            element.Clear();
            element.SendKeys(nom);
        }

        public void SetDescription(string description)
        {
            var element = _wait.Until(d => d.FindElement(InputDescription));
            element.Clear();
            element.SendKeys(description);
        }

        public void ClickEnregistrer()
        {
            _wait.Until(d => d.FindElement(BtnEnregistrer)).Click();
        }

        public bool IsCategorieInTable(string nom)
        {
            try
            {
                return _wait.Until(d => d.FindElement(
                    By.XPath($"//table//td[text()='{nom}']"))).Displayed;
            }
            catch
            {
                return false;
            }
        }

        public void AddCategorie(string nom, string description)
        {
            ClickAddButton();
            SetNom(nom);
            SetDescription(description);
            ClickEnregistrer();
        }
    }
}
