USE [phuquy]
GO

/****** Object:  Table [dbo].[cms_data]    Script Date: 14/04/2020 3:48:49 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[cms_data](
	[id] [bigint] NOT NULL,
	[str_src_domain] [varchar](255) NULL,
	[str_src_url] [varchar](500) NULL,
	[str_src_category] [nvarchar](500) NULL,
	[int_src_date] [int] NULL,
	[str_seo_path] [varchar](500) NULL,
	[str_seo_keyword] [nvarchar](500) NULL,
	[str_seo_description] [nvarchar](1000) NULL,
	[str_title] [nvarchar](500) NULL,
	[str_text] [nvarchar](max) NULL,
	[str_html] [nvarchar](max) NULL,
	[str_img_uuids] [varchar](max) NULL,
	[str_tags] [nvarchar](1000) NULL,
	[int_group_id] [int] NULL,
	[str_categories] [nvarchar](1000) NULL,
	[int_create_date] [int] NULL,
	[int_create_time] [int] NULL,
	[int_city_id] [int] NULL,
	[int_distict_id] [int] NULL,
	[int_counter] [int] NULL,
	[int_state] [int] NULL,
	[str_pub_domains] [varchar](500) NULL,
	[int_pub_create_date] [int] NULL,
	[int_pub_create_time] [int] NULL,
 CONSTRAINT [PK_cms_data] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


