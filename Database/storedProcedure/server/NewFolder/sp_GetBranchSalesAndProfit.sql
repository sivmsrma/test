CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_GetBranchSalesAndProfit`(IN branch_id INT)
BEGIN
    DECLARE firm_id VARCHAR(50);
    
    -- Get firm_id based on branch
    SELECT firmid INTO firm_id FROM company_details WHERE id = branch_id;

    -- Return only purchase and profit (for now)
    SELECT 
        b.id AS branch_id,
        b.shopName AS branch_name,
        
        -- COALESCE((
        --     SELECT SUM(s.total_amount)
        --     FROM sales s
        --     WHERE s.branch_id = branch_id
        -- ), 0) AS total_sales,

        COALESCE((
            SELECT SUM(p.NetAmount)
            FROM purchases p
            WHERE p.firm_id = firm_id
        ), 0) AS total_purchases,

        -- Profit based on purchase only for now (sales is commented)
        COALESCE((
            SELECT SUM(p.NetAmount)
            FROM purchases p
            WHERE p.firm_id = firm_id
        ), 0) AS total_profit

    FROM company_details b
    WHERE b.id = branch_id;
END



CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_GetAllUsers`()
BEGIN
    SELECT user_name,user_type,assigned_branch FROM users;
END