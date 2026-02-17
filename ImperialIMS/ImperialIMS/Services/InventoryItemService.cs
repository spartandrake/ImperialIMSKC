using ImperialIMS.Models;
using ImperialIMS.Repos;

namespace ImperialIMS.Services
{
    public class InventoryItemService : ServiceBase<InventoryItem>
    {
        private readonly ILogger<InventoryItem> _logger;
        public InventoryItemService(IRepo<InventoryItem> repo, IConfiguration configuration, ILogger<InventoryItem> logger) : base(repo, configuration, logger)
        {
        }
        private void DecrementStock(InventoryItem item, int quantity)
        {
            if (quantity <= 0)
            {
                _logger.LogWarning("Attempted to decrement stock for {ItemName} by a non-positive quantity: {Quantity}.", item.Item.Name, quantity);
                return;
            }
            else if (quantity > item.StockCount)
            {
                _logger.LogWarning("Attempted to decrement stock for {ItemName} by {Quantity}, but only {AvailableQuantity} available.", item.Item.Name, quantity, item.StockCount);
                return;
            }
            else
            {
                item.StockCount -= quantity;
                _logger.LogInformation("Decremented stock for {ItemName} by {Quantity}. New stock count: {NewStockCount}.", item.Item.Name, quantity, item.StockCount);
            }
            if (item.ReorderLevel <= item.StockCount)
            {
                //order more stock
                //alert regional administrator
            }
            else
            {
                _logger.LogWarning("Attempted to decrement stock for {ItemName} by {Quantity}, but only {AvailableQuantity} available.", item.Item.Name, quantity, item.StockCount);
            }
        }
        private void IncrementStock(InventoryItem item, int quantity)
        {
            if (quantity <= 0)
            {
                _logger.LogWarning("Attempted to increment stock for {ItemName} by a non-positive quantity: {Quantity}.", item.Item.Name, quantity);
                return;
            }
            else
            {
                item.StockCount += quantity;
                _logger.LogInformation("Incremented stock for {ItemName} by {Quantity}. New stock count: {NewStockCount}.", item.Item.Name, quantity, item.StockCount);
            }
            if (item.StockCount >= item.MaxStockLevel)
            {
                //create alert for overstock
                _logger.LogWarning("Stock for {ItemName} has reached or exceeded the maximum stock level of {MaxStockLevel}. Current stock count: {CurrentStockCount}.", item.Item.Name, item.MaxStockLevel, item.StockCount);
            }
        }
    }
}
