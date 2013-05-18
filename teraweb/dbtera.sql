-- phpMyAdmin SQL Dump
-- version 3.5.2.2
-- http://www.phpmyadmin.net
--
-- Host: 127.0.0.1
-- Generation Time: May 17, 2013 at 11:22 PM
-- Server version: 5.5.27
-- PHP Version: 5.4.7

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Database: `dbtera`
--

-- --------------------------------------------------------

--
-- Table structure for table `accounts`
--

CREATE TABLE IF NOT EXISTS `accounts` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  `password` varchar(32) NOT NULL,
  `accesslevel` int(11) NOT NULL DEFAULT '0',
  `membership` int(11) NOT NULL DEFAULT '0',
  `lastonlineutc` bigint(20) NOT NULL,
  `coins` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table `character`
--

CREATE TABLE IF NOT EXISTS `character` (
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
  `lastonline` int(11) NOT NULL,
  `deleted` smallint(6) DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table `character_data`
--

CREATE TABLE IF NOT EXISTS `character_data` (
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
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table `guilds`
--

CREATE TABLE IF NOT EXISTS `guilds` (
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
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table `guild_events`
--

CREATE TABLE IF NOT EXISTS `guild_events` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `gid` int(11) NOT NULL,
  `initiator` varchar(50) NOT NULL,
  `date` int(11) NOT NULL,
  `args` longtext NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table `guild_ranks`
--

CREATE TABLE IF NOT EXISTS `guild_ranks` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `gid` int(11) NOT NULL,
  `rankprivileges` int(11) NOT NULL,
  `rankname` varchar(50) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table `inventory`
--

CREATE TABLE IF NOT EXISTS `inventory` (
  `accountname` varchar(50) NOT NULL,
  `playerid` int(11) NOT NULL,
  `itemid` int(11) NOT NULL,
  `amount` int(11) NOT NULL,
  `color` int(11) NOT NULL,
  `slot` int(5) NOT NULL,
  `storagetype` enum('Inventory','CharacterWarehouse','AccountWarehouse','GuildWarehouse','Trade') NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `quests`
--

CREATE TABLE IF NOT EXISTS `quests` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `characterid` int(11) NOT NULL,
  `questid` int(11) NOT NULL,
  `status` enum('None','Start','Reward','Complete','Locked') NOT NULL,
  `step` int(11) NOT NULL,
  `counters` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table `servers`
--

CREATE TABLE IF NOT EXISTS `servers` (
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

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
