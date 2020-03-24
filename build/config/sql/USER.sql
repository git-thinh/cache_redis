SELECT
	u.UserID as id, 
	ISNULL(u.CalloutToken,'') as str_call_out_tooken, 
	u.ApproveLevel as int_approve_level, 
	ISNULL(u.UserPosition,0) as str_user_position, 
	ug.GroupID as group_id, 
	u.UserName as str_user_name, 
	ISNULL( u.[POLStatus],0) as int_pol_status, 
	--ISNULL( [POLRegion],0) as int_pol_region, 
	0 as int_pol_region, 
	g.[Name] as str_group_name, 
	u.UserFullName as str_full_name, 
	'12345@abc' as str_pass_word, 
	u.[UserPass] as str_pass,
	(CASE 
		WHEN u.ApproveLevel = 1 AND UserPosition = 4 THEN N'Nhân viên cửa hàng' 
		WHEN u.ApproveLevel = 1 AND UserPosition = 3 THEN N'Quản lý CH' 
		WHEN u.ApproveLevel = 2 THEN 'QLKV' END) 
			as str_possition, 
	(CASE 
		WHEN ug.GroupID = 44  THEN N'1' else (select top(1) s.ShopID from pos.[GroupShop] gs  inner JOIN pos.[ShopDetail] s ON gs.ShopCode = s.Code where g.GroupID = gs.GroupID) 
			end) as shop_id, 
	(CASE 
		WHEN ug.GroupID = 44  THEN N'Hỗ trợ khách hàng' else  (select top(1) s.[Name] from pos.[GroupShop] gs  inner JOIN pos.[ShopDetail] s ON gs.ShopCode = s.Code where g.GroupID = gs.GroupID) 
			end) as str_shop_name, 
	(case u.UserID 
		when 617 then 1  when 1810  then 1 when 619 then 1 else 0 
			end) as bit_admin_caller 
	,isnull(u.UserEmail,'') as str_user_email 
	,(CASE
		WHEN u.ApproveLevel = 2 THEN (STUFF(',' + 
			(SELECT ',' + CONVERT(NVARCHAR(20), g.GroupID)
				FROM pos.[User] _u_qlkv
					INNER JOIN pos.[UserGroup] ug ON ug.UserID = _u_qlkv.UserID
					INNER JOIN pos.GroupShop gs ON gs.GroupID = ug.GroupID
					INNER JOIN pos.[Group] g ON gs.ShopCode = g.Code
				WHERE _u_qlkv.UserID = u.UserID and g.Status = 1
				FOR xml path('')
			) + ',',1,1, '')) ELSE '' 
			END) as group_qlkv
FROM [pos].[User]  u
	left JOIN pos.[UserGroup] ug ON ug.UserID = u.UserID
	left JOIN pos.[Group] g ON ug.GroupID = g.GroupID  AND g.STATUS = 1
where u.Status =1  order by u.UserID asc