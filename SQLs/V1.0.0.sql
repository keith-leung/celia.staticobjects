-- ----------------------------
-- Table structure for staticobjects_image_elements
-- ----------------------------
CREATE TABLE staticobjects_image_elements (
  [object_id] nvarchar(100) NOT NULL,
  [src_file_name] nvarchar(100) NULL,
  [storage_id] nvarchar(100) NOT NULL,
  [extension] nvarchar(100) NULL,
  [store_with_src_file_name] tinyint DEFAULT 0 NOT NULL,
  [file_path] nvarchar(1024) NULL,
  [is_published] tinyint DEFAULT 0 NOT NULL,
  [ctime] datetime2 NOT NULL,
  [utime] datetime2 NULL,
  PRIMARY KEY CLUSTERED ([object_id])
)
GO

CREATE NONCLUSTERED INDEX [IX_images_storageid]
ON staticobjects_image_elements (
  [storage_id]
)
GO


-- ----------------------------
-- Table structure for staticobjects_image_element_tran_items
-- ----------------------------
CREATE TABLE staticobjects_image_element_tran_items (
  [object_id] nvarchar(100) NOT NULL,
  [image_id] nvarchar(100) NOT NULL,
  [extension] nvarchar(100) NULL,
  [file_path] nvarchar(1024) NULL,
  [tran_item_type] nvarchar(100) DEFAULT 0 NOT NULL,
  [ctime] datetime2 NOT NULL,
  [utime] datetime2 NULL,
  PRIMARY KEY CLUSTERED ([object_id])
)
GO

CREATE NONCLUSTERED INDEX [IX_imagetrams_imageid]
ON staticobjects_image_element_tran_items (
  [image_id]
)
GO


-- ----------------------------
-- Table structure for staticobjects_image_element_tran_items
-- ----------------------------
CREATE TABLE staticobjects_image_element_tran_items (
  [storage_id] nvarchar(100) NOT NULL,
  [storage_name] nvarchar(100) NULL,
  [storage_type] nvarchar(100) NOT NULL,
  [storage_access_key] nvarchar(1024) NOT NULL,
  [download_host] nvarchar(100) DEFAULT 0 NOT NULL,
  [output_host] nvarchar(100) NULL,
  [publish_storage_id] nvarchar(100) NOT NULL,
  [publish_storage_access_key] nvarchar(1024) NOT NULL,
  [publish_host] nvarchar(100) NOT NULL,
  [publish_output_host] nvarchar(100) NULL,
  PRIMARY KEY CLUSTERED ([storage_id])
)
GO


-- ----------------------------
-- Table structure for staticobjects_service_apps_storages_relations
-- ----------------------------
CREATE TABLE staticobjects_service_apps_storages_relations (
  [id] int NOT NULL,
  [app_id] nvarchar(100) NOT NULL,
  [storage_id] nvarchar(100) DEFAULT 0 NOT NULL,
  [store_with_src_file_name] tinyint NULL,
  [ctime] datetime2 NOT NULL,
  [utime] datetime2 NULL,
  PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

CREATE NONCLUSTERED INDEX [IX_app_stors_appidstorageid]
ON staticobjects_service_apps_storages_relations (
  [app_id],
  [storage_id]
)
GO 


-- ----------------------------
-- Table structure for __migrationversions
-- ----------------------------
CREATE TABLE __migrationversions (
  [VersionCode] nvarchar(255) NOT NULL,
  [Version] nvarchar(255) NOT NULL,
  [ctime] datetime2(7)  NULL,
  PRIMARY KEY CLUSTERED ([VersionCode])
)
GO 

INSERT INTO __migrationversions(VersionCode, [Version]) VALUES ('20190129001', 'V1.0.0');