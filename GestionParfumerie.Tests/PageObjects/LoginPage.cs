using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace GestionParfumerie.Tests.PageObjects
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        private By EmailInput => By.Id("email");
        private By PasswordInput => By.Id("password");
        private By LoginButton => By.CssSelector("button[type='submit']");
        private By ErrorMessage => By.CssSelector(".alert-danger");

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        public void NavigateToLoginPage()
        {
            _driver.Navigate().GoToUrl($"{Helpers.DriverHelper.GetBaseUrl()}/login");
        }

        public void EnterEmail(string email)
        {
            var emailField = _wait.Until(d => d.FindElement(EmailInput));
            emailField.Clear();
            emailField.SendKeys(email);
        }

        public void EnterPassword(string password)
        {
            var passwordField = _driver.FindElement(PasswordInput);
            passwordField.Clear();
            passwordField.SendKeys(password);
        }

        public void ClickLoginButton()
        {
            var loginBtn = _driver.FindElement(LoginButton);
            loginBtn.Click();
        }

        public void Login(string email, string password)
        {
            EnterEmail(email);
            EnterPassword(password);
            ClickLoginButton();
        }

        public bool IsErrorMessageDisplayed()
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
                var element = wait.Until(d => d.FindElement(ErrorMessage));
                return element.Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public string GetErrorMessage()
        {
            try
            {
                return _driver.FindElement(ErrorMessage).Text;
            }
            catch (NoSuchElementException)
            {
                return string.Empty;
            }
        }

        public bool IsLoginSuccessful()
        {
            Thread.Sleep(2000);
            return !_driver.Url.Contains("/login");
        }

        public string GetCurrentUrl()
        {
            return _driver.Url;
        }
    }
}