using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace IMS.UITests
{
    public class TestBase : IDisposable
    {
        protected IWebDriver Driver;
        protected WebDriverWait Wait;
        protected const string BaseUrl = "https://localhost:7213";

        protected const string TestUserEmail = "test@imperialims.com";
        protected const string TestUserPassword = "Test1234!";
        protected const string TestAdminEmail = "admin@imperialims.com";
        protected const string TestAdminPassword = "Admin1234!";

        public TestBase()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());

            var options = new ChromeOptions();
            options.AddArgument("--ignore-certificate-errors"); // trust localhost self-signed cert
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");

            Driver = new ChromeDriver(options);
            Driver.Manage().Window.Maximize();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
        }

        protected void NavigateTo(string path = "") =>
            Driver.Navigate().GoToUrl(BaseUrl + path);

        protected void Login(string email, string password)
        {
            NavigateTo("/Identity/Account/Login");
            Driver.FindElement(By.Name("Input.Email")).SendKeys(email);
            Driver.FindElement(By.Name("Input.Password")).SendKeys(password);
            Driver.FindElement(By.Id("login-submit")).Click();
            Wait.Until(d => !d.Url.Contains("/Account/Login"));
        }

        protected void LoginAsTestUser() => Login(TestUserEmail, TestUserPassword);
        protected void LoginAsAdmin() => Login(TestAdminEmail, TestAdminPassword);

        protected IWebElement WaitForElement(By by) =>
            Wait.Until(d => d.FindElement(by));

        public void Dispose()
        {
            Driver.Quit();
            Driver.Dispose();
        }
    }
}
