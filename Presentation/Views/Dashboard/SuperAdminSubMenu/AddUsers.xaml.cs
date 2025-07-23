using System.Windows;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Application.Logging;
using Terret_Billing.Presentation.Models;
using Terret_Billing.Domain.Interfaces;
using System.Collections.ObjectModel;
using Terret_Billing.Infrastructure.Data.Repositories;
using Terret_Billing.Infrastructure.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using Terret_Billing.Presentation.Views.Dashboard.ManagerSubMenu;

namespace Terret_Billing.Presentation.Views.Dashboard.SuperAdminSubMenu
{

    public partial class AddUsers : Window
    {
        private User _currentUser;
        private AddUsersViewModel _viewModel;
        private readonly IUserRepository _userRepository;
        //public ObservableCollection<AddUsersViewModel> Users { get; }


        public AddUsers(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;

            _viewModel = new AddUsersViewModel(_currentUser);
            DataContext = _viewModel;
            IDatabaseHelper helper = new MySqlDatabaseHelper();
            _userRepository = new UserRepository(helper);
            LoadUser();

        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private async void LoadUser()
        {
            
            var userlist = await _userRepository.GetAllAsync(_currentUser.Id);
            
            foreach (var SingleUser in userlist)
            {

                _viewModel.Users.Add(new UserModel
                {
                    Username = SingleUser.UserName,
                    AssignedBranch = SingleUser.assigned_branch,
                    UserRole = SingleUser.user_type
                });
            }
        }

         
    }
}
