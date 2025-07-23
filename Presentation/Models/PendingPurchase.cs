using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Terret_Billing.Presentation.Models
{
    public class PendingPurchase : INotifyPropertyChanged
    {
        private int _id;
        private string _billNo;
        private DateTime _purchaseDate;
        private decimal _netAmount;
        private decimal _paidAmount;
        private decimal _remainingAmount;

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

        public DateTime PurchaseDate
        {
            get => _purchaseDate;
            set
            {
                _purchaseDate = value;
                OnPropertyChanged();
            }
        }

        public decimal NetAmount
        {
            get => _netAmount;
            set
            {
                _netAmount = value;
                CalculateRemainingAmount();
                OnPropertyChanged();
            }
        }

        public decimal PaidAmount
        {
            get => _paidAmount;
            set
            {
                _paidAmount = value;
                CalculateRemainingAmount();
                OnPropertyChanged();
            }
        }

        public decimal RemainingAmount
        {
            get => _remainingAmount;
            set
            {
                _remainingAmount = value;
                OnPropertyChanged();
            }
        }

        private void CalculateRemainingAmount()
        {
            RemainingAmount = NetAmount - PaidAmount;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}