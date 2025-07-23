using Mysqlx.Datatypes;
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
using Terret_Billing.Infrastructure.Data.Repositories;
using Terret_Billing.Infrastructure.Helpers;
using Terret_Billing.Presentation.Commands;
using Terret_Billing.Presentation.Models;

namespace Terret_Billing.Presentation.Views.Dashboard.ManagerSubMenu
{

    public class AddUsersViewModel : INotifyPropertyChanged
    {
        #region Private Fields

        private readonly IUserRepository _userRepository;
        private readonly IBranchRepository _branchRepository;
        private UserModel _user;
        private bool _isPasswordVisible;
        private bool _isConfirmPasswordVisible;
        private Branch _selectedBranch;
        private ObservableCollection<Branch> _branches;
        private bool _isBusy;
        private string _errorMessage;
        private readonly User _currentUser;
        public ObservableCollection<UserModel> Users { get; }

        // Paging properties
        private int _pageNumber = 1;
        private int _pageSize = 10;
        private int _totalCount;
        public int PageNumber
        {
            get => _pageNumber;
            set { _pageNumber = value; OnPropertyChanged(); OnPropertyChanged(nameof(TotalPages)); }
        }
        public int PageSize
        {
            get => _pageSize;
            set { _pageSize = value; OnPropertyChanged(); OnPropertyChanged(nameof(TotalPages)); }
        }
        public int TotalCount
        {
            get => _totalCount;
            set { _totalCount = value; OnPropertyChanged(); OnPropertyChanged(nameof(TotalPages)); }
        }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }

        #endregion

        #region Constructor

        public AddUsersViewModel(User user)
        {
            IDatabaseHelper helper = new MySqlDatabaseHelper();
            _userRepository = new UserRepository(helper);
            _branchRepository = new BranchRepository(helper);
            Users = new ObservableCollection<UserModel>();

            _user = new UserModel
            {
                IsActive = true
            };
            _currentUser = user;

            SaveCommand = new Commands.RelayCommand(ExecuteSave, parameter => CanExecuteSave(parameter));
            CancelCommand = new Commands.RelayCommand(ExecuteCancel, parameter => true);
            TogglePasswordVisibilityCommand = new Commands.RelayCommand(ExecuteTogglePasswordVisibility, parameter => true);
            ToggleConfirmPasswordVisibilityCommand = new Commands.RelayCommand(ExecuteToggleConfirmPasswordVisibility, parameter => true);
            NextPageCommand = new Commands.RelayCommand(_ => NextPage(), _ => PageNumber < TotalPages);
            PreviousPageCommand = new Commands.RelayCommand(_ => PreviousPage(), _ => PageNumber > 1);

            LoadBranchesAsync();
            LoadUsers();
        }


        #endregion

        #region Properties

        public UserModel User
        {
            get => _user;
            set
            {
                if (_user != value)
                {
                    _user = value;
                    OnPropertyChanged();
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public bool IsPasswordVisible
        {
            get => _isPasswordVisible;
            set
            {
                if (_isPasswordVisible != value)
                {
                    _isPasswordVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsConfirmPasswordVisible
        {
            get => _isConfirmPasswordVisible;
            set
            {
                if (_isConfirmPasswordVisible != value)
                {
                    _isConfirmPasswordVisible = value;
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
                    if (_user != null)
                    {
                        _user.FirmId = _selectedBranch?.FirmId;
                        _user.AssignedBranch = _selectedBranch?.ShopName;
                    }
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

        #endregion

        #region Commands

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand TogglePasswordVisibilityCommand { get; }
        public ICommand ToggleConfirmPasswordVisibilityCommand { get; }

        private async void ExecuteSave(object parameter)
        {
            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                // Validate input
                Application.Logging.Logger.LogInfo("Calling ValidateInput...");
                if (!ValidateInput())
                {
                    MessageBox.Show("Validation failed: " + ErrorMessage, "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Application.Logging.Logger.LogInfo($"ValidateInput returned false. ErrorMessage: {ErrorMessage}");
                    return;
                }
                Application.Logging.Logger.LogInfo("ValidateInput returned true.");

                // Create user object
                var user = new User
                {
                    user_name = User.Username,
                    email = User.Email,
                    phone_number = User.PhoneNumber,
                    password = User.Password,
                    user_type = User.UserRole,
                    assigned_branch = User.AssignedBranch,
                    firm_id = User.FirmId,
                    profile_image = User.ProfileImage,
                    created_by = _currentUser.id,
                    years_of_experience = 0,
                    address = User.Address,
                    gender = User.Gender,
                    aadhar_front = User.AadharFront,
                    aadhar_back = User.AadharBack,
                    pancard = User.Pancard,
                    resume = User.Resume,
                    certificate = User.Certificate,
                    others = User.Others,
                    salary = User.Salary,
                    CreatedOn = User.CreatedOn ?? DateTime.Now,
                    Salary = User.Salary ?? 0

                };
                // Null check before usage
                if (User == null)
                {
                    MessageBox.Show("User data is missing. Cannot continue.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (_userRepository == null)
                {
                    MessageBox.Show("User repository is not initialized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }


                // Save user to database
                Application.Logging.Logger.LogInfo($"Attempting to create user: {User.Username}");
                var userId = await _userRepository.AddAsync(user);
                MessageBox.Show($"User Created Succesfully", "Debug", MessageBoxButton.OK, MessageBoxImage.Information);

                // Reload paged users after save
                LoadUsers();

                if (userId > 0)
                {
                    // Save user permissions
                    Application.Logging.Logger.LogInfo($"Attempting to save permissions for userId: {userId}");
                    await SaveUserPermissionsAsync(userId);

                    Application.Logging.Logger.LogInfo($"User {User.Username} created successfully with ID {userId}");
                    MessageBox.Show("User created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    Application.Logging.Logger.LogInfo("Success message box shown. Closing window...");

                    // Close the window
                    ExecuteCancel(null);
                }

            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
                Application.Logging.Logger.LogError("Error during user save process", ex);
                MessageBox.Show($"Insert Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsBusy = false;
                Application.Logging.Logger.LogInfo("ExecuteSave finished.");
            }
        }

        private bool CanExecuteSave(object parameter)
        {
            Application.Logging.Logger.LogInfo($"CanExecuteSave: IsBusy={IsBusy}, Username={User?.Username}, Email={User?.Email}, UserRole={User?.UserRole}, SelectedBranch is null: {SelectedBranch == null}");

            return !IsBusy &&
                   !string.IsNullOrWhiteSpace(User?.Username) &&
                   !string.IsNullOrWhiteSpace(User?.Email) &&
                   !string.IsNullOrWhiteSpace(User?.Password) &&
                   !string.IsNullOrWhiteSpace(User?.UserRole) &&
                   SelectedBranch != null;
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

        private void ExecuteTogglePasswordVisibility(object parameter)
        {
            IsPasswordVisible = !IsPasswordVisible;
        }

        private void ExecuteToggleConfirmPasswordVisibility(object parameter)
        {
            IsConfirmPasswordVisible = !IsConfirmPasswordVisible;
        }

        #endregion

        #region Helper Methods

        private async void LoadBranchesAsync()
        {
            try
            {
                IsBusy = true;

                var branches = await _branchRepository.GetAllCompanyDetailsAsync(_currentUser.id);
                Branches = new ObservableCollection<Branch>(branches);

                if (Branches.Count > 0)
                {
                    SelectedBranch = Branches[0];
                }

                Application.Logging.Logger.LogInfo("Branches loaded successfully");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to load branches: {ex.Message}";
                Application.Logging.Logger.LogError("Error loading branches", ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void LoadUsers()
        {
            var (users, totalCount) = await _userRepository.GetAllUsersPagedAsync(PageNumber, PageSize, _currentUser.id);
            Users.Clear();
            foreach (var user in users)
            {
                Users.Add(new UserModel
                {
                    Username = user.user_name,
                    AssignedBranch = user.assigned_branch,
                    UserRole = user.user_type,
                    Email = user.email,
                    PhoneNumber = user.phone_number,
                    FirmId = user.firm_id
                });
            }
            TotalCount = totalCount;
        }

        private void NextPage()
        {
            if (PageNumber < TotalPages)
            {
                PageNumber++;
                LoadUsers();
            }
        }
        private void PreviousPage()
        {
            if (PageNumber > 1)
            {
                PageNumber--;
                LoadUsers();
            }
        }

        private bool ValidateInput()
        {
            // Validate username
            if (string.IsNullOrWhiteSpace(User.Username))
            {
                ErrorMessage = "Username is required.";
                return false;
            }

            // Validate email
            if (string.IsNullOrWhiteSpace(User.Email) || !IsValidEmail(User.Email))
            {
                ErrorMessage = "Please enter a valid email address.";
                return false;
            }

            // Validate phone number
            if (string.IsNullOrWhiteSpace(User.PhoneNumber))
            {
                ErrorMessage = "Phone number is required.";
                return false;
            }

            // Validate password
            if (string.IsNullOrWhiteSpace(User.Password))
            {
                ErrorMessage = "Password is required.";
                return false;
            }

            // Validate password strength
            if (User.Password.Length < 6)
            {
                ErrorMessage = "Password must be at least 6 characters long.";
                return false;
            }

            // Validate user role
            if (string.IsNullOrWhiteSpace(User.UserRole))
            {
                ErrorMessage = "Please select a user role.";
                return false;
            }

            // Validate branch
            if (SelectedBranch == null)
            {
                ErrorMessage = "Please select a branch.";
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

        private async Task SaveUserPermissionsAsync(int userId)
        {
            try
            {
                // Create permissions object
                var permissions = new UserPermission
                {
                    user_id = userId,
                    can_create_users = User.CanCreateUsers,
                    can_edit_company_settings = User.CanEditCompanySettings,
                    can_view_reports = User.CanViewReports,
                    can_create_edit_invoices = User.CanCreateEditInvoices,
                    can_manage_inventory = User.CanManageInventory,
                    is_active = User.IsActive,
                    branch_id = SelectedBranch.id,
                    created_on = DateTime.Now
                };

                // Save permissions to database
                await _userRepository.SaveUserPermissionsAsync(permissions);

                Application.Logging.Logger.LogInfo($"Permissions saved for user ID {userId}");
            }
            catch (Exception ex)
            {
                Application.Logging.Logger.LogError($"Error saving permissions for user ID {userId}", ex);
                throw;
            }
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
