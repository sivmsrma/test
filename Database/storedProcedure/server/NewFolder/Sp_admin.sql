
CREATE  PROCEDURE `sp_login_user_or_admin`(
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
END  $$

DELIMITER ;
