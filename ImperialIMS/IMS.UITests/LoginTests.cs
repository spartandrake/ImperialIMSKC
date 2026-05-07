using OpenQA.Selenium;

namespace IMS.UITests
{
    public class LoginTests : TestBase
    {
        [Fact]
        public void LoginPage_Loads_With_Form()
        {
            NavigateTo("/Identity/Account/Login");

            Assert.NotNull(Driver.FindElement(By.Name("Input.Email")));
            Assert.NotNull(Driver.FindElement(By.Name("Input.Password")));
            Assert.NotNull(Driver.FindElement(By.Id("login-submit")));
        }

        [Fact]
        public void LoginPage_Shows_Validation_Error_On_Empty_Submit()
        {
            NavigateTo("/Identity/Account/Login");

            Driver.FindElement(By.Id("login-submit")).Click();

            // Browser HTML5 validation or server-side validation keeps us on the login page
            Assert.Contains("/Account/Login", Driver.Url);
        }

        [Fact]
        public void LoginPage_Shows_Error_On_Invalid_Credentials()
        {
            NavigateTo("/Identity/Account/Login");

            Driver.FindElement(By.Name("Input.Email")).SendKeys("nobody@invalid.com");
            Driver.FindElement(By.Name("Input.Password")).SendKeys("WrongPassword1!");
            Driver.FindElement(By.Id("login-submit")).Click();

            var errorSummary = WaitForElement(By.CssSelector(".text-danger[role='alert']"));
            Assert.False(string.IsNullOrWhiteSpace(errorSummary.Text));
        }

        [Fact]
        public void LoginPage_Has_Register_Link()
        {
            NavigateTo("/Identity/Account/Login");

            var registerLink = Driver.FindElement(By.CssSelector("a[href*='/Account/Register']"));
            Assert.NotNull(registerLink);
        }

        [Fact]
        public void LoginPage_Has_Forgot_Password_Link()
        {
            NavigateTo("/Identity/Account/Login");

            var forgotLink = Driver.FindElement(By.Id("forgot-password"));
            Assert.NotNull(forgotLink);
        }

        // Requires a test user to exist in the dev database — update credentials in TestBase
        [Fact]
        public void Login_With_Valid_Credentials_Redirects_Away_From_Login()
        {
            Login(TestUserEmail, TestUserPassword);

            Assert.DoesNotContain("/Account/Login", Driver.Url);
        }

        [Fact]
        public void Authenticated_User_Can_Logout()
        {
            Login(TestUserEmail, TestUserPassword);

            var logoutForm = Driver.FindElement(By.CssSelector("form[action*='/Account/Logout']"));
            logoutForm.FindElement(By.CssSelector("button[type='submit']")).Click();

            WaitForElement(By.CssSelector("a[href*='/Account/Login']"));
            Assert.Contains("/", Driver.Url);
        }
    }
}
