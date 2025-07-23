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
