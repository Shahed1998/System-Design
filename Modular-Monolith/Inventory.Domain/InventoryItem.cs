using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Domain
{
    public class InventoryItem
    {
        public Guid Id { get; private set; }
        public string? SKU { get; private set; }
        public int Quantity { get; private set; }

        public InventoryItem(Guid id, string? sku, int initialQuantity) 
        {
            if(string.IsNullOrEmpty(SKU)) throw new ArgumentException("SKU can't be empty");
            
            if(initialQuantity < 0) throw new ArgumentException("Initital Quantity can't be empty");

            Id = id;
            SKU = sku;
            Quantity = initialQuantity;
            
        }
    }
}
