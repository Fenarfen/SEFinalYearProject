using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustardRM.Common.Models.Entities;

public class Inventory
{
    public class StockItem
    {
        public int ID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal CostPrice { get; set; }
        public int StockLevel { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class Review
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int StockItemID { get; set; }
        public string Body { get; set; }
        public float Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class Subcategory
    {
        public int ID { get; set; }
        public int CategoryID { get; set; }
        public string SubcategoryName { get; set; }
        public string SubcategoryDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
