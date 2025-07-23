using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Terret_Billing.Presentation.Models
{
    public class BranchModel : INotifyPropertyChanged
    {
        private string _firmId;
        private string _registrationNo;
        private string _shopName;
        private string _firmDescription;
        private string _address;
        private string _state;
        private string _district;
        private string _city;
        private string _pincode;
        private string _phoneNumber;
        private string _email;
        private string _websiteName;
        private string _comments;
        private string _whatsappLink;
        private string _facebookLink;
        private string _instagramLink;
        private string _eInvoiceApiId;
        private string _eInvoiceApiKey;
        private string _eInvoiceUsername;
        private string _eInvoicePassword;
        private string _paymentBankDetails;
        private string _accountHolderName;
        private string _paymentBankACNo;
        private string _paymentBankIFSCCode;
        private string _paymentDeclaration;
        private DateTime? _financialYearStartDate;
        private string _cashBalance;
        private string _gstin;
        private string _panNumber;
        private string _principalAmtStart;
        private string _principalAmtEnd;
        private string _formHeader;
        private string _formFooter;
        private string _firmType;
        private string _logoPath;
        private string _leftImagePath;
        private string _signaturePath;
        private string _qrCodePath;
        private bool _isActive;

        public string FirmId
        {
            get => _firmId;
            set
            {
                if (_firmId != value)
                {
                    _firmId = value;
                    OnPropertyChanged();
                }
            }
        }

        public string RegistrationNo
        {
            get => _registrationNo;
            set
            {
                if (_registrationNo != value)
                {
                    _registrationNo = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ShopName
        {
            get => _shopName;
            set
            {
                if (_shopName != value)
                {
                    _shopName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string FirmDescription
        {
            get => _firmDescription;
            set
            {
                if (_firmDescription != value)
                {
                    _firmDescription = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged();
                }
            }
        }

        public string State
        {
            get => _state;
            set
            {
                if (_state != value)
                {
                    _state = value;
                    OnPropertyChanged();
                }
            }
        }

        public string District
        {
            get => _district;
            set
            {
                if (_district != value)
                {
                    _district = value;
                    OnPropertyChanged();
                }
            }
        }

        public string City
        {
            get => _city;
            set
            {
                if (_city != value)
                {
                    _city = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Pincode
        {
            get => _pincode;
            set
            {
                if (_pincode != value)
                {
                    _pincode = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }

        public string WebsiteName
        {
            get => _websiteName;
            set
            {
                if (_websiteName != value)
                {
                    _websiteName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Comments
        {
            get => _comments;
            set
            {
                if (_comments != value)
                {
                    _comments = value;
                    OnPropertyChanged();
                }
            }
        }

        public string WhatsappLink
        {
            get => _whatsappLink;
            set
            {
                if (_whatsappLink != value)
                {
                    _whatsappLink = value;
                    OnPropertyChanged();
                }
            }
        }

        public string FacebookLink
        {
            get => _facebookLink;
            set
            {
                if (_facebookLink != value)
                {
                    _facebookLink = value;
                    OnPropertyChanged();
                }
            }
        }

        public string InstagramLink
        {
            get => _instagramLink;
            set
            {
                if (_instagramLink != value)
                {
                    _instagramLink = value;
                    OnPropertyChanged();
                }
            }
        }

        public string EInvoiceApiId
        {
            get => _eInvoiceApiId;
            set
            {
                if (_eInvoiceApiId != value)
                {
                    _eInvoiceApiId = value;
                    OnPropertyChanged();
                }
            }
        }

        public string EInvoiceApiKey
        {
            get => _eInvoiceApiKey;
            set
            {
                if (_eInvoiceApiKey != value)
                {
                    _eInvoiceApiKey = value;
                    OnPropertyChanged();
                }
            }
        }

        public string EInvoiceUsername
        {
            get => _eInvoiceUsername;
            set
            {
                if (_eInvoiceUsername != value)
                {
                    _eInvoiceUsername = value;
                    OnPropertyChanged();
                }
            }
        }

        public string EInvoicePassword
        {
            get => _eInvoicePassword;
            set
            {
                if (_eInvoicePassword != value)
                {
                    _eInvoicePassword = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PaymentBankDetails
        {
            get => _paymentBankDetails;
            set
            {
                if (_paymentBankDetails != value)
                {
                    _paymentBankDetails = value;
                    OnPropertyChanged();
                }
            }
        }

        public string AccountHolderName
        {
            get => _accountHolderName;
            set
            {
                if (_accountHolderName != value)
                {
                    _accountHolderName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PaymentBankACNo
        {
            get => _paymentBankACNo;
            set
            {
                if (_paymentBankACNo != value)
                {
                    _paymentBankACNo = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PaymentBankIFSCCode
        {
            get => _paymentBankIFSCCode;
            set
            {
                if (_paymentBankIFSCCode != value)
                {
                    _paymentBankIFSCCode = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PaymentDeclaration
        {
            get => _paymentDeclaration;
            set
            {
                if (_paymentDeclaration != value)
                {
                    _paymentDeclaration = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime? FinancialYearStartDate
        {
            get => _financialYearStartDate;
            set
            {
                if (_financialYearStartDate != value)
                {
                    _financialYearStartDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public string CashBalance
        {
            get => _cashBalance;
            set
            {
                if (_cashBalance != value)
                {
                    _cashBalance = value;
                    OnPropertyChanged();
                }
            }
        }

        public string GSTIN
        {
            get => _gstin;
            set
            {
                if (_gstin != value)
                {
                    _gstin = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PANNumber
        {
            get => _panNumber;
            set
            {
                if (_panNumber != value)
                {
                    _panNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PrincipalAmtStart
        {
            get => _principalAmtStart;
            set
            {
                if (_principalAmtStart != value)
                {
                    _principalAmtStart = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PrincipalAmtEnd
        {
            get => _principalAmtEnd;
            set
            {
                if (_principalAmtEnd != value)
                {
                    _principalAmtEnd = value;
                    OnPropertyChanged();
                }
            }
        }

        public string FormHeader
        {
            get => _formHeader;
            set
            {
                if (_formHeader != value)
                {
                    _formHeader = value;
                    OnPropertyChanged();
                }
            }
        }

        public string FormFooter
        {
            get => _formFooter;
            set
            {
                if (_formFooter != value)
                {
                    _formFooter = value;
                    OnPropertyChanged();
                }
            }
        }

        public string FirmType
        {
            get => _firmType;
            set
            {
                if (_firmType != value)
                {
                    _firmType = value;
                    OnPropertyChanged();
                }
            }
        }

        public string LogoPath
        {
            get => _logoPath;
            set
            {
                if (_logoPath != value)
                {
                    _logoPath = value;
                    OnPropertyChanged();
                }
            }
        }

        public string LeftImagePath
        {
            get => _leftImagePath;
            set
            {
                if (_leftImagePath != value)
                {
                    _leftImagePath = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SignaturePath
        {
            get => _signaturePath;
            set
            {
                if (_signaturePath != value)
                {
                    _signaturePath = value;
                    OnPropertyChanged();
                }
            }
        }

        public string QrCodePath
        {
            get => _qrCodePath;
            set
            {
                if (_qrCodePath != value)
                {
                    _qrCodePath = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 