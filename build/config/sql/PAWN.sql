SELECT 
	*
	,isnull((select * 
			from [SplitStringToTable](str_url,'&')),'') 
		as str_campaign 
FROM mobile.pol_pawn 
order by id asc