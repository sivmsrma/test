using Mysqlx.Crud;

--Server Bill Headers Table with Composite Primary Key
CREATE TABLE IF NOT EXISTS BillHeaders (
    Id INT AUTO_INCREMENT,
    LocalId INT NOT NULL,
    FirmId VARCHAR(20) NOT NULL,
    BillNo VARCHAR(50) NULL,
    BillDate DATETIME NOT NULL,
    PartyId INT NOT NULL,
    TotalAmount DECIMAL(18,2) NOT NULL,
    TaxAmount DECIMAL(18,2) NOT NULL,
    DiscountAmount DECIMAL(18,2) NOT NULL,
    NetAmount DECIMAL(18,2) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (LocalId, FirmId),
    UNIQUE KEY `IX_BillHeaders_Id` (Id),
    FOREIGN KEY (PartyId) REFERENCES Party(Id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--Update the BillItems table to reference the composite key
CREATE TABLE IF NOT EXISTS BillItems (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    BillLocalId INT NOT NULL,
    BillFirmId VARCHAR(20) NOT NULL,
    ItemId INT NOT NULL,
    Barcode VARCHAR(50) NULL,
    Description VARCHAR(255) NULL,
    Purity VARCHAR(20) NULL,
    HSNCode VARCHAR(20) NULL,
    PCS INT DEFAULT 1,
    GrossWt DECIMAL(18,3) DEFAULT 0,
    LessWt DECIMAL(18,3) DEFAULT 0,
    NetWt DECIMAL(18,3) DEFAULT 0,
    DiamondCt DECIMAL(18,3) DEFAULT 0,
    DiamondRate DECIMAL(18,2) DEFAULT 0,
    DiaCharge DECIMAL(18,2) DEFAULT 0,
    StoneCt DECIMAL(18,3) DEFAULT 0,
    StoneCharge DECIMAL(18,2) DEFAULT 0,
    FinalWeight DECIMAL(18,3) DEFAULT 0,
    MetalType VARCHAR(50) NULL,
    HUID VARCHAR(50) NULL,
    Hallmark VARCHAR(50) NULL,
    HmTax DECIMAL(18,2) DEFAULT 0,
    Quantity DECIMAL(18,3) NOT NULL,
    Rate DECIMAL(18,2) NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    TaxRate DECIMAL(5,2) NOT NULL,
    TaxAmount DECIMAL(18,2) NOT NULL,
    MakingCharge DECIMAL(18,2) DEFAULT 0,
    NetPrice DECIMAL(18,2) DEFAULT 0,
    FinalAmount DECIMAL(18,2) DEFAULT 0,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (BillLocalId, BillFirmId) REFERENCES BillHeaders(LocalId, FirmId) ON DELETE CASCADE,
    FOREIGN KEY (ItemId) REFERENCES Items(Id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--Update the BillPayments table to reference the composite key
CREATE TABLE IF NOT EXISTS BillPayments (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    BillLocalId INT NOT NULL,
    BillFirmId VARCHAR(20) NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    PaymentMode VARCHAR(50) NOT NULL,
    ReferenceNo VARCHAR(100) NULL,
    PaymentDate DATETIME NOT NULL,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (BillLocalId, BillFirmId) REFERENCES BillHeaders(LocalId, FirmId) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--Update the stored procedures to handle the composite key
DELIMITER //

-- Add Bill Header (Server)
CREATE PROCEDURE sp_AddBillHeader_server(
    IN p_LocalId INT,
    IN p_FirmId VARCHAR(20),
    IN p_BillNo VARCHAR(50),
    IN p_BillDate DATETIME,
    IN p_PartyId INT,
    IN p_TotalAmount DECIMAL(18,2),
    IN p_TaxAmount DECIMAL(18,2),
    IN p_DiscountAmount DECIMAL(18,2),
    IN p_NetAmount DECIMAL(18,2)
)
BEGIN
    -- Generate bill number if not provided
    IF p_BillNo IS NULL OR p_BillNo = '' THEN
        SET p_BillNo = CONCAT('BILL-', DATE_FORMAT(NOW(), '%Y%m%d-'), LPAD(FLOOR(RAND() * 10000), 4, '0'));
END IF;

INSERT INTO BillHeaders (
        LocalId, FirmId, BillNo, BillDate, PartyId,
    TotalAmount, TaxAmount, DiscountAmount, NetAmount, CreatedAt
    ) VALUES (
        p_LocalId, p_FirmId, p_BillNo, p_BillDate, p_PartyId,
    p_TotalAmount, p_TaxAmount, p_DiscountAmount, p_NetAmount, NOW()
    )
    ON DUPLICATE KEY UPDATE
        BillNo = p_BillNo,
    BillDate = p_BillDate,
    PartyId = p_PartyId,
    TotalAmount = p_TotalAmount,
    TaxAmount = p_TaxAmount,
    DiscountAmount = p_DiscountAmount,
    NetAmount = p_NetAmount,
    UpdatedAt = NOW();
END //

-- Add Bill Item (Server)
CREATE PROCEDURE sp_AddBillItem_server(
    IN p_BillLocalId INT,
    IN p_BillFirmId VARCHAR(20),
    IN p_ItemId INT,
    IN p_Barcode VARCHAR(50),
    IN p_Description VARCHAR(255),
    IN p_Purity VARCHAR(20),
    IN p_HSNCode VARCHAR(20),
    IN p_PCS INT,
    IN p_GrossWt DECIMAL(18,3),
    IN p_LessWt DECIMAL(18,3),
    IN p_NetWt DECIMAL(18,3),
    IN p_DiamondCt DECIMAL(18,3),
    IN p_DiamondRate DECIMAL(18,2),
    IN p_DiaCharge DECIMAL(18,2),
    IN p_StoneCt DECIMAL(18,3),
    IN p_StoneCharge DECIMAL(18,2),
    IN p_FinalWeight DECIMAL(18,3),
    IN p_MetalType VARCHAR(50),
    IN p_HUID VARCHAR(50),
    IN p_Hallmark VARCHAR(50),
    IN p_HmTax DECIMAL(18,2),
    IN p_Quantity DECIMAL(18,3),
    IN p_Rate DECIMAL(18,2),
    IN p_Amount DECIMAL(18,2),
    IN p_TaxRate DECIMAL(5,2),
    IN p_TaxAmount DECIMAL(18,2),
    IN p_MakingCharge DECIMAL(18,2),
    IN p_NetPrice DECIMAL(18,2),
    IN p_FinalAmount DECIMAL(18,2)
)
BEGIN
    INSERT INTO BillItems (
        BillLocalId, BillFirmId, ItemId, Barcode, Description, Purity, HSNCode, PCS,
        GrossWt, LessWt, NetWt, DiamondCt, DiamondRate, DiaCharge,
        StoneCt, StoneCharge, FinalWeight, MetalType, HUID, Hallmark, HmTax,
        Quantity, Rate, Amount, TaxRate, TaxAmount, MakingCharge, NetPrice, FinalAmount
    ) VALUES (
        p_BillLocalId, p_BillFirmId, p_ItemId, p_Barcode, p_Description, p_Purity, p_HSNCode, p_PCS,
        p_GrossWt, p_LessWt, p_NetWt, p_DiamondCt, p_DiamondRate, p_DiaCharge,
        p_StoneCt, p_StoneCharge, p_FinalWeight, p_MetalType, p_HUID, p_Hallmark, p_HmTax,
        p_Quantity, p_Rate, p_Amount, p_TaxRate, p_TaxAmount, p_MakingCharge, p_NetPrice, p_FinalAmount
    );
END //

-- Add Bill Payment (Server)
CREATE PROCEDURE sp_AddBillPayment_server(
    IN p_BillLocalId INT,
    IN p_BillFirmId VARCHAR(20),
    IN p_Amount DECIMAL(18,2),
    IN p_PaymentMode VARCHAR(50),
    IN p_ReferenceNo VARCHAR(100),
    IN p_PaymentDate DATETIME
)
BEGIN
    INSERT INTO BillPayments (
        BillLocalId, BillFirmId, Amount, PaymentMode, ReferenceNo, PaymentDate
    ) VALUES (
        p_BillLocalId, p_BillFirmId, p_Amount, p_PaymentMode, p_ReferenceNo, p_PaymentDate
    );
END //

DELIMITER;