/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50529
Source Host           : localhost:3306
Source Database       : tera

Target Server Type    : MYSQL
Target Server Version : 50529
File Encoding         : 65001

Date: 2013-05-18 13:00:34
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `accounts`
-- ----------------------------
DROP TABLE IF EXISTS `accounts`;
CREATE TABLE `accounts` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  `password` varchar(32) NOT NULL,
  `accesslevel` int(11) NOT NULL DEFAULT '0',
  `membership` int(11) NOT NULL DEFAULT '0',
  `lastonlineutc` bigint(20) NOT NULL DEFAULT '0',
  `coins` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of accounts
-- ----------------------------

-- ----------------------------
-- Table structure for `character`
-- ----------------------------
DROP TABLE IF EXISTS `character`;
CREATE TABLE `character` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `accountname` varchar(50) NOT NULL,
  `level` int(11) NOT NULL,
  `exp` bigint(20) NOT NULL,
  `exprecoverable` bigint(20) NOT NULL,
  `mount` int(11) NOT NULL,
  `uisettings` text,
  `guildaccepted` bit(1) NOT NULL,
  `praisegiven` bit(1) NOT NULL,
  `lastpraise` int(11) NOT NULL DEFAULT '-1',
  `currentbanksection` int(11) NOT NULL,
  `creationdate` int(11) NOT NULL,
  `lastonline` int(11) NOT NULL DEFAULT '0',
  `deleted` smallint(6) DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of character
-- ----------------------------

-- ----------------------------
-- Table structure for `character_data`
-- ----------------------------
DROP TABLE IF EXISTS `character_data`;
CREATE TABLE `character_data` (
  `playerid` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  `gender` enum('Male','Female','','') NOT NULL,
  `race` enum('Human','HighElf','Aman','Castanic','Popori','Elin','Baraka') NOT NULL,
  `playerclass` enum('Warrior','Lancer','Slayer','Berserker','Sorcerer','Archer','Priest','Mystic','Elementalist') NOT NULL,
  `data` longtext NOT NULL,
  `details` longtext NOT NULL,
  `mapid` int(11) NOT NULL,
  `x` float NOT NULL,
  `y` float NOT NULL,
  `z` float NOT NULL,
  `h` int(11) NOT NULL,
  PRIMARY KEY (`playerid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of character_data
-- ----------------------------

-- ----------------------------
-- Table structure for `guilds`
-- ----------------------------
DROP TABLE IF EXISTS `guilds`;
CREATE TABLE `guilds` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `guildname` varchar(50) NOT NULL,
  `guildranks` longtext NOT NULL,
  `guildlogo` longtext CHARACTER SET latin1 COLLATE latin1_bin,
  `guildhistory` longtext,
  `level` int(11) NOT NULL,
  `guildadd` longtext,
  `guildtitle` varchar(50) DEFAULT NULL,
  `guildmessage` text,
  `praises` int(11) NOT NULL DEFAULT '0',
  `creationdate` int(11) NOT NULL,
  `guildmembers` longtext NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of guilds
-- ----------------------------

-- ----------------------------
-- Table structure for `guild_events`
-- ----------------------------
DROP TABLE IF EXISTS `guild_events`;
CREATE TABLE `guild_events` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `gid` int(11) NOT NULL,
  `initiator` varchar(50) NOT NULL,
  `date` int(11) NOT NULL,
  `args` longtext NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of guild_events
-- ----------------------------

-- ----------------------------
-- Table structure for `guild_ranks`
-- ----------------------------
DROP TABLE IF EXISTS `guild_ranks`;
CREATE TABLE `guild_ranks` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `gid` int(11) NOT NULL,
  `rankprivileges` int(11) NOT NULL,
  `rankname` varchar(50) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of guild_ranks
-- ----------------------------

-- ----------------------------
-- Table structure for `inventory`
-- ----------------------------
DROP TABLE IF EXISTS `inventory`;
CREATE TABLE `inventory` (
  `accountname` varchar(50) NOT NULL,
  `playerid` int(11) NOT NULL,
  `itemid` int(11) NOT NULL,
  `amount` int(11) NOT NULL,
  `color` int(11) NOT NULL,
  `slot` int(5) NOT NULL,
  `storagetype` enum('Inventory','CharacterWarehouse','AccountWarehouse','GuildWarehouse','Trade') NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of inventory
-- ----------------------------

-- ----------------------------
-- Table structure for `quests`
-- ----------------------------
DROP TABLE IF EXISTS `quests`;
CREATE TABLE `quests` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `characterid` int(11) NOT NULL,
  `questid` int(11) NOT NULL,
  `status` enum('None','Start','Reward','Complete','Locked') NOT NULL,
  `step` int(11) NOT NULL,
  `counters` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of quests
-- ----------------------------

-- ----------------------------
-- Table structure for `servers`
-- ----------------------------
DROP TABLE IF EXISTS `servers`;
CREATE TABLE `servers` (
  `id` int(11) NOT NULL,
  `title` varchar(50) NOT NULL,
  `small_text` text NOT NULL,
  `img` longtext NOT NULL,
  `ip` varchar(22) NOT NULL,
  `port` int(5) NOT NULL,
  `category` varchar(3) NOT NULL,
  `name` varchar(25) NOT NULL,
  `crowdness` text NOT NULL,
  `open` int(11) NOT NULL,
  `permission_mask` varchar(15) NOT NULL DEFAULT '0x00000000',
  `server_stat` varchar(15) NOT NULL DEFAULT '0x00000001',
  `language` varchar(2) NOT NULL DEFAULT 'en',
  `l_visible` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of servers
-- ----------------------------

-- ----------------------------
-- Table structure for `skills`
-- ----------------------------
DROP TABLE IF EXISTS `skills`;
CREATE TABLE `skills` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PlayerId` int(11) NOT NULL DEFAULT '0',
  `SkillId` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of skills
-- ----------------------------
