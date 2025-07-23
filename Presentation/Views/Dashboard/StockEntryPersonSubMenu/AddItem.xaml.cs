using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using Dapper;
using MySql.Data.MySqlClient;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Application.Services;
using Terret_Billing.Presentation.ViewModels;
using System.Configuration;
using Terret_Billing.Infrastructure.Data.Repositories;
using System.Collections.Generic;
using Terret_Billing.Infrastructure.Data;
using System.Security.Cryptography.X509Certificates;
using Terret_Billing.Application.Interfaces;

namespace Terret_Billing.Presentation.Views.Dashboard.StockEntryPersonSubMenu
{
    public partial class AddItem : Window
    {
        private readonly List<string> _connectionString;
        private readonly IDatabaseHelper _databaseHelper;
        private  User _currentUser;
        private string _username;
        private string _firmId;
        private readonly Action<string> _log;

        private ObservableCollection<HsnItem> _hsnItems = new ObservableCollection<HsnItem>();
        public ObservableCollection<HsnItem> HsnItems
        {
            get { return _hsnItems; }
            set { _hsnItems = value; }
        }
        public AddItem(IDatabaseHelper databaseHelper)
        {
            InitializeComponent();
            _databaseHelper = databaseHelper ?? throw new ArgumentNullException(nameof(databaseHelper));
            

            _currentUser = new User();
            _connectionString = _databaseHelper.GetConnectionString();
            InitializeViewModel();
        }

        public AddItem(string metalType = null, User user = null)
        {
            InitializeComponent();
            
            // Initialize database helper with default implementation
            _databaseHelper = new MySqlDatabaseHelper();
            _connectionString = _databaseHelper.GetConnectionString();
            
            // Set the current user
            _currentUser = user ?? throw new ArgumentNullException(nameof(user), "User cannot be null. Please ensure user is properly logged in.");
            
            InitializeViewModel(metalType);
        }

        public void SetUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _currentUser = user;
            _log($"Manager Dashboard started for user: {_currentUser.user_name},{_currentUser.id}");
            this.Title = $"CreateAccount - {_currentUser.user_name},{_currentUser.id}";
            // Initialize the ViewModel with the party service and current user
            
            // Load any user-specific data
            LoadUserData();
        }
        private void LoadUserData()
        {
            // Load user-specific data here
        }

        private void InitializeViewModel(string metalType = null)
        {
            // Create ItemRepository instance with database helper
            var itemRepository = new ItemRepository(_databaseHelper);
            
            // Get the first connection string (local) for ItemService
            var connectionString = _databaseHelper.GetConnectionString().FirstOrDefault() ?? 
                throw new InvalidOperationException("No connection strings available");
                
            // Create ItemService instance with the repository and connection string
            var itemService = new ItemService(itemRepository, connectionString);

            // Create and set DataContext to AddItemViewModel
            var viewModel = new AddItemViewModel(itemService,  _currentUser);
            this.DataContext = viewModel;

            if (!string.IsNullOrEmpty(metalType))
            {
                // Set MetalType property on the ViewModel and ensure it's properly capitalized
                viewModel.MetalType = metalType.Trim().ToLower() switch
                {
                    "gold" => "Gold",
                    "silver" => "Silver",
                    "diamond" => "Diamond",
                    _ => metalType  // Keep original if not recognized
                };
            }
        }

        private void LoadHsnList(string metalType)
        {
            // This method is no longer needed here as the ViewModel handles loading HSN list
            // Get connection string from App.config
            // var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["BillingDb"].ConnectionString;
            // using (IDbConnection db = new MySqlConnection(connectionString))
            // {
            //     string sql = "SELECT hsn_id AS HsnId, metal AS Metal, hsn_code AS Hsn FROM hsn_list WHERE metal LIKE @Metal";
            //     var items = db.Query<HsnItem>(sql, new { Metal = "%" + metalType + "%" }).ToList();
            //     HsnItems.Clear();
            //     foreach (var item in items)
            //         HsnItems.Add(item);
            // }
        }

        private void SaveHsn_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Save HSN logic - This should also be moved to the ViewModel
            MessageBox.Show("Save HSN logic will be moved to ViewModel");
        }
    }
}
