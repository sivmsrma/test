using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Terret_Billing.Presentation.Models
{
    public class Payment : INotifyPropertyChanged
    {
        private int _id;
        private int _partyId;
        private int? _billId;
        private decimal _amount;
        private DateTime _paymentDate;
        private string _paymentMode;
        private string _referenceNumber;
        private string _remarks;
        private DateTime _createdAt;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public int PartyId
        {
            get => _partyId;
            set
            {
                _partyId = value;
                OnPropertyChanged();
            }
        }

        public int? BillId
        {
            get => _billId;
            set
            {
                _billId = value;
                OnPropertyChanged();
            }
        }

        public decimal Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged();
            }
        }

        public DateTime PaymentDate
        {
            get => _paymentDate;
            set
            {
                _paymentDate = value;
                OnPropertyChanged();
            }
        }

        public string PaymentMode
        {
            get => _paymentMode;
            set
            {
                _paymentMode = value;
                OnPropertyChanged();
            }
        }

        public string ReferenceNumber
        {
            get => _referenceNumber;
            set
            {
                _referenceNumber = value;
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

        public DateTime CreatedAt
        {
            get => _createdAt;
            set
            {
                _createdAt = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}