using System;

namespace Terret_Billing.Domain.Entities
{
    /// <summary>
    /// Entity representing user permissions
    /// </summary>
    public class UserPermission
    {
        // Database column properties
        public int id { get; set; }
        public int user_id { get; set; }
        public int branch_id { get; set; }
        public bool can_create_users { get; set; }
        public bool can_edit_company_settings { get; set; }
        public bool can_view_reports { get; set; }
        public bool can_create_edit_invoices { get; set; }
        public bool can_manage_inventory { get; set; }
        public bool is_active { get; set; }
        public DateTime? created_on { get; set; }
        public int? created_by { get; set; }
        
        // C# naming convention properties
        public int Id { get { return id; } set { id = value; } }
        public int UserId { get { return user_id; } set { user_id = value; } }
        public int BranchId { get { return branch_id; } set { branch_id = value; } }
        public bool CanCreateUsers { get { return can_create_users; } set { can_create_users = value; } }
        public bool CanEditCompanySettings { get { return can_edit_company_settings; } set { can_edit_company_settings = value; } }
        public bool CanViewReports { get { return can_view_reports; } set { can_view_reports = value; } }
        public bool CanCreateEditInvoices { get { return can_create_edit_invoices; } set { can_create_edit_invoices = value; } }
        public bool CanManageInventory { get { return can_manage_inventory; } set { can_manage_inventory = value; } }
        public bool IsActive { get { return is_active; } set { is_active = value; } }
        public DateTime? CreatedOn { get { return created_on; } set { created_on = value; } }
        public int? CreatedBy { get { return created_by; } set { created_by = value; } }
    }
}
