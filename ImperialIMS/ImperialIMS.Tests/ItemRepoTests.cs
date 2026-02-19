using ImperialIMS.Data;
using ImperialIMS.Models;
using ImperialIMS.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace ImperialIMS.Tests
{
    public class ItemRepoTests
    {
        [Fact]
        public void Find_Should_Return_Item_When_Id_Exists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            var config = new ConfigurationBuilder().Build();
            using var context = new ApplicationDbContext(options, config);
            context.Categories.Add(new Category { Name = "Test Category" });
            context.SaveChanges();
            var repo = new ItemRepo(config);
            var item = new Item { Id = 1, Name = "Test Item" };
            repo.Add(item);
            // Act
            var result = repo.Find(1);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(item.Id, result.Id);
            Assert.Equal(item.Name, result.Name);
        }
        //T Find(int? id);
        [Fact]
        public void Find_Should_Return_Null_When_Id_Does_Not_Exist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            var config = new ConfigurationBuilder().Build();
            using var context = new ApplicationDbContext(options, config);
            context.Categories.Add(new Category { Name = "Test Category" });
            context.SaveChanges();
            var repo = new ItemRepo(config);
            // Act
            var result = repo.Find(999);
            // Assert
            Assert.Null(result);
        }
        //IEnumerable<T> GetRange(int skip, int take);
        [Fact]
        public void GetRange_Should_Return_Correct_Number_Of_Items()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            var config = new ConfigurationBuilder().Build();
            using var context = new ApplicationDbContext(options, config);
            context.Categories.Add(new Category { Name = "Test Category" });
            context.SaveChanges();
            var repo = new ItemRepo(config);
            for (int i = 1; i <= 10; i++)
            {
                repo.Add(new Item { Id = i, Name = $"Item {i}" });
            }
            // Act
            var result = repo.GetRange(0, 5);
            // Assert
            Assert.Equal(5, result.Count());
        }
        [Fact]
        public void GetRange_Should_Return_Correct_Items()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            var config = new ConfigurationBuilder().Build();
            using var context = new ApplicationDbContext(options, config);
            context.Categories.Add(new Category { Name = "Test Category" });
            context.SaveChanges();
            var repo = new ItemRepo(config);
            for (int i = 1; i <= 10; i++)
            {
                repo.Add(new Item { Id = i, Name = $"Item {i}" });
            }
            // Act
            var result = repo.GetRange(5, 5);
            // Assert
            Assert.Equal(5, result.Count());
            Assert.Equal(6, result.First().Id);
            Assert.Equal(10, result.Last().Id);
        }
        [Fact]
        public void GetRange_Should_Return_Empty_When_Skip_Exceeds_Count()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            var config = new ConfigurationBuilder().Build();
            using var context = new ApplicationDbContext(options, config);
            context.Categories.Add(new Category { Name = "Test Category" });
            context.SaveChanges();
            var repo = new ItemRepo(config);
            for (int i = 1; i <= 10; i++)
            {
                repo.Add(new Item { Id = i, Name = $"Item {i}" });
            }
            // Act
            var result = repo.GetRange(15, 5);
            // Assert
            Assert.Empty(result);
        }
        [Fact]
        public void GetRange_Should_Return_Remaining_Items_When_Take_Exceeds_Count()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            var config = new ConfigurationBuilder().Build();
            using var context = new ApplicationDbContext(options, config);
            context.Categories.Add(new Category { Name = "Test Category" });
            context.SaveChanges();
            var repo = new ItemRepo(config);
            for (int i = 1; i <= 10; i++)
            {
                repo.Add(new Item { Id = i, Name = $"Item {i}" });
            }
            // Act
            var result = repo.GetRange(8, 5);
            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal(9, result.First().Id);
            Assert.Equal(10, result.Last().Id);
        }
        [Fact]
        public void GetRange_Should_Return_All_Items_When_Skip_And_Take_Exceed_Count()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            var config = new ConfigurationBuilder().Build();
            using var context = new ApplicationDbContext(options, config);
            context.Categories.Add(new Category { Name = "Test Category" });
            context.SaveChanges();
            var repo = new ItemRepo(config);
            for (int i = 1; i <= 10; i++)
            {
                repo.Add(new Item { Id = i, Name = $"Item {i}" });
            }
            // Act
            var result = repo.GetRange(0, 15);
            // Assert
            Assert.Equal(10, result.Count());
        }
        //IQueryable<T> Search(Expression<Func<T, bool>> filter = null, IOrderedQueryable<T> orderBy = null);
        [Fact]
        public void Search_Should_Return_Items_Matching_Filter()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            var config = new ConfigurationBuilder().Build();
            using var context = new ApplicationDbContext(options, config);
            context.Categories.Add(new Category { Name = "Test Category" });
            context.SaveChanges();
            var repo = new ItemRepo(config);
            repo.Add(new Item { Id = 1, Name = "Test Item 1" });
            repo.Add(new Item { Id = 2, Name = "Test Item 2" });
            repo.Add(new Item { Id = 3, Name = "Another Item" });
            // Act
            var result = repo.Search(x => x.Name.Contains("Test"));
            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, item => Assert.Contains("Test", item.Name));
        }
        //int Add(T entity, bool persist = true);
        [Fact]
        public void Add_Should_Add_Item_To_Repo()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            var config = new ConfigurationBuilder().Build();
            using var context = new ApplicationDbContext(options, config);
            context.Categories.Add(new Category { Name = "Test Category" });
            context.SaveChanges();
            var repo = new ItemRepo(config);
            var item = new Item { Id = 1, Name = "Test Item" };
            // Act
            repo.Add(item);
            var result = repo.Find(1);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(item.Id, result.Id);
            Assert.Equal(item.Name, result.Name);
        }
        //int AddRange(IEnumerable<T> entities, bool persist = true);
        [Fact]
        public void AddRange_Should_Add_Items_To_Repo()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            var config = new ConfigurationBuilder().Build();
            using var context = new ApplicationDbContext(options, config);
            context.Categories.Add(new Category { Name = "Test Category" });
            context.SaveChanges();
            var repo = new ItemRepo(config);
            var items = new List<Item>
            {
                new Item { Id = 1, Name = "Test Item 1" },
                new Item { Id = 2, Name = "Test Item 2" },
                new Item { Id = 3, Name = "Test Item 3" }
            };
            // Act
            repo.AddRange(items);
            // Assert
            foreach (var item in items)
            {
                var result = repo.Find(item.Id);
                Assert.NotNull(result);
                Assert.Equal(item.Id, result.Id);
                Assert.Equal(item.Name, result.Name);
            }
        }
        //int Update(T entity, bool persist = true);
        [Fact]
        public void Update_Should_Modify_Existing_Item()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            var config = new ConfigurationBuilder().Build();
            using var context = new ApplicationDbContext(options, config);
            context.Categories.Add(new Category { Name = "Test Category" });
            context.SaveChanges();
            var repo = new ItemRepo(config);
            var item = new Item { Id = 1, Name = "Test Item" };
            repo.Add(item);
            // Act
            item.Name = "Updated Item";
            repo.Update(item);
            var result = repo.Find(1);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(item.Id, result.Id);
            Assert.Equal("Updated Item", result.Name);
        }
        //int UpdateRange(IEnumerable<T> entities, bool persist = true);
        [Fact]
        public void UpdateRange_Should_Modify_Existing_Items()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            var config = new ConfigurationBuilder().Build();
            using var context = new ApplicationDbContext(options, config);
            context.Categories.Add(new Category { Name = "Test Category" });
            context.SaveChanges();
            var repo = new ItemRepo(config);
            var items = new List<Item>
            {
                new Item { Id = 1, Name = "Test Item 1" },
                new Item { Id = 2, Name = "Test Item 2" },
                new Item { Id = 3, Name = "Test Item 3" }
            };
            repo.AddRange(items);
            // Act
            items[0].Name = "Updated Item 1";
            items[1].Name = "Updated Item 2";
            items[2].Name = "Updated Item 3";
            repo.UpdateRange(items);
            // Assert
            foreach (var item in items)
            {
                var result = repo.Find(item.Id);
                Assert.NotNull(result);
                Assert.Equal(item.Id, result.Id);
                Assert.Equal($"Updated Item {item.Id}", result.Name);
            }
        }
        //int Delete(T entity, bool persist = true);
        [Fact]
        public void Delete_Should_Remove_Item_From_Repo()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            var config = new ConfigurationBuilder().Build();
            using var context = new ApplicationDbContext(options, config);
            context.Categories.Add(new Category { Name = "Test Category" });
            context.SaveChanges();
            var repo = new ItemRepo(config);
            var item = new Item { Id = 1, Name = "Test Item" };
            repo.Add(item);
            // Act
            repo.Delete(item);
            var result = repo.Find(1);
            // Assert
            Assert.Null(result);
        }
        //int Delete(int id, long timestamp, bool persist = true);
        [Fact]
        public void Delete_Should_Remove_Item_By_Id()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            var config = new ConfigurationBuilder().Build();
            using var context = new ApplicationDbContext(options, config);
            context.Categories.Add(new Category { Name = "Test Category" });
            context.SaveChanges();
            var repo = new ItemRepo(config);
            var item = new Item { Id = 1, Name = "Test Item" };
            repo.Add(item);
            // Act
            repo.Delete(1, item.TimeStamp);
            var result = repo.Find(1);
            // Assert
            Assert.Null(result);
        }
        //int DeleteRange(IEnumerable<T> entities, bool persist = true);
        [Fact]
        public void DeleteRange_Should_Remove_Items_From_Repo()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            var config = new ConfigurationBuilder().Build();
            using var context = new ApplicationDbContext(options, config);
            context.Categories.Add(new Category { Name = "Test Category" });
            context.SaveChanges();
            var repo = new ItemRepo(config);
            var items = new List<Item>
            {
                new Item { Id = 1, Name = "Test Item 1" },
                new Item { Id = 2, Name = "Test Item 2" },
                new Item { Id = 3, Name = "Test Item 3" }
            };
            repo.AddRange(items);
            // Act
            repo.DeleteRange(items);
            // Assert
            foreach (var item in items)
            {
                var result = repo.Find(item.Id);
                Assert.Null(result);
            }
        }
    }
}
