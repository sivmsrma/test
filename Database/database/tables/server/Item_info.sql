CREATE TABLE `Item_info` (
  `item_id` BIGINT AUTO_INCREMENT,
  `metal_type` VARCHAR(100)   DEFAULT NULL,
  `category` VARCHAR(100)   DEFAULT NULL,
  `sub_category` VARCHAR(100)   DEFAULT NULL,
  `design` VARCHAR(100)   DEFAULT NULL,
  `hsn_id` INT(11) DEFAULT NULL,
  `hsn_code` VARCHAR(50)   DEFAULT NULL,
  `short_name` VARCHAR(10)   NOT NULL,
  `firmid` VARCHAR(10)   NOT NULL,
  `local_id` BIGINT(20) NOT NULL,
  `IsDataPostOnServer` TINYINT(1) DEFAULT NULL,
  PRIMARY KEY (`firmid`, `local_id`),             -- ✅ Composite primary key
  UNIQUE KEY (`item_id`),                         -- ✅ Unique key for tracking
  CONSTRAINT `fk_iteminfo_hsnid` 
    FOREIGN KEY (`hsn_id`) REFERENCES `Hsn_list`(`hsn_id`) 
    ON DELETE SET NULL ON UPDATE CASCADE
);

