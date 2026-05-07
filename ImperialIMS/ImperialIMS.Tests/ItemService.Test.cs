using ImperialIMS.Models;
using ImperialIMS.Repos;
using ImperialIMS.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Linq.Expressions;

namespace ImperialIMS.Tests
{
    public class ItemServiceTests
    {
        private readonly IRepo<Item> _repo;
        private readonly ILogger<Item> _logger;
        private readonly ItemService _svc;

        public ItemServiceTests()
        {
            _repo = Substitute.For<IRepo<Item>>();
            _logger = Substitute.For<ILogger<Item>>();
            var config = new ConfigurationBuilder().Build();
            _svc = new ItemService(_repo, config, _logger);
        }
        private void SetupSearch(IEnumerable<Item> items)
        {
            _repo.Search(Arg.Any<Expression<Func<Item, bool>>>())
                 .Returns(items.AsQueryable());
        }

        //Get

        [Fact]
        public void Get_Returns_Item_When_Found()
        {
            var item = new Item { Id = 5, Name = "Widget" };
            SetupSearch(new[] { item });

            var result = _svc.Get(5);

            Assert.Equal(5, result.Id);
            Assert.Equal("Widget", result.Name);
        }

        [Fact]
        public void Get_Returns_Empty_Item_When_Not_Found()
        {
            SetupSearch(Enumerable.Empty<Item>());

            var result = _svc.Get(999);

            Assert.Equal(0, result.Id);
        }

        //GetAll

        [Fact]
        public void GetAll_Returns_Items_From_Repo()
        {
            SetupSearch(new[]
            {
                new Item { Id = 1, Name = "A" },
                new Item { Id = 2, Name = "B" }
            });

            var result = _svc.GetAll();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetAll_Returns_Empty_When_Repo_Has_No_Items()
        {
            SetupSearch(Enumerable.Empty<Item>());

            var result = _svc.GetAll();

            Assert.Empty(result);
        }

        //GetRecycleBin

        [Fact]
        public void GetRecycleBin_Returns_Deleted_Items()
        {
            SetupSearch(new[]
            {
                new Item { Id = 1, Name = "Deleted", IsDeleted = true }
            });

            var result = _svc.GetRecycleBin();

            Assert.Single(result);
            Assert.True(result[0].IsDeleted);
        }

        //Add

        [Fact]
        public void Add_Calls_Repo_Add_With_Item()
        {
            var item = new Item { Id = 1, Name = "New Item" };

            _svc.Add(item);

            _repo.Received(1).Add(item);
        }

        //Update

        [Fact]
        public void Update_Calls_Repo_Update_With_Item()
        {
            var item = new Item { Id = 1, Name = "Updated Item" };

            _svc.Update(item);

            _repo.Received(1).Update(item);
        }

        //Delete (soft)

        [Fact]
        public async Task Delete_Sets_IsDeleted_True_And_Calls_Repo_Update()
        {
            var item = new Item { Id = 1, Name = "Item", IsDeleted = false };
            SetupSearch(new[] { item });

            await _svc.Delete(1);

            _repo.Received(1).Update(Arg.Is<Item>(i => i.IsDeleted));
        }

        //Remove (hard delete)

        [Fact]
        public async Task Remove_Calls_Repo_Delete_With_Item()
        {
            var item = new Item { Id = 1, Name = "Item" };
            SetupSearch(new[] { item });

            await _svc.Remove(1);

            _repo.Received(1).Delete(item);
        }

        //UnDelete

        [Fact]
        public async Task UnDelete_Sets_IsDeleted_False_And_Calls_Repo_Update()
        {
            var item = new Item { Id = 1, Name = "Item", IsDeleted = true };
            SetupSearch(new[] { item });

            await _svc.UnDelete(1);

            _repo.Received(1).Update(Arg.Is<Item>(i => !i.IsDeleted));
        }

        //Search

        [Fact]
        public void Search_Returns_Items_Matching_Name()
        {
            _repo.Search().Returns(new List<Item>
            {
                new Item { Id = 1, Name = "Widget Alpha" },
                new Item { Id = 2, Name = "Gadget Beta" },
                new Item { Id = 3, Name = "widget gamma" }
            }.AsQueryable());

            var result = _svc.Search("widget");

            Assert.Equal(2, result.Count);
            Assert.All(result, i => Assert.Contains("widget", i.Name, StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void Search_Returns_Items_Matching_Description()
        {
            _repo.Search().Returns(new List<Item>
            {
                new Item { Id = 1, Name = "Part", Description = "Contains a sprocket" },
                new Item { Id = 2, Name = "Other", Description = null }
            }.AsQueryable());

            var result = _svc.Search("sprocket");

            Assert.Single(result);
            Assert.Equal(1, result[0].Id);
        }

        [Fact]
        public void Search_Is_Case_Insensitive()
        {
            _repo.Search().Returns(new List<Item>
            {
                new Item { Id = 1, Name = "BOLT" },
                new Item { Id = 2, Name = "bolt" },
                new Item { Id = 3, Name = "Bolt" }
            }.AsQueryable());

            var result = _svc.Search("bolt");

            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void Search_Returns_Empty_List_When_No_Match()
        {
            _repo.Search().Returns(new List<Item>
            {
                new Item { Id = 1, Name = "Widget" }
            }.AsQueryable());

            var result = _svc.Search("xyz");

            Assert.Empty(result);
        }

        [Fact]
        public void Search_Returns_Empty_List_On_Exception()
        {
            _repo.Search().Returns(_ => throw new Exception("DB error"));

            var result = _svc.Search("anything");

            Assert.Empty(result);
        }
    }
}
