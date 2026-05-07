using ImperialIMS.Models;
using ImperialIMS.Repos;
using ImperialIMS.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace ImperialIMS.Tests
{
    public class InventoryItemServiceTests
    {
        private readonly IRepo<InventoryItem> _repo;
        private readonly IRepo<InventoryHistory> _history;
        private readonly InventoryItemService _svc;

        public InventoryItemServiceTests()
        {
            _repo = Substitute.For<IRepo<InventoryItem>>();
            _history = Substitute.For<IRepo<InventoryHistory>>();
            var logger = Substitute.For<ILogger<InventoryItem>>();
            var config = new ConfigurationBuilder().Build();
            _svc = new InventoryItemService(_repo, _history, config, logger);
        }

        private static InventoryItem MakeInventoryItem(int id, int stock, int max = 100, int reorder = 5) =>
            new InventoryItem
            {
                Id = id,
                StockCount = stock,
                MaxStockLevel = max,
                ReorderLevel = reorder,
                Item = new Item { Id = id, Name = $"Item {id}" }
            };

        // --- UpdateStock: item not found ---

        [Fact]
        public void UpdateStock_Does_Nothing_When_Item_Not_Found()
        {
            _repo.Find(99).Returns((InventoryItem)null);

            _svc.UpdateStock(99, 5, true);

            _repo.DidNotReceive().Update(Arg.Any<InventoryItem>());
            _history.DidNotReceive().Add(Arg.Any<InventoryHistory>());
        }

        // --- UpdateStock: increment ---

        [Fact]
        public void UpdateStock_Increments_StockCount_By_Quantity()
        {
            var item = MakeInventoryItem(1, stock: 10);
            _repo.Find(1).Returns(item);

            _svc.UpdateStock(1, 5, isIncrement: true);

            _repo.Received(1).Update(Arg.Is<InventoryItem>(i => i.StockCount == 15));
        }

        [Fact]
        public void UpdateStock_Creates_History_Record_On_Increment()
        {
            var item = MakeInventoryItem(1, stock: 10);
            _repo.Find(1).Returns(item);

            _svc.UpdateStock(1, 5, isIncrement: true);

            _history.Received(1).Add(Arg.Is<InventoryHistory>(h =>
                h.OldStock == 10 && h.NewStock == 15 && h.ChangeReason == "Increment"));
        }

        [Fact]
        public void UpdateStock_Does_Not_Increment_When_Quantity_Is_Zero()
        {
            var item = MakeInventoryItem(1, stock: 10);
            _repo.Find(1).Returns(item);

            _svc.UpdateStock(1, 0, isIncrement: true);

            _repo.Received(1).Update(Arg.Is<InventoryItem>(i => i.StockCount == 10));
            _history.DidNotReceive().Add(Arg.Any<InventoryHistory>());
        }

        [Fact]
        public void UpdateStock_Does_Not_Increment_When_Quantity_Is_Negative()
        {
            var item = MakeInventoryItem(1, stock: 10);
            _repo.Find(1).Returns(item);

            _svc.UpdateStock(1, -3, isIncrement: true);

            _repo.Received(1).Update(Arg.Is<InventoryItem>(i => i.StockCount == 10));
            _history.DidNotReceive().Add(Arg.Any<InventoryHistory>());
        }

        // --- UpdateStock: decrement ---

        [Fact]
        public void UpdateStock_Decrements_StockCount_By_Quantity()
        {
            var item = MakeInventoryItem(1, stock: 10);
            _repo.Find(1).Returns(item);

            _svc.UpdateStock(1, 4, isIncrement: false);

            _repo.Received(1).Update(Arg.Is<InventoryItem>(i => i.StockCount == 6));
        }

        [Fact]
        public void UpdateStock_Creates_History_Record_On_Decrement()
        {
            var item = MakeInventoryItem(1, stock: 10);
            _repo.Find(1).Returns(item);

            _svc.UpdateStock(1, 4, isIncrement: false);

            _history.Received(1).Add(Arg.Is<InventoryHistory>(h =>
                h.OldStock == 10 && h.NewStock == 6 && h.ChangeReason == "Decrement"));
        }

        [Fact]
        public void UpdateStock_Does_Not_Decrement_When_Quantity_Exceeds_Stock()
        {
            var item = MakeInventoryItem(1, stock: 5);
            _repo.Find(1).Returns(item);

            _svc.UpdateStock(1, 10, isIncrement: false);

            _repo.Received(1).Update(Arg.Is<InventoryItem>(i => i.StockCount == 5));
            _history.DidNotReceive().Add(Arg.Any<InventoryHistory>());
        }

        [Fact]
        public void UpdateStock_Does_Not_Decrement_When_Quantity_Is_Zero_Or_Negative()
        {
            var item = MakeInventoryItem(1, stock: 10);
            _repo.Find(1).Returns(item);

            _svc.UpdateStock(1, 0, isIncrement: false);

            _repo.Received(1).Update(Arg.Is<InventoryItem>(i => i.StockCount == 10));
            _history.DidNotReceive().Add(Arg.Any<InventoryHistory>());
        }
    }
}
