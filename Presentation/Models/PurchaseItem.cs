using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Globalization;

namespace Terret_Billing.Presentation.Models
{
    public class PurchaseItem : INotifyPropertyChanged
    {

        public string firm_id { get; set; }

        public string updated_by { get; set; } = string.Empty;
        public string? Admin_user { get; set; }
        
        public DateTime updated_time { get; set; } = DateTime.Now;


        public long? LocalId { get; set; }
        private int? _id;
        private int? _purchaseId;
        private string _metal;
        private string _hsnCode;
        private string _huidNo;
        private string _itemName;
        private string _purity;
        private int? _pcs;
        private decimal? _grossWt;
        private decimal? _diaCt;
        private decimal? _stoneCt;
        private decimal? _lessWt;
        private decimal? _waste;
        private decimal? _netWt;
        private decimal? _rate;
        private decimal? _amount;
        private decimal? _stoneCharge;
        private decimal? _diaCharge;
        private decimal? _drate;
        private decimal? _diaAmount;
        private decimal? _sdiaCt;
        private decimal? _sdiaCharge;
        private decimal? _sdiaAmount;
        private decimal? _stAmount;
        private decimal? _netPrice;
        private decimal? _taxRate;
        private decimal? _taxAmount;
        private decimal? _netAmount;
        private string _taxType;

        private string _rateText;
        private string _grossWtText;
        private string _diaCtText;
        private string _stoneCtText;
        private string _lessWtText;
        private string _wasteText;
        private string _taxRateText;
        private string _stAmountText;
        private string _sdiaAmountText;
        private string _sdiaCtText;
        private string _diaAmountText;
        
        private const decimal CaratToGramConversionFactor = 0.2m; // 1 carat = 0.2 grams

        public int? Id
        {
            get => _id;
            set { _id = value > 0 ? value : null; OnPropertyChanged(); }
        }

        public int? PurchaseId
        {
            get => _purchaseId;
            set { _purchaseId = value > 0 ? value : null; OnPropertyChanged(); }
        }

        public string Metal
        {
            get => _metal;
            set { _metal = string.IsNullOrWhiteSpace(value) ? null : value; OnPropertyChanged(); }
        }

        public string HSNCode
        {
            get => _hsnCode;
            set { _hsnCode = string.IsNullOrWhiteSpace(value) ? null : value; OnPropertyChanged(); }
        }

        public string HuidNo
        {
            get => _huidNo;
            set { _huidNo = string.IsNullOrWhiteSpace(value) ? null : value; OnPropertyChanged(); }
        }

        public string ItemName
        {
            get => _itemName;
            set { _itemName = string.IsNullOrWhiteSpace(value) ? null : value; OnPropertyChanged(); }
        }

        public string Purity
        {
            get => _purity;
            set { _purity = string.IsNullOrWhiteSpace(value) ? null : value; OnPropertyChanged(); }
        }

        public string TaxType
        {
            get => _taxType;
            set { _taxType = string.IsNullOrWhiteSpace(value) ? null : value; OnPropertyChanged(); }
        }

        public int? Pcs
        {
            get => _pcs;
            set { _pcs = value > 0 ? value : null; OnPropertyChanged(); }
        }

        public decimal? GrossWt
        {
            get => _grossWt;
            set { _grossWt = value > 0 ? value : null; CalculateNetWt(); OnPropertyChanged(); }
        }

        public string GrossWtText
        {
            get => _grossWtText ?? _grossWt?.ToString() ?? string.Empty;
            set
            {
                _grossWtText = value;
                GrossWt = decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : null;
                OnPropertyChanged();
            }
        }

        public string StAmountText
        {
            get => _stAmountText ?? StAmount?.ToString("N2") ?? string.Empty;
            set
            {
                _stAmountText = value;
                OnPropertyChanged();
            }
        }

        public string SDiaAmountText
        {
            get => _sdiaAmountText ?? SDiaAmount?.ToString("N2") ?? string.Empty;
            set
            {
                _sdiaAmountText = value;
                OnPropertyChanged();
            }
        }

        public decimal? DiaCt
        {
            get => _diaCt;
            set 
            { 
                if (_diaCt == value) return;
                _diaCt = value >= 0 ? value : null; 
                CalculateChargesAndAmount();
                CalculateAmount(); // Recalculate the total amount
                OnPropertyChanged();
                OnPropertyChanged(nameof(DiaAmount));
                OnPropertyChanged(nameof(DiaAmountText));
            }
        }

        public string DiaCtText
        {
            get => _diaCtText ?? _diaCt?.ToString() ?? string.Empty;
            set
            {
                _diaCtText = value;
                DiaCt = decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : null;
                OnPropertyChanged();
            }
        }

        public decimal? StoneCt
        {
            get => _stoneCt;
            set { _stoneCt = value >= 0 ? value : null; CalculateChargesAndAmount(); OnPropertyChanged(); }
        }

        public string StoneCtText
        {
            get => _stoneCtText ?? _stoneCt?.ToString() ?? string.Empty;
            set
            {
                _stoneCtText = value;
                StoneCt = decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : null;
                OnPropertyChanged();
            }
        }

        public decimal? SDiaCt
        {
            get => _sdiaCt;
            set { _sdiaCt = value >= 0 ? value : null; CalculateChargesAndAmount(); OnPropertyChanged(); }
        }

        public string SDiaCtText
        {
            get => _sdiaCtText ?? _sdiaCt?.ToString() ?? string.Empty;
            set
            {
                _sdiaCtText = value;
                SDiaCt = decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : null;
                OnPropertyChanged();
            }
        }

        public string DiaAmountText
        {
            get => _diaAmountText ?? _diaAmount?.ToString("N2") ?? string.Empty;
            set
            {
                _diaAmountText = value;
                OnPropertyChanged();
            }
        }

        public decimal? LessWt
        {
            get => _lessWt;
            set { _lessWt = value >= 0 ? value : null; CalculateNetWt(); OnPropertyChanged(); }
        }

        public string LessWtText
        {
            get => _lessWtText ?? _lessWt?.ToString() ?? string.Empty;
            set
            {
                _lessWtText = value;
                LessWt = decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : null;
                OnPropertyChanged();
            }
        }

        public decimal? Waste
        {
            get => _waste;
            set
            {
                if (value < 0) throw new ArgumentException("Waste cannot be negative.");
                _waste = value >= 0 ? value : null;
                CalculateNetWt();
                OnPropertyChanged();
            }
        }

        public string WasteText
        {
            get => _wasteText ?? _waste?.ToString() ?? string.Empty;
            set
            {
                _wasteText = value;
                Waste = decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : null;
                OnPropertyChanged();
            }
        }

        public decimal? NetWt
        {
            get => _netWt;
            set
            {
                if (_netWt == value) return;
                _netWt = value >= 0 ? value : null;
                CalculateAmount();
                OnPropertyChanged();
            }
        }

        public decimal? Rate
        {
            get => _rate;
            set { _rate = value > 0 ? value : null; CalculateChargesAndAmount(); OnPropertyChanged(); }
        }

        public string RateText
        {
            get => _rateText ?? _rate?.ToString() ?? string.Empty;
            set
            {
                _rateText = value;
                Rate = decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : null;
                OnPropertyChanged();
            }
        }

        public decimal? Amount
        {
            get => _amount;
            set { _amount = value >= 0 ? value : null; OnPropertyChanged(); CalculateTaxAndNetAmount(); }
        }

        public decimal? StoneCharge
        {
            get => _stoneCharge;
            set { _stoneCharge = value >= 0 ? value : null; CalculateAmount(); OnPropertyChanged(); }
        }

        public decimal? Drate
        {
            get => _drate;
            set 
            { 
                if (_drate == value) return;
                _drate = value >= 0 ? value : null; 
                CalculateChargesAndAmount();
                CalculateAmount(); // Recalculate the total amount
                OnPropertyChanged();
                OnPropertyChanged(nameof(DiaAmount));
                OnPropertyChanged(nameof(DiaAmountText));
            }
        }

        public decimal? DiaAmount
        {
            get => _diaAmount;
            set 
            { 
                _diaAmount = value >= 0 ? value : null; 
                OnPropertyChanged();
            }
        }

        public decimal? DiaCharge
        {
            get => _diaCharge;
            set { _diaCharge = value >= 0 ? value : null; CalculateAmount(); OnPropertyChanged(); }
        }

        public decimal? SDiaCharge
        {
            get => _sdiaCharge;
            set { _sdiaCharge = value >= 0 ? value : null; CalculateAmount(); OnPropertyChanged(); }
        }

        public decimal? SDiaAmount
        {
            get => _sdiaAmount;
            private set 
            { 
                _sdiaAmount = value >= 0 ? value : null; 
                OnPropertyChanged();
            }
        }

        public string _SDiaAmountText
        {
            get => _sdiaAmountText ?? SDiaAmount?.ToString("N2") ?? string.Empty;
            set
            {
                _sdiaAmountText = value;
                OnPropertyChanged();
            }
        }

        public decimal? StAmount
        {
            get => _stAmount;
            private set 
            { 
                _stAmount = value >= 0 ? value : null; 
                OnPropertyChanged();
            }
        }

        public string _StAmountText
        {
            get => _stAmountText ?? StAmount?.ToString("N2") ?? string.Empty;
            set
            {
                _stAmountText = value;
                OnPropertyChanged();
            }
        }

        public decimal? _Drate
        {
            get => _drate;
            set 
            { 
                _drate = value >= 0 ? value : null; 
                CalculateChargesAndAmount(); 
                OnPropertyChanged();
                OnPropertyChanged(nameof(DiaAmount));
            }
        }

        public decimal? NetPrice
        {
            get => _netPrice;
            private set { _netPrice = value >= 0 ? value : null; OnPropertyChanged(); }
        }

        public decimal? TaxRate
        {
            get => _taxRate;
            set { _taxRate = value >= 0 ? value : null; OnPropertyChanged(); CalculateTaxAndNetAmount(); }
        }

        public string TaxRateText
        {
            get => _taxRateText ?? _taxRate?.ToString() ?? string.Empty;
            set
            {
                _taxRateText = value;
                TaxRate = decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : null;
                OnPropertyChanged();
            }
        }

        public decimal? TaxAmount
        {
            get => _taxAmount;
            private set { _taxAmount = value >= 0 ? value : null; OnPropertyChanged(); }
        }

        public decimal? NetAmount
        {
            get => _netAmount;
            private set
            {
                if (_netAmount == value) return;
                _netAmount = value >= 0 ? value : null;
                OnPropertyChanged();
            }
        }

        public void CalculateNetWt()
        {
            if (!GrossWt.HasValue)
            {
                NetWt = null;
                return;
            }

            decimal lessWt = LessWt ?? 0;
            decimal netWt = Math.Max(0, GrossWt.Value - lessWt);
            if (NetWt != netWt)
            {
                NetWt = netWt;
            }
        }

        public void CalculateAmount()
        {
            decimal total = 0;
            if (_netWt.HasValue && _rate.HasValue)
            {
                total += _netWt.Value * _rate.Value;
            }
            if (_diaAmount.HasValue)
            {
                total += _diaAmount.Value;
            }
            if (_stoneCharge.HasValue)
            {
                total += _stoneCharge.Value;
            }

            Amount = total > 0 ? total : null;
        }

        public void CalculateChargesAndAmount()
        {
            // Calculate DiaAmount as DiaCt * Drate
            if (_diaCt.HasValue && _drate.HasValue)
            {
                DiaAmount = _diaCt.Value * _drate.Value;
            }
            else
            {
                DiaAmount = null;
            }

            // Calculate DiaCharge using Rate (₹/gm)
            if (_diaCt.HasValue && _diaCt.Value > 0 && _rate.HasValue)
            {
                decimal diaWeightInGrams = _diaCt.Value * CaratToGramConversionFactor;
                DiaCharge = diaWeightInGrams * _rate.Value;
            }
            else
            {
                DiaCharge = null;
            }

            // Calculate Stone amount (StAmount)
            if (_stoneCt.HasValue && _rate.HasValue)
            {
                decimal stoneWeightInGrams = _stoneCt.Value * CaratToGramConversionFactor;
                StAmount = stoneWeightInGrams * _rate.Value;
            }
            else
            {
                StAmount = null;
            }

            // Calculate Small Diamond amount (SDiaAmount)
            if (_sdiaCt.HasValue && _rate.HasValue)
            {
                decimal sdiaWeightInGrams = _sdiaCt.Value * CaratToGramConversionFactor;
                SDiaAmount = sdiaWeightInGrams * _rate.Value;
            }
            else
            {
                SDiaAmount = null;
            }

            // Calculate StoneCharge using Rate (₹/gm)
            if (_stoneCt.HasValue && _stoneCt.Value > 0 && _rate.HasValue)
            {
                decimal stoneWeightInGrams = _stoneCt.Value * CaratToGramConversionFactor;
                StoneCharge = stoneWeightInGrams * _rate.Value;
            }
            else
            {
                StoneCharge = null;
            }

            // Calculate Small Diamond Charge
            if (_sdiaCt.HasValue && _sdiaCt.Value > 0 && _rate.HasValue)
            {
                decimal sdiaWeightInGrams = _sdiaCt.Value * CaratToGramConversionFactor;
                SDiaCharge = sdiaWeightInGrams * _rate.Value;
            }
            else
            {
                SDiaCharge = null;
            }

            // Trigger amount calculation
            CalculateAmount();
            OnPropertyChanged(nameof(DiaAmount));
            OnPropertyChanged(nameof(DiaCharge));
            OnPropertyChanged(nameof(StAmount));
            OnPropertyChanged(nameof(SDiaAmount));
            OnPropertyChanged(nameof(StoneCharge));
            OnPropertyChanged(nameof(SDiaCharge));
            OnPropertyChanged(nameof(Amount));
            OnPropertyChanged(nameof(SDiaCtText));
            OnPropertyChanged(nameof(SDiaCtText));
        }

        public void CalculateNetPrice()
        {
            decimal total = 0;
            if (_amount.HasValue) total += _amount.Value;
            if (_taxAmount.HasValue) total += _taxAmount.Value;

            NetPrice = total > 0 ? total : null;
        }

        public void CalculateTaxAndNetAmount()
        {
            if (!_amount.HasValue)
            {
                TaxAmount = null;
                NetAmount = null;
                NetPrice = null;
                return;
            }

            decimal taxAmount = 0;
            if (_taxRate.HasValue && _taxRate.Value > 0)
            {
                taxAmount = Math.Round(_amount.Value * (_taxRate.Value / 100), 2);
            }

            TaxAmount = taxAmount;
            NetAmount = _amount.Value + taxAmount;
            CalculateNetPrice();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public decimal? Diact => DiaCt;
        public decimal? StoneWt => StoneCt;
        public string Tax_Type => TaxType;
        public string Tax_Amount => TaxAmount?.ToString("0.00") ?? "0.00";
        
        // Helper property to bind Drate in XAML
        public string DrateText
        {
            get => _drate?.ToString() ?? string.Empty;
            set
            {
                if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var v))
                {
                    _drate = v;
                }
                else
                {
                    _drate = null;
                }
            }
        }

    }
}