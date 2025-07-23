use billing_testing;

CREATE TABLE IF NOT EXISTS Company_details (
  `Id` BIGINT AUTO_INCREMENT PRIMARY KEY,
  `user_id` INT DEFAULT NULL,
  `FirmID` VARCHAR(50)   NOT NULL,
  `RegistrationNo` VARCHAR(100)   DEFAULT NULL,
  `ShopName` VARCHAR(150)   DEFAULT NULL,
  `FirmDescription` TEXT   DEFAULT NULL,
  `Address` TEXT   DEFAULT NULL,
  `State` VARCHAR(100)   DEFAULT NULL,
  `District` VARCHAR(100)   DEFAULT NULL,
  `City` VARCHAR(100)   DEFAULT NULL,
  `Pincode` VARCHAR(20)   DEFAULT NULL,
  `PhoneNumber` VARCHAR(20)   DEFAULT NULL,
  `Email` VARCHAR(100)   DEFAULT NULL,
  `WebsiteName` VARCHAR(100)   DEFAULT NULL,
  `FirmType` VARCHAR(50)   DEFAULT NULL,
  `Comments` TEXT   DEFAULT NULL,
  `WhatsAppLink` VARCHAR(255)   DEFAULT NULL,
  `FacebookLink` VARCHAR(255)   DEFAULT NULL,
  `InstagramLink` VARCHAR(255)   DEFAULT NULL,
  `EnvioceAlPid` VARCHAR(100)   DEFAULT NULL,
  `EnvioceApiKey` VARCHAR(100)   DEFAULT NULL,
  `EnvioceUsername` VARCHAR(100)   DEFAULT NULL,
  `EnviocePassword` VARCHAR(100)   DEFAULT NULL,
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
  UNIQUE KEY (`FirmID`)
);


CREATE TABLE IF NOT EXISTS `Diamond_tagging_entry` (
  `id` BIGINT AUTO_INCREMENT PRIMARY KEY,
  `Firm_id` VARCHAR(255) DEFAULT NULL,
  `user_name` VARCHAR(255) DEFAULT NULL,
  `Stock_Type` VARCHAR(50) DEFAULT NULL,
  `Particular` VARCHAR(255) DEFAULT NULL,
  `Party_Name` VARCHAR(150) DEFAULT NULL,
  `Invoice_No` VARCHAR(100) DEFAULT NULL,
  `Date` DATE DEFAULT NULL,
  `Image` VARCHAR(255) DEFAULT NULL,
  `Category` VARCHAR(255) DEFAULT NULL,
  `SubCategory` VARCHAR(255) DEFAULT NULL,
  `Design` VARCHAR(255) DEFAULT NULL,
  `Purity` VARCHAR(50) DEFAULT NULL,
  `HSN_No` VARCHAR(50) DEFAULT NULL,
  `HUID_No` VARCHAR(50) DEFAULT NULL,
  `Size` VARCHAR(50) DEFAULT NULL,
  `pcs` INT(11) DEFAULT NULL,
  `Gross_wt` DECIMAL(10,3) DEFAULT NULL,
  `Less_wt` INT(20) DEFAULT NULL,
  `Diamond_Ct` DECIMAL(10,3) DEFAULT NULL,
  `Diamond_wt_gm` DECIMAL(10,3) DEFAULT NULL,
  `Net_wt` DECIMAL(10,3) DEFAULT NULL,
  `Diamond_Rate` DECIMAL(10,2) DEFAULT NULL,
  `Diamond_Amt` DECIMAL(10,2) DEFAULT NULL,
  `Net_Rate` DECIMAL(10,2) DEFAULT NULL,
  `Net_Amt` DECIMAL(10,2) DEFAULT NULL,
  `Stone_Ct` DECIMAL(10,3) DEFAULT NULL,
  `Stone_Amt` DECIMAL(10,2) DEFAULT NULL,
  `Description` VARCHAR(255) DEFAULT NULL,
  `Drop_Down` VARCHAR(100) DEFAULT NULL,
  `Other_Charges` DECIMAL(10,2) DEFAULT NULL,
  `Value` DECIMAL(10,2) DEFAULT NULL,
  `Final_Amount` DECIMAL(10,2) DEFAULT NULL,
  `Gross_Amount` DECIMAL(10,2) DEFAULT NULL,
  `Remark` VARCHAR(255) DEFAULT NULL,
  `SrNo` VARCHAR(255) DEFAULT NULL,
  `Barcode` VARCHAR(255) DEFAULT NULL,
  `Item_id` INT(11) DEFAULT NULL,
  `Comment` VARCHAR(255) DEFAULT NULL,
  `IsDataPostOnServer` TINYINT(1) DEFAULT NULL
);

CREATE TABLE IF NOT EXISTS `GoldandSilver_tagging_entry` (
  `GoldSilver_id` BIGINT AUTO_INCREMENT PRIMARY KEY,
  `Firm_id` VARCHAR(255)   NOT NULL,
  `User_name` VARCHAR(255)   DEFAULT NULL,
  `Stock_Type` VARCHAR(50)   DEFAULT NULL,
  `Particular` VARCHAR(255)   DEFAULT NULL,
  `Party_Name` VARCHAR(150)   DEFAULT NULL,
  `Invoice_No` VARCHAR(100)   DEFAULT NULL,
  `Entry_Date` DATE DEFAULT NULL,
  `Image` VARCHAR(255)   DEFAULT NULL,
  `Category` VARCHAR(255)   DEFAULT NULL,
  `SubCategory` VARCHAR(255)   DEFAULT NULL,
  `Design` VARCHAR(255)   DEFAULT NULL,
  `Purity` VARCHAR(50)   DEFAULT NULL,
  `HSN_No` VARCHAR(50)   DEFAULT NULL,
  `Size` VARCHAR(50)   DEFAULT NULL,
  `pcs` INT DEFAULT NULL,
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
  `IsDataPostOnServer` TINYINT(1) DEFAULT NULL
);



CREATE TABLE  IF NOT EXISTS `Hsn_list` (
  `hsn_id` BIGINT AUTO_INCREMENT PRIMARY KEY,
  `firmid` VARCHAR(50)   NOT NULL,
  `metal` VARCHAR(100)   DEFAULT NULL,
  `hsn_code` VARCHAR(50)   DEFAULT NULL
  );


CREATE TABLE IF NOT EXISTS `Item_info` (
  `item_id` BIGINT AUTO_INCREMENT PRIMARY KEY,
  `metal_type` VARCHAR(100)   DEFAULT NULL,
  `category` VARCHAR(100)   DEFAULT NULL,
  `sub_category` VARCHAR(100)   DEFAULT NULL,
  `design` VARCHAR(100)   DEFAULT NULL,
  `hsn_id` BIGINT DEFAULT NULL,
  `hsn_code` VARCHAR(50)   DEFAULT NULL,
  `short_name` VARCHAR(10)   NOT NULL,
  `firmid` VARCHAR(10)   NOT NULL,
  `IsDataPostOnServer` TINYINT(1) DEFAULT NULL,
CONSTRAINT `fk_iteminfo_hsnid`
    FOREIGN KEY (`hsn_id`) REFERENCES `Hsn_list`(`hsn_id`)
    ON DELETE SET NULL ON UPDATE CASCADE
);

CREATE TABLE  IF NOT EXISTS`Party` (
    `Id` BIGINT AUTO_INCREMENT PRIMARY KEY,
    `FirmId` VARCHAR(50) NOT NULL,
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
    `IsDataPostOnServer` BOOLEAN DEFAULT FALSE
);



CREATE TABLE  IF NOT EXISTS `PurchaseItems` (
  `Id` BIGINT AUTO_INCREMENT PRIMARY KEY,
  `PurchaseId` INT  DEFAULT NULL,
  `firm_id` VARCHAR(20) NOT NULL,
  `Admin_user` VARCHAR(20) DEFAULT NULL,
  `Metal` VARCHAR(50) DEFAULT NULL,
  `HSNCode` VARCHAR(50) DEFAULT NULL,
  `HuidNo` VARCHAR(50) DEFAULT NULL,
  `ItemName` VARCHAR(100) DEFAULT NULL,
  `Purity` VARCHAR(20) DEFAULT NULL,
  `Pcs` INT  DEFAULT NULL,
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
  `IsDataPostOnServer` TINYINT(1) DEFAULT NULL
);


CREATE TABLE IF NOT EXISTS `Purchases` (
  `Id` BIGINT AUTO_INCREMENT PRIMARY KEY,
  `firm_id` VARCHAR(20) NOT NULL,
  `Admin_user` VARCHAR(20) DEFAULT NULL,
  `BillNo` VARCHAR(50) NOT NULL,
  `PurchaseDate` DATETIME NOT NULL,
  `SupplierId` bigint NOT NULL,
  `Remarks` VARCHAR(500)   DEFAULT NULL,
  `TotalAmount` DECIMAL(18,2) NOT NULL,
  `Tax` DECIMAL(18,2) NOT NULL,
  `Discount` DECIMAL(18,2) NOT NULL,
  `NetAmount` DECIMAL(18,2) NOT NULL,
  `CreatedAt` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_by` VARCHAR(30)   DEFAULT NULL,
  `updated_time` TIMESTAMP NULL DEFAULT NULL,
  `IsDataPostOnServer` TINYINT(1) DEFAULT NULL,
FOREIGN KEY (`SupplierId`) REFERENCES `Party`(`ID`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE
);



CREATE TABLE  IF NOT EXISTS `Stock_tagging` (
  `stock_id` BIGINT AUTO_INCREMENT PRIMARY KEY,
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
  `IsDataPostOnServer` TINYINT(1),
  `PurchaseItemId` INT(11)
);




CREATE TABLE `Users` (
  `Id` BIGINT AUTO_INCREMENT PRIMARY KEY ,      
  `firm_id` VARCHAR(50)   NOT NULL,
  `user_name` VARCHAR(100)   DEFAULT NULL,
  `email` VARCHAR(100)   DEFAULT NULL,
  `phone_number` VARCHAR(20)   DEFAULT NULL Unique,
  `password` VARCHAR(255)   DEFAULT NULL,
  `user_type` VARCHAR(50)   DEFAULT NULL,

  `profile_Image` LONGBLOB DEFAULT NULL,
  `created_by` VARCHAR(100) DEFAULT NULL,
  `years_of_exp` TEXT DEFAULT NULL,
  `address` TEXT DEFAULT NULL,
  `gender` VARCHAR(10) DEFAULT NULL,
  `aadhar_front` LONGBLOB DEFAULT NULL,
  `aadhar_back` LONGBLOB DEFAULT NULL,
  `pancard` LONGBLOB DEFAULT NULL,
  `resume` LONGBLOB DEFAULT NULL,
  `certificate` LONGBLOB DEFAULT NULL,
  `others` LONGBLOB DEFAULT NULL,
  `salary` DECIMAL(10,2) DEFAULT NULL,
  `assigned_branch` VARCHAR(100) DEFAULT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `IsDataPostOnServer` TINYINT(1) DEFAULT NULL,
  CONSTRAINT `fk_users_firm_id`
    FOREIGN KEY (`firm_id`) REFERENCES `company_details`(`FirmID`)
);


CREATE TABLE `Users_Admin` (
  `Id` BIGINT AUTO_INCREMENT PRIMARY KEY,    
  `user_name` VARCHAR(100) DEFAULT NULL,
  `email` VARCHAR(100) DEFAULT NULL,
  `phone_number` VARCHAR(20) DEFAULT NULL,
  `password` VARCHAR(255) DEFAULT NULL,
  `user_type` VARCHAR(50) DEFAULT NULL,
  `profile_Image` LONGBLOB DEFAULT NULL,
  `created_by` VARCHAR(100) DEFAULT NULL,
  `years_of_exp` TEXT DEFAULT NULL,
  `address` TEXT DEFAULT NULL,
  `gender` VARCHAR(10) DEFAULT NULL,
  `aadhar_front` LONGBLOB DEFAULT NULL,
  `aadhar_back` LONGBLOB DEFAULT NULL,
  `pancard` LONGBLOB DEFAULT NULL,
  `resume` LONGBLOB DEFAULT NULL,
  `certificate` LONGBLOB DEFAULT NULL,
  `others` LONGBLOB DEFAULT NULL,
  `salary` DECIMAL(10,2) DEFAULT NULL,
  `assigned_branch` VARCHAR(100) DEFAULT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `IsDataPostOnServer` TINYINT(1) DEFAULT NULL
);




 CREATE TABLE  GenrateTagNumber
 (
 id int   Auto_Increment Primary Key,
 Short_name varchar(10),
 Maxid int,
 FirmId varchar(100)
 );











 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_InsertCompanyDetailsAndReturnId_local`(
    IN user_id INT,
    IN FirmID VARCHAR(50),
    IN RegistrationNo VARCHAR(100),
    IN ShopName VARCHAR(150),
    IN FirmDescription TEXT,
    IN Address TEXT,
    IN State VARCHAR(100),
    IN District VARCHAR(100),
    IN City VARCHAR(100),
    IN Pincode VARCHAR(20),
    IN PhoneNumber VARCHAR(20),
    IN Email VARCHAR(100),
    IN WebsiteName VARCHAR(100),
    IN Comments TEXT,
    IN WhatsAppLink VARCHAR(255),
    IN FacebookLink VARCHAR(255),
    IN InstagramLink VARCHAR(255),
    IN EInvoiceApiId VARCHAR(100),
    IN EInvoiceApiKey VARCHAR(100),
    IN EInvoiceUsername VARCHAR(100),
    IN EInvoicePassword VARCHAR(100),
    IN PaymentBankDetails TEXT,
    IN AccountHolderName VARCHAR(255),
    IN PaymentBankACNo VARCHAR(50),
    IN PaymentBankIFSCCode VARCHAR(50),
    IN PaymentDeclaration TEXT,
    IN FinancialYearStartDate DATE,
    IN CashBalance DECIMAL(18,2),
    IN GSTIN VARCHAR(50),
    IN PANNumber VARCHAR(20),
    IN PrincipalAmtStart DECIMAL(18,2),
    IN PrincipalAmtEnd DECIMAL(18,2),
    IN FormHeader TEXT,
    IN FormFooter TEXT,
    IN LogoPath LONGBLOB,
    IN LeftImagePath LONGBLOB,
    IN SignaturePath LONGBLOB,
    IN QrCodePath LONGBLOB,
    IN IsDataPostOnServer TINYINT(1),
    IN server_id  INT
    
)
BEGIN
    INSERT INTO Company_details (
        user_id, FirmID, RegistrationNo, ShopName, FirmDescription, Address, State, District, City, Pincode, PhoneNumber, Email, WebsiteName,
        Comments, WhatsAppLink, FacebookLink, InstagramLink, EnvioceAlPid, EnvioceApiKey, EnvioceUsername, EnviocePassword,
        PaymentBankDetails, AccountHolderName, PaymentBankACNo, PaymentBankIFSCCode, PaymentDeclaration, FinancialYearStartDate,
        CashBalance, GSTIN, PANNumber, PrincipalPalnStart, PrincipalPalnEnd, FormHeader, FormFooter,
        CompanyLogo, FirmLogo, OwnerSignature, QRcode, IsDataPostOnServer,server_id
    )
    VALUES (
        user_id, FirmID, RegistrationNo, ShopName, FirmDescription, Address, State, District, City, Pincode, PhoneNumber, Email, WebsiteName,
        Comments, WhatsAppLink, FacebookLink, InstagramLink, EInvoiceApiId, EInvoiceApiKey, EInvoiceUsername, EInvoicePassword,
        PaymentBankDetails, AccountHolderName, PaymentBankACNo, PaymentBankIFSCCode, PaymentDeclaration, FinancialYearStartDate,
        CashBalance, GSTIN, PANNumber, PrincipalAmtStart, PrincipalAmtEnd, FormHeader, FormFooter,
        LogoPath, LeftImagePath, SignaturePath, QrCodePath, IsDataPostOnServer,server_id
    );

   
END