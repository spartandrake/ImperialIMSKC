using ImperialIMS.Models;
using ImperialIMS.Repos;

namespace ImperialIMS.Services
{
    public class InventoryItemService : ServiceBase<InventoryItem>
    {
        private readonly ILogger<InventoryItem> _logger;
        private readonly IRepo<InventoryItem> _repo;
        private readonly IRepo<InventoryHistory> _history;
        private IConfiguration _configuration { get; set; }
        private InventoryItem _item { get; set; }
        private List<InventoryItem> _items { get; set; }
        public InventoryItemService(IRepo<InventoryItem> repo, IRepo<InventoryHistory> history, IConfiguration configuration, ILogger<InventoryItem> logger) : base(repo, configuration, logger)
        {
            _repo = repo;
            _history = history;
            _logger = logger;
            _configuration = configuration;
        }
        public new void Update(InventoryItem value)
        {
            try
            {
                var old = _repo.Find(value.Id);
                if (old.StockCount != value.StockCount)
                {
                    var historyRecord = new InventoryHistory
                    {
                        InventoryItemId = value.Id,
                        OldStock = old.StockCount,
                        NewStock = value.StockCount,
                        ChangeReason = value.StockCount > old.StockCount ? "Increment" : "Decrement",
                        ChangedAt = DateTime.UtcNow
                    };
                    _history.Add(historyRecord);
                    _logger.LogInformation("Recorded inventory history for {ItemName}: change of {ChangeAmount} on {ChangeDate}.", historyRecord.InventoryItem.Item.Name, historyRecord.NewStock - historyRecord.OldStock, historyRecord.ChangedAt);
                }
                _repo.Update(value);
                _repo.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating value with Id {Id}. " + ex.Message, value.Id);
            }
        }
        public void UpdateStock(int itemId, int quantity, bool isIncrement)
        {
            var item = _repo.Find(itemId);
            if (item == null)
            {
                _logger.LogWarning("Attempted to update stock for InventoryItem with ID {ItemId}, but it was not found.", itemId);
                return;
            }
            int oldStockCount = item.StockCount;
            if (isIncrement)
            {
                IncrementStock(item, quantity);
            }
            else
            {
                DecrementStock(item, quantity);
            }
            _repo.Update(item);
            if(oldStockCount != item.StockCount)
            {
                var historyRecord = new InventoryHistory
                {
                    InventoryItemId = item.Id,
                    OldStock = oldStockCount,
                    NewStock = item.StockCount,
                    ChangeReason = isIncrement ? "Increment" : "Decrement",
                    ChangedAt = DateTime.UtcNow
                };
                _history.Add(historyRecord);
                _logger.LogInformation("Recorded inventory history for {ItemName}: change of {ChangeAmount} on {ChangeDate}.", item.Id, historyRecord.NewStock - historyRecord.OldStock,  historyRecord.ChangedAt);
            }
        }
        private void DecrementStock(InventoryItem item, int quantity)
        {
            if (quantity <= 0)
            {
                _logger.LogWarning("Attempted to decrement stock for {ItemName} by a non-positive quantity: {Quantity}.", item.Id, quantity);
                return;
            }
            else if (quantity > item.StockCount)
            {
                _logger.LogWarning("Attempted to decrement stock for {ItemName} by {Quantity}, but only {AvailableQuantity} available.", item.Id, quantity, item.StockCount);
                return;
            }
            else
            {
                item.StockCount -= quantity;
                _logger.LogInformation("Decremented stock for {ItemName} by {Quantity}. New stock count: {NewStockCount}.", item.Id, quantity, item.StockCount);
            }
            if (item.ReorderLevel <= item.StockCount)
            {
                //order more stock
                //alert regional administrator
            }
            else
            {
                _logger.LogWarning("Attempted to decrement stock for {ItemName} by {Quantity}, but only {AvailableQuantity} available.", item.Id, quantity, item.StockCount);
            }
        }
        private void IncrementStock(InventoryItem item, int quantity)
        {
            if (quantity <= 0)
            {
                _logger.LogWarning("Attempted to increment stock for {ItemName} by a non-positive quantity: {Quantity}.", item.Id, quantity);
                return;
            }
            else
            {
                item.StockCount += quantity;
                _logger.LogInformation("Incremented stock for {ItemName} by {Quantity}. New stock count: {NewStockCount}.", item.Id, quantity, item.StockCount);
            }
            if (item.StockCount >= item.MaxStockLevel)
            {
                //create alert for overstock
                _logger.LogWarning("Stock for {ItemName} has reached or exceeded the maximum stock level of {MaxStockLevel}. Current stock count: {CurrentStockCount}.", item.Id, item.MaxStockLevel, item.StockCount);
            }
        }
    }
}
