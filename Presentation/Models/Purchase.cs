using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Terret_Billing.Presentation.Models
{
    public class Purchase : INotifyPropertyChanged
    {
        private int _id;
        private string _billNo;
        private DateTime _purchaseDate = DateTime.Now;

        private int _supplierId;
        private string _remarks;
        private decimal _totalAmount;
        private decimal _tax;
        private decimal _taxAmount;
        private decimal _discount;
        private decimal _netAmount;
        public long? LocalId { get; set; }
        public string? firm_id { get; set; }
        public string? Admin_user { get; set; }
        public string updated_by { get; set; } = string.Empty;
        public DateTime updated_time { get; set; } = DateTime.Now;
        public string? Party_name { get; set; }
        public int _createdby { get; set; }


        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public string BillNo
        {
            get => _billNo;
            set
            {
                _billNo = value;
                OnPropertyChanged();
            }
        }
        public int Createdby
        {
            get => _createdby;
            set
            {
                _createdby = value;
                
            }
        }
        public DateTime PurchaseDate
        {
            get => _purchaseDate;
            set
            {
                _purchaseDate = value;
                OnPropertyChanged();
            }
        }


        public int SupplierId
        {
            get => _supplierId;
            set
            {
                _supplierId = value;
                OnPropertyChanged();
            }
        }

        public string Remarks
        {
            get => _remarks;
            set
            {
                _remarks = value;
                OnPropertyChanged();
            }
        }

        public decimal TotalAmount
        {
            get => _totalAmount;
            set
            {
                _totalAmount = value;
                OnPropertyChanged();
            }
        }

        public decimal Tax
        {
            get => _tax;
            set
            {
                _tax = value;
                OnPropertyChanged();
            }
        }

        public decimal Discount
        {
            get => _discount;
            set
            {
                // Round to 2 decimal places for currency
                _discount = Math.Round(value, 2);
                CalculateNetAmount();
                OnPropertyChanged();
            }
        }

        public decimal NetAmount
        {
            get => _netAmount;
            set
            {
                _netAmount = value;
                OnPropertyChanged();
            }
        }
        public decimal TaxAmount
        {
            get => _taxAmount;
            set
            {
                _taxAmount = value;
                OnPropertyChanged();
            }
        }
        //public void CalculateTax(decimal taxPercentage)
        //{
        //    if (taxPercentage < 0) taxPercentage = 0;
        //    Tax = Math.Round(TotalAmount * (taxPercentage / 100), 2);
        //    TaxAmount = Tax; // Set TaxAmount to match Tax for backward compatibility
        //    CalculateNetAmount();
        //}

        public void CalculateTotalAmount(decimal itemsTotal, decimal discount = 0)
        {
            TotalAmount = Math.Max(0, itemsTotal);
            Discount = Math.Max(0, discount); // Ensure discount is not negative
            CalculateNetAmount();
        }

        private void CalculateNetAmount()
        {
            decimal total = TotalAmount + TaxAmount;
            // Apply discount (can be positive or negative)
            decimal finalAmount = total + Discount;
            // Ensure amount doesn't go below zero
            NetAmount = Math.Max(0, finalAmount);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
