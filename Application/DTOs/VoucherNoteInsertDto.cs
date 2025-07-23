using System;

namespace Terret_Billing.Application.DTOs
{
    public class VoucherNoteInsertDto
    {
        public string VoucherNoteNumber { get; set; }
        public string UserName { get; set; }
        public string FirmId { get; set; }
        public string AssignedBranch { get; set; }
        public string PhoneNumber { get; set; }
        public string ShopName { get; set; }
        public string State { get; set; }
        public string GSTIN { get; set; }
        public string Address { get; set; }
        public string UserNameR { get; set; }
        public string FirmIdR { get; set; }
        public string AssignedBranchR { get; set; }
        public string PhoneNumberR { get; set; }
        public string ShopNameR { get; set; }
        public string StateR { get; set; }
        public string GSTINR { get; set; }
        public string AddressR { get; set; }
        public DateTime Date { get; set; }
        public string Barcode { get; set; }
    }
} 