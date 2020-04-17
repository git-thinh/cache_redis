--server=THINHNUC\MSSQL;database=phuquy;UId=sa; Password=dev@123;Connection Timeout=180

create table @ckv___(id bigint, api varchar(255));
insert into @ckv___ values(@id,'cms_data');

INSERT INTO [cms_data]
(
[id]
,[str_src_domain]
,[str_src_url]
,[str_src_category]
,[int_src_date]
,[str_seo_path]
,[str_seo_keyword]
,[str_seo_description]
,[str_title]
,[str_text]
,[str_html]
,[str_img_uuids]
,[str_tags]
,[int_group_id]
,[str_categories]
,[int_create_date]
,[int_create_time]
,[int_city_id]
,[int_distict_id]
,[int_counter]
,[int_state]
,[str_pub_domains]
,[int_pub_create_date]
,[int_pub_create_time]
)
     VALUES
(
@id
,@str_src_domain
,@str_src_url
,@str_src_category
,@int_src_date
,@str_seo_path
,@str_seo_keyword
,@str_seo_description
,@str_title
,@str_text
,@str_html
,@str_img_uuids
,@str_tags
,@int_group_id
,@str_categories
,@int_create_date
,@int_create_time
,@int_city_id
,@int_distict_id
,@int_counter
,@int_state
,@str_pub_domains
,@int_pub_create_date
,@int_pub_create_time
);

select * from @ckv___;

