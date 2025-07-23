

DELIMITER //

CREATE PROCEDURE `sp_InsertPurchase` (
    IN BillNo VARCHAR(50),
    IN PurchaseDate DATE,
    IN SupplierId INT,
    IN Remarks TEXT,
    IN TotalAmount DECIMAL(12,2),
    IN Tax DECIMAL(10,2),
    IN Discount DECIMAL(10,2),
    IN NetAmount DECIMAL(12,2),
    OUT PurchaseId INT
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    START TRANSACTION;

    INSERT INTO Purchases (
        BillNo,
        PurchaseDate,
        SupplierId,
        Remarks,
        TotalAmount,
        Tax,
        Discount,
        NetAmount
    ) VALUES (
        BillNo,
        PurchaseDate,
        SupplierId,
        Remarks,
        TotalAmount,
        Tax,
        Discount,
        NetAmount
    );

    SET out_PurchaseId = LAST_INSERT_ID();

    COMMIT;
END //

DELIMITER ;




















---- Server failed procedure
DELIMITER //

CREATE PROCEDURE `sp_serverfailed_InsertPurchase` (
    IN BillNo VARCHAR(50),
    IN PurchaseDate DATE,
    IN SupplierId INT,
    IN Remarks TEXT,
    IN TotalAmount DECIMAL(12,2),
    IN Tax DECIMAL(10,2),
    IN Discount DECIMAL(10,2),
    IN NetAmount DECIMAL(12,2),
    OUT PurchaseId INT
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    START TRANSACTION;

    INSERT INTO Purchases (
        BillNo,
        PurchaseDate,
        SupplierId,
        Remarks,
        TotalAmount,
        Tax,
        Discount,
        NetAmount
    ) VALUES (
        BillNo,
        PurchaseDate,
        SupplierId,
        Remarks,
        TotalAmount,
        Tax,
        Discount,
        NetAmount
    );

    SET out_PurchaseId = LAST_INSERT_ID();

    COMMIT;
END //

DELIMITER ;


