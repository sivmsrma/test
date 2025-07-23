CREATE TABLE billing_testing. Metal_type (
    id INT AUTO_INCREMENT PRIMARY KEY,
    metal_name VARCHAR(100) NOT NULL
);
INSERT INTO billing_testing. Metal_type (metal_name) VALUES
('Gold'),
('Silver'),
('Platinum'),
('Diamond'),
('Gold with Diamond'),
('Gold with Stone'),
('Silver with Stone');
