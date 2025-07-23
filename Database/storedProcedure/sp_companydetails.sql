DELIMITER $$

CREATE PROCEDURE `sp_InsertCompanyDetailsAndReturnId_server`(
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
    IN local_id BIGINT,
    OUT inserted_id BIGINT
)
BEGIN
    INSERT INTO Company_details (
        user_id, FirmID, RegistrationNo, ShopName, FirmDescription, Address, State, District, City, Pincode, PhoneNumber, Email, WebsiteName,
        Comments, WhatsAppLink, FacebookLink, InstagramLink, EnvioceAlPid, EnvioceApiKey, EnvioceUsername, EnviocePassword,
        PaymentBankDetails, AccountHolderName, PaymentBankACNo, PaymentBankIFSCCode, PaymentDeclaration, FinancialYearStartDate,
        CashBalance, GSTIN, PANNumber, PrincipalPalnStart, PrincipalPalnEnd, FormHeader, FormFooter,
        CompanyLogo, FirmLogo, OwnerSignature, QRcode, IsDataPostOnServer,local_id
    )
    VALUES (
        user_id, FirmID, RegistrationNo, ShopName, FirmDescription, Address, State, District, City, Pincode, PhoneNumber, Email, WebsiteName,
        Comments, WhatsAppLink, FacebookLink, InstagramLink, EInvoiceApiId, EInvoiceApiKey, EInvoiceUsername, EInvoicePassword,
        PaymentBankDetails, AccountHolderName, PaymentBankACNo, PaymentBankIFSCCode, PaymentDeclaration, FinancialYearStartDate,
        CashBalance, GSTIN, PANNumber, PrincipalAmtStart, PrincipalAmtEnd, FormHeader, FormFooter,
        LogoPath, LeftImagePath, SignaturePath, QrCodePath, IsDataPostOnServer,local_id
    );

    SET inserted_id = LAST_INSERT_ID();
END$$

DELIMITER ;