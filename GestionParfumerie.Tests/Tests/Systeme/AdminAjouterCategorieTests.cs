using OpenQA.Selenium;
using GestionParfumerie.Tests.PageObjects;
using Xunit;

namespace GestionParfumerie.Tests.Tests.Systeme
{
    public class AdminAjouterCategorieTests : IDisposable
    {
        private readonly IWebDriver _driver;

        public AdminAjouterCategorieTests()
        {
            _driver = Helpers.DriverHelper.GetChromeDriver();
        }

        [Fact]
        public void Admin_Ajoute_Categorie_Avec_Succes()
        {
            var loginPage = new LoginPage(_driver);
            var categoriePage = new CategoriePage(_driver);

            string catName = "CatTest" + Guid.NewGuid().ToString().Substring(0, 5);

            
            loginPage.NavigateToLoginPage();
            loginPage.Login("admin@admin.com", "Admin123!");
            Assert.True(loginPage.IsLoginSuccessful());

            categoriePage.GoToPage();

            categoriePage.AddCategorie(catName, "Catégorie créée automatiquement pour test");

            Assert.True(categoriePage.IsCategorieInTable(catName));
        }

        public void Dispose()
        {
            _driver.Quit();
        }
    }
}
