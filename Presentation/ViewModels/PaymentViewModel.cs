using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Terret_Billing.Presentation.Models;
using Terret_Billing.Presentation.Commands;

namespace Terret_Billing.Presentation.ViewModels
{
    public class PaymentViewModel : INotifyPropertyChanged
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["BillingDb"].ConnectionString;

        // Properties for payment type
        private bool _isBillWisePayment = true;
        private bool _isPartyWisePayment;

        // Properties for selected items
        private Customer _selectedParty;
        private PendingPurchase _selectedPurchase;
        private Payment _currentPayment;

        // Collections
        public ObservableCollection<Customer> Parties { get; private set; }
        public ObservableCollection<PendingPurchase> PendingPurchases { get; private set; }

        // Commands
        public ICommand SaveCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }

        public ICommand CancelCommand { get; }

        public PaymentViewModel()
        {
            // Initialize collections
            Parties = new ObservableCollection<Customer>();
            PendingPurchases = new ObservableCollection<PendingPurchase>();

            // Initialize commands
            SaveCommand = new RelayCommand(SavePayment);
            ClearCommand = new RelayCommand(ClearForm);
            CancelCommand = new RelayCommand(CloseWindow);

            // Initialize objects
            CurrentPayment = new Payment
            {
                PaymentDate = DateTime.Now
            };
        }

        // Properties with change notification
        public bool IsBillWisePayment
        {
            get => _isBillWisePayment;
            set
            {
                _isBillWisePayment = value;
                if (value)
                {
                    IsPartyWisePayment = false;
                }
                OnPropertyChanged();
            }
        }

        public bool IsPartyWisePayment
        {
            get => _isPartyWisePayment;
            set
            {
                _isPartyWisePayment = value;
                if (value)
                {
                    IsBillWisePayment = false;
                }
                OnPropertyChanged();
            }
        }

        public Customer SelectedParty
        {
            get => _selectedParty;
            set
            {
                _selectedParty = value;
                if (_selectedParty != null)
                {
                    CurrentPayment.Id = _selectedParty.Id;
                    LoadPendingPurchases();
                }
                OnPropertyChanged();
            }
        }

        public PendingPurchase SelectedPurchase
        {
            get => _selectedPurchase;
            set
            {
                _selectedPurchase = value;
                if (_selectedPurchase != null && IsBillWisePayment)
                {
                    CurrentPayment.BillId = _selectedPurchase.Id;
                    CurrentPayment.Amount = _selectedPurchase.RemainingAmount;
                }
                OnPropertyChanged();
            }
        }

        public Payment CurrentPayment
        {
            get => _currentPayment;
            set
            {
                _currentPayment = value;
                OnPropertyChanged();
            }
        }

        // Methods
        public void LoadParties()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand("SELECT Id, Name, MobileNumber FROM Party WHERE IsActive = 1", connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Parties.Add(new Customer
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    MobileNumber = reader.GetString(2)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading parties: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadPendingPurchases()
        {
            if (SelectedParty == null) return;

            PendingPurchases.Clear();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    // Query to get pending bills for the selected party
                    string query = @"
                        SELECT p.Id, p.BillNo, p.PurchaseDate, p.NetAmount,
                               IFNULL(SUM(pm.Amount), 0) AS PaidAmount,
                               p.NetAmount - IFNULL(SUM(pm.Amount), 0) AS RemainingAmount
                        FROM Purchases p
                        LEFT JOIN Payments pm ON p.Id = pm.BillId
                        WHERE p.SupplierId = @SupplierId
                        GROUP BY p.Id, p.BillNo, p.PurchaseDate, p.NetAmount
                        HAVING p.NetAmount - IFNULL(SUM(pm.Amount), 0) > 0
                        ORDER BY p.PurchaseDate DESC";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SupplierId", SelectedParty.Id);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PendingPurchases.Add(new PendingPurchase
                                {
                                    Id = reader.GetInt32(0),
                                    BillNo = reader.GetString(1),
                                    PurchaseDate = reader.GetDateTime(2),
                                    NetAmount = reader.GetDecimal(3),
                                    PaidAmount = reader.GetDecimal(4),
                                    RemainingAmount = reader.GetDecimal(5)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading pending purchases: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SavePayment(object parameter)
        {
            try
            {
                if (SelectedParty == null)
                {
                    MessageBox.Show("Please select a party.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (CurrentPayment.Amount <= 0)
                {
                    MessageBox.Show("Please enter a valid amount.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace((string)CurrentPayment.PaymentMode))
                {
                    MessageBox.Show("Please select a payment mode.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (IsBillWisePayment && SelectedPurchase == null)
                {
                    MessageBox.Show("Please select a bill.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = @"
                        INSERT INTO Payments (
                            PartyId,
                            BillId,
                            Amount,
                            PaymentDate,
                            PaymentMode,
                            ReferenceNumber,
                            Remarks,
                            CreatedAt
                        ) VALUES (
                            ?PartyId,
                            ?BillId,
                            ?Amount,
                            ?PaymentDate,
                            ?PaymentMode,
                            ?ReferenceNumber,
                            ?Remarks,
                            ?CreatedAt
                        )";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("?PartyId", SelectedParty.Id);

                        if (IsBillWisePayment && SelectedPurchase != null)
                        {
                            command.Parameters.AddWithValue("?BillId", SelectedPurchase.Id);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("?BillId", DBNull.Value);
                        }

                        command.Parameters.AddWithValue("?Amount", CurrentPayment.Amount);
                        command.Parameters.AddWithValue("?PaymentDate", CurrentPayment.PaymentDate);
                        command.Parameters.AddWithValue("?PaymentMode", CurrentPayment.PaymentMode);
                        command.Parameters.AddWithValue("?ReferenceNumber", CurrentPayment.ReferenceNumber ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("?Remarks", CurrentPayment.Remarks ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("?CreatedAt", DateTime.Now);

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Payment saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // Refresh data
                LoadPendingPurchases();

                // Clear form
                ClearForm(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving payment: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearForm(object parameter)
        {
            CurrentPayment = new Payment
            {
                PaymentDate = DateTime.Now,
                PartyId = SelectedParty?.Id ?? 0
            };

            SelectedPurchase = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CloseWindow(object parameter)
        {
            if (parameter is Window window)
            {
                window.Close();
            }
        }
    }

}