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
  `IsDataPostOnServer` TINYINT(1),
  `PurchaseItemId` INT(11)
);
