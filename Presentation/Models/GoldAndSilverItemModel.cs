using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Terret_Billing.Presentation.Models
{
    public class GoldAndSilverItemModel : INotifyPropertyChanged
    {
        private string _metalType;
        private string _category;
        private string _subCategory;
        private string _design;
        private string _hsnNo;
        private string _barcode;
        private int? _itemId;
        private decimal _grossWt;
        private decimal _lessWt;
        private decimal _netWt;
        private string _purity;
        private string _size;
        private int _pcs;
        private string _firmId;
        private string _partyName;
        private string _invoiceNo;

        public string MetalType
        {
            get => _metalType;
            set
            {
                if (_metalType != value)
                {
                    _metalType = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Category
        {
            get => _category;
            set
            {
                if (_category != value)
                {
                    _category = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SubCategory
        {
            get => _subCategory;
            set
            {
                if (_subCategory != value)
                {
                    _subCategory = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Design
        {
            get => _design;
            set
            {
                if (_design != value)
                {
                    _design = value;
                    OnPropertyChanged();
                }
            }
        }

        public string HSNNo
        {
            get => _hsnNo;
            set
            {
                if (_hsnNo != value)
                {
                    _hsnNo = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Barcode
        {
            get => _barcode;
            set
            {
                if (_barcode != value)
                {
                    _barcode = value;
                    OnPropertyChanged();
                }
            }
        }

        public int? ItemId
        {
            get => _itemId;
            set
            {
                if (_itemId != value)
                {
                    _itemId = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal GrossWt
        {
            get => _grossWt;
            set
            {
                if (_grossWt != value)
                {
                    _grossWt = value;
                    OnPropertyChanged();
                    UpdateNetWeight();
                }
            }
        }

        public decimal LessWt
        {
            get => _lessWt;
            set
            {
                if (_lessWt != value)
                {
                    _lessWt = value;
                    OnPropertyChanged();
                    UpdateNetWeight();
                }
            }
        }

        public decimal NetWt
        {
            get => _netWt;
            private set
            {
                if (_netWt != value)
                {
                    _netWt = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Purity
        {
            get => _purity;
            set
            {
                if (_purity != value)
                {
                    _purity = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Size
        {
            get => _size;
            set
            {
                if (_size != value)
                {
                    _size = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Pcs
        {
            get => _pcs;
            set
            {
                if (_pcs != value)
                {
                    _pcs = value;
                    OnPropertyChanged();
                }
            }
        }

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

        public string PartyName
        {
            get => _partyName;
            set
            {
                if (_partyName != value)
                {
                    _partyName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string InvoiceNo
        {
            get => _invoiceNo;
            set
            {
                if (_invoiceNo != value)
                {
                    _invoiceNo = value;
                    OnPropertyChanged();
                }
            }
        }

        private void UpdateNetWeight()
        {
            NetWt = GrossWt - LessWt;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}