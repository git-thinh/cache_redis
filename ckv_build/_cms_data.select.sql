--server=THINHNUC\MSSQL;database=phuquy;UId=sa; Password=dev@123;Connection Timeout=180

select m___.*
from cms_data as m___
--<1> where m___.id = <ID>
--<2> <IDS>
order by m___.id desc