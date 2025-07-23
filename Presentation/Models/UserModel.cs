using System;

namespace Terret_Billing.Presentation.Models
{
    public class UserModel
    {
        private string _username;
        public string Username
        {
            get => _username;
            set => _username = value ?? string.Empty;
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => _email = value ?? string.Empty;
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set => _phoneNumber = value ?? string.Empty;
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => _password = value ?? string.Empty;
        }

        private string _userRole;
        public string UserRole
        {
            get => _userRole;
            set => _userRole = value ?? string.Empty;
        }

        private string _assignedBranch;
        public string AssignedBranch
        {
            get => _assignedBranch;
            set => _assignedBranch = value ?? string.Empty;
        }

        private string _firmId;
        public string FirmId
        {
            get => _firmId;
            set => _firmId = value ?? string.Empty;
        }

        private string _profileImage;
        public string ProfileImage
        {
            get => _profileImage;
            set => _profileImage = value ?? string.Empty;
        }
        public string CreatedBy { get; set; } 
        public int YearsOfExperience { get; set; } = 0;

        private string _address;
        public string Address
        {
            get => _address;
            set => _address = value ?? string.Empty;
        }

        private string _gender;
        public string Gender
        {
            get => _gender;
            set => _gender = value ?? string.Empty;
        }

        private string _aadharFront;
        public string AadharFront
        {
            get => _aadharFront;
            set => _aadharFront = value ?? string.Empty;
        }

        private string _aadharBack;
        public string AadharBack
        {
            get => _aadharBack;
            set => _aadharBack = value ?? string.Empty;
        }

        private string _pancard;
        public string Pancard
        {
            get => _pancard;
            set => _pancard = value ?? string.Empty;
        }

        private string _resume;
        public string Resume
        {
            get => _resume;
            set => _resume = value ?? string.Empty;
        }

        private string _certificate;
        public string Certificate
        {
            get => _certificate;
            set => _certificate = value ?? string.Empty;
        }

        private string _others;
        public string Others
        {
            get => _others;
            set => _others = value ?? string.Empty;
        }
        private DateTime? created_on;

        public DateTime? CreatedOn
        {
            get => created_on ?? DateTime.Now;
            set => created_on = value;
        }


        public decimal? Salary { get; set; }

        public bool IsActive { get; set; }
        public bool CanCreateUsers { get; set; }
        public bool CanEditCompanySettings { get; set; }
        public bool CanViewReports { get; set; }
        public bool CanCreateEditInvoices { get; set; }
        public bool CanManageInventory { get; set; }
    }
}
