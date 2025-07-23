using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Terret_Billing.Presentation.Models
{
    public class DiamondItemModel : INotifyPropertyChanged
    {
        private string _PartyName;
        private string _InvoiceNumber;
        private string _Particular;
        private string _MetalType;
        private string _category;
        private string _subCategory;
        private string _design;
        private string _diaE_HsnNo;
        private string _huidNo;
        private string _barcode;
        private int? _itemId;
        private decimal _diaENetWt;
        private decimal _weight;
        private decimal _carat;
        private string _clarity;
        private string _color;
        private string _cut;
        private string _shape;
        private string _polish;
        private string _symmetry;
        private string _fluorescence;
        private decimal _length;
        private decimal _width;
        private decimal _depth;
        private decimal _depthPercentage;
        private decimal _tablePercentage;
        private string _certificateNumber;
        private string _certificateType;
        private System.DateTime? _certificateDate;
        private decimal _purchasePrice;
        private decimal _sellingPrice;
        private decimal _makingCharges;
        private decimal _totalPrice;
        private string _origin;
        private string _comments;
        private bool _isAvailable;
        private System.DateTime _createdDate;
        private System.DateTime? _modifiedDate;
        private int? _firmId;
        private string _partyName;
        private string _invoiceNo;
        private string _pcs;
        private string _size;
        private string _diaEStockType;
        private string _purity;
        private decimal? _grossWt;
        private decimal? _diamondCt;
        private decimal? _diamondWtGm;
        private decimal? _diamondRate;
        private decimal? _diamondAmt;
        private decimal? _netRate;
        private decimal? _netAmt;
        private decimal? _stoneCt;
        private decimal? _stoneWt;
        private decimal? _stoneAmt;
        private string _description;
        private string _dropDown;
        private decimal? _otherCharges;
        private decimal? _value;
        private decimal? _finalAmount;
        private decimal? _purchaseAmount;
        private string _remarks;
        private string _bucket;

        public string Particular
        {
            get => _Particular;
            set
            {
                if (_Particular != value)
                {
                    _Particular = value;
                    OnPropertyChanged();
                }
            }
        }
        public string InvoiceNumber
        {
            get => _InvoiceNumber;
            set
            {
                if (_InvoiceNumber != value)
                {
                    _InvoiceNumber = value;
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
        

        public string MetalType
        {
            get => _MetalType;
            set
            {
                if (_MetalType != value)
                {
                    _MetalType = value;
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

        public string HsnNo
        {
            get => _diaE_HsnNo;
            set
            {
                if (_diaE_HsnNo != value)
                {
                    _diaE_HsnNo = value;
                    OnPropertyChanged();
                }
            }
        }

        public string HUID_No
        {
            get => _huidNo;
            set
            {
                if (_huidNo != value)
                {
                    _huidNo = value;
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

        public decimal Dia_E_Net_wt
        {
            get => _diaENetWt;
            set
            {
                if (_diaENetWt != value)
                {
                    _diaENetWt = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal Weight
        {
            get => _weight;
            set
            {
                if (_weight != value)
                {
                    _weight = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal Carat
        {
            get => _carat;
            set
            {
                if (_carat != value)
                {
                    _carat = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Clarity
        {
            get => _clarity;
            set
            {
                if (_clarity != value)
                {
                    _clarity = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Color
        {
            get => _color;
            set
            {
                if (_color != value)
                {
                    _color = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Cut
        {
            get => _cut;
            set
            {
                if (_cut != value)
                {
                    _cut = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Shape
        {
            get => _shape;
            set
            {
                if (_shape != value)
                {
                    _shape = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Polish
        {
            get => _polish;
            set
            {
                if (_polish != value)
                {
                    _polish = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Symmetry
        {
            get => _symmetry;
            set
            {
                if (_symmetry != value)
                {
                    _symmetry = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Fluorescence
        {
            get => _fluorescence;
            set
            {
                if (_fluorescence != value)
                {
                    _fluorescence = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal Length
        {
            get => _length;
            set
            {
                if (_length != value)
                {
                    _length = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal Width
        {
            get => _width;
            set
            {
                if (_width != value)
                {
                    _width = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal Depth
        {
            get => _depth;
            set
            {
                if (_depth != value)
                {
                    _depth = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal DepthPercentage
        {
            get => _depthPercentage;
            set
            {
                if (_depthPercentage != value)
                {
                    _depthPercentage = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal TablePercentage
        {
            get => _tablePercentage;
            set
            {
                if (_tablePercentage != value)
                {
                    _tablePercentage = value;
                    OnPropertyChanged();
                }
            }
        }

        public string CertificateNumber
        {
            get => _certificateNumber;
            set
            {
                if (_certificateNumber != value)
                {
                    _certificateNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        public string CertificateType
        {
            get => _certificateType;
            set
            {
                if (_certificateType != value)
                {
                    _certificateType = value;
                    OnPropertyChanged();
                }
            }
        }

        public System.DateTime? CertificateDate
        {
            get => _certificateDate;
            set
            {
                if (_certificateDate != value)
                {
                    _certificateDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal PurchasePrice
        {
            get => _purchasePrice;
            set
            {
                if (_purchasePrice != value)
                {
                    _purchasePrice = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal SellingPrice
        {
            get => _sellingPrice;
            set
            {
                if (_sellingPrice != value)
                {
                    _sellingPrice = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal MakingCharges
        {
            get => _makingCharges;
            set
            {
                if (_makingCharges != value)
                {
                    _makingCharges = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal TotalPrice
        {
            get => _totalPrice;
            set
            {
                if (_totalPrice != value)
                {
                    _totalPrice = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Origin
        {
            get => _origin;
            set
            {
                if (_origin != value)
                {
                    _origin = value;
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

        public bool IsAvailable
        {
            get => _isAvailable;
            set
            {
                if (_isAvailable != value)
                {
                    _isAvailable = value;
                    OnPropertyChanged();
                }
            }
        }

        public System.DateTime CreatedDate
        {
            get => _createdDate;
            set
            {
                if (_createdDate != value)
                {
                    _createdDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public System.DateTime? ModifiedDate
        {
            get => _modifiedDate;
            set
            {
                if (_modifiedDate != value)
                {
                    _modifiedDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public int? FirmId
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

        public string Pcs
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

        public string Dia_E_Stock_Type
        {
            get => _diaEStockType;
            set
            {
                if (_diaEStockType != value)
                {
                    _diaEStockType = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal? Dia_E_Gross_wt
        {
            get => _grossWt;
            set
            {
                if (_grossWt != value)
                {
                    _grossWt = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal? Dia_E_Diamond_Ct
        {
            get => _diamondCt;
            set
            {
                if (_diamondCt != value)
                {
                    _diamondCt = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal? Dia_E_Diamond_wt_gm
        {
            get => _diamondWtGm;
            set
            {
                if (_diamondWtGm != value)
                {
                    _diamondWtGm = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal? Dia_E_Diamond_Rate
        {
            get => _diamondRate;
            set
            {
                if (_diamondRate != value)
                {
                    _diamondRate = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal? Dia_E_Diamond_Amt
        {
            get => _diamondAmt;
            set
            {
                if (_diamondAmt != value)
                {
                    _diamondAmt = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal? Dia_E_Net_Rate
        {
            get => _netRate;
            set
            {
                if (_netRate != value)
                {
                    _netRate = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal? Dia_E_Net_Amt
        {
            get => _netAmt;
            set
            {
                if (_netAmt != value)
                {
                    _netAmt = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal? Dia_E_Stone_Ct
        {
            get => _stoneCt;
            set
            {
                if (_stoneCt != value)
                {
                    _stoneCt = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal? Dia_E_Stone_Wt
        {
            get => _stoneWt;
            set
            {
                if (_stoneWt != value)
                {
                    _stoneWt = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal? Dia_E_Stone_Amt
        {
            get => _stoneAmt;
            set
            {
                if (_stoneAmt != value)
                {
                    _stoneAmt = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Drop_Down
        {
            get => _dropDown;
            set
            {
                if (_dropDown != value)
                {
                    _dropDown = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal? Other_Charges
        {
            get => _otherCharges;
            set
            {
                if (_otherCharges != value)
                {
                    _otherCharges = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal? Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal? Final_Amount
        {
            get => _finalAmount;
            set
            {
                if (_finalAmount != value)
                {
                    _finalAmount = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal? Purchase_Amount
        {
            get => _purchaseAmount;
            set
            {
                if (_purchaseAmount != value)
                {
                    _purchaseAmount = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Remarks
        {
            get => _remarks;
            set
            {
                if (_remarks != value)
                {
                    _remarks = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Bucket
        {
            get => _bucket;
            set
            {
                if (_bucket != value)
                {
                    _bucket = value;
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