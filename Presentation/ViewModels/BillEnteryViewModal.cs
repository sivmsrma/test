using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Terret_Billing.Application.DTOs;
using Terret_Billing.Application.Services;
using Terret_Billing.Application.Services.Interfaces;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Domain.Interfaces;
using Terret_Billing.Presentation.Commands;
using Terret_Billing.Presentation.Models;
using Terret_Billing.Presentation.Models.Request;


namespace Terret_Billing.Presentation.ViewModels
{
    public class BillingViewModel : INotifyPropertyChanged

    {
        private readonly IBillRepository _billRepository;
        private readonly IPartyRepository _partyRepository;
        private readonly IBillEntryService _billEntryService;
        private readonly IRateService _rateService;
        private readonly User _currentUser;
        private Customer _selectedParty;
        private ObservableCollection<TaggingItemDto> _billingItems = new ObservableCollection<TaggingItemDto>();
        private ObservableCollection<Customer> _parties = new ObservableCollection<Customer>();
        private ObservableCollection<Customer> _filteredParties = new ObservableCollection<Customer>();
        //public ObservableCollection<BillPayment> Payments { get; set; } = new ObservableCollection<BillPayment>();


        private decimal _totalAmount;
        private decimal _taxAmount;
        private decimal _discountAmount;
        private decimal _grandTotal;
        private bool _isLoading;
        private string _searchText;
        private bool _isSearching;
        private string _barcode;
        private string _description;
        private string _item;
        private string _purity;
        private string _hsn;
        private int _pcs;
        private decimal _grossWt;
        private decimal _lessWt;
        private decimal _netWt;
        private decimal _diamondCt;
        private decimal _diamondRate;
        private decimal _diaCharge;
        private decimal _stoneCt;
        private decimal _finalWeight;
        private decimal _amount;
        private decimal _rate;
        private decimal _makingCharge;
        private decimal _netPrice;
        private decimal _taxPercent = 3;
        private decimal _taxTotalAmt;
        private decimal _finalAmount;
        private decimal _roundOff;
        private string _metalType;
        private string _huid;
        private decimal _stoneCharge;
        private string _hallmark;
        private decimal _hmTax;
        private ObservableCollection<object> _items = new ObservableCollection<object>();
        public CompanyInfo Company { get; set; }
        // ... existing code ...
        private decimal _currentPaymentAmount;
        public decimal CurrentPaymentAmount
        {
            get => _currentPaymentAmount;
            set { _currentPaymentAmount = value; OnPropertyChanged(nameof(CurrentPaymentAmount)); }
        }
        // ... existing code ...
        public BillingViewModel(IPartyRepository partyRepository, IBillEntryService billEntryService,
            IRateService rateService, User user, IBillRepository billRepository)
        {
            _partyRepository = partyRepository ?? throw new ArgumentNullException(nameof(partyRepository));
            _billEntryService = billEntryService ?? throw new ArgumentNullException(nameof(billEntryService));
            _rateService = rateService ?? throw new ArgumentNullException(nameof(rateService));
            _currentUser = user ?? throw new ArgumentNullException(nameof(user));
            _billRepository = billRepository ?? throw new ArgumentNullException(nameof(billRepository));

            // Initialize commands with named methods instead of lambdas
            LookupItemCommand = new AsyncRelayCommand(ExecuteLookupItem, CanExecuteLookupItem);
            AddCurrentItemCommand = new RelayCommand(ExecuteAddCurrentItem);
            SaveBillCommand = new AsyncRelayCommand(ExecuteSaveBill);
            ClearBillCommand = new RelayCommand(ExecuteClearBill);
            SearchPartyCommand = new AsyncRelayCommand(ExecuteSearchParties);
            PayCommand = new RelayCommand(ExecuteProcessPayment);


            //_ = LoadPartiesAsync();
        }

        // Command execution methods
        public ICommand PrintBillCommand { get; }
        private async Task ExecuteLookupItem(object param) => await LookupItemAsync(param);
        private bool CanExecuteLookupItem(object _) => !string.IsNullOrWhiteSpace(Barcode);
        private void ExecuteAddCurrentItem(object _) => AddCurrentItem();
        private async Task ExecuteSaveBill(object _) => await SaveBillAsync();
        private void ExecuteClearBill(object _) => ClearBill();
        private async Task ExecuteSearchParties(object _) => await SearchPartiesAsync();
        private void ExecuteProcessPayment(object _) => ProcessPayment();

        #region Properties
        public string Barcode
        {
            get => _barcode;
            set
            {
                if (SetProperty(ref _barcode, value))
                    (LookupItemCommand as AsyncRelayCommand)?.RaiseCanExecuteChanged();
            }
        }
        public string BillNo { get; set; }
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                    RecalculateTotals();
                }
            }
        }
        public string Item
        {
            get => _item;
            set
            {
                if (_item != value)
                {
                    _item = value;
                    OnPropertyChanged(nameof(Item));
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
                    OnPropertyChanged(nameof(Purity));
                }
            }
        }
        public string HSN
        {
            get => _hsn;
            set
            {
                if (_hsn != value)
                {
                    _hsn = value;
                    OnPropertyChanged(nameof(HSN));
                }
            }
        }
        public int PCS
        {
            get => _pcs;
            set
            {
                if (_pcs != value)
                {
                    _pcs = value;
                    OnPropertyChanged(nameof(PCS));
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
                    OnPropertyChanged(nameof(GrossWt));
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
                    OnPropertyChanged(nameof(LessWt));
                }
            }
        }
        public decimal NetWt
        {
            get => _netWt;
            set
            {
                if (_netWt != value)
                {
                    _netWt = value;
                    OnPropertyChanged(nameof(NetWt));
                }
            }
        }
        public decimal DiamondCt
        {
            get => _diamondCt;
            set
            {
                if (_diamondCt != value)
                {
                    _diamondCt = value;
                    OnPropertyChanged(nameof(DiamondCt));
                }
            }
        }
        public decimal DiamondRate
        {
            get => _diamondRate;
            set
            {
                if (_diamondRate != value)
                {
                    _diamondRate = value;
                    OnPropertyChanged(nameof(DiamondRate));
                }
            }
        }
        public decimal DiaCharge
        {
            get => _diaCharge;
            set
            {
                if (_diaCharge != value)
                {
                    _diaCharge = value;
                    OnPropertyChanged(nameof(DiaCharge));
                }
            }
        }
        public decimal StoneCt
        {
            get => _stoneCt;
            set
            {
                if (_stoneCt != value)
                {
                    _stoneCt = value;
                    OnPropertyChanged(nameof(StoneCt));
                }
            }
        }
        public decimal FinalWeight
        {
            get => _finalWeight;
            set
            {
                if (_finalWeight != value)
                {
                    _finalWeight = value;
                    OnPropertyChanged(nameof(FinalWeight));
                }
            }
        }
        public decimal Amount
        {
            get => _amount;
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    OnPropertyChanged(nameof(Amount));
                }
            }
        }
        public decimal Rate
        {
            get => _rate;
            set
            {
                if (_rate != value)
                {
                    _rate = value;
                    OnPropertyChanged(nameof(Rate));
                }
            }
        }
        public decimal MakingCharge
        {
            get => _makingCharge;
            set
            {
                if (_makingCharge != value)
                {
                    _makingCharge = value;
                    OnPropertyChanged(nameof(MakingCharge));
                }
            }
        }
        public decimal NetPrice
        {
            get => _netPrice;
            set
            {
                if (_netPrice != value)
                {
                    _netPrice = value;
                    OnPropertyChanged(nameof(NetPrice));
                }
            }
        }
        public decimal Tax
        {
            get => _taxPercent;
            set
            {
                if (_taxPercent != value)
                {
                    _taxPercent = value;
                    OnPropertyChanged(nameof(Tax));
                    RecalculateTotals();
                }
            }
        }
        public decimal TaxTotalAmt
        {
            get => _taxTotalAmt;
            set
            {
                if (_taxTotalAmt != value)
                {
                    _taxTotalAmt = value;
                    OnPropertyChanged(nameof(TaxTotalAmt));
                }
            }
        }
        public decimal FinalAmount
        {
            get => _finalAmount;
            set
            {
                if (_finalAmount != value)
                {
                    _finalAmount = value;
                    OnPropertyChanged(nameof(FinalAmount));
                }
            }
        }
        public decimal RoundOff
        {
            get => _roundOff;
            set
            {
                if (_roundOff != value)
                {
                    _roundOff = value;
                    OnPropertyChanged(nameof(RoundOff));
                    //RecalculateTotals();
                }
            }
        }
        public string MetalType
        {
            get => _metalType;
            set
            {
                if (_metalType != value)
                {
                    _metalType = value;
                    OnPropertyChanged(nameof(MetalType));
                }
            }
        }
        public string HUID
        {
            get => _huid;
            set
            {
                if (_huid != value)
                {
                    _huid = value;
                    OnPropertyChanged(nameof(HUID));
                }
            }
        }
        public decimal StoneCharge
        {
            get => _stoneCharge;
            set
            {
                if (_stoneCharge != value)
                {
                    _stoneCharge = value;
                    OnPropertyChanged(nameof(StoneCharge));
                    RecalculateTotals();
                }
            }
        }
        public string Hallmark
        {
            get => _hallmark;
            set
            {
                if (_hallmark != value)
                {
                    _hallmark = value;
                    OnPropertyChanged(nameof(Hallmark));
                }
            }
        }
        public decimal HmTax
        {
            get => _hmTax;
            set
            {
                if (_hmTax != value)
                {
                    _hmTax = value;
                    OnPropertyChanged(nameof(HmTax));
                    RecalculateTotals();
                }
            }
        }
        public ObservableCollection<object> Items
        {
            get => _items;
            set
            {
                if (_items != value)
                {
                    _items = value;
                    OnPropertyChanged(nameof(Items));
                }
            }
        }
        public ObservableCollection<TaggingItemDto> BillingItems => _billingItems;
        public ObservableCollection<Customer> Parties => _parties;
        public ObservableCollection<Customer> FilteredParties
        {
            get => _filteredParties;
            set
            {
                if (_filteredParties != value)
                {
                    _filteredParties = value;
                    OnPropertyChanged(nameof(FilteredParties));
                }
            }
        }
        public Customer SelectedParty
        {
            get => _selectedParty;
            set
            {
                if (_selectedParty != value)
                {
                    _selectedParty = value;
                    OnPropertyChanged(nameof(SelectedParty));
                    OnPartySelected(value);
                }
            }
        }
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    if (!_isSearching) SearchPartiesAsync();
                }
            }
        }
        public decimal TotalAmount
        {
            get => _totalAmount;
            set
            {
                if (_totalAmount != value)
                {
                    _totalAmount = value;
                    OnPropertyChanged(nameof(TotalAmount));
                    CalculateGrandTotal();
                }
            }
        }
        public decimal TaxAmount
        {
            get => _taxAmount;
            set
            {
                if (_taxAmount != value)
                {
                    _taxAmount = value;
                    OnPropertyChanged(nameof(TaxAmount));
                    CalculateGrandTotal();
                }
            }
        }
        public decimal DiscountAmount
        {
            get => _discountAmount;
            set
            {
                if (_discountAmount != value)
                {
                    _discountAmount = value;
                    OnPropertyChanged(nameof(DiscountAmount));
                    CalculateGrandTotal();
                }
            }
        }
        public decimal GrandTotal
        {
            get => _grandTotal;
            private set
            {
                if (_grandTotal != value)
                {
                    _grandTotal = value;
                    OnPropertyChanged(nameof(GrandTotal));
                }
            }
        }
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged(nameof(IsLoading));
                }
            }
        }
        
       

        private string _currentPaymentAmountText = "0.00";
        public string CurrentPaymentAmountText
        {
            get => _currentPaymentAmountText;
            set
            {
                _currentPaymentAmountText = value;
                OnPropertyChanged(nameof(CurrentPaymentAmountText));
                // Jab bhi value change ho, decimal me convert karne ki koshish karo
                if (decimal.TryParse(value, out var amt))
                    CurrentPaymentAmount = amt;
                else
                    CurrentPaymentAmount = 0;
            }
        }
        private ObservableCollection<string> _paymentModes = new ObservableCollection<string> { "Cash", "UPI", "Bank Transfer", "Cheque", "Card" };
        public ObservableCollection<string> PaymentModes
        {
            get => _paymentModes;
            set { _paymentModes = value; OnPropertyChanged(nameof(PaymentModes)); }
        }

        private string _selectedPaymentMode;
        public string SelectedPaymentMode
        {
            get => _selectedPaymentMode;
            set { _selectedPaymentMode = value; OnPropertyChanged(nameof(SelectedPaymentMode)); }
        }

        private string _currentReferenceNo;
        public string CurrentReferenceNo
        {
            get => _currentReferenceNo;
            set { _currentReferenceNo = value; OnPropertyChanged(nameof(CurrentReferenceNo)); }
        }

        private DateTime _currentPaymentDate = DateTime.Today;
        public DateTime CurrentPaymentDate
        {
            get => _currentPaymentDate;
            set { _currentPaymentDate = value; OnPropertyChanged(nameof(CurrentPaymentDate)); }
        }
        //public ObservableCollection<BillPayment> Payments { get; set; } = new ObservableCollection<BillPayment>();


        #endregion

        #region Commands
        public IAsyncRelayCommand LookupItemCommand { get; }
        public ICommand AddCurrentItemCommand { get; }
        public ICommand SaveBillCommand { get; }
        public ICommand ClearBillCommand { get; }
        public ICommand SearchPartyCommand { get; }
        public ICommand PayCommand { get; }
        #endregion

        #region Public Methods
        private async Task LookupItemAsync(object parameter = null)
        {
            if (string.IsNullOrWhiteSpace(Barcode)) return;

            try
            {
                IsLoading = true;
                var item = await _billEntryService.FindItemAsync(Barcode.Trim());
                if (item == null)
                {
                    MessageBox.Show($"No item found for barcode {Barcode}", "Not Found", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                HSN = item.HSN;

                PopulateFromDto(item);
                bool isDiamondJewellery = item.DiamondCt > 0;
                string metalKey = DetermineMetalKey(item, isDiamondJewellery);
                MetalType = char.ToUpper(metalKey[0]) + metalKey.Substring(1);

                try
                {
                    var rateInfo = await _rateService.GetLatestRateAsync(metalKey);
                    if (rateInfo != null)
                    {
                        if (isDiamondJewellery)
                        {
                            DiamondRate = rateInfo.RatePerGram;
                            var goldRateInfo = await _rateService.GetLatestRateAsync("gold");
                            if (goldRateInfo != null)
                            {
                                Rate = item.Purity?.Any(char.IsDigit) == true
                                    ? _rateService.ConvertRate(goldRateInfo.RatePerGram, item.Purity)
                                    : goldRateInfo.RatePerGram;
                                MakingCharge = _rateService.CalculateMaking(FinalWeight, goldRateInfo);
                                DiaCharge = Math.Round(DiamondCt * DiamondRate, 2);
                            }
                        }
                        else
                        {
                            Rate = (metalKey == "gold" && item.Purity?.Any(char.IsDigit) == true)
                                ? _rateService.ConvertRate(rateInfo.RatePerGram, item.Purity)
                                : rateInfo.RatePerGram;
                            MakingCharge = _rateService.CalculateMaking(FinalWeight, rateInfo);
                            DiaCharge = 0;
                        }
                        NotifyProperties(nameof(Rate), nameof(MakingCharge), nameof(DiaCharge), nameof(DiamondRate));
                        RecalculateTotals();
                    }
                }
                catch (Exception rateEx)
                {
                    System.Diagnostics.Debug.WriteLine($"Rate processing error: {rateEx}");
                    MessageBox.Show($"Error processing rates: {rateEx.Message}", "Rate Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                //CommandManager.InvalidateRequerySuggested();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lookup failed: {ex}");
                MessageBox.Show($"Lookup failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        

        private void ExecuteAddPayment()
        {
            if (CurrentPaymentAmount > 0 && !string.IsNullOrEmpty(SelectedPaymentMode))
            {
                Payments.Add(new BillPayment
                {
                    Amount = CurrentPaymentAmount,
                    PaymentMode = SelectedPaymentMode,
                    ReferenceNo = CurrentReferenceNo,
                    PaymentDate = CurrentPaymentDate
                });

                // Clear input fields after adding
                CurrentPaymentAmount = 0;
                SelectedPaymentMode = null;
                CurrentReferenceNo = string.Empty;
                CurrentPaymentDate = DateTime.Now;
            }
            OnPropertyChanged(nameof(Payments));
        }
        private string DetermineMetalKey(TaggingItemDto item, bool isDiamondJewellery)
        {
            if (isDiamondJewellery) return "diamond";
            if (!string.IsNullOrWhiteSpace(item.MetalType)) return item.MetalType.ToLower();
            if (item.Barcode?.StartsWith("SL", StringComparison.OrdinalIgnoreCase) == true) return "silver";
            return item.Item?.ToLower().Contains("silver") == true ? "silver" : "gold";
        }

        public void RecalculateTotals()
        {
            try
            {
                decimal basePrice = FinalWeight * Rate + DiaCharge + StoneCharge;
                decimal netPrice = Math.Round(basePrice + MakingCharge, 2);
                decimal taxTotalAmt = Math.Round(netPrice * (Tax / 100), 2);
                decimal hmTaxAmount = Math.Round(netPrice * (HmTax / 100), 2);
                decimal finalAmount = RoundOff != 0 ? RoundOff : Math.Round(netPrice + taxTotalAmt + hmTaxAmount, 2);

                if (RoundOff != 0)
                {
                    MakingCharge = Math.Round(MakingCharge - (netPrice + taxTotalAmt + hmTaxAmount - RoundOff), 2);
                    netPrice = Math.Round(basePrice + MakingCharge, 2);
                    taxTotalAmt = Math.Round(netPrice * (Tax / 100), 2);
                    hmTaxAmount = Math.Round(netPrice * (HmTax / 100), 2);
                }
                Amount = basePrice;
                NetPrice = netPrice;
                TaxTotalAmt = taxTotalAmt;
                FinalAmount = finalAmount;
                TotalAmount = BillingItems.Sum(item => item.Amount);
                GrandTotal = finalAmount;
                TotalBillAmount = TotalAmount;

                // Update tax amount for the current item
                TaxAmount = taxTotalAmt + hmTaxAmount;

                // Notify all relevant properties
                NotifyProperties(
                    nameof(NetPrice),
                    nameof(TaxTotalAmt),
                    nameof(FinalAmount),
                    nameof(TotalAmount),
                    nameof(GrandTotal),
                    nameof(TotalBillAmount),
                    nameof(TotalTax),
                    nameof(TotalMakingCharge),
                    nameof(TotalHmTax),
                    nameof(NetAmount),
                    nameof(TaxAmount),
                    nameof(Amount)

                );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"RecalculateTotals failed: {ex}");
                throw;
            }
        }

        private void PopulateFromDto(TaggingItemDto dto)
        {
            if (dto == null) return;

            // Store current values to avoid unnecessary updates
            var currentBarcode = Barcode;
            var currentDescription = Description;
            var currentItem = Item;

            Barcode = dto.Barcode;
            Description = dto.Description ?? string.Empty;
            Item = dto.Item ?? string.Empty;
            Purity = dto.Purity ?? string.Empty;
            HSN = dto.HSN ?? string.Empty;
            PCS = dto.PCS;
            GrossWt = dto.GrossWt;
            LessWt = dto.LessWt;
            NetWt = dto.NetWt;
            DiamondCt = dto.DiamondCt;
            DiamondRate = dto.DiamondRate;
            MetalType = dto.MetalType ?? string.Empty;
            StoneCt = dto.StoneCt;
            FinalWeight = dto.FinalWeight;
            Amount = dto.Amount;
            HUID = string.Empty;
            StoneCharge = 0;
            Hallmark = string.Empty;
            HmTax = 0;

            // Force UI update by raising property changed events
            OnPropertyChanged(nameof(Description));
            OnPropertyChanged(nameof(Item));
            OnPropertyChanged(nameof(Purity));
            // Add other properties as needed
        }

        public async Task LoadPartiesAsync()
        {
            try
            {
                IsLoading = true;
                var parties = (await _partyRepository.GetAllAsync())?.Select(p => new Customer
                {
                    Id = p.Id,
                    Name = p.Name,
                    MobileNumber = p.MobileNumber,
                    GSTNumber = p.GSTNumber,
                    Address = p.Address,
                    Village = p.Village,
                    PinCode = p.PinCode,
                    AccountNumber = p.AccountNumber,
                    State = p.State,
                    StateCode = p.StateCode,
                    MailId = p.Email,
                    PANNumber = p.PANNumber
                }).ToList();

                if (parties?.Any() == true)
                {
                    await System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        _parties.Clear();
                        _filteredParties.Clear();
                        foreach (var party in parties)
                        {
                            _parties.Add(party);
                            _filteredParties.Add(party);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading parties: {ex}");
                MessageBox.Show($"Error loading parties: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SearchPartiesAsync()
        {
            if (_isSearching) return;

            try
            {
                _isSearching = true;
                IsLoading = true;

                // Clear previous search results
                await System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    _filteredParties.Clear();
                });

                if (string.IsNullOrWhiteSpace(SearchText) || SearchText.Length < 3)
                {
                    await ResetFilteredPartiesAsync();
                    return;
                }

                var searchTerm = SearchText.Trim();
                var searchTermLower = searchTerm.ToLower();

                // Search in local collection first
                var localResults = _parties
                    .Where(p => (p.MobileNumber != null && p.MobileNumber.Trim() == searchTerm) ||
                               (p.Name != null && p.Name.ToLower().Contains(searchTermLower)) ||
                               (p.GSTNumber != null && p.GSTNumber.ToLower().Contains(searchTermLower)))
                    .ToList();

                // If no results in local collection, try searching in database
                if (!localResults.Any())
                {
                    var dbResults = (await _partyRepository.GetAllAsync())
                        ?.Where(p => (p.MobileNumber != null && p.MobileNumber.Trim() == searchTerm) ||
                                   (p.Name != null && p.Name.ToLower().Contains(searchTermLower)) ||
                                   (p.GSTNumber != null && p.GSTNumber.ToLower().Contains(searchTermLower)))
                        .Select(p => new Customer
                        {
                            Id = p.Id,
                            Name = p.Name,
                            MobileNumber = p.MobileNumber,
                            GSTNumber = p.GSTNumber,
                            Address = p.Address,
                            Village = p.Village,
                            PinCode = p.PinCode,
                            AccountNumber = p.AccountNumber,
                            State = p.State,
                            StateCode = p.StateCode,
                            MailId = p.Email,
                            PANNumber = p.PANNumber
                        })
                        .Where(p => !_parties.Any(ep => ep.Id == p.Id)) // Only get new customers
                        .ToList();

                    if (dbResults?.Any() == true)
                    {
                        // Add new customers to the main collection
                        await System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
                        {
                            foreach (var newCustomer in dbResults)
                            {
                                _parties.Add(newCustomer);
                            }
                            localResults.AddRange(dbResults);
                        });
                    }
                }

                // Update the filtered parties collection on the UI thread
                await System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    _filteredParties.Clear();
                    foreach (var item in localResults.OrderBy(p => p.Name))
                    {
                        _filteredParties.Add(item);
                    }
                    AutoSelectPartyIfSingleMatch();
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Search parties failed: {ex}");
                await System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
                    MessageBox.Show($"Error searching parties: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error));
            }
            finally
            {
                _isSearching = false;
                IsLoading = false;
            }
        }

        private async Task ResetFilteredPartiesAsync()
        {
            await System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
            {
                _filteredParties.Clear();
                foreach (var party in _parties)
                {
                    _filteredParties.Add(party);
                }
            });
        }
       
        private void AutoSelectPartyIfSingleMatch()
        {
            if (_filteredParties.Count == 1)
            {
                var party = _filteredParties[0];
                if (party.Name?.Equals(SearchText, StringComparison.OrdinalIgnoreCase) == true ||
                    party.MobileNumber == SearchText ||
                    party.GSTNumber?.Equals(SearchText, StringComparison.OrdinalIgnoreCase) == true)
                {
                    SelectedParty = party;
                }
            }
        }
        public void HandleEnterKey()
        {
            if (!string.IsNullOrWhiteSpace(Barcode))
            {
                AddCurrentItem();
            }
        }

        private bool _isUpdatingParty = false;

        private void OnPartySelected(Customer selectedParty)
        {
            if (selectedParty == null || _isUpdatingParty) return;

            try
            {
                _isUpdatingParty = true;

                // Store the current selected party
                var previousParty = _selectedParty;

                // Update the selected party
                _selectedParty = selectedParty;

                // Update the search text with the selected party's name
                SearchText = selectedParty?.Name ?? string.Empty;

                // Notify UI that the selected party has changed
                OnPropertyChanged(nameof(SelectedParty));

                // Force UI to update all bindings to the SelectedParty properties
                if (previousParty != null)
                {
                    // Notify changes for previous party to clear old values
                    OnPropertyChanged($"SelectedParty.{nameof(Customer.Name)}");
                    OnPropertyChanged($"SelectedParty.{nameof(Customer.MobileNumber)}");
                    OnPropertyChanged($"SelectedParty.{nameof(Customer.GSTNumber)}");
                    OnPropertyChanged($"SelectedParty.{nameof(Customer.Address)}");
                }

                // Notify changes for new party
                if (selectedParty != null)
                {
                    OnPropertyChanged($"SelectedParty.{nameof(Customer.Name)}");
                    OnPropertyChanged($"SelectedParty.{nameof(Customer.MobileNumber)}");
                    OnPropertyChanged($"SelectedParty.{nameof(Customer.GSTNumber)}");
                    OnPropertyChanged($"SelectedParty.{nameof(Customer.Address)}");
                }

                // Force the UI to refresh all bindings
                CommandManager.InvalidateRequerySuggested();

                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    try
                    {
                        // Close the dropdown
                        var comboBox = System.Windows.Application.Current.Windows.OfType<Window>()
                            .FirstOrDefault()?.FindName("Cmb_No") as System.Windows.Controls.ComboBox;
                        if (comboBox != null)
                        {
                            comboBox.SetValue(System.Windows.Controls.ComboBox.IsDropDownOpenProperty, false);
                        }

                        // Focus on the next relevant field (e.g., barcode entry)
                        var barcodeBox = System.Windows.Application.Current.Windows.OfType<Window>()
                            .FirstOrDefault()?.FindName("BarcodeEntry") as System.Windows.UIElement;
                        barcodeBox?.Focus();

                        // Force update the text boxes
                        var customerNameBox = System.Windows.Application.Current.Windows.OfType<Window>()
                            .FirstOrDefault()?.FindName("Txt_CustomerName") as System.Windows.Controls.TextBox;
                        var gstBox = System.Windows.Application.Current.Windows.OfType<Window>()
                            .FirstOrDefault()?.FindName("Txt_GST") as System.Windows.Controls.TextBox;
                        var mobileBox = System.Windows.Application.Current.Windows.OfType<Window>()
                            .FirstOrDefault()?.FindName("Txt_Mobile") as System.Windows.Controls.TextBox;

                        if (selectedParty != null)
                        {
                            if (customerNameBox != null) customerNameBox.Text = selectedParty.Name ?? string.Empty;
                            if (gstBox != null) gstBox.Text = selectedParty.GSTNumber ?? string.Empty;
                            if (mobileBox != null) mobileBox.Text = selectedParty.MobileNumber ?? string.Empty;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error in UI update: {ex}");
                    }
                }, System.Windows.Threading.DispatcherPriority.Render);

                // Debug output to verify the selected party
                if (selectedParty != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Selected Party: {selectedParty.Name ?? "[No Name]"}, " +
                        $"Mobile: {selectedParty.MobileNumber ?? "[No Mobile]"}, " +
                        $"GST: {selectedParty.GSTNumber ?? "[No GST]"}");
                }
            }
            finally
            {
                _isUpdatingParty = false;
            }
        }





        private ICommand _addItemCommand;
        public ICommand AddItemCommand => _addItemCommand ??= new RelayCommand(_ => AddCurrentItem());

        private void AddCurrentItem()
        {
            if (string.IsNullOrWhiteSpace(Barcode))
            {
                MessageBox.Show("Please enter a barcode first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var item = new TaggingItemDto
                {
                    Barcode = Barcode,
                    Description = Item,
                    Item = Item,
                    Purity = Purity,
                    HSN = HSN,
                    PCS = PCS,
                    GrossWt = GrossWt,
                    LessWt = LessWt,
                    NetWt = NetWt,
                    DiamondCt = DiamondCt,
                    DiamondRate = DiamondRate,
                    DiaCharge = DiaCharge,
                    StoneCt = StoneCt,
                    FinalWeight = FinalWeight,
                    MetalType = MetalType,
                    NetPrice = NetPrice,
                    TaxAmount = TaxAmount,
                    FAmount = Amount,
                    MakingCharges = MakingCharge,
                    HMCharges = HmTax,
                    BillRoundOff = RoundOff,
                    Amount = FinalAmount
                };

                // Add to the collection
                _billingItems.Add(item);

                // Update the totals
                TotalAmount = _billingItems.Sum(i => i.FAmount);
                TotalBillAmount = TotalAmount; // Set to the sum of all items

                // Recalculate all tax-related amounts
                CalculateGrandTotal();

                // Notify all relevant properties
                NotifyProperties(
                    nameof(TotalBillAmount),
                    nameof(TotalAmount),
                    nameof(TotalTax),
                    nameof(TotalHmTax),
                    nameof(TotalMakingCharge),
                    nameof(NetAmount),
                    nameof(GrandTotal),
                    nameof(Amount)
                );

                // Clear the fields for the next entry
                ClearItemFields();

                // Focus back to barcode field
                System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    var window = System.Windows.Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
                    var barcodeBox = window?.FindName("BarcodeEntry") as UIElement;
                    barcodeBox?.Focus();
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding item: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearItemFields()
        {
            Barcode = string.Empty;
            Description = string.Empty;
            Item = string.Empty;
            Purity = string.Empty;
            HSN = string.Empty;
            PCS = 0;
            GrossWt = 0;
            LessWt = 0;
            NetWt = 0;
            DiamondCt = 0;
            DiamondRate = 0;
            DiaCharge = 0;
            StoneCt = 0;
            FinalWeight = 0;
            Rate = 0;
            MakingCharge = 0;
            NetPrice = 0;
            Tax = 3;
            TaxTotalAmt = 0;
            FinalAmount = 0;
            RoundOff = 0;
            MetalType = string.Empty;
            Amount = 0;
        }

        private void ClearBill()
        {
            _billingItems.Clear();
            ClearItemFields();
            SelectedParty = null;
            TotalAmount = 0;
            TaxAmount = 0;
            DiscountAmount = 0;
            GrandTotal = 0;
            Payments.Clear(); // Payment list ko clear karo
            CashPayment = 0;
            UpiPayment = 0;
            BankTransferPayment = 0;
            ChequePayment = 0;
            CardPayment = 0;
            TotalAdvance = 0;
            
            // ...aur jo bhi fields hai...
            OnPropertyChanged(nameof(Payments));

            NotifyProperties(nameof(SelectedParty), nameof(TotalAmount), nameof(TaxAmount),
                            nameof(DiscountAmount), nameof(GrandTotal));
        }

        private async Task SaveBillAsync()
        {
            try
            {
                // Validate required data
                if (SelectedParty == null)
                {
                    MessageBox.Show("Please select a customer before saving the bill.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (_currentUser == null)
                {
                    MessageBox.Show("User session is not valid. Please log in again.", "Session Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (_billingItems == null || !_billingItems.Any())
                {
                    MessageBox.Show("Cannot save an empty bill. Please add items to the bill.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // 1. Create bill header
                var bill = new Bill
                {
                    BillNo = null, // Will be generated by the repository
                    BillDate = DateTime.Now,
                    PartyId = SelectedParty.Id,
                    TotalAmount = TotalBillAmount,
                    TaxAmount = TotalTax,
                    DiscountAmount = TotalDiscount,
                    NetAmount = GrandTotal,
                    FirmId = _currentUser.firm_id,
                    IsSynced = false
                };

                // 2. Create bill items with all details
                var items = _billingItems.Select(item => new BillItem
                {
                    ItemId = item.Id,
                    Barcode = item.Barcode,
                    Description = item.Description,
                    Purity = item.Purity,
                    HSNCode = item.HSN,
                    PCS = item.PCS,
                    GrossWt = item.GrossWt,
                    LessWt = item.LessWt,
                    NetWt = item.NetWt,
                    DiamondCt = item.DiamondCt,
                    DiamondRate = item.DiamondRate,
                    DiaCharge = item.DiaCharge,
                    StoneCt = item.StoneCt,
                    StoneCharge = item.StoneCharge,
                    FinalWeight = item.FinalWeight,
                    MetalType = item.MetalType,
                    HUID = item.HUID,
                    Hallmark = item.Hallmark,
                    HmTax = item.HmTax,
                    Quantity = item.Quantity,
                    Rate = item.Rate,
                    Amount = item.Amount,
                    TaxRate = item.TaxRate,
                    TaxAmount = item.TaxAmount,
                    MakingCharge = item.MakingCharge,
                    NetPrice = item.NetPrice,
                    FinalAmount = item.FinalAmount
                }).ToList();

                // 3. Create payment
                var payment = new BillPayment
                {
                    Amount = GrandTotal,
                    PaymentMode = "", // Or get from UI
                    ReferenceNo = string.Empty,
                    PaymentDate = DateTime.Now
                };
                // Create BillRequest object
                var billRequest = new Terret_Billing.Presentation.Models.Request.BillRequest
                {
                    Bill = bill,
                    BillItem = items,
                    BillPayment = Payments.ToList() // UI se jitne bhi payment add kiye hain, sab save honge
                };

                // 4. Save to local database
                await _billRepository.AddAsync(billRequest);
                
                // 5. Load the saved bill for printing
                await LoadBillForPrintingAsync((int)billRequest.Bill.Id);
                
                MessageBox.Show("Bill saved and synced successfully!");
               
                ClearBill();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving bill: {ex.Message}");
            }
        }

        private async Task LoadBillForPrintingAsync(int billId)
        {
            try
            {
                var (bill, items, payments, company) = await _billRepository.GetFullBillForPrintAsync(billId, _currentUser.firm_id);
                
                // Set company info for printing
                Company = new CompanyInfo
                {
                    Name = company?.ShopName ?? "Company Name",
                    Address = company?.Address ?? "Company Address",
                    Phone = company?.PhoneNumber ?? "Phone",
                    Mobile = company?.PhoneNumber ?? "Mobile",
                    GSTIN = company?.GSTIN ?? "GSTIN"
                };

                // Convert BillItems to TaggingItemDto for printing
                _billingItems.Clear();
                foreach (var item in items)
                {
                    var taggingItem = new TaggingItemDto
                    {
                        Id = (int)item.ItemId,
                        Barcode = item.Barcode,
                        Description = item.Description,
                        Item = item.Description, // You might want to get the actual item name
                        Purity = item.Purity,
                        HSN = item.HSNCode,
                        PCS = item.PCS,
                        GrossWt = item.GrossWt,
                        LessWt = item.LessWt,
                        NetWt = item.NetWt,
                        DiamondCt = item.DiamondCt,
                        DiamondRate = item.DiamondRate,
                        DiaCharge = item.DiaCharge,
                        StoneCt = item.StoneCt,
                        StoneCharge = item.StoneCharge,
                        FinalWeight = item.FinalWeight,
                        MetalType = item.MetalType,
                        HUID = item.HUID,
                        Hallmark = item.Hallmark,
                        HmTax = item.HmTax,
                        Quantity = item.Quantity,
                        Rate = item.Rate,
                        Amount = item.Amount,
                        TaxRate = item.TaxRate,
                        TaxAmount = item.TaxAmount,
                        MakingCharge = item.MakingCharge,
                        NetPrice = item.NetPrice,
                        FinalAmount = item.FinalAmount
                    };
                    _billingItems.Add(taggingItem);
                }
                BillNo = bill.BillNo;
                OnPropertyChanged(nameof(BillNo));
                // Set totals
                TotalAmount = bill.TotalAmount;
                TaxAmount = bill.TaxAmount;
                DiscountAmount = bill.DiscountAmount;
                GrandTotal = bill.NetAmount;

                // Notify UI updates
                OnPropertyChanged(nameof(BillingItems));
                OnPropertyChanged(nameof(TotalAmount));
                OnPropertyChanged(nameof(TaxAmount));
                OnPropertyChanged(nameof(DiscountAmount));
                OnPropertyChanged(nameof(GrandTotal));
                OnPropertyChanged(nameof(Company));
                // Show print preview
                ShowPrintPreview();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading bill for printing: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowPrintPreview()
        {
            // Create and show print preview window
            var previewWindow = new Terret_Billing.Presentation.Views.PrintPreviewWindow();
            previewWindow.SetBillData(this);
            previewWindow.ShowDialog();
        }


        private void CalculateGrandTotal()
        {
            // Calculate total tax from all items in the grid
            decimal totalTax = BillingItems.Sum(item => item.TaxAmount);
            decimal totalHmTax = BillingItems.Sum(item => item.HMCharges);
            decimal totalMakingCharge = BillingItems.Sum(item => item.MakingCharges);

            // Calculate grand total including all charges and taxes
            GrandTotal = TotalAmount + totalTax + totalHmTax + totalMakingCharge - DiscountAmount;

            // Notify UI of all property changes
            NotifyProperties(
                nameof(GrandTotal),
                nameof(TotalTax),
                nameof(TotalHmTax),
                nameof(TotalMakingCharge),
                nameof(NetAmount)
            );
        }

        private void NotifyProperties(params string[] propertyNames)
        {
            foreach (var name in propertyNames)
            {
                OnPropertyChanged(name);
            }
        }
        #endregion

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, Action callback = null, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            callback?.Invoke();
            return true;
        }
        #endregion

        #region Bill Properties
        private ObservableCollection<BillPayment> _payments = new ObservableCollection<BillPayment>();
        public ObservableCollection<BillPayment> Payments
        {
            get => _payments;
            set => SetProperty(ref _payments, value);
        }
        #endregion

        // Totals
        private decimal _totalBillAmount;
        public decimal TotalBillAmount
        {
            get => _totalBillAmount;
            set => SetProperty(ref _totalBillAmount, value);
        }
        public decimal TotalTax => BillingItems.Sum(item => item.TaxAmount);
        public decimal TotalMakingCharge => BillingItems.Sum(item => item.MakingCharges);
        public decimal TotalHmTax => BillingItems.Sum(item => item.HMCharges);

        private decimal _totalDiscount;
        public decimal TotalDiscount
        {
            get => _totalDiscount;
            set => SetProperty(ref _totalDiscount, value);
        }

        private decimal _totalAdvance;
        public decimal TotalAdvance
        {
            get => _totalAdvance;
            set => SetProperty(ref _totalAdvance, value);
        }

        public decimal NetAmount => TotalBillAmount + TotalTax + TotalMakingCharge + TotalHmTax - TotalDiscount - TotalAdvance;

        private decimal _amountPaid;
        public decimal AmountPaid
        {
            get => _amountPaid;
            set => SetProperty(ref _amountPaid, value);
        }

        public decimal BalanceAmount => NetAmount - AmountPaid;

        // Payment properties
        private decimal _cashPayment;
        public decimal CashPayment
        {
            get => _cashPayment;
            set { if (SetProperty(ref _cashPayment, value)) UpdateAmountPaid(); }
        }

        private decimal _upiPayment;
        public decimal UpiPayment
        {
            get => _upiPayment;
            set { if (SetProperty(ref _upiPayment, value)) UpdateAmountPaid(); }
        }

        private decimal _bankTransferPayment;
        public decimal BankTransferPayment
        {
            get => _bankTransferPayment;
            set { if (SetProperty(ref _bankTransferPayment, value)) UpdateAmountPaid(); }
        }

        private decimal _chequePayment;
        public decimal ChequePayment
        {
            get => _chequePayment;
            set { if (SetProperty(ref _chequePayment, value)) UpdateAmountPaid(); }
        }

        private decimal _cardPayment;
        public decimal CardPayment
        {
            get => _cardPayment;
            set { if (SetProperty(ref _cardPayment, value)) UpdateAmountPaid(); }
        }

        private DateTime _paymentDate = DateTime.Today;
        public DateTime PaymentDate
        {
            get => _paymentDate;
            set => SetProperty(ref _paymentDate, value);
        }

        // Helper to update AmountPaid
        private void UpdateAmountPaid()
        {
            AmountPaid = CashPayment + UpiPayment + BankTransferPayment + ChequePayment + CardPayment;
        }

        private void UpdateTotals()
        {
            OnPropertyChanged(nameof(TotalBillAmount));
            OnPropertyChanged(nameof(TotalTax));
            OnPropertyChanged(nameof(TotalMakingCharge));
            OnPropertyChanged(nameof(TotalHmTax));
            OnPropertyChanged(nameof(NetAmount));
            OnPropertyChanged(nameof(BalanceAmount));
        }

        private void ProcessPayment()
        {
            if (CurrentPaymentAmount <= 0)
            {
                MessageBox.Show("Please enter a valid payment amount.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrEmpty(SelectedPaymentMode))
            {
                MessageBox.Show("Please select a payment mode.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var payment = new BillPayment
            {
                Amount = CurrentPaymentAmount,
                PaymentMode = SelectedPaymentMode,
                ReferenceNo = CurrentReferenceNo,
                PaymentDate = CurrentPaymentDate
            };

            Payments.Add(payment);
            TotalAdvance += CurrentPaymentAmount;

            // Reset fields
            CurrentPaymentAmount = 0;
            SelectedPaymentMode = null;
            CurrentReferenceNo = string.Empty;
            CurrentPaymentDate = DateTime.Today;
            UpdateTotals();
        }
        private string GetPaymentReference(string mode)
        {
            return $"{mode}-REF-{DateTime.Now:yyyyMMddHHmmssfff}";
        }


        //private async Task PrintBillAsync(int billId)
        //{
        //    // Fetch full bill, items, payments and company details
        //    var (bill, items, payments, company) = await _billRepository.GetFullBillForPrintAsync(billId, _currentUser.firm_id);

        //    if (bill == null)
        //    {
        //        MessageBox.Show("Bill not found.");
        //        return;
        //    }

        //    FlowDocument doc = new FlowDocument
        //    {
        //        PagePadding = new Thickness(40),
        //        ColumnWidth = double.PositiveInfinity
        //    };

        //    // Company Header
        //    var header = new Paragraph
        //    {
        //        TextAlignment = TextAlignment.Center
        //    };
        //    header.Inlines.Add(new Bold(new Run($"{company.ShopName}\n")));
        //    header.Inlines.Add(new Run($"{company.Address}, {company.City}, {company.State} - {company.Pincode}\n"));
        //    header.Inlines.Add(new Run($"Phone: {company.PhoneNumber} | GSTIN: {company.GSTIN}\n"));
        //    header.Inlines.Add(new Run($"PAN: {company.PANNumber}\n"));
        //    doc.Blocks.Add(header);

        //    // Customer and Bill Info
        //    var infoTable = new Table();
        //    infoTable.Columns.Add(new TableColumn());
        //    infoTable.Columns.Add(new TableColumn());
        //    var infoGroup = new TableRowGroup();
        //    infoTable.RowGroups.Add(infoGroup);

        //    var row1 = new TableRow();
        //    row1.Cells.Add(new TableCell(new Paragraph(new Run($"Invoice No.: {bill.BillNo}"))));
        //    row1.Cells.Add(new TableCell(new Paragraph(new Run($"Date: {bill.BillDate:dd-MM-yyyy}"))));
        //    infoGroup.Rows.Add(row1);

        //    // Optionally fetch and add party name/number if you have Party table joined

        //    doc.Blocks.Add(infoTable);

        //    // Items Table
        //    var itemTable = new Table();
        //    for (int i = 0; i < 11; i++)
        //        itemTable.Columns.Add(new TableColumn());

        //    var itemHeader = new TableRow();
        //    string[] headers = { "No.", "Item", "PCS", "Purity", "Gross Wt", "Net Wt", "Rate", "Amount", "Making Rate", "Making Amt", "Total" };
        //    foreach (var h in headers)
        //        itemHeader.Cells.Add(new TableCell(new Paragraph(new Bold(new Run(h)))));

        //    var itemGroup = new TableRowGroup();
        //    itemGroup.Rows.Add(itemHeader);

        //    int index = 1;
        //    foreach (var item in items)
        //    {
        //        var row = new TableRow();
        //        row.Cells.Add(new TableCell(new Paragraph(new Run(index.ToString()))));
        //        row.Cells.Add(new TableCell(new Paragraph(new Run(item.Description))));
        //        row.Cells.Add(new TableCell(new Paragraph(new Run(item.PCS.ToString()))));
        //        row.Cells.Add(new TableCell(new Paragraph(new Run(item.Purity))));
        //        row.Cells.Add(new TableCell(new Paragraph(new Run(item.GrossWt.ToString("N3")))));
        //        row.Cells.Add(new TableCell(new Paragraph(new Run(item.NetWt.ToString("N3")))));
        //        row.Cells.Add(new TableCell(new Paragraph(new Run(item.Rate.ToString("N2")))));
        //        row.Cells.Add(new TableCell(new Paragraph(new Run(item.Amount.ToString("N2")))));
        //        row.Cells.Add(new TableCell(new Paragraph(new Run(item.MakingCharge.ToString("N2")))));
        //        row.Cells.Add(new TableCell(new Paragraph(new Run(item.MakingCharge.ToString("N2"))))); // same as rate
        //        row.Cells.Add(new TableCell(new Paragraph(new Run(item.FinalAmount.ToString("N2")))));
        //        itemGroup.Rows.Add(row);
        //        index++;
        //    }

        //    itemTable.RowGroups.Add(itemGroup);
        //    doc.Blocks.Add(itemTable);

        //    // Totals
        //    var totalPara = new Paragraph(new Bold(new Run($"Net Amount: ₹ {bill.NetAmount:N2}")));
        //    totalPara.TextAlignment = TextAlignment.Right;
        //    doc.Blocks.Add(totalPara);

        //    // Payments
        //    if (payments.Any())
        //    {
        //        doc.Blocks.Add(new Paragraph(new Bold(new Run("Payment Details:"))));
        //        foreach (var payment in payments)
        //        {
        //            var p = new Paragraph(new Run($"{payment.PaymentMode} - ₹ {payment.Amount:N2} | Ref: {payment.ReferenceNo} | {payment.PaymentDate:dd-MM-yyyy}"));
        //            doc.Blocks.Add(p);
        //        }
        //    }

        //    // Declaration
        //    doc.Blocks.Add(new Paragraph(new Bold(new Run("Declaration:"))));
        //    doc.Blocks.Add(new Paragraph(new Run(company.PaymentDeclaration ?? "We declare that this invoice shows the actual price of the goods and that all particulars are true.")));

        //    // Print Preview
        //    PrintDialog pd = new PrintDialog();
        //    if (pd.ShowDialog() == true)
        //    {
        //        IDocumentPaginatorSource idpSource = doc;
        //        pd.PrintDocument(idpSource.DocumentPaginator, "Bill Print");
        //    }
        //}


        //private void PrintBill()
        //{
        //    FlowDocument doc = new FlowDocument();
        //    doc.PagePadding = new Thickness(40);
        //    doc.ColumnWidth = double.PositiveInfinity;

        //    // Company Header
        //    var header = new Paragraph();
        //    header.Inlines.Add(new Bold(new Run($"{Company?.Name}\n")));
        //    header.Inlines.Add(new Run($"{Company?.Address}\n"));
        //    header.Inlines.Add(new Run($"Mobile: {Company?.Mobile}   Phone: {Company?.Phone}\n"));
        //    header.Inlines.Add(new Run($"GSTIN: {Company?.GSTIN}\n"));
        //    doc.Blocks.Add(header);

        //    // Bill/Customer Info Table
        //    Table infoTable = new Table();
        //    infoTable.Columns.Add(new TableColumn());
        //    infoTable.Columns.Add(new TableColumn());
        //    var infoRowGroup = new TableRowGroup();
        //    infoTable.RowGroups.Add(infoRowGroup);

        //    var row1 = new TableRow();
        //    row1.Cells.Add(new TableCell(new Paragraph(new Run($"Invoice No.: {Bill.}"))));
        //    row1.Cells.Add(new TableCell(new Paragraph(new Run($"Date: {Date:dd-MM-yyyy}"))));
        //    infoRowGroup.Rows.Add(row1);

        //    var row2 = new TableRow();
        //    row2.Cells.Add(new TableCell(new Paragraph(new Run($"Customer: {SelectedParty?.Name}"))));
        //    row2.Cells.Add(new TableCell(new Paragraph(new Run($"Mobile: {SelectedParty?.MobileNumber}"))));
        //    infoRowGroup.Rows.Add(row2);

        //    doc.Blocks.Add(infoTable);

        //    // Items Table
        //    Table itemsTable = new Table();
        //    for (int i = 0; i < 11; i++) itemsTable.Columns.Add(new TableColumn());
        //    var itemsHeader = new TableRow();
        //    string[] headers = { "NO.", "ITEM DESCRIPTION", "PCS", "PURITY", "GROSS WEIGHT", "NET WEIGHT", "RATE PER GRAM", "AMOUNT", "LABOUR RATE PER GR.", "LABOUR AMOUNT", "TOTAL AMOUNT" };
        //    foreach (var h in headers) itemsHeader.Cells.Add(new TableCell(new Paragraph(new Bold(new Run(h)))));
        //    var itemsGroup = new TableRowGroup();
        //    itemsGroup.Rows.Add(itemsHeader);

        //    int idx = 1;
        //    foreach (var item in BillingItems)
        //    {
        //        var row = new TableRow();
        //        row.Cells.Add(new TableCell(new Paragraph(new Run(idx.ToString()))));
        //        row.Cells.Add(new TableCell(new Paragraph(new Run(item.Item))));
        //        row.Cells.Add(new TableCell(new Paragraph(new Run(item.PCS.ToString()))));
        //        row.Cells.Add(new TableCell(new Paragraph(new Run(item.Purity))));
        //        row.Cells.Add(new TableCell(new Paragraph(new Run(item.GrossWt.ToString("N3")))));
        //        row.Cells.Add(new TableCell(new Paragraph(new Run(item.NetWt.ToString("N3")))));
        //        row.Cells.Add(new TableCell(new Paragraph(new Run(item.Rate.ToString("N2")))));
        //        row.Cells.Add(new TableCell(new Paragraph(new Run(item.Amount.ToString("N2")))));
        //        row.Cells.Add(new TableCell(new Paragraph(new Run(item.MakingCharges.ToString("N2")))));
        //        row.Cells.Add(new TableCell(new Paragraph(new Run(item.MakingCharges.ToString("N2")))));
        //        row.Cells.Add(new TableCell(new Paragraph(new Run(item.FinalAmount.ToString("N2")))));
        //        itemsGroup.Rows.Add(row);
        //        idx++;
        //    }
        //    itemsTable.RowGroups.Add(itemsGroup);
        //    doc.Blocks.Add(itemsTable);

        //    // Tax and Total
        //    doc.Blocks.Add(new Paragraph(new Run($"Grand Total: {GrandTotal:N2}")));

        //    // Declaration
        //    doc.Blocks.Add(new Paragraph(new Bold(new Run("Declaration:"))));
        //    doc.Blocks.Add(new Paragraph(new Run("We declare that this invoice shows the actual price of the goods described and that all particulars are true and correct.")));

        //    // Print
        //    PrintDialog pd = new PrintDialog();
        //    if (pd.ShowDialog() == true)
        //    {
        //        IDocumentPaginatorSource idpSource = doc;
        //        pd.PrintDocument(idpSource.DocumentPaginator, "Bill Print");
        //    }
        //}

    }
}


// Simple RelayCommand implementation for ICommand
public class RelayCommand : ICommand
{
    private readonly Action<object> _execute;
    private readonly Predicate<object> _canExecute;

    public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

    public void Execute(object parameter) => _execute(parameter);

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}



