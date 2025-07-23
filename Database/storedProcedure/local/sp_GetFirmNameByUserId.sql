

---for getting FirmId--------------------
DELIMITER $$

CREATE PROCEDURE `sp_GetFirmNameByUserId` (
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
END $$

DELIMITER ;







-------login procedure -------------------------------
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
END









--party isdatpostonserver------------------------------------------------

CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_MarkPartyAsSynced`(
    IN p_LocalId BIGINT
)
BEGIN
    UPDATE Party
    SET IsDataPostOnServer = 1,
        UpdatedAt = NOW()
    WHERE Id = p_LocalId;
END










--create addparty--------------------------------------------------------

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
END