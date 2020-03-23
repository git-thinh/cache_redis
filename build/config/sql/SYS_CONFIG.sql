SELECT 
     ROW_NUMBER() OVER(ORDER BY [Code] ASC) AS id
    ,[Code] as str_code 
    ,[Type] as str_type 
    ,[Value] as str_value 
    ,[Name] as str_name 
    ,[Status] as int_status 
FROM [pos].[SysConfig] 
where Type='PawnOnlineOption'