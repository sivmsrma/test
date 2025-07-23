using System;

namespace Terret_Billing.Domain.Entities
{
    public class User
    {
        public int id { get; set; }

        private string _user_name;
        public string user_name
        {
            get => _user_name ?? string.Empty;
            set => _user_name = value;
        }

        private string _email;
        public string email
        {
            get => _email ?? string.Empty;
            set => _email = value;
        }

        private string _phone_number;
        public string phone_number
        {
            get => _phone_number ?? string.Empty;
            set => _phone_number = value;
        }

        private string _password;
        public string password
        {
            get => _password ?? string.Empty;
            set => _password = value;
        }

        private string _user_type;
        public string user_type
        {
            get => _user_type ?? string.Empty;
            set => _user_type = value;
        }

        private string _profile_image;
        public string profile_image
        {
            get => _profile_image ?? string.Empty;
            set => _profile_image = value;
        }

        public int created_by { get; set; } 
        public int years_of_experience { get; set; } = 0;
        public DateTime? created_on { get; set; }

        private string _address;
        public string address
        {
            get => _address ?? string.Empty;
            set => _address = value;
        }

        private string _gender;
        public string gender
        {
            get => _gender ?? string.Empty;
            set => _gender = value;
        }

        private string _aadhar_front;
        public string aadhar_front
        {
            get => _aadhar_front ?? string.Empty;
            set => _aadhar_front = value;
        }

        private string _aadhar_back;
        public string aadhar_back
        {
            get => _aadhar_back ?? string.Empty;
            set => _aadhar_back = value;
        }

        private string _pancard;
        public string pancard
        {
            get => _pancard ?? string.Empty;
            set => _pancard = value;
        }

        private string _resume;
        public string resume
        {
            get => _resume ?? string.Empty;
            set => _resume = value;
        }

        private string _certificate;
        public string certificate
        {
            get => _certificate ?? string.Empty;
            set => _certificate = value;
        }

        private string _others;
        public string others
        {
            get => _others ?? string.Empty;
            set => _others = value;
        }

        public decimal? salary { get; set; }

        private string _assigned_branch;
        public string assigned_branch
        {
            get => _assigned_branch ?? string.Empty;
            set => _assigned_branch = value;
        }

        private string _assigned_branch_name;
        public string assigned_branch_name
        {
            get => _assigned_branch_name ?? string.Empty;
            set => _assigned_branch_name = value;
        }

        private string _firm_id;
        public string firm_id
        {
            get => _firm_id ?? string.Empty;
            set => _firm_id = value;
        }
        private int _server_id;
        public int server_id
        {
            get => _server_id ;
            set => _server_id = value;
        }

        // PascalCase aliases (optional)
        public int Id { get => id; set => id = value; }
        public string UserName { get => user_name; set => user_name = value; }
        public string Email { get => email; set => email = value; }
        public string PhoneNumber { get => phone_number; set => phone_number = value; }
        public string Password { get => password; set => password = value; }
        public string UserType { get => user_type; set => user_type = value; }
        public string ProfileImage { get => profile_image; set => profile_image = value; }
        public int CreatedBy { get => created_by; set => created_by = value; }
        public int YearsOfExperience { get => years_of_experience; set => years_of_experience = value; }
        public DateTime? CreatedOn { get => created_on; set => created_on = value; }
        public string Address { get => address; set => address = value; }
        public string Gender { get => gender; set => gender = value; }
        public string AadharFront { get => aadhar_front; set => aadhar_front = value; }
        public string AadharBack { get => aadhar_back; set => aadhar_back = value; }
        public string Pancard { get => pancard; set => pancard = value; }
        public string Resume { get => resume; set => resume = value; }
        public string Certificate { get => certificate; set => certificate = value; }
        public string Others { get => others; set => others = value; }
        public decimal? Salary { get => salary; set => salary = value; }
        public string AssignedBranch { get => assigned_branch; set => assigned_branch = value; }
        public string AssignedBranchName { get => assigned_branch_name; set => assigned_branch_name = value; }
        public string FirmId { get => firm_id; set => firm_id = value; }
        public int ServerId { get => server_id; set => server_id = value; }
    }
}
