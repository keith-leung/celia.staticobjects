SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for __migrationversions
-- ----------------------------
DROP TABLE IF EXISTS `__migrationversions`;
CREATE TABLE `__migrationversions` (
  `VersionCode` varchar(100) NOT NULL COMMENT '主键，人工设置一个值，例如：20180809001，代表2018年8月9日第一次更新',
  `Version` varchar(100) NOT NULL COMMENT '代码版本，表示这个更新后的数据库版本是跟哪个代码版本的',
  `ctime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '执行时间戳',
  PRIMARY KEY (`VersionCode`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;

-- ----------------------------
-- Table structure for staticobjects_image_element_tran_items
-- ----------------------------
DROP TABLE IF EXISTS `staticobjects_image_element_tran_items`;
CREATE TABLE `staticobjects_image_element_tran_items`  (
  `id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '主键',
  `image_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '图片object_id',
  `extension` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '扩展名',
  `tran_item_type` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '图片转换规则名称',
  `file_path` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '域名以下的相对路径',
  `APPID` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` int(11) NOT NULL DEFAULT 0,
  `ctime` datetime(0) NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `utime` datetime(0) NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP(0),
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `EntityIndex`(`APPID`, `IsDeleted`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Compact;

-- ----------------------------
-- Table structure for staticobjects_image_elements
-- ----------------------------
DROP TABLE IF EXISTS `staticobjects_image_elements`;
CREATE TABLE `staticobjects_image_elements`  (
  `id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '主键',
  `src_file_name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '上传时使用的文件名',
  `storage_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '存储ID，所对应存储在哪个存储介质',
  `extension` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '扩展名，文件处理需要用这个进行分类',
  `store_with_src_file_name` int(11) NOT NULL DEFAULT 0 COMMENT '0: 使用object_id + \".\" + extension的形式组成文件名；\r\n1：使用上传原始文件名作为存储文件名；',
  `file_path` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '域名以下的相对路径',
  `is_published` int(255) NOT NULL DEFAULT 0 COMMENT '0：未发布；1：已发布（外网可见）',
  `APPID` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` int(11) NULL DEFAULT 0,
  `ctime` datetime(0) NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `utime` datetime(0) NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP(0),
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `EntityIndex`(`APPID`, `IsDeleted`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Compact;

-- ----------------------------
-- Table structure for staticobjects_service_apps
-- ----------------------------
DROP TABLE IF EXISTS `staticobjects_service_apps`;
CREATE TABLE `staticobjects_service_apps`  (
  `app_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '接入方ID，唯一',
  `app_secret` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '接入方密钥，用于在需要的时候生成URL签名',
  `description` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '描述',
  `ctime` datetime(0) NOT NULL COMMENT '创建时间',
  `utime` datetime(0) NULL DEFAULT NULL COMMENT '更新时间',
  `locktime` datetime(0) NULL DEFAULT NULL COMMENT '锁定时间，如果不为空或者大于当前时间，则代表到这个时间之前锁定不许访问',
  PRIMARY KEY (`app_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Compact;

-- ----------------------------
-- Table structure for staticobjects_service_apps_storages_relations
-- ----------------------------
DROP TABLE IF EXISTS `staticobjects_service_apps_storages_relations`;
CREATE TABLE `staticobjects_service_apps_storages_relations`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `app_id` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT 'service_apps的主键，代表接入方',
  `storage_id` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT 'storages的主键，代表存储对象',
  `store_with_src_file_name` tinyint(255) NULL DEFAULT NULL COMMENT '是否以源文件名储存，如果1则按照文件名储存，0则是按照object_id存',
  `ctime` datetime(0) NOT NULL COMMENT '创建时间',
  `utime` datetime(0) NULL DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `IX_app_stor_relations`(`app_id`, `storage_id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Compact;

-- ----------------------------
-- Table structure for staticobjects_storages
-- ----------------------------
DROP TABLE IF EXISTS `staticobjects_storages`;
CREATE TABLE `staticobjects_storages`  (
  `storage_id` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '存储ID，必须唯一，并且极有可能组成URL的一部分',
  `storage_name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '存储名称，相当于短描述，用于管理',
  `storage_type` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0' COMMENT '0：本地存储（单机模式）；\r\n1：NFS；\r\n2：Azure Storage；\r\n3：阿里云OSS；',
  `storage_access_key` varchar(1024) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '用于云服务Storage的访问密钥',
  `download_host` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '用于内网下载所使用的下载域名+端口',
  `output_host` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT 'storage的“外网”下载所使用的域名+端口',
  `publish_storage_id` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '发布存储ID，可以是自身存储ID，适用于全内网没必要做限制的环境',
  `publish_storage_access_key` varchar(1024) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '用于云服务Publish Storage的访问密钥',
  `publish_host` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '用于外网下载所使用的下载域名+端口',
  `publish_output_host` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '发布storage的“外网”下载所使用的域名+端口',
  PRIMARY KEY (`storage_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Compact;

SET FOREIGN_KEY_CHECKS = 1;
