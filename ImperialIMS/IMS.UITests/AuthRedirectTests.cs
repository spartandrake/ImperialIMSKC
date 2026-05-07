using OpenQA.Selenium;

namespace IMS.UITests
{
    /// <summary>
    /// Verifies that protected pages redirect unauthenticated users to the login page.
    /// </summary>
    public class AuthRedirectTests : TestBase
    {
        private void AssertRedirectsToLogin()
        {
            WaitForElement(By.Id("login-submit"));
            Assert.Contains("/Account/Login", Driver.Url);
        }

        [Fact]
        public void AdminItem_Redirects_Unauthenticated_User_To_Login()
        {
            NavigateTo("/Admin/Item");
            AssertRedirectsToLogin();
        }

        [Fact]
        public void AdminInventory_Redirects_Unauthenticated_User_To_Login()
        {
            NavigateTo("/Admin/Inventory");
            AssertRedirectsToLogin();
        }

        [Fact]
        public void AdminUsers_Redirects_Unauthenticated_User_To_Login()
        {
            NavigateTo("/Admin/Users");
            AssertRedirectsToLogin();
        }

        [Fact]
        public void AdminStorageFacility_Redirects_Unauthenticated_User_To_Login()
        {
            NavigateTo("/Admin/StorageFacility");
            AssertRedirectsToLogin();
        }

        [Fact]
        public void AdminRecycleBin_Redirects_Unauthenticated_User_To_Login()
        {
            NavigateTo("/Admin/RecycleBin");
            AssertRedirectsToLogin();
        }

        [Fact]
        public void AdminReports_Redirects_Unauthenticated_User_To_Login()
        {
            NavigateTo("/Admin/Reports/InventoryChanges");
            AssertRedirectsToLogin();
        }

        [Fact]
        public void AdminItem_Loads_For_Admin_User()
        {
            LoginAsAdmin();
            NavigateTo("/Admin/Item");

            Assert.DoesNotContain("/Account/Login", Driver.Url);
            Assert.DoesNotContain("Access Denied", Driver.Title);
        }
    }
}
