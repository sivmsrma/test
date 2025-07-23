CREATE TABLE `Company_details` (
  `Id` BIGINT AUTO_INCREMENT,
  `user_id` INT(11) DEFAULT NULL,
  `FirmID` VARCHAR(50)   NOT NULL,
  `RegistrationNo` VARCHAR(100)   DEFAULT NULL,
  `ShopName` VARCHAR(150)   DEFAULT NULL,
  `FirmDescription` TEXT   DEFAULT NULL,
  `Address` TEXT   DEFAULT NULL,
  `State` VARCHAR(100) DEFAULT NULL,
  `District` VARCHAR(100) DEFAULT NULL,
  `City` VARCHAR(100) DEFAULT NULL,
  `Pincode` VARCHAR(20) DEFAULT NULL,
  `PhoneNumber` VARCHAR(20) DEFAULT NULL,
  `Email` VARCHAR(100) DEFAULT NULL,
  `WebsiteName` VARCHAR(100) DEFAULT NULL,
  `FirmType` VARCHAR(50) DEFAULT NULL,
  `Comments` TEXT   DEFAULT NULL,
  `WhatsAppLink` VARCHAR(255) DEFAULT NULL,
  `FacebookLink` VARCHAR(255) DEFAULT NULL,
  `InstagramLink` VARCHAR(255) DEFAULT NULL,
  `EnvioceAlPid` VARCHAR(100)  DEFAULT NULL,
  `EnvioceApiKey` VARCHAR(100)  DEFAULT NULL,
  `EnvioceUsername` VARCHAR(100) DEFAULT NULL,
  `EnviocePassword` VARCHAR(100) DEFAULT NULL,
  `PaymentBankDetails` TEXT   DEFAULT NULL,
  `AccountHolderName` VARCHAR(255)   DEFAULT NULL,
  `PaymentBankACNo` VARCHAR(50)   DEFAULT NULL,
  `PaymentBankIFSCCode` VARCHAR(50)   DEFAULT NULL,
  `PaymentDeclaration` TEXT   DEFAULT NULL,
  `FinancialYearStartDate` DATE DEFAULT NULL,
  `CashBalance` DECIMAL(18,2) DEFAULT NULL,
  `GSTIN` VARCHAR(50)   DEFAULT NULL,
  `PANNumber` VARCHAR(20)   DEFAULT NULL,
  `PrincipalPalnStart` DECIMAL(18,2) DEFAULT NULL,
  `PrincipalPalnEnd` DECIMAL(18,2) DEFAULT NULL,
  `FormHeader` TEXT   DEFAULT NULL,
  `FormFooter` TEXT   DEFAULT NULL,
  `CompanyLogo` LONGBLOB,
  `FirmLogo` LONGBLOB,
  `OwnerSignature` LONGBLOB,
  `QRcode` LONGBLOB,
  `IsDataPostOnServer` TINYINT(1) DEFAULT NULL,
  `local_id` BIGINT(20) NOT NULL,
  PRIMARY KEY (`FirmID`, `local_id`),            -- ✅ Composite Primary Key
  UNIQUE KEY `UNQ_Company_Id` (`Id`)             -- ✅ Unique Auto-increment Id
);


CREATE TABLE `Diamond_tagging_entry` (
  `Dia_E_D_id` BIGINT AUTO_INCREMENT,
  `Dia_E_Firm_id` VARCHAR(255)   DEFAULT NULL,
  `Dia_E_user_name` VARCHAR(255)   DEFAULT NULL,
  `Dia_E_Stock_Type` VARCHAR(50)   DEFAULT NULL,
  `Dia_E_Particular` VARCHAR(255)   DEFAULT NULL,
  `Dia_E_Party_Name` VARCHAR(150)   DEFAULT NULL,
  `Dia_E_Invoice_No` VARCHAR(100)   DEFAULT NULL,
  `Dia_E_Date` DATE DEFAULT NULL,
  `Dia_E_Image` VARCHAR(255)   DEFAULT NULL,
  `Dia_E_Category` VARCHAR(255)   DEFAULT NULL,
  `Dia_E_SubCategory` VARCHAR(255)   DEFAULT NULL,
  `Dia_E_Design` VARCHAR(255)   DEFAULT NULL,
  `Dia_E_Purity` VARCHAR(50)   DEFAULT NULL,
  `Dia_E_HSN_No` VARCHAR(50)   DEFAULT NULL,
  `Dia_E_HUID_No` VARCHAR(50)   DEFAULT NULL,
  `Dia_E_Size` VARCHAR(50)   DEFAULT NULL,
  `Dia_E_pcs` INT(11) DEFAULT NULL,
  `Dia_E_Gross_wt` DECIMAL(10,3) DEFAULT NULL,
  `Dia_E_Less_wt` INT(20) DEFAULT NULL,
  `Dia_E_Diamond_Ct` DECIMAL(10,3) DEFAULT NULL,
  `Dia_E_Diamond_wt_gm` DECIMAL(10,3) DEFAULT NULL,
  `Dia_E_Net_wt` DECIMAL(10,3) DEFAULT NULL,
  `Dia_E_Diamond_Rate` DECIMAL(10,2) DEFAULT NULL,
  `Dia_E_Diamond_Amt` DECIMAL(10,2) DEFAULT NULL,
  `Dia_E_Net_Rate` DECIMAL(10,2) DEFAULT NULL,
  `Dia_E_Net_Amt` DECIMAL(10,2) DEFAULT NULL,
  `Dia_E_Stone_Ct` DECIMAL(10,3) DEFAULT NULL,
  `Dia_E_Stone_Amt` DECIMAL(10,2) DEFAULT NULL,
  `Dia_E_Description` VARCHAR(255)   DEFAULT NULL,
  `Dia_E_Drop_Down` VARCHAR(100)   DEFAULT NULL,
  `Dia_E_Other_Charges` DECIMAL(10,2) DEFAULT NULL,
  `Dia_E_Value` DECIMAL(10,2) DEFAULT NULL,
  `Dia_E_Final_Amount` DECIMAL(10,2) DEFAULT NULL,
  `Dia_E_Gross_Amount` DECIMAL(10,2) DEFAULT NULL,
  `Dia_E_Remark` VARCHAR(255)   DEFAULT NULL,
  `Dia_E_SrNo` VARCHAR(255)   DEFAULT NULL,
  `Dia_E_Barcode` VARCHAR(255)   DEFAULT NULL,
  `Dia_E_Item_id` INT(11) DEFAULT NULL,
  `Dia_E_Comment` VARCHAR(255)   DEFAULT NULL,
  `local_id` BIGINT(20) NOT NULL,
  `IsDataPostOnServer` TINYINT(1) DEFAULT NULL,

  PRIMARY KEY (`Dia_E_Firm_id`, `local_id`),     -- ✅ Composite primary key
  UNIQUE KEY `UNQ_Dia_E_D_id` (`Dia_E_D_id`)      -- ✅ Dia_E_D_id still unique with AUTO_INCREMENT
);



CREATE TABLE `GoldandSilver_tagging_entry` (
  `GoldSilver_id` BIGINT AUTO_INCREMENT,
  `Firm_id` VARCHAR(255)   NOT NULL,
  `User_name` VARCHAR(255) DEFAULT NULL,
  `Stock_Type` VARCHAR(50) DEFAULT NULL,
  `Particular` VARCHAR(255) DEFAULT NULL,
  `Party_Name` VARCHAR(150) DEFAULT NULL,
  `Invoice_No` VARCHAR(100) DEFAULT NULL,
  `Entry_Date` DATE DEFAULT NULL,
  `Image` VARCHAR(255)   DEFAULT NULL,
  `Category` VARCHAR(255)   DEFAULT NULL,
  `SubCategory` VARCHAR(255)   DEFAULT NULL,
  `Design` VARCHAR(255)   DEFAULT NULL,
  `Purity` VARCHAR(50)   DEFAULT NULL,
  `HSN_No` VARCHAR(50)   DEFAULT NULL,
  `Size` VARCHAR(50)   DEFAULT NULL,
  `pcs` INT(11) DEFAULT NULL,
  `Gross_wt` DECIMAL(10,3) DEFAULT NULL,
  `Less_wt` DECIMAL(10,3) DEFAULT NULL,
  `Net_wt` DECIMAL(10,3) DEFAULT NULL,
  `Net_Rate` DECIMAL(10,2) DEFAULT NULL,
  `Net_Amount` DECIMAL(10,2) DEFAULT NULL,
  `Other_Charges` DECIMAL(10,2) DEFAULT NULL,
  `Final_Amount` DECIMAL(10,2) DEFAULT NULL,
  `Drop_Down` VARCHAR(100)   DEFAULT NULL,
  `Remark` VARCHAR(255)   DEFAULT NULL,
  `SrNo` VARCHAR(255)   DEFAULT NULL,
  `Barcode` VARCHAR(255)   DEFAULT NULL,
  `Item_id` INT(11) DEFAULT NULL,
  `Comment` VARCHAR(255)   DEFAULT NULL,
  `local_id` BIGINT(20) NOT NULL,
  `IsDataPostOnServer` TINYINT(1) DEFAULT NULL,
  PRIMARY KEY (`Firm_id`, `local_id`),              -- ✅ Composite PK
  UNIQUE KEY `UNQ_GoldSilver_id` (`GoldSilver_id`)  -- ✅ Maintain AUTO_INCREMENT
);


CREATE TABLE `Hsn_list` (
  `hsn_id` BIGINT AUTO_INCREMENT,
  `firmid` VARCHAR(50)   NOT NULL,
  `local_id` BIGINT(20) NOT NULL,
  `metal` VARCHAR(100)   DEFAULT NULL,
  `hsn_code` VARCHAR(50)   DEFAULT NULL,
  PRIMARY KEY (`firmid`, `local_id`),     -- Composite Primary Key
  UNIQUE KEY (`hsn_id`)                   -- Keep hsn_id unique
);


CREATE TABLE `Item_info` (
  `item_id` BIGINT AUTO_INCREMENT,
  `metal_type` VARCHAR(100)   DEFAULT NULL,
  `category` VARCHAR(100)   DEFAULT NULL,
  `sub_category` VARCHAR(100)   DEFAULT NULL,
  `design` VARCHAR(100)   DEFAULT NULL,
  `hsn_id` INT(11) DEFAULT NULL,
  `hsn_code` VARCHAR(50)   DEFAULT NULL,
  `short_name` VARCHAR(10)   NOT NULL,
  `firmid` VARCHAR(10)   NOT NULL,
  `local_id` BIGINT(20) NOT NULL,
  `IsDataPostOnServer` TINYINT(1) DEFAULT NULL,
  PRIMARY KEY (`firmid`, `local_id`),             -- ✅ Composite primary key
  UNIQUE KEY (`item_id`),                         -- ✅ Unique key for tracking
  CONSTRAINT `fk_iteminfo_hsnid` 
    FOREIGN KEY (`hsn_id`) REFERENCES `Hsn_list`(`hsn_id`) 
    ON DELETE SET NULL ON UPDATE CASCADE
);


CREATE TABLE `Party` (
    `Id` BIGINT AUTO_INCREMENT PRIMARY KEY,
    `LocalId` BIGINT NOT NULL,
    `Name` VARCHAR(100) NOT NULL,
    `MobileNumber` VARCHAR(15),
    `Gender` ENUM('Male', 'Female', 'Other'),
    `Email` VARCHAR(100),
    `GSTNumber` VARCHAR(20),
    `PANNumber` VARCHAR(10),
    `Address` TEXT,
    `State` VARCHAR(100),
    `StateCode` VARCHAR(10),
    `District` VARCHAR(100),
    `City` VARCHAR(100),
    `Village` VARCHAR(100),
    `PinCode` VARCHAR(10),
    `AccNo` VARCHAR(20),
    `AccountType` VARCHAR(50),
    `Ifsc` VARCHAR(15),
    `BankName` VARCHAR(100),
    `Branch` VARCHAR(100),
    `Narration` TEXT,
    `CreatedAt` DATETIME DEFAULT CURRENT_TIMESTAMP,
    `UpdatedAt` DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    `IsActive` BOOLEAN DEFAULT TRUE,
    `IsDataPostOnServer` BOOLEAN DEFAULT FALSE,
);


CREATE TABLE `PurchaseItems` (
  `Id` BIGINT AUTO_INCREMENT,
  `local_id` BIGINT(20) NOT NULL,
  `PurchaseId` INT(11) DEFAULT NULL,
  `firm_id` VARCHAR(20) NOT NULL,
  `Admin_user` VARCHAR(20) DEFAULT NULL,
  `Metal` VARCHAR(50) DEFAULT NULL,
  `HSNCode` VARCHAR(50) DEFAULT NULL,
  `HuidNo` VARCHAR(50) DEFAULT NULL,
  `ItemName` VARCHAR(100) DEFAULT NULL,
  `Purity` VARCHAR(20) DEFAULT NULL,
  `Pcs` INT(11) DEFAULT NULL,
  `GrossWt` DECIMAL(18,3) DEFAULT NULL,
  `Diact` DECIMAL(18,3) DEFAULT NULL,
  `StoneWt` DECIMAL(18,3) DEFAULT NULL,
  `LessWt` DECIMAL(18,3) DEFAULT NULL,
  `NetWt` DECIMAL(18,3) DEFAULT NULL,
  `Rate` DECIMAL(18,2) DEFAULT NULL,
  `amount` DECIMAL(18,2) DEFAULT NULL,
  `stoneCharge` DECIMAL(18,2) DEFAULT NULL,
  `DiaCharge` DECIMAL(18,2) DEFAULT 0.00,
  `Tax_Type` VARCHAR(30) DEFAULT NULL,
  `Tax_Amount` VARCHAR(50) DEFAULT NULL,
  `NetPrice` DECIMAL(18,2) DEFAULT NULL,
  `updated_by` VARCHAR(30) DEFAULT NULL,
  `updated_time` DATE DEFAULT NULL,
  `IsDataPostOnServer` TINYINT(1) DEFAULT NULL,
  PRIMARY KEY (`firm_id`, `local_id`),     -- ✅ Composite primary key
  UNIQUE KEY (`Id`)                         -- ✅ Still keeps `Id` as unique identifier
);


CREATE TABLE `Purchases` (
  `Id` BIGINT AUTO_INCREMENT,
  `firm_id` VARCHAR(20) NOT NULL,
  `local_id` BIGINT(20) NOT NULL,
  `Admin_user` VARCHAR(20) DEFAULT NULL,
  `BillNo` VARCHAR(50) NOT NULL,
  `PurchaseDate` DATETIME NOT NULL,
  `SupplierId` INT(11) NOT NULL,
  `Remarks` VARCHAR(500)   DEFAULT NULL,
  `TotalAmount` DECIMAL(18,2) NOT NULL,
  `Tax` DECIMAL(18,2) NOT NULL,
  `Discount` DECIMAL(18,2) NOT NULL,
  `NetAmount` DECIMAL(18,2) NOT NULL,
  `CreatedAt` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_by` VARCHAR(30)   DEFAULT NULL,
  `updated_time` TIMESTAMP NULL DEFAULT NULL,
  `local_id` BIGINT(20) DEFAULT NULL,
  `IsDataPostOnServer` TINYINT(1) DEFAULT NULL,
  PRIMARY KEY (`firm_id`, `local_id`),
  UNIQUE KEY (`Id`),  -- Still keeps auto-incremented Id unique
  FOREIGN KEY (`SupplierId`) REFERENCES `Party`(`id`) 
    ON DELETE RESTRICT 
    ON UPDATE CASCADE
);


CREATE TABLE `Stock_tagging` (
  `stock_id` BIGINT AUTO_INCREMENT,
  `stock_type` VARCHAR(100),
  `metal_type` VARCHAR(100),
  `entry_date` DATE,
  `party_name` VARCHAR(150),
  `invoice_number` VARCHAR(50),
  `purity` VARCHAR(100),
  `total_weight` DECIMAL(10,3),
  `total_carat` DECIMAL(10,3),
  `completed_weight` DECIMAL(10,3),
  `completed_carat` DECIMAL(10,3),
  `pending_weight` DECIMAL(10,3),
  `pending_carat` DECIMAL(10,3),
  `firmid` VARCHAR(10) NOT NULL,
  `local_id` BIGINT(20) NOT NULL,
  `IsDataPostOnServer` TINYINT(1),
  `PurchaseItemId` INT(11),
  UNIQUE (`stock_id`), -- stock_id ko UNIQUE banaya gaya hai
  PRIMARY KEY (`firmid`, `local_id`), 
);


CREATE TABLE `Users` (
  `Id` BIGINT AUTO_INCREMENT,         -- Unique Auto-increment ID
  `firm_id` VARCHAR(50)   NOT NULL,
  `local_id` BIGINT(20) NOT NULL,
  `user_name` VARCHAR(100)   DEFAULT NULL,
  `email` VARCHAR(100)   DEFAULT NULL,
  `phone_number` VARCHAR(20)   DEFAULT NULL,
  `password` VARCHAR(255)   DEFAULT NULL,
  `user_type` VARCHAR(50)   DEFAULT NULL,
  `profile_Image` LONGBLOB DEFAULT NULL,
  `created_by` VARCHAR(100)   DEFAULT NULL,
  `years_of_exp` TEXT   DEFAULT NULL,
  `address` TEXT   DEFAULT NULL,
  `gender` VARCHAR(10)   DEFAULT NULL,
  `aadhar_front` LONGBLOB DEFAULT NULL,
  `aadhar_back` LONGBLOB DEFAULT NULL,
  `pancard` LONGBLOB DEFAULT NULL,
  `resume` LONGBLOB DEFAULT NULL,
  `certificate` LONGBLOB DEFAULT NULL,
  `others` LONGBLOB DEFAULT NULL,
  `salary` DECIMAL(10,2) DEFAULT NULL,
  `assigned_branch` VARCHAR(100)   DEFAULT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `IsDataPostOnServer` TINYINT(1) DEFAULT NULL,
  PRIMARY KEY (`firm_id`, `local_id`),              -- Composite Primary Key
  UNIQUE KEY (`Id`),                                -- Unique ID tracking
  CONSTRAINT `fk_users_firm_id`
    FOREIGN KEY (`firm_id`) REFERENCES `company_details`(`Id`)
);

