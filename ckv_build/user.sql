--server=192.168.10.37;database=Release_FB51_App;UId=mobile; Password=HjdbFV7jos9bc6lw;Connection Timeout=180
----thêm bảng tạm #table_qlpk
SELECT 
	t.groupid,_u_qlkv.UserID,gss.ShopCode,_u_qlkv.UserPosition into #table_qlpk 
FROM pos.[User] _u_qlkv
	INNER JOIN pos.[UserGroup] ug ON ug.UserID = _u_qlkv.UserID
	INNER JOIN pos.GroupShop gss ON gss.GroupID = ug.GroupID
	INNER JOIN (SELECT DISTINCT s.Code,g.GroupID,g.[Name] 
				FROM pos.ShopDetail s
					LEFT OUTER JOIN pos.GroupShop gs ON s.Code = gs.ShopCode
					JOIN pos.[Group] g ON gs.GroupID = g.GroupID AND g.Status = 1 AND g.IsShop = 1 AND g.GroupID <> 8 AND s.ShopID NOT IN (20, 29, 154/*sale*/)
				) t on t.Code=gss.ShopCode
where (UserPosition=15 or UserPosition=17) and _u_qlkv.UserID!=1221
------end thêm bảng tạm

select *,(select top 1 isnull(UserFullName,'') from pos.[User] where UserID=group__.group_pk) as str_group_pk_name
from (SELECT
	u.UserID as id, 
	ISNULL(u.CalloutToken,'') as str_call_out_tooken, 
	u.ApproveLevel as int_approve_level, 
	ISNULL(u.UserPosition,0) as str_user_position, 
	ug.GroupID as group_id, 
	u.UserName as str_user_name, 
	ISNULL( u.[POLStatus],0) as int_pol_status, 
	ISNULL( [POLRegion],0) as int_pol_region, 
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
		 when 617 then 1  when 1810  then 1 when 619 then 1 when 2001 then 1 when 523 then 1 else 0 
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
	,(CASE
		WHEN ug.GroupID in (SELECT GroupID FROM #table_qlpk) THEN (select top 1 kh.UserID From (SELECT groupid,UserID,ShopCode,UserPosition, 
		(CASE
			WHEN ((select COUNT(h.groupid) from (select t.groupid FROM #table_qlpk where t.groupid=GroupID)h)>1)
			THEN N'1' else '2' end) as qlpk
			FROM #table_qlpk t ) kh where kh.GroupID=ug.GroupID and( (kh.qlpk=1 and kh.UserPosition!=15) or kh.qlpk=2 ))
			else 0
		end) as group_pk
		,STUFF(',' + 
			(SELECT ',' + CONVERT(NVARCHAR(20), k.GroupID)
				FROM #table_qlpk k where k.UserID=u.UserID
				FOR xml path('')
			) + ',',1,1, '') as str_group_pk
	FROM [pos].[User]  u
		left JOIN pos.[UserGroup] ug ON ug.UserID = u.UserID
		left JOIN pos.[Group] g ON ug.GroupID = g.GroupID  AND g.STATUS = 1
	where u.Status =1) group__
 order by group__.id asc
 
----xóa bảng tạm #table_qlpk
drop table #table_qlpk
----end xóa bảng tạm