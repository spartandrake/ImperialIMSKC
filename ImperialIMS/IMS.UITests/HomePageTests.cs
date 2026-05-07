using OpenQA.Selenium;

namespace IMS.UITests
{
    public class HomePageTests : TestBase
    {
        [Fact]
        public void HomePage_Loads_Without_Error()
        {
            NavigateTo("/");

            Assert.DoesNotContain("error", Driver.Title, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("exception", Driver.PageSource, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void HomePage_Has_Navigation_Bar()
        {
            NavigateTo("/");

            var navbar = Driver.FindElement(By.CssSelector(".navbar"));
            Assert.NotNull(navbar);
        }

        [Fact]
        public void HomePage_Has_Search_Nav_Link()
        {
            NavigateTo("/");

            var link = Driver.FindElement(By.CssSelector("a[href='/Search']"));
            Assert.Equal("Search", link.Text.Trim(), ignoreCase: true);
        }

        [Fact]
        public void HomePage_Has_Login_Link_When_Not_Authenticated()
        {
            NavigateTo("/");

            var loginLink = Driver.FindElement(By.CssSelector("a[href*='/Identity/Account/Login']"));
            Assert.NotNull(loginLink);
        }
    }
}
