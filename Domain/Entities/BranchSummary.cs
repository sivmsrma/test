namespace Terret_Billing.Domain.Entities
{
    /// <summary>
    /// Entity representing a branch summary with sales, purchases, and profit information
    /// </summary>
    public class BranchSummary
    {
        // Database column properties
        public int branch_id { get; set; }
        public string branch_name { get; set; }
        public string manager_name { get; set; }
        public decimal total_sales { get; set; }
        public decimal total_purchases { get; set; }
        public decimal total_profit { get; set; }
        
        // C# naming convention properties
        public int BranchId { get { return branch_id; } set { branch_id = value; } }
        public string BranchName { get { return branch_name; } set { branch_name = value; } }
        public string ManagerName { get { return manager_name; } set { manager_name = value; } }
        public decimal TotalSales { get { return total_sales; } set { total_sales = value; } }
        public decimal TotalPurchases { get { return total_purchases; } set { total_purchases = value; } }
        public decimal TotalProfit { get { return total_profit; } set { total_profit = value; } }
    }
}
