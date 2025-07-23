SELECT * FROM billing_testing.users_admin;
use billing_testing;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_AddParty_local`(
    IN Name VARCHAR(100),
    IN MobileNumber VARCHAR(15),
    IN Gender ENUM('Male', 'Female', 'Other'),
    IN MailId VARCHAR(100),
    IN GSTNumber VARCHAR(20),
    IN PANNumber VARCHAR(10),
    IN Address TEXT,
    IN State VARCHAR(100),
    IN StateCode VARCHAR(10),
    IN District VARCHAR(100),
    IN City VARCHAR(100),
    IN Village VARCHAR(100),
    IN PinCode VARCHAR(10),
    IN AccountNumber VARCHAR(20),
    IN AccountType VARCHAR(50),
    IN Ifsc VARCHAR(15),
    IN BankName VARCHAR(100),
    IN BankBranch VARCHAR(100),
    IN Narration TEXT,
    IN CreatedByUserId BIGINT,

    OUT NewPartyId BIGINT,     -- ← local Id
    OUT OutFirmId VARCHAR(50)  -- ← also return FirmId
)
BEGIN
    DECLARE v_firm_id VARCHAR(50) DEFAULT NULL;
    DECLARE v_user_name VARCHAR(100) DEFAULT NULL;

    -- Convert empty strings to NULL
    SET Name = NULLIF(Name, '');
    SET MobileNumber = NULLIF(MobileNumber, '');
    SET Gender = NULLIF(Gender, '');
    SET MailId = NULLIF(MailId, '');
    SET GSTNumber = NULLIF(GSTNumber, '');
    SET PANNumber = NULLIF(PANNumber, '');
    SET Address = NULLIF(Address, '');
    SET State = NULLIF(State, '');
    SET StateCode = NULLIF(StateCode, '');
    SET District = NULLIF(District, '');
    SET City = NULLIF(City, '');
    SET Village = NULLIF(Village, '');
    SET PinCode = NULLIF(PinCode, '');
    SET AccountNumber = NULLIF(AccountNumber, '');
    SET AccountType = NULLIF(AccountType, '');
    SET Ifsc = NULLIF(Ifsc, '');
    SET BankName = NULLIF(BankName, '');
    SET BankBranch = NULLIF(BankBranch, '');
    SET Narration = NULLIF(Narration, '');

    -- Required field checks
    IF Name IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Party name is required';
    END IF;

    IF GSTNumber IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'GST number is required';
    END IF;

    -- Fetch user info from users table
    SELECT firm_id, user_name
    INTO v_firm_id, v_user_name
    FROM users
    WHERE Id = CreatedByUserId
    LIMIT 1;

    IF v_firm_id IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'User not found';
    END IF;

    -- Insert into Party table
    INSERT INTO Party (
        FirmId, Name, MobileNumber, Gender, Email, GSTNumber, PANNumber,
        Address, State, StateCode, District, City, Village, PinCode,
        AccNo, AccountType, Ifsc, BankName, Branch, Narration,
        CreatedAt, UpdatedAt, IsActive, IsDataPostOnServer
    ) VALUES (
        v_firm_id, Name, MobileNumber, Gender, MailId, GSTNumber, PANNumber,
        Address, State, StateCode, District, City, Village, PinCode,
        AccountNumber, AccountType, Ifsc, BankName, BankBranch, Narration,
        NOW(), NULL, 1, 0
    );

    -- Return the auto-generated local ID and firm ID
    SET NewPartyId = LAST_INSERT_ID();
    SET OutFirmId = v_firm_id;
END$$
DELIMITER ;



DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_GetAllParties`()
BEGIN
    SELECT
        Id, Name, MobileNumber, Gender, Email, GSTNumber, PANNumber,
        Address, State, StateCode, District, City, Village, PinCode,
        AccNo, AccountType, Ifsc, BankName, Branch, Narration,
        CreatedAt, UpdatedAt, IsActive
    FROM Party;
END$$
DELIMITER ;


DELIMITER $$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_GetFirmNameByUserId`(
    IN p_UserId INT
)
BEGIN
    DECLARE v_FirmId VARCHAR(50);

    -- Get the Firm ID from the users table
    SELECT firm_id INTO v_FirmId
    FROM users
    WHERE Id = p_UserId;

    -- Return the ShopName and FirmID from company_details
    SELECT
        ShopName,
        FirmID
    FROM company_details
    WHERE FirmID = v_FirmId;
END$$
DELIMITER ;



DELIMITER $$

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
    OUT inserted_id BIGINT
)
BEGIN
    INSERT INTO Company_details (
        user_id, FirmID, RegistrationNo, ShopName, FirmDescription, Address, State, District, City, Pincode,
        PhoneNumber, Email, WebsiteName, Comments, WhatsAppLink, FacebookLink, InstagramLink,
        EInvoiceApiId, EInvoiceApiKey, EInvoiceUsername, EInvoicePassword,
        PaymentBankDetails, AccountHolderName, PaymentBankACNo, PaymentBankIFSCCode, PaymentDeclaration,
        FinancialYearStartDate, CashBalance, GSTIN, PANNumber, PrincipalAmtStart, PrincipalAmtEnd,
        FormHeader, FormFooter, CompanyLogo, FirmLogo, OwnerSignature, QRcode, IsDataPostOnServer
    )
    VALUES (
        user_id, FirmID, RegistrationNo, ShopName, FirmDescription, Address, State, District, City, Pincode,
        PhoneNumber, Email, WebsiteName, Comments, WhatsAppLink, FacebookLink, InstagramLink,
        EInvoiceApiId, EInvoiceApiKey, EInvoiceUsername, EInvoicePassword,
        PaymentBankDetails, AccountHolderName, PaymentBankACNo, PaymentBankIFSCCode, PaymentDeclaration,
        FinancialYearStartDate, CashBalance, GSTIN, PANNumber, PrincipalAmtStart, PrincipalAmtEnd,
        FormHeader, FormFooter, LogoPath, LeftImagePath, SignaturePath, QrCodePath, IsDataPostOnServer
    );

    SET inserted_id = LAST_INSERT_ID();
END$$

DELIMITER ;





DELIMITER $$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_InsertStockTaggingFromPurchase_Local`(
    IN purchaseId BIGINT
)
BEGIN
    INSERT INTO stock_tagging (
        stock_type,
        metal_type,
        entry_date,
        party_name,
        invoice_number,
        purity,
        total_weight,
        total_carat,
        completed_weight,
        completed_carat,
        pending_weight,
        pending_carat,
        firmid,
        IsDataPostOnServer,
        PurchaseItemId
    )
    SELECT
        'Purchase',
        pi.Metal,
        NOW(),
        p.Name,
        pur.BillNo,
        pi.Purity,
        pi.NetWt,
        pi.Diact,
        0.000,
        0.000,
        pi.NetWt,
        pi.Diact,
        pi.firm_id,
        0,
        pi.Id
    FROM PurchaseItems pi
    JOIN Purchases pur ON pur.Id = pi.PurchaseId
    JOIN Party p ON p.id = pur.SupplierId
    WHERE pi.PurchaseId = purchaseId;
END$$

DELIMITER ;
DELIMITER $$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_InsertPurchaseItem`(
    IN PurchaseId BIGINT,
    IN firm_id VARCHAR(20),
    IN Admin_user VARCHAR(20),
    IN Metal VARCHAR(50),
    IN HSNCode VARCHAR(50),
    IN HuidNo VARCHAR(50),
    IN ItemName VARCHAR(100),
    IN Purity VARCHAR(20),
    IN Pcs INT,
    IN GrossWt DECIMAL(18,3),
    IN Diact DECIMAL(18,3),
    IN StoneWt DECIMAL(18,3),
    IN LessWt DECIMAL(18,3),
    IN NetWt DECIMAL(18,3),
    IN Rate DECIMAL(18,2),
    IN amount DECIMAL(18,2),
    IN stoneCharge DECIMAL(18,2),
    IN DiaCharge DECIMAL(18,2),
    IN Tax_Type VARCHAR(30),
    IN Tax_Amount DECIMAL(18,2),
    IN NetPrice DECIMAL(18,2),
    IN updated_by VARCHAR(30),
    IN updated_time DATE,
    OUT Id BIGINT
)
BEGIN
    INSERT INTO PurchaseItems (
        PurchaseId, firm_id, Admin_user, Metal, HSNCode, HuidNo,
        ItemName, Purity, Pcs, GrossWt, Diact, StoneWt, LessWt, NetWt,
        Rate, amount, stoneCharge, DiaCharge, Tax_Type, Tax_Amount,
        NetPrice, updated_by, updated_time, IsDataPostOnServer
    )
    VALUES (
        PurchaseId, firm_id, Admin_user, Metal, HSNCode, HuidNo,
        ItemName, Purity, Pcs, GrossWt, Diact, StoneWt, LessWt, NetWt,
        Rate, amount, stoneCharge, DiaCharge, Tax_Type, Tax_Amount,
        NetPrice, updated_by, updated_time, 0
    );

    SET Id = LAST_INSERT_ID();
END$$

DELIMITER ;



DELIMITER $$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_InsertPurchaseItem`(
    IN PurchaseId BIGINT,
    IN firm_id VARCHAR(20),
    IN Admin_user VARCHAR(20),
    IN Metal VARCHAR(50),
    IN HSNCode VARCHAR(50),
    IN HuidNo VARCHAR(50),
    IN ItemName VARCHAR(100),
    IN Purity VARCHAR(20),
    IN Pcs INT,
    IN GrossWt DECIMAL(18,3),
    IN Diact DECIMAL(18,3),
    IN StoneWt DECIMAL(18,3),
    IN LessWt DECIMAL(18,3),
    IN NetWt DECIMAL(18,3),
    IN Rate DECIMAL(18,2),
    IN amount DECIMAL(18,2),
    IN stoneCharge DECIMAL(18,2),
    IN DiaCharge DECIMAL(18,2),
    IN Tax_Type VARCHAR(30),
    IN Tax_Amount VARCHAR(50),
    IN NetPrice DECIMAL(18,2),
    IN updated_by VARCHAR(30),
    IN updated_time DATE,
    OUT Id BIGINT
)
BEGIN
    INSERT INTO PurchaseItems (
        PurchaseId, firm_id, Admin_user, Metal, HSNCode, HuidNo,
        ItemName, Purity, Pcs, GrossWt, Diact, StoneWt, LessWt, NetWt,
        Rate, amount, stoneCharge, DiaCharge, Tax_Type, Tax_Amount,
        NetPrice, updated_by, updated_time, IsDataPostOnServer
    )
    VALUES (
        PurchaseId, firm_id, Admin_user, Metal, HSNCode, HuidNo,
        ItemName, Purity, Pcs, GrossWt, Diact, StoneWt, LessWt, NetWt,
        Rate, amount, stoneCharge, DiaCharge, Tax_Type, Tax_Amount,
        NetPrice, updated_by, updated_time, 0
    );

    SET Id = LAST_INSERT_ID();
END$$
DELIMITER ;


DELIMITER $$


CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_InsertPurchase`(
    IN firm_id VARCHAR(20),
    IN Admin_user VARCHAR(20),
    IN BillNo VARCHAR(50),
    IN PurchaseDate DATETIME,
    IN SupplierId INT,
    IN Remarks VARCHAR(500),
    IN TotalAmount DECIMAL(18,2),
    IN Tax DECIMAL(18,2),
    IN Discount DECIMAL(18,2),
    IN NetAmount DECIMAL(18,2),
    IN updated_by VARCHAR(30),
    IN updated_time DATETIME,
    OUT PurchaseId BIGINT
)
BEGIN
    INSERT INTO Purchases (
        firm_id, Admin_user, BillNo, PurchaseDate, SupplierId,
        Remarks, TotalAmount, Tax, Discount, NetAmount,
        CreatedAt, updated_by, updated_time, IsDataPostOnServer
    )
    VALUES (
        firm_id, Admin_user, BillNo, PurchaseDate, SupplierId,
        Remarks, TotalAmount, Tax, Discount, NetAmount,
        NOW(), updated_by, updated_time, 0
    );

    SET PurchaseId = LAST_INSERT_ID();
END
$$
DELIMITER ;


DELIMITER $$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_InsertUser_local`(
    IN user_name VARCHAR(50),
    IN email VARCHAR(50),
    IN phone_number VARCHAR(20),
    IN password VARCHAR(50),
    IN user_type VARCHAR(50),
    IN profile_image TEXT,
    IN created_by varchar(50),
    IN years_of_experience INT,  
    IN address TEXT,
    IN gender VARCHAR(10),
    IN aadhar_front TEXT,
    IN aadhar_back TEXT,
    IN pancard TEXT,
    IN resume TEXT,
    IN certificate TEXT,
    IN others TEXT,
    IN salary DECIMAL(10,2),
    IN assigned_branch VARCHAR(50),
    IN firm_id VARCHAR(50),
    OUT inserted_id BIGINT
)
BEGIN
    INSERT INTO users (
        user_name, email, phone_number, password, user_type, profile_image,
        created_by, years_of_exp, address, gender,
        aadhar_front, aadhar_back, pancard, resume, certificate, others,
        salary, assigned_branch, firm_id
    ) VALUES (
        user_name, email, phone_number, password, user_type, profile_image,
        created_by, years_of_experience, address, gender,  
        aadhar_front, aadhar_back, pancard, resume, certificate, others,
        salary, assigned_branch, firm_id
    );

    SET inserted_id = LAST_INSERT_ID();
END$$
DELIMITER ;



DELIMITER $$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_login_user_or_admin`(
    IN p_username VARCHAR(100),
    IN p_password VARCHAR(255)
)
BEGIN
    DECLARE v_id INT;

    -- First, check in users_admin
    SELECT
        id INTO v_id
    FROM users_admin
    WHERE (email = p_username OR phone_number = p_username OR user_name = p_username)
      AND password = p_password
    LIMIT 1;

    IF v_id IS NOT NULL THEN
        -- If match found in users_admin, return full data from there
        SELECT
            id, user_name, email, phone_number, user_type,
            NULL AS firm_id, 'admin' AS source
        FROM users_admin
        WHERE id = v_id;
    ELSE
        -- If no match found in users_admin, check in users table
        SELECT
            id, user_name, email, phone_number, user_type,
            firm_id, 'user' AS source
        FROM users
        WHERE (email = p_username OR phone_number = p_username OR user_name = p_username)
          AND password = p_password
        LIMIT 1;
    END IF;
END$$
DELIMITER ;


DELIMITER $$


CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_MarkPartyAsSynced`(
    IN p_LocalId BIGINT
)
BEGIN
    UPDATE Party
    SET IsDataPostOnServer = 1,
        UpdatedAt = NOW()
    WHERE Id = p_LocalId;
END$$
DELIMITER ;


DELIMITER $$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_MarkPurchaseAndItemsPosted`(
    IN purchaseId BIGINT
)
BEGIN
    UPDATE Purchases
    SET IsDataPostOnServer = 1
    WHERE Id = purchaseId;

    UPDATE PurchaseItems
    SET IsDataPostOnServer = 1
    WHERE PurchaseId = purchaseId;
END$$

DELIMITER ;


DELIMITER $$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_UpdateUserPostStatus`(
    IN id BIGINT
)
BEGIN
    UPDATE Users
    SET IsDataPostOnServer = 1
    WHERE Id = id;
END$$

DELIMITER ;



DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_GetBranchSalesAndProfit`(IN branch_id INT)
BEGIN
    DECLARE firm_id VARCHAR(50);
    
    -- Get firm_id based on branch
    SELECT firmid INTO firm_id FROM company_details WHERE id = branch_id;

    -- Return only purchase and profit (for now)
    SELECT 
        b.id AS branch_id,
        b.shopName AS branch_name,
        
        -- COALESCE((
        --     SELECT SUM(s.total_amount)
        --     FROM sales s
        --     WHERE s.branch_id = branch_id
        -- ), 0) AS total_sales,

        COALESCE((
            SELECT SUM(p.NetAmount)
            FROM purchases p
            WHERE p.firm_id = firm_id
        ), 0) AS total_purchases,

        -- Profit based on purchase only for now (sales is commented)
        COALESCE((
            SELECT SUM(p.NetAmount)
            FROM purchases p
            WHERE p.firm_id = firm_id
        ), 0) AS total_profit

    FROM company_details b
    WHERE b.id = branch_id;
END$$

DELIMITER ;





CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_GetAllUsers`()
BEGIN
    SELECT * FROM users;
END











  DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_InsertIntoGoldAndSilverTaggingEntry_local`(
    IN Firm_id VARCHAR(255),
    IN User_name VARCHAR(255),
    IN Metal_type VARCHAR(255),
    IN Stock_Type VARCHAR(50),
    IN Particular VARCHAR(255),
    IN Party_Name VARCHAR(150),
    IN Invoice_No VARCHAR(100),
    IN Entry_Date DATE,
    IN Image VARCHAR(255),
    IN Category VARCHAR(255),
    IN SubCategory VARCHAR(255),
    IN Design VARCHAR(255),
    IN Purity VARCHAR(50),
    IN HSN_No VARCHAR(50),
    IN SizeVal VARCHAR(50),
    IN pcs INT,
    IN Gross_wt DECIMAL(10,3),
    IN Less_wt DECIMAL(10,3),
    IN Net_wt DECIMAL(10,3),
    IN Net_Rate DECIMAL(10,2),
    IN Net_Amount DECIMAL(10,2),
    IN Other_Charges DECIMAL(10,2),
    IN Final_Amount DECIMAL(10,2),
    IN Drop_Down VARCHAR(100),
    IN Remark VARCHAR(255),
    IN Item_id INT(11),
    IN CommentVal VARCHAR(255),
    IN IsDataPostOnServer TINYINT(1)
)
BEGIN
    DECLARE v_short_name VARCHAR(255);
    DECLARE maxTagId INT;
    DECLARE CompanyId INT;
    DECLARE newBarcode VARCHAR(255);

    -- Get Company ID
    SELECT id INTO CompanyId
    FROM company_details 
    WHERE FirmId = Firm_id
    LIMIT 1;

    -- First attempt: Exact match
    SELECT short_name INTO v_short_name
    FROM item_info 
    WHERE 
        TRIM(category) = TRIM(Category)
        AND TRIM(sub_category) = TRIM(SubCategory)
        AND TRIM(design) = TRIM(Design)
        AND TRIM(metal_type) = TRIM(Metal_type)
    LIMIT 1;

    -- Fallback: Flexible match
    IF v_short_name IS NULL OR v_short_name = '' THEN
        SELECT short_name INTO v_short_name
        FROM item_info 
        WHERE 
            TRIM(category) = TRIM(Category)
            AND TRIM(sub_category) = TRIM(SubCategory)
            AND TRIM(design) = TRIM(Design)
            AND TRIM(metal_type) LIKE CONCAT('%', TRIM(Metal_type), '%')
        LIMIT 1;
    END IF;

    -- Tag number generation logic
    IF EXISTS (SELECT 1 FROM GenrateTagNumber WHERE Short_name = v_short_name AND FirmId = Firm_id) THEN
        SELECT MaxId + 1 INTO maxTagId 
        FROM GenrateTagNumber 
        WHERE Short_name = v_short_name AND FirmId = Firm_id;

        UPDATE GenrateTagNumber 
        SET MaxId = maxTagId 
        WHERE Short_name = v_short_name AND FirmId = Firm_id;
    ELSE
        SET maxTagId = 1;
        INSERT INTO GenrateTagNumber(Short_name, FirmId, MaxId)
        VALUES(v_short_name, Firm_id, maxTagId);
    END IF;

    -- Generate Barcode
    SET newBarcode = CONCAT(v_short_name, maxTagId, '-', CompanyId);

    -- Final Insert
    INSERT INTO GoldandSilver_tagging_entry (
        Firm_id,
        User_name,
        Stock_Type,
        Particular,
        Party_Name,
        Invoice_No,
        Entry_Date,
        Image,
        Category,
        SubCategory,
        Design,
        Purity,
        HSN_No,
        Size,
        pcs,
        Gross_wt,
        Less_wt,
        Net_wt,
        Net_Rate,
        Net_Amount,
        Other_Charges,
        Final_Amount,
        Drop_Down,
        Remark,
        SrNo,
        Barcode,
        Item_id,
        Comment,
        IsDataPostOnServer
    ) VALUES (
        Firm_id,
        User_name,
        Stock_Type,
        Particular,
        Party_Name,
        Invoice_No,
        Entry_Date,
        Image,
        Category,
        SubCategory,
        Design,
        Purity,
        HSN_No,
        SizeVal,
        pcs,
        Gross_wt,
        Less_wt,
        Net_wt,
        Net_Rate,
        Net_Amount,
        Other_Charges,
        Final_Amount,
        Drop_Down,
        Remark,
        SrNo,
        newBarcode,
        Item_id,
        CommentVal,
        IsDataPostOnServer
    );

    SELECT LAST_INSERT_ID() AS GoldSilver_id;
END //
DELIMITER ;


DELIMITER //

CREATE PROCEDURE sp_GetPendingWeightByFilters(
    IN metal_type VARCHAR(100),
    IN party_name VARCHAR(150),
    IN invoice_number VARCHAR(50),
    IN purity VARCHAR(100)
)
BEGIN
    SELECT *
    FROM billing_testing.stock_tagging
    WHERE
        (metal_type IS NULL OR metal_type = metal_type)
        AND (party_name IS NULL OR party_name = party_name)
        AND (invoice_number IS NULL OR invoice_number = invoice_number)
        AND (purity IS NULL OR purity = purity);
END //

DELIMITER ;