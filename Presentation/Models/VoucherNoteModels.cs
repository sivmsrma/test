using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Terret_Billing.Presentation.Models
{
    public class VoucherNoteModel : INotifyPropertyChanged
    {
        private string _voucherNoteNumber;
        public string VoucherNoteNumber { get => _voucherNoteNumber; set { _voucherNoteNumber = value; OnPropertyChanged(); } }
        private string _barcode;
        public string Barcode { get => _barcode; set { _barcode = value; OnPropertyChanged(); } }
        private string _category;
        public string Category { get => _category; set { _category = value; OnPropertyChanged(); } }
        private string _subCategory;
        public string SubCategory { get => _subCategory; set { _subCategory = value; OnPropertyChanged(); } }
        private string _design;
        public string Design { get => _design; set { _design = value; OnPropertyChanged(); } }
        private string _purity;
        public string Purity { get => _purity; set { _purity = value; OnPropertyChanged(); } }
        private string _hsnNo;
        public string HSNNo { get => _hsnNo; set { _hsnNo = value; OnPropertyChanged(); } }
        private string _huidNo;
        public string HUIDNo { get => _huidNo; set { _huidNo = value; OnPropertyChanged(); } }
        private string _size;
        public string Size { get => _size; set { _size = value; OnPropertyChanged(); } }
        private string _pcs;
        public string PCs { get => _pcs; set { _pcs = value; OnPropertyChanged(); } }
        private string _grossWt;
        public string GrossWt { get => _grossWt; set { _grossWt = value; OnPropertyChanged(); } }
        private string _lessWt;
        public string LessWt { get => _lessWt; set { _lessWt = value; OnPropertyChanged(); } }
        private string _netWt;
        public string NetWt { get => _netWt; set { _netWt = value; OnPropertyChanged(); } }
        private string _diamondCt;
        public string DiamondCt { get => _diamondCt; set { _diamondCt = value; OnPropertyChanged(); } }
        private string _diamondWtGm;
        public string DiamondWtGm { get => _diamondWtGm; set { _diamondWtGm = value; OnPropertyChanged(); } }
        private string _tunchPercent;
        public string TunchPercent { get => _tunchPercent; set { _tunchPercent = value; OnPropertyChanged(); } }
        private string _tunchWt;
        public string TunchWt { get => _tunchWt; set { _tunchWt = value; OnPropertyChanged(); } }
        private string _wastePercent;
        public string WastePercent { get => _wastePercent; set { _wastePercent = value; OnPropertyChanged(); } }
        private string _wasteFAmount;
        public string WasteFAmount { get => _wasteFAmount; set { _wasteFAmount = value; OnPropertyChanged(); } }
        private string _stoneCt;
        public string StoneCt { get => _stoneCt; set { _stoneCt = value; OnPropertyChanged(); } }
        private string _stoneWtGm;
        public string StoneWtGm { get => _stoneWtGm; set { _stoneWtGm = value; OnPropertyChanged(); } }
        private string _finalWt;
        public string FinalWt { get => _finalWt; set { _finalWt = value; OnPropertyChanged(); } }
        private string _comment;
        public string Comment { get => _comment; set { _comment = value; OnPropertyChanged(); } }
        private string _phoneNumber;
        public string PhoneNumber { get => _phoneNumber; set { _phoneNumber = value; OnPropertyChanged(); } }
        private string _shopName;
        public string ShopName { get => _shopName; set { _shopName = value; OnPropertyChanged(); } }
        private string _state;
        public string State { get => _state; set { _state = value; OnPropertyChanged(); } }
        private string _gstin;
        public string GSTIN { get => _gstin; set { _gstin = value; OnPropertyChanged(); } }
        private string _address;
        public string Address { get => _address; set { _address = value; OnPropertyChanged(); } }
        private string _mobile;
        public string Mobile { get => _mobile; set { _mobile = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 