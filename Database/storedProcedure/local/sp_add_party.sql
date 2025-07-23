DELIMITER $$

CREATE PROCEDURE `sp_AddParty_local` (
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
    IN LocalId BIGINT,
    OUT NewPartyId BIGINT   -- <-- Yeh OUT parameter hai jo return karega insert hua ID
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
        LocalId, Name, MobileNumber, Gender, Email, GSTNumber, PANNumber,
        Address, State, StateCode, District, City, Village, PinCode,
        AccNo, AccountType, Ifsc, BankName, Branch, Narration,
        CreatedAt, UpdatedAt, IsActive, IsDataPostOnServer
    ) VALUES (
        LocalId, Name, MobileNumber, Gender, MailId, GSTNumber, PANNumber,
        Address, State, StateCode, District, City, Village, PinCode,
        AccountNumber, AccountType, Ifsc, BankName, BankBranch, Narration,
        NOW(), NULL, 1, 0
    );

    -- Return the newly inserted Id
    SET NewPartyId = LAST_INSERT_ID();
END$$

DELIMITER ;
