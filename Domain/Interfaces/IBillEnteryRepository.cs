using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terret_Billing.Presentation.Models;
using Terret_Billing.Domain.Entities;

namespace Terret_Billing.Domain.Interfaces
{
    public interface IBillRepository
    {
        Task AddAsync(Terret_Billing.Presentation.Models.Request.BillRequest billRequest);
        Task<(Bill Bill, List<BillItem> Items, List<BillPayment> Payments, Branch Company)> GetFullBillForPrintAsync(int billId, string firmId);
    }
}
