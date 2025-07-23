CREATE TABLE `Hsn_list` (
  `hsn_id` BIGINT AUTO_INCREMENT,
  `firmid` VARCHAR(50)   NOT NULL,
  `local_id` BIGINT(20) NOT NULL,
  `metal` VARCHAR(100)   DEFAULT NULL,
  `hsn_code` VARCHAR(50)   DEFAULT NULL,
  PRIMARY KEY (`firmid`, `local_id`),     -- Composite Primary Key
  UNIQUE KEY (`hsn_id`)                   -- Keep hsn_id unique
);
