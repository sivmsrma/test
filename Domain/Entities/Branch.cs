using System;

namespace Terret_Billing.Domain.Entities
{
    public class Branch
    {
        // Database column properties
        public int id { get; set; }
        public string shop_name { get; set; }
        public string manager_name { get; set; }
        public string address { get; set; }
        public string phone_number { get; set; }
        public string email { get; set; }
        public DateTime? created_on { get; set; }
        public int? created_by { get; set; }
        public long? server_id { get; set; }

        // C# naming convention properties
        public int Id { get { return id; } set { id = value; } }
        public string ShopName { get { return shop_name; } set { shop_name = value; } }
        public string ManagerName { get { return manager_name; } set { manager_name = value; } }
        public string Address { get { return address; } set { address = value; } }
        public string PhoneNumber { get { return phone_number; } set { phone_number = value; } }
        public string Email { get { return email; } set { email = value; } }
        public DateTime? CreatedOn { get { return created_on; } set { created_on = value; } }
        public int? CreatedBy { get { return created_by; } set { created_by = value; } }

        // Additional properties for New Branch
        public string firm_id { get; set; }
        public string registration_no { get; set; }
        public string firm_description { get; set; }
        public string state { get; set; }
        public string district { get; set; }
        public string city { get; set; }
        public string pincode { get; set; }
        public string website_name { get; set; }
        public string comments { get; set; }
        public string whatsapp_link { get; set; }
        public string facebook_link { get; set; }
        public string instagram_link { get; set; }
        public string einvoice_api_id { get; set; }
        public string einvoice_api_key { get; set; }
        public string einvoice_username { get; set; }
        public string einvoice_password { get; set; }
        public string payment_bank_details { get; set; }
        public string account_holder_name { get; set; }
        public string payment_bank_ac_no { get; set; }
        public string payment_bank_ifsc_code { get; set; }
        public string payment_declaration { get; set; }
        public DateTime? financial_year_start_date { get; set; }
        public string cash_balance { get; set; }
        public string gstin { get; set; }
        public string pan_number { get; set; }
        public string principal_amt_start { get; set; }
        public string principal_amt_end { get; set; }
        public string form_header { get; set; }
        public string form_footer { get; set; }
        public string firm_type { get; set; }
        public string logo_path { get; set; }
        public string left_image_path { get; set; }
        public string signature_path { get; set; }
        public string qr_code_path { get; set; }
        public long  server_Id { get; set; }
        public bool isDataPostOnServer { get; set; }

        // PascalCase for C#
        public string FirmId { get { return firm_id; } set { firm_id = value; } }
        public string RegistrationNo { get { return registration_no; } set { registration_no = value; } }
        public string FirmDescription { get { return firm_description; } set { firm_description = value; } }
        public string State { get { return state; } set { state = value; } }
        public string District { get { return district; } set { district = value; } }
        public string City { get { return city; } set { city = value; } }
        public string Pincode { get { return pincode; } set { pincode = value; } }
        public string WebsiteName { get { return website_name; } set { website_name = value; } }
        public string Comments { get { return comments; } set { comments = value; } }
        public string WhatsappLink { get { return whatsapp_link; } set { whatsapp_link = value; } }
        public string FacebookLink { get { return facebook_link; } set { facebook_link = value; } }
        public string InstagramLink { get { return instagram_link; } set { instagram_link = value; } }
        public string EInvoiceApiId { get { return einvoice_api_id; } set { einvoice_api_id = value; } }
        public string EInvoiceApiKey { get { return einvoice_api_key; } set { einvoice_api_key = value; } }
        public string EInvoiceUsername { get { return einvoice_username; } set { einvoice_username = value; } }
        public string EInvoicePassword { get { return einvoice_password; } set { einvoice_password = value; } }
        public string PaymentBankDetails { get { return payment_bank_details; } set { payment_bank_details = value; } }
        public string AccountHolderName { get { return account_holder_name; } set { account_holder_name = value; } }
        public string PaymentBankACNo { get { return payment_bank_ac_no; } set { payment_bank_ac_no = value; } }
        public string PaymentBankIFSCCode { get { return payment_bank_ifsc_code; } set { payment_bank_ifsc_code = value; } }
        public string PaymentDeclaration { get { return payment_declaration; } set { payment_declaration = value; } }
        public DateTime? FinancialYearStartDate { get { return financial_year_start_date; } set { financial_year_start_date = value; } }
        public string CashBalance { get { return cash_balance; } set { cash_balance = value; } }
        public string GSTIN { get { return gstin; } set { gstin = value; } }
        public string PANNumber { get { return pan_number; } set { pan_number = value; } }
        public string PrincipalAmtStart { get { return principal_amt_start; } set { principal_amt_start = value; } }
        public string PrincipalAmtEnd { get { return principal_amt_end; } set { principal_amt_end = value; } }
        public string FormHeader { get { return form_header; } set { form_header = value; } }
        public string FormFooter { get { return form_footer; } set { form_footer = value; } }
        public string FirmType { get { return firm_type; } set { firm_type = value; } }
        public string LogoPath { get { return logo_path; } set { logo_path = value; } }
        public string LeftImagePath { get { return left_image_path; } set { left_image_path = value; } }
        public string SignaturePath { get { return signature_path; } set { signature_path = value; } }
        public string QrCodePath { get { return qr_code_path; } set { qr_code_path = value; } }
        public long ServerId { get { return server_Id; } set { server_id = value; } }
        public bool IsDataPostOnServer { get { return isDataPostOnServer; } set { isDataPostOnServer = false; } }
}
}
