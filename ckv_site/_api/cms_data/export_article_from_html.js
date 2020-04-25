
//_config = {}
//_para = {}

try {
    var sql = " \
        select m___.* \
        from cms_data as m___ \
        --<1> where m___.id = <ID> \
        --<2> <IDS> \
        order by m___.id desc \
";

    api.cache_all(sql);
} catch (e) {
    ;
}


