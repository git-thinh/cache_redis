select
     [RegionID] as id
    ,[Name] as str_name
    ,isnull([ParentID],0) as int_pid 
    ,isnull((select [Name] 
			from pos.region as b 
			where b.[RegionID] = a.[ParentID] ),'') 
		as str_parent_name 
from pos.region as a 
where [status] = 1