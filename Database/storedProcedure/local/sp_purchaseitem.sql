DELIMITER //

CREATE PROCEDURE `sp_InsertPurchaseItem` (
    IN in_PurchaseId INT,
    IN in_Metal VARCHAR(50),
    IN in_HSNCode VARCHAR(10),
    IN in_HuidNo VARCHAR(50),
    IN in_ItemName VARCHAR(100),
    IN in_Purity VARCHAR(20),
    IN in_Pcs INT,
    IN in_GrossWt DECIMAL(10,3),
    IN in_DiaCt DECIMAL(10,3),
    IN in_StoneCt DECIMAL(10,3),
    IN in_LessWt DECIMAL(10,3),
    IN in_Waste DECIMAL(10,3),
    IN in_NetWt DECIMAL(10,3),
    IN in_Rate DECIMAL(10,2),
    IN in_Amount DECIMAL(12,2),
    IN in_StoneCharge DECIMAL(10,2),
    IN in_DiaCharge DECIMAL(10,2),
    IN in_NetPrice DECIMAL(12,2)
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    START TRANSACTION;

    INSERT INTO PurchaseItems (
        PurchaseId,
        Metal,
        HSNCode,
        HuidNo,
        ItemName,
        Purity,
        Pcs,
        GrossWt,
        DiaCt,
        StoneCt,
        LessWt,
        Waste,
        NetWt,
        Rate,
        Amount,
        StoneCharge,
        DiaCharge,
        NetPrice
    ) VALUES (
        in_PurchaseId,
        in_Metal,
        in_HSNCode,
        in_HuidNo,
        in_ItemName,
        in_Purity,
        in_Pcs,
        in_GrossWt,
        in_DiaCt,
        in_StoneCt,
        in_LessWt,
        in_Waste,
        in_NetWt,
        in_Rate,
        in_Amount,
        in_StoneCharge,
        in_DiaCharge,
        in_NetPrice
    );

    COMMIT;
END //

DELIMITER ;




