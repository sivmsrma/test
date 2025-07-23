using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Terret_Billing.Application.Logging;
using Terret_Billing.Application.Services;
using Terret_Billing.Application.Services.Interfaces;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Domain.Interfaces;
using Terret_Billing.Infrastructure.Data;
using Terret_Billing.Infrastructure.Helpers;
using Terret_Billing.Presentation.Models;

namespace Terret_Billing.Presentation.ViewModels
{
    public class NewBranchFormViewModel : INotifyPropertyChanged
    {
        #region Private Fields

        private readonly IBranchRepository _branchRepository;
        private BranchModel _branch;
        private ObservableCollection<Branch> _branches;
        private Branch _selectedBranch;
        private bool _isBusy;
        private string _errorMessage;
        private bool _isMyFirmSelected;
        private bool _isSelfSelected;
        private readonly User _currentUser;

        #endregion

        #region Constructor

        public NewBranchFormViewModel(User currentuser)
        {
            _currentUser = currentuser;
            // Initialize services
            //var connectionString = ConfigurationManager.ConnectionStrings["BillingDb"].ConnectionString;
            //_branchService = new BranchService(connectionString);
            IDatabaseHelper helper = new MySqlDatabaseHelper(); // Replace with actual implementation
            _branchRepository = new BranchRepository(helper);


            // Initialize branch model
            _branch = new BranchModel
            {
                IsActive = true,
                FinancialYearStartDate = new DateTime(DateTime.Now.Year, 4, 1)
            };

            // Initialize commands
            SaveCommand = new RelayCommand(ExecuteSave);
            CancelCommand = new RelayCommand(ExecuteCancel);
            RefreshCommand = new RelayCommand(async _ => await LoadBranchesAsync());
            DeleteBranchCommand = new RelayCommand(async param => await DeleteBranchAsync(param as Branch));
            SelectFirmTypeCommand = new RelayCommand(SelectFirmType);
            UploadLogoCommand = new RelayCommand(_ => UploadLogo());
            UploadLeftImageCommand = new RelayCommand(_ => UploadLeftImage());
            UploadSignatureCommand = new RelayCommand(_ => UploadSignature());
            UploadQrCodeCommand = new RelayCommand(_ => UploadQrCode());

            // Load branches
            LoadBranchesAsync();
        }

        #endregion

        #region Properties

        public BranchModel Branch
        {
            get => _branch;
            set
            {
                if (_branch != value)
                {
                    _branch = value;
                    OnPropertyChanged();
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public ObservableCollection<Branch> Branches
        {
            get => _branches;
            set
            {
                if (_branches != value)
                {
                    _branches = value;
                    OnPropertyChanged();
                }
            }
        }

        public Branch SelectedBranch
        {
            get => _selectedBranch;
            set
            {
                if (_selectedBranch != value)
                {
                    _selectedBranch = value;
                    OnPropertyChanged();
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged();
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsMyFirmSelected
        {
            get => _isMyFirmSelected;
            set
            {
                if (_isMyFirmSelected != value)
                {
                    _isMyFirmSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsSelfSelected
        {
            get => _isSelfSelected;
            set
            {
                if (_isSelfSelected != value)
                {
                    _isSelfSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Commands

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand DeleteBranchCommand { get; }
        public ICommand SelectFirmTypeCommand { get; }
        public ICommand UploadLogoCommand { get; }
        public ICommand UploadLeftImageCommand { get; }
        public ICommand UploadSignatureCommand { get; }
        public ICommand UploadQrCodeCommand { get; }

        private async void ExecuteSave(object parameter)
        {
            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                // Validate input
                Terret_Billing.Application.Logging.Logger.LogInfo("Value of Branch.ShopName before validation: " + Branch.ShopName);
                Terret_Billing.Application.Logging.Logger.LogInfo("Calling ValidateInput...");
                if (!ValidateInput())
                {
                    MessageBox.Show("Validation failed: " + ErrorMessage, "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Terret_Billing.Application.Logging.Logger.LogInfo($"ValidateInput returned false. ErrorMessage: {ErrorMessage}");
                    return;
                }
                Terret_Billing.Application.Logging.Logger.LogInfo("ValidateInput returned true.");

                // Create branch object
                var branch = new Branch
                {
                    FirmId = Branch.FirmId,
                    RegistrationNo = Branch.RegistrationNo,
                    ShopName = Branch.ShopName,
                    FirmDescription = Branch.FirmDescription,
                    Address = Branch.Address,
                    State = Branch.State,
                    District = Branch.District,
                    City = Branch.City,
                    Pincode = Branch.Pincode,
                    PhoneNumber = Branch.PhoneNumber,
                    Email = Branch.Email,
                    WebsiteName = Branch.WebsiteName,
                    Comments = Branch.Comments,
                    WhatsappLink = Branch.WhatsappLink,
                    FacebookLink = Branch.FacebookLink,
                    InstagramLink = Branch.InstagramLink,
                    EInvoiceApiId = Branch.EInvoiceApiId,
                    EInvoiceApiKey = Branch.EInvoiceApiKey,
                    EInvoiceUsername = Branch.EInvoiceUsername,
                    EInvoicePassword = Branch.EInvoicePassword,
                    PaymentBankDetails = Branch.PaymentBankDetails,
                    AccountHolderName = Branch.AccountHolderName,
                    PaymentBankACNo = Branch.PaymentBankACNo,
                    PaymentBankIFSCCode = Branch.PaymentBankIFSCCode,
                    PaymentDeclaration = Branch.PaymentDeclaration,
                    FinancialYearStartDate = Branch.FinancialYearStartDate,
                    CashBalance = Branch.CashBalance,
                    GSTIN = Branch.GSTIN,
                    PANNumber = Branch.PANNumber,
                    PrincipalAmtStart = Branch.PrincipalAmtStart,
                    PrincipalAmtEnd = Branch.PrincipalAmtEnd,
                    FormHeader = Branch.FormHeader,
                    FormFooter = Branch.FormFooter,
                    FirmType = Branch.FirmType,
                    LogoPath = Branch.LogoPath,
                    LeftImagePath = Branch.LeftImagePath,
                    SignaturePath = Branch.SignaturePath,
                    QrCodePath = Branch.QrCodePath
                };

                // Save branch to database
                Terret_Billing.Application.Logging.Logger.LogInfo($"Attempting to create branch: {Branch.ShopName}");
                var result = await _branchRepository.InsertCompanyDetailsAsync(branch, _currentUser.Id);

                if (result > 1)
                {
                    Terret_Billing.Application.Logging.Logger.LogInfo($"Branch {Branch.ShopName} created successfully");
                    MessageBox.Show("Branch created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    await LoadBranchesAsync();
                    ExecuteCancel(null);
                }
                else
                {
                    ErrorMessage = "Failed to create branch. Please try again.";
                    Terret_Billing.Application.Logging.Logger.LogError($"Failed to create branch {Branch.ShopName}");
                    MessageBox.Show("Failed to create branch.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
                Terret_Billing.Application.Logging.Logger.LogError("Error during branch save process", ex);
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsBusy = false;
                Terret_Billing.Application.Logging.Logger.LogInfo("ExecuteSave finished.");
            }
        }

        private void ExecuteCancel(object parameter)
        {
            if (parameter is Window window)
            {
                window.Close();
            }
            else
            {
                foreach (Window win in System.Windows.Application.Current.Windows)
                {
                    if (win.DataContext == this)
                    {
                        win.Close();
                        break;
                    }
                }
            }
        }

        private void SelectFirmType(object type)
        {
            if (type?.ToString() == "MY FIRM")
            {
                IsMyFirmSelected = true;
                IsSelfSelected = false;
                Branch.FirmType = "MY FIRM";
            }
            else
            {
                IsMyFirmSelected = false;
                IsSelfSelected = true;
                Branch.FirmType = "SELF";
            }
        }

        private void UploadLogo()
        {
            // Implement file dialog logic
        }

        private void UploadLeftImage()
        {
            // Implement file dialog logic
        }

        private void UploadSignature()
        {
            // Implement file dialog logic
        }

        private void UploadQrCode()
        {
            // Implement file dialog logic
        }

        #endregion

        #region Helper Methods

        private async Task LoadBranchesAsync()
        {
            try
            {
                IsBusy = true;
                var branches = await _branchRepository.GetAllCompanyDetailsAsync(_currentUser.id);
                Branches = new ObservableCollection<Branch>(branches);
                Terret_Billing.Application.Logging.Logger.LogInfo("Branches loaded successfully");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to load branches: {ex.Message}";
                Terret_Billing.Application.Logging.Logger.LogError("Error loading branches", ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task DeleteBranchAsync(Branch branch)
        {
            if (branch == null) return;
            try
            {
                IsBusy = true;
                var result = await _branchRepository.DeleteBranchAsync(branch.id);
                if (result)
                {
                    Terret_Billing.Application.Logging.Logger.LogInfo($"Branch {branch.shop_name} deleted");
                    await LoadBranchesAsync();
                }
                else
                {
                    ErrorMessage = "Failed to delete branch.";
                    Terret_Billing.Application.Logging.Logger.LogError($"Failed to delete branch {branch.shop_name}");
                }
            }
            catch (Exception ex)
            {
                Terret_Billing.Application.Logging.Logger.LogError("Error deleting branch", ex);
                ErrorMessage = $"Error: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool ValidateInput()
        {
            Terret_Billing.Application.Logging.Logger.LogInfo("FirmId value in ValidateInput: " + Branch.FirmId);
            if (string.IsNullOrWhiteSpace(Branch.FirmId))
            {
                ErrorMessage = "Firm ID is required.";
                //FirmId.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(Branch.ShopName))
            {
                ErrorMessage = "Shop Name is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Branch.Address))
            {
                ErrorMessage = "Address is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Branch.State))
            {
                ErrorMessage = "State is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Branch.District))
            {
                ErrorMessage = "District is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Branch.City))
            {
                ErrorMessage = "City is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Branch.Pincode))
            {
                ErrorMessage = "Pincode is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Branch.PhoneNumber))
            {
                ErrorMessage = "Phone Number is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Branch.Email))
            {
                ErrorMessage = "Email is required.";
                return false;
            }

            if (!IsValidEmail(Branch.Email))
            {
                ErrorMessage = "Please enter a valid email address.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Branch.FirmType))
            {
                ErrorMessage = "Please select a firm type (MY FIRM or SELF).";
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^[0-9]{10}$");
        }

        private bool IsValidGSTIN(string gstin)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(gstin, @"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$");
        }

        private bool IsValidPAN(string pan)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(pan, @"^[A-Z]{5}[0-9]{4}[A-Z]{1}$");
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}