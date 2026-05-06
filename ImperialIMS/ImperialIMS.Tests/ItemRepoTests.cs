using ImperialIMS.Data;
using ImperialIMS.Models;
using ImperialIMS.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ImperialIMS.Tests
{
    public class ItemRepoTests
    {
        private static (ItemRepo repo, ApplicationDbContext db) CreateRepo(string testName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(testName)
                .Options;
            var config = new ConfigurationBuilder().Build();
            var db = new ApplicationDbContext(options, config);
            var repo = new ItemRepo(options, config, db);
            return (repo, db);
        }

        [Fact]
        public void Find_Should_Return_Item_When_Id_Exists()
        {
            var (repo, _) = CreateRepo(nameof(Find_Should_Return_Item_When_Id_Exists));
            var item = new Item { Id = 1, Name = "Test Item" };
            repo.Add(item);

            var result = repo.Find(1);

            Assert.NotNull(result);
            Assert.Equal(item.Id, result.Id);
            Assert.Equal(item.Name, result.Name);
        }

        [Fact]
        public void Find_Should_Return_Null_When_Id_Does_Not_Exist()
        {
            var (repo, _) = CreateRepo(nameof(Find_Should_Return_Null_When_Id_Does_Not_Exist));

            var result = repo.Find(999);

            Assert.Null(result);
        }

        [Fact]
        public void GetRange_Should_Return_Correct_Number_Of_Items()
        {
            var (repo, _) = CreateRepo(nameof(GetRange_Should_Return_Correct_Number_Of_Items));
            for (int i = 1; i <= 10; i++)
                repo.Add(new Item { Id = i, Name = $"Item {i}" });

            var result = repo.GetRange(0, 5);

            Assert.Equal(5, result.Count());
        }

        [Fact]
        public void GetRange_Should_Return_Correct_Items()
        {
            var (repo, _) = CreateRepo(nameof(GetRange_Should_Return_Correct_Items));
            for (int i = 1; i <= 10; i++)
                repo.Add(new Item { Id = i, Name = $"Item {i}" });

            var result = repo.GetRange(5, 5);

            Assert.Equal(5, result.Count());
            Assert.Equal(6, result.First().Id);
            Assert.Equal(10, result.Last().Id);
        }

        [Fact]
        public void GetRange_Should_Return_Empty_When_Skip_Exceeds_Count()
        {
            var (repo, _) = CreateRepo(nameof(GetRange_Should_Return_Empty_When_Skip_Exceeds_Count));
            for (int i = 1; i <= 10; i++)
                repo.Add(new Item { Id = i, Name = $"Item {i}" });

            var result = repo.GetRange(15, 5);

            Assert.Empty(result);
        }

        [Fact]
        public void GetRange_Should_Return_Remaining_Items_When_Take_Exceeds_Count()
        {
            var (repo, _) = CreateRepo(nameof(GetRange_Should_Return_Remaining_Items_When_Take_Exceeds_Count));
            for (int i = 1; i <= 10; i++)
                repo.Add(new Item { Id = i, Name = $"Item {i}" });

            var result = repo.GetRange(8, 5);

            Assert.Equal(2, result.Count());
            Assert.Equal(9, result.First().Id);
            Assert.Equal(10, result.Last().Id);
        }

        [Fact]
        public void GetRange_Should_Return_All_Items_When_Skip_Is_Zero_And_Take_Exceeds_Count()
        {
            var (repo, _) = CreateRepo(nameof(GetRange_Should_Return_All_Items_When_Skip_Is_Zero_And_Take_Exceeds_Count));
            for (int i = 1; i <= 10; i++)
                repo.Add(new Item { Id = i, Name = $"Item {i}" });

            var result = repo.GetRange(0, 15);

            Assert.Equal(10, result.Count());
        }

        [Fact]
        public void Search_Should_Return_Items_Matching_Filter()
        {
            var (repo, _) = CreateRepo(nameof(Search_Should_Return_Items_Matching_Filter));
            repo.Add(new Item { Id = 1, Name = "Test Item 1" });
            repo.Add(new Item { Id = 2, Name = "Test Item 2" });
            repo.Add(new Item { Id = 3, Name = "Another Item" });

            var result = repo.Search(x => x.Name.Contains("Test"));

            Assert.Equal(2, result.Count());
            Assert.All(result, item => Assert.Contains("Test", item.Name));
        }

        [Fact]
        public void Add_Should_Add_Item_To_Repo()
        {
            var (repo, _) = CreateRepo(nameof(Add_Should_Add_Item_To_Repo));
            var item = new Item { Id = 1, Name = "Test Item" };

            repo.Add(item);
            var result = repo.Find(1);

            Assert.NotNull(result);
            Assert.Equal(item.Id, result.Id);
            Assert.Equal(item.Name, result.Name);
        }

        [Fact]
        public void AddRange_Should_Add_Items_To_Repo()
        {
            var (repo, _) = CreateRepo(nameof(AddRange_Should_Add_Items_To_Repo));
            var items = new List<Item>
            {
                new Item { Id = 1, Name = "Test Item 1" },
                new Item { Id = 2, Name = "Test Item 2" },
                new Item { Id = 3, Name = "Test Item 3" }
            };

            repo.AddRange(items);

            foreach (var item in items)
            {
                var result = repo.Find(item.Id);
                Assert.NotNull(result);
                Assert.Equal(item.Id, result.Id);
                Assert.Equal(item.Name, result.Name);
            }
        }

        [Fact]
        public void Update_Should_Modify_Existing_Item()
        {
            var (repo, _) = CreateRepo(nameof(Update_Should_Modify_Existing_Item));
            var item = new Item { Id = 1, Name = "Test Item" };
            repo.Add(item);

            item.Name = "Updated Item";
            repo.Update(item);
            var result = repo.Find(1);

            Assert.NotNull(result);
            Assert.Equal("Updated Item", result.Name);
        }

        [Fact]
        public void UpdateRange_Should_Modify_Existing_Items()
        {
            var (repo, _) = CreateRepo(nameof(UpdateRange_Should_Modify_Existing_Items));
            var items = new List<Item>
            {
                new Item { Id = 1, Name = "Test Item 1" },
                new Item { Id = 2, Name = "Test Item 2" },
                new Item { Id = 3, Name = "Test Item 3" }
            };
            repo.AddRange(items);

            items[0].Name = "Updated Item 1";
            items[1].Name = "Updated Item 2";
            items[2].Name = "Updated Item 3";
            repo.UpdateRange(items);

            foreach (var item in items)
            {
                var result = repo.Find(item.Id);
                Assert.NotNull(result);
                Assert.Equal($"Updated Item {item.Id}", result.Name);
            }
        }

        [Fact]
        public void Delete_Should_Remove_Item_From_Repo()
        {
            var (repo, _) = CreateRepo(nameof(Delete_Should_Remove_Item_From_Repo));
            var item = new Item { Id = 1, Name = "Test Item" };
            repo.Add(item);

            repo.Delete(item);
            var result = repo.Find(1);

            Assert.Null(result);
        }

        [Fact]
        public void Delete_Should_Remove_Item_By_Id()
        {
            var (repo, _) = CreateRepo(nameof(Delete_Should_Remove_Item_By_Id));
            var item = new Item { Id = 1, Name = "Test Item" };
            repo.Add(item);

            repo.Delete(1, item.TimeStamp);
            var result = repo.Find(1);

            Assert.Null(result);
        }

        [Fact]
        public void DeleteRange_Should_Remove_Items_From_Repo()
        {
            var (repo, _) = CreateRepo(nameof(DeleteRange_Should_Remove_Items_From_Repo));
            var items = new List<Item>
            {
                new Item { Id = 1, Name = "Test Item 1" },
                new Item { Id = 2, Name = "Test Item 2" },
                new Item { Id = 3, Name = "Test Item 3" }
            };
            repo.AddRange(items);

            repo.DeleteRange(items);

            foreach (var item in items)
                Assert.Null(repo.Find(item.Id));
        }
    }
}
