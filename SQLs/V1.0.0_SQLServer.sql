-- ----------------------------
-- Table structure for __migrationversions
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[__migrationversions]') AND type IN ('U'))
	DROP TABLE [dbo].[__migrationversions]
GO

CREATE TABLE [dbo].[__migrationversions] (
  [VersionCode] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Version] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [ctime] datetime2(7)  NULL
)
GO


-- ----------------------------
-- Table structure for staticobjects_image_element_tran_items
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[staticobjects_image_element_tran_items]') AND type IN ('U'))
	DROP TABLE [dbo].[staticobjects_image_element_tran_items]
GO

CREATE TABLE [dbo].[staticobjects_image_element_tran_items] (
  [object_id] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [image_id] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [extension] nvarchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [tran_item_type] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [file_path] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [APPID] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [IsDeleted] int DEFAULT ((0)) NOT NULL,
  [ctime] datetime2(7) DEFAULT (getdate()) NOT NULL,
  [utime] datetime2(7)  NULL
)
GO


-- ----------------------------
-- Table structure for staticobjects_image_elements
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[staticobjects_image_elements]') AND type IN ('U'))
	DROP TABLE [dbo].[staticobjects_image_elements]
GO

CREATE TABLE [dbo].[staticobjects_image_elements] (
  [object_id] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [src_file_name] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [storage_id] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [extension] nvarchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [store_with_src_file_name] bit  NOT NULL,
  [file_path] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [is_published] bit  NOT NULL,
  [APPID] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [IsDeleted] int DEFAULT ((0)) NULL,
  [ctime] datetime2(7) DEFAULT (getdate()) NOT NULL,
  [utime] datetime2(7)  NULL
)
GO


-- ----------------------------
-- Table structure for staticobjects_service_apps
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[staticobjects_service_apps]') AND type IN ('U'))
	DROP TABLE [dbo].[staticobjects_service_apps]
GO

CREATE TABLE [dbo].[staticobjects_service_apps] (
  [app_id] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [app_secret] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [description] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [ctime] datetime2(7)  NOT NULL,
  [utime] datetime2(7)  NULL,
  [locktime] datetime2(7)  NULL
)
GO


-- ----------------------------
-- Table structure for staticobjects_service_apps_storages_relations
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[staticobjects_service_apps_storages_relations]') AND type IN ('U'))
	DROP TABLE [dbo].[staticobjects_service_apps_storages_relations]
GO

CREATE TABLE [dbo].[staticobjects_service_apps_storages_relations] (
  [id] int  NOT NULL,
  [app_id] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [storage_id] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [store_with_src_file_name] tinyint  NULL,
  [ctime] datetime2(7)  NOT NULL,
  [utime] datetime2(7)  NULL
)
GO


-- ----------------------------
-- Table structure for staticobjects_storages
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[staticobjects_storages]') AND type IN ('U'))
	DROP TABLE [dbo].[staticobjects_storages]
GO

CREATE TABLE [dbo].[staticobjects_storages] (
  [storage_id] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [storage_name] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [storage_type] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [storage_access_key] nvarchar(1024) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [download_host] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [output_host] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [publish_storage_id] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [publish_storage_access_key] nvarchar(1024) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [publish_host] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [publish_output_host] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL
)
GO


-- ----------------------------
-- Primary Key structure for table __migrationversions
-- ----------------------------
ALTER TABLE [dbo].[__migrationversions] ADD CONSTRAINT [PK____migrat__8C291688F95A127F] PRIMARY KEY CLUSTERED ([VersionCode])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Indexes structure for table staticobjects_image_element_tran_items
-- ----------------------------
CREATE NONCLUSTERED INDEX [EntityIndex]
ON [dbo].[staticobjects_image_element_tran_items] (
  [APPID] ASC,
  [IsDeleted] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table staticobjects_image_element_tran_items
-- ----------------------------
ALTER TABLE [dbo].[staticobjects_image_element_tran_items] ADD CONSTRAINT [PK__staticob__3213E83F8A057601] PRIMARY KEY CLUSTERED ([object_id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Indexes structure for table staticobjects_image_elements
-- ----------------------------
CREATE NONCLUSTERED INDEX [EntityIndex]
ON [dbo].[staticobjects_image_elements] (
  [APPID] ASC,
  [IsDeleted] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table staticobjects_image_elements
-- ----------------------------
ALTER TABLE [dbo].[staticobjects_image_elements] ADD CONSTRAINT [PK__staticob__3213E83F16711DEB] PRIMARY KEY CLUSTERED ([object_id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Primary Key structure for table staticobjects_service_apps
-- ----------------------------
ALTER TABLE [dbo].[staticobjects_service_apps] ADD CONSTRAINT [PK__staticob__6F8A0A344B55FD27] PRIMARY KEY CLUSTERED ([app_id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Indexes structure for table staticobjects_service_apps_storages_relations
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_app_stor_relations]
ON [dbo].[staticobjects_service_apps_storages_relations] (
  [app_id] ASC,
  [storage_id] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table staticobjects_service_apps_storages_relations
-- ----------------------------
ALTER TABLE [dbo].[staticobjects_service_apps_storages_relations] ADD CONSTRAINT [PK__staticob__3213E83FA5AAED7E] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Primary Key structure for table staticobjects_storages
-- ----------------------------
ALTER TABLE [dbo].[staticobjects_storages] ADD CONSTRAINT [PK__staticob__AB53044A2C0A5B1C] PRIMARY KEY CLUSTERED ([storage_id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

