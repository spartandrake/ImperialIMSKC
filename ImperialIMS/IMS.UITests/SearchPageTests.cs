using OpenQA.Selenium;

namespace IMS.UITests
{
    public class SearchPageTests : TestBase
    {
        [Fact]
        public void SearchPage_Requires_Authentication()
        {
            NavigateTo("/Search");

            // Unauthenticated users should be redirected to login
            Assert.Contains("/Account/Login", Driver.Url);
        }

        [Fact]
        public void SearchPage_Loads_With_Search_Form_When_Authenticated()
        {
            Login(TestUserEmail, TestUserPassword);
            NavigateTo("/Search");

            Assert.NotNull(Driver.FindElement(By.Name("Query")));
            Assert.NotNull(Driver.FindElement(By.Name("FacilityId")));
            Assert.NotNull(Driver.FindElement(By.CssSelector("button[type='submit']")));
        }

        [Fact]
        public void SearchPage_Shows_Results_Table()
        {
            Login(TestUserEmail, TestUserPassword);
            NavigateTo("/Search");

            var table = Driver.FindElement(By.CssSelector("table"));
            Assert.NotNull(table);
        }

        [Fact]
        public void SearchPage_FacilityId_Dropdown_Has_Options()
        {
            Login(TestUserEmail, TestUserPassword);
            NavigateTo("/Search");

            var facilitySelect = Driver.FindElement(By.Name("FacilityId"));
            var options = facilitySelect.FindElements(By.TagName("option"));
            Assert.NotEmpty(options);
        }
    }
}
