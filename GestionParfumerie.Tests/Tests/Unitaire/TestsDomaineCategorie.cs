using OpenQA.Selenium;
using GestionParfumerie.Tests.Helpers;
using GestionParfumerie.Tests.PageObjects;

namespace GestionParfumerie.Tests.Tests.Unitaire
{
    public class LoginTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly LoginPage _loginPage;

        public LoginTests()
        {
            // Setup - Initialiser le driver avant chaque test
            _driver = DriverHelper.GetChromeDriver();
            _loginPage = new LoginPage(_driver);
        }

        [Fact]
        public void Test01_OpenLoginPage_ShouldDisplayLoginForm()
        {
            // Arrange & Act
            _loginPage.NavigateToLoginPage();

            // Assert
            string currentUrl = _driver.Url.ToLower();
            Assert.True(currentUrl.Contains("login"), $"L'URL devrait contenir 'login'. URL actuelle: {currentUrl}");
        }

        [Fact]
        public void Test02_LoginWithValidCredentials_ShouldSucceed()
        {
            // Arrange
            _loginPage.NavigateToLoginPage();
            string validEmail = "admin@test.com"; // Utilise un compte existant dans ta DB
            string validPassword = "Password123!"; // Adapte selon ton compte

            // Act
            _loginPage.Login(validEmail, validPassword);

            // Assert
            Thread.Sleep(3000); // Attendre la redirection
            Assert.True(_loginPage.IsLoginSuccessful(), "Le login devrait réussir");

            string currentUrl = _driver.Url.ToLower();
            Assert.False(currentUrl.Contains("login"), "Ne devrait plus être sur la page login");
        }

        [Fact]
        public void Test03_LoginWithInvalidEmail_ShouldFail()
        {
            // Arrange
            _loginPage.NavigateToLoginPage();
            string invalidEmail = "wrongemail@test.com";
            string password = "Password123!";

            // Act
            _loginPage.Login(invalidEmail, password);

            // Assert
            Thread.Sleep(2000); // Attendre le message d'erreur

            string currentUrl = _driver.Url;
            bool hasError = _loginPage.IsErrorMessageDisplayed();
            bool staysOnLogin = currentUrl.Contains("login");

            Assert.True(hasError || staysOnLogin,
                "Une erreur devrait s'afficher ou rester sur la page login");
        }

        [Fact]
        public void Test04_LoginWithInvalidPassword_ShouldFail()
        {
            // Arrange
            _loginPage.NavigateToLoginPage();
            string validEmail = "admin@test.com";
            string invalidPassword = "WrongPassword123!";

            // Act
            _loginPage.Login(validEmail, invalidPassword);

            // Assert
            Thread.Sleep(2000);

            string currentUrl = _driver.Url;
            bool hasError = _loginPage.IsErrorMessageDisplayed();
            bool staysOnLogin = currentUrl.Contains("login");

            Assert.True(hasError || staysOnLogin,
                "Une erreur devrait s'afficher ou rester sur la page login");
        }

        [Fact]
        public void Test05_LoginWithEmptyFields_ShouldFail()
        {
            // Arrange
            _loginPage.NavigateToLoginPage();

            // Act
            _loginPage.Login("", "");

            // Assert
            Thread.Sleep(1000);

            string currentUrl = _driver.Url.ToLower();
            Assert.True(currentUrl.Contains("login"), "Devrait rester sur la page login");
        }

        public void Dispose()
        {
            // Cleanup - Fermer le navigateur après chaque test
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}