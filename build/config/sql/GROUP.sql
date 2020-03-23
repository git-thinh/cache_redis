SELECT 
     GroupID as id 
    ,isnull(ParentID,0) as parent_id 
    ,Code as str_code 
    ,[Name] as str_name 
    ,[Status] as int_status 
FROM [pos].[Group] 
where IsShop=1 or GroupID=44 or GroupID=58