using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Terret_Billing.Presentation.Models;
using Terret_Billing.Presentation.Commands;
using Terret_Billing.Application.Interfaces;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Terret_Billing.Domain.Entities;

namespace Terret_Billing.Presentation.ViewModels
{
    public class AddPartyViewModel : INotifyPropertyChanged
    {
        private readonly IPartyService _partyService;
        private Customer _customer;
        private readonly User _currentUser;

        public Customer Customer
        {
            get => _customer;
            set
            {
                _customer = value;
                OnPropertyChanged(nameof(Customer));
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand ValidateGSTCommand { get; }

        public AddPartyViewModel(IPartyService partyService, User currentUser)
        {
            
            _partyService = partyService ?? throw new ArgumentNullException(nameof(partyService));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            Customer = new Customer();
            SaveCommand = new RelayCommand(SaveParty);
            CancelCommand = new RelayCommand(CloseWindow);
        }      

        private async void SaveParty(object parameter)
        {
            try
            
            {
               
                  ValidateCustomer(Customer);
                 Customer.CreatedByUserId = _currentUser.server_id;
                Customer.FirmId = _currentUser.firm_id;
                 await _partyService.CreatePartyAsync(Customer);

                MessageBox.Show($"Party added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                CloseWindow(parameter);

            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding party: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        public void ValidateCustomer(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.Name))

                throw new ArgumentException("Name is required.", nameof(customer.Name));

            if (string.IsNullOrWhiteSpace(customer.MobileNumber))
                throw new ArgumentException("Mobile number is required.", nameof(customer.MobileNumber));

            // Validate mobile number format
            if (!Regex.IsMatch(customer.MobileNumber, @"^\d{10}$"))
                throw new ArgumentException("Mobile number must be exactly 10 digits.", nameof(customer.MobileNumber));

            // Validate email format if provided
            if (!string.IsNullOrWhiteSpace(customer.MailId) &&
                !Regex.IsMatch(customer.MailId, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Invalid email format.", nameof(customer.MailId));

            // Validate GST number if provided
            if (!string.IsNullOrEmpty(customer.GSTNumber))
            {
                ValidateGSTAsync(customer.GSTNumber);
            }
        }

        private void ValidateGSTAsync(string gstNumber)
        {
            if (!string.IsNullOrWhiteSpace(gstNumber))
            {
                if (!ValidateGSTNumberAsync(gstNumber))
                    throw new ArgumentException("Invalid GST number format.", nameof(gstNumber));

            }
        }

        public bool ValidateGSTNumberAsync(string gstNumber)
        {
            if (string.IsNullOrWhiteSpace(gstNumber))
                return false;

            // GST Number format: 2 digit state code + 10 digit PAN + 1 digit entity number + 1 digit default (Z) + 1 digit checksum
            return Regex.IsMatch(gstNumber, @"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$");
        }



        private void CloseWindow(object parameter)
        {
            if (parameter is Window window)
            {
                window.Close();
                
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 