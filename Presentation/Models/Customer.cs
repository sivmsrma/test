using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Terret_Billing.Presentation.Helpers;

namespace Terret_Billing.Presentation.Models
{
    public class Customer : INotifyPropertyChanged
    {
        public int CreatedByUserId { get; set; }
        public long? LocalId { get; set; }
        public string FirmId { get; set; }

        private int _id;
        private string _name;
        private string _mobileNumber;
        private string _gender;
        private string _mailId;
        private string _gstNumber;
        private string _panNumber;
        private string _address;
        private string _state;
        private string _stateCode;
        private string _district;
        private string _city;
        private string _village;      
        private string _pinCode;
        private string _accountNumber;
        private string _accountType;
        private string _ifsc;
        private string _bankName;
        private string _bankBranch;
        private string  _narration;
        private DateTime _createdAt;
        private DateTime _updatedAt;
        private bool _isActive;
        private int _created_by;
            

        /// <summary>
        /// ////  Below is used to set customer properties
        /// </summary>
        

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }
        
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public int CreatedBy
        {
            get => _created_by;
            set
            {
                _created_by = value;
                 
            }
        }


        public string MobileNumber
        {
            get => _mobileNumber;
            set
            {
                _mobileNumber = value;
                OnPropertyChanged();
            }
        }
        
        public string Gender
        {
            get => _gender;
            set
            {
                _gender = value;

                OnPropertyChanged();

            }
        }

        public string MailId
        {
            get => _mailId;
            set
            {
                _mailId = value;
                OnPropertyChanged();
            }
        }

        public string GSTNumber
        {
            get => _gstNumber;
            set
            {
                _gstNumber = value;
                OnPropertyChanged();

                // Auto-extract PAN and State Code from GST
                if (!string.IsNullOrWhiteSpace(value) && value.Length >= 15)
                {
                        // GST Number format: 2 digit state code + 10 digit PAN + remaining digits
                       string gst = value.Trim().ToUpper();

                        // Extract state code (first 2 characters)
                        StateCode = gst.Substring(0, 2);

                        // Extract PAN (characters 3-12)
                        PANNumber =gst.Substring(2, 10);

                    State = GenericHelpers.GetStateName(StateCode);

                }
            }
        }



        public string PANNumber
        {
            get => _panNumber;
            set
            {
                _panNumber = value;
                OnPropertyChanged();
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }

        public string State
        {
            get => _state;
            set
            {
                _state = value;
                OnPropertyChanged();
               
            }
        }

        public string StateCode
        {
            get => _stateCode;
            set
            {
                _stateCode = value;
                OnPropertyChanged();
            }
        }

        public string District
        {
            get => _district;
            set
            {
                _district = value;
                OnPropertyChanged();
            }
        }

        public string City
        {
            get => _city;
            set
            {
                _city = value;
                OnPropertyChanged();
            }
        }

        public string Village
        {
            get => _village;
            set
            {
                _village = value;
                OnPropertyChanged();
            }
        } 

        public string PinCode
        {
            get => _pinCode;
            set
            {
                _pinCode = value;
                OnPropertyChanged();
            }
        }

        public string AccountNumber
        {
            get => _accountNumber;
            set
            {
                _accountNumber = value;
                OnPropertyChanged();
            }
        }

        public string AccountType
        {
            get => _accountType;
            set
            {
                _accountType = value;
                OnPropertyChanged();
            }
        }

        public string Ifsc
        {
            get => _ifsc;
            set
            {
                _ifsc = value;
                OnPropertyChanged();
            }
        }

        public string BankName
        {
            get => _bankName;
            set
            {
                _bankName = value;
                OnPropertyChanged();
            }
        }

        public string BankBranch
        {
            get => _bankBranch;
            set
            {
                _bankBranch = value;
                OnPropertyChanged();
            }
        }

        public string Narration
        {
            get => _narration;
            set
            {
                _narration = value;
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

        public DateTime UpdatedAt
        {
            get => _updatedAt;
            set
            {
                _updatedAt = value;
                OnPropertyChanged();
            }
        }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string error = null;
                switch (columnName)
                {
                    case nameof(Name):
                        if (string.IsNullOrWhiteSpace(Name))
                            error = "Name is required.";
                        break;
                    case nameof(MobileNumber):
                        if (string.IsNullOrWhiteSpace(MobileNumber) || MobileNumber.Length != 10 || !long.TryParse(MobileNumber, out _))
                            error = "Mobile number must be 10 digits.";
                        break;
                    case nameof(Gender):
                        if (string.IsNullOrWhiteSpace(Gender))
                            error = "Gender is required.";
                        break;
                    case nameof(State):
                        if (string.IsNullOrWhiteSpace(State))
                            error = "State is required.";
                        break;
                    case nameof(PinCode):
                        if (string.IsNullOrWhiteSpace(PinCode) || PinCode.Length != 6 || !int.TryParse(PinCode, out _))
                            error = "PIN code must be 6 digits.";
                        break;
                    case nameof(GSTNumber):
                        if (!string.IsNullOrWhiteSpace(GSTNumber) && GSTNumber.Length != 15)
                            error = "GST number must be 15 characters.";
                        break;
                }
                return error;
            }
        }
    }
}