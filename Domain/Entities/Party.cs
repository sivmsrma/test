using System;

namespace Terret_Billing.Domain.Entities
{
    public class Party
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string MobileNumber { get; private set; }
        public string Gender { get; private set; }
        public string Email { get; private set; }
        public string GSTNumber { get; private set; }
        public string PANNumber { get; private set; }
        public string Address { get; private set; }
        public string State { get; private set; }
        public string StateCode { get; private set; }
        public string District { get; private set; }
        public string City { get; private set; }
        public string Village { get; private set; }
        public string ColonyTower { get; private set; }
        public string PinCode { get; private set; }
        public string AccountNumber { get; private set; }
        public string AccountType { get; private set; }
        public string Ifsc { get; private set; }
        public string BankName { get; private set; }
        public string BankBranch { get; private set; }
        public string Narration { get; private set; }
        public DateTime? CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public bool IsActive { get; private set; }

        private Party() { } // For EF Core

        public static Party Create(
            string name,
            string mobileNumber,
            string gender = null,
            string email = null,
            string gstNumber = null,
            string panNumber = null,
            string address = null,
            string state = null,
            string stateCode = null,
            string district = null,
            string city = null,
            string village = null,
            string colonyTower = null,
            string pinCode = null,
            string accountNumber = null,
            string accountType = null,
            string ifsc = null,
            string bankName = null,
            string bankBranch = null,
            string narration = null)
        {
            return new Party
            {
                Name = name,
                MobileNumber = mobileNumber,
                Gender = gender,
                Email = email,
                GSTNumber = gstNumber,
                PANNumber = panNumber,
                Address = address,
                State = state,
                StateCode = stateCode,
                District = district,
                City = city,
                Village = village,
                ColonyTower = colonyTower,
                PinCode = pinCode,
                AccountNumber = accountNumber,
                AccountType = accountType,
                Ifsc = ifsc,
                BankName = bankName,
                BankBranch = bankBranch,
                Narration = narration,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                IsActive = true
            };
        }

        public void Update(
            string name = null,
            string mobileNumber = null,
            string gender = null,
            string email = null,
            string gstNumber = null,
            string panNumber = null,
            string address = null,
            string state = null,
            string stateCode = null,
            string district = null,
            string city = null,
            string village = null,
            string colonyTower = null,
            string pinCode = null,
            string accountNumber = null,
            string accountType = null,
            string ifsc = null,
            string bankName = null,
            string bankBranch = null,
            string narration = null)
        {
            if (name != null) Name = name;
            if (mobileNumber != null) MobileNumber = mobileNumber;
            if (gender != null) Gender = gender;
            if (email != null) Email = email;
            if (gstNumber != null) GSTNumber = gstNumber;
            if (panNumber != null) PANNumber = panNumber;
            if (address != null) Address = address;
            if (state != null) State = state;
            if (stateCode != null) StateCode = stateCode;
            if (district != null) District = district;
            if (city != null) City = city;
            if (village != null) Village = village;
            if (colonyTower != null) ColonyTower = colonyTower;
            if (pinCode != null) PinCode = pinCode;
            if (accountNumber != null) AccountNumber = accountNumber;
            if (accountType != null) AccountType = accountType;
            if (ifsc != null) Ifsc = ifsc;
            if (bankName != null) BankName = bankName;
            if (bankBranch != null) BankBranch = bankBranch;
            if (narration != null) Narration = narration;
            
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }
    }
} 