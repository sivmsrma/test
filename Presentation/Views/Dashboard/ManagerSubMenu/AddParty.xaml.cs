using Google.Protobuf.WellKnownTypes;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input; // Required for KeyEventArgs
using Terret_Billing.Application.Interfaces;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Presentation.Models;
using Terret_Billing.Presentation.ViewModels;
using Terret_Billing.Application.Logging;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using Terret_Billing.Application.Services;

namespace Terret_Billing.Presentation.Views.Dashboard.ManagerSubMenu
{
    /// <summary>
    /// Interaction logic for AddParty.xaml
    /// </summary>
    public partial class AddParty : Window

    {
        private readonly Action<string> _log;
        private User _currentUser;
        private readonly IPartyService _partyService;
        

        public AddParty(IPartyService partyService ,User user)
        {
            //InitializeComponent();
            //_log = Logger.LogInfo;
            //DataContext = new AddPartyViewModel(partyService);
            //txtName.Focus(); // Set initial focus to Name field (ensure x:Name="NameTextBox" exists in XAML)
            InitializeComponent();
            _partyService = partyService;
            _currentUser = user;
            _log = Logger.LogInfo;
            txtName.Focus();
            this.Closed += (s, e) => {  };
            SetUser();


        }

        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && sender is Control control)
            {
                control.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                e.Handled = true;
            }
        }

        public void SetUser()
        {
             _log($"Manager Dashboard started for user: {_currentUser.user_name},{_currentUser.id}");
            this.Title = $"CreateAccount - {_currentUser.user_name},{_currentUser.id}";
            // Initialize the ViewModel with the party service and current user
            DataContext = new AddPartyViewModel(_partyService, _currentUser);
            // Load any user-specific data
            LoadUserData();
        }
        private void LoadUserData()
        {
            // Load user-specific data here
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var customer = new Customer
            {
                Name = txtName.Text,
                MobileNumber = txtMobile.Text,
                MailId = txtEmail.Text,
                GSTNumber = txtGSTNumber.Text
            };

            if (!ValidateCustomer(customer, out string error))
            {
                MessageBox.Show(error, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);

                // Map keywords to corresponding controls
                var focusMap = new Dictionary<string, Control>(StringComparer.OrdinalIgnoreCase)
        {
            { "Name", txtName },
            { "Mobile", txtMobile },
            { "email", txtEmail },
            { "GST", txtGSTNumber }
        };

                // Find first matching key in error message and set focus
                foreach (var key in focusMap.Keys)
                {
                    if (error.Contains(key, StringComparison.OrdinalIgnoreCase))
                    {
                        focusMap[key].Focus();
                        break;
                    }
                }

                return;
            }
        }


        public bool ValidateCustomer(Customer customer, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(customer.Name))
            {
                errorMessage = "Name is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(customer.MobileNumber))
            {
                errorMessage = "Mobile number is required.";
                return false;
            }

            if (!Regex.IsMatch(customer.MobileNumber, @"^\d{10}$"))
            {
                errorMessage = "Mobile number must be exactly 10 digits.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(customer.MailId) &&
                !Regex.IsMatch(customer.MailId, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                errorMessage = "Invalid email format.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(customer.GSTNumber) && !ValidateGSTNumberAsync(customer.GSTNumber))
            {
                errorMessage = "Invalid GST number format.";
                return false;
            }

            return true;
        }

        public bool ValidateGSTNumberAsync(string gstNumber)
        {
            if (string.IsNullOrWhiteSpace(gstNumber))
                return false;

            // GST Number format: 2 digit state code + 10 digit PAN + 1 digit entity number + 1 digit default (Z) + 1 digit checksum
            return Regex.IsMatch(gstNumber, @"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$");
        }







        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            txtName.Clear();
            txtMobile.Clear();
            txtEmail.Clear();
            txtGSTNumber.Clear();
            txtPanNumber.Clear();
            txtAddress.Clear();
            txtState.Clear();
            txtStateCode.Clear();
            txtDistrict.Clear();
            txtCity.Clear();
            txtVillage.Clear();
            txtPinCode.Clear();
            txtAccountNumber.Clear();
            txtIfsc.Clear();
            txtBankName.Clear();
            txtBankBranch.Clear();
            txtNarration.Clear();
        }

        
    }
}