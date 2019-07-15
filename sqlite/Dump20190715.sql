-- MySQL dump 10.13  Distrib 8.0.16, for Win64 (x86_64)
--
-- Host: localhost    Database: ait
-- ------------------------------------------------------
-- Server version	8.0.16

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
 SET NAMES utf8 ;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `arno_accounts`
--

DROP TABLE IF EXISTS `arno_accounts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `arno_accounts` (
  `acc_id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `acc_login` varchar(45) NOT NULL,
  `acc_pass` varchar(100) DEFAULT NULL,
  `acc_email` varchar(45) DEFAULT NULL,
  `acc_active` bit(1) DEFAULT b'0',
  `acc_create` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `acc_lastupdate` datetime DEFAULT NULL,
  PRIMARY KEY (`acc_id`),
  UNIQUE KEY `usr_id_UNIQUE` (`acc_id`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `arno_accounts`
--

LOCK TABLES `arno_accounts` WRITE;
/*!40000 ALTER TABLE `arno_accounts` DISABLE KEYS */;
INSERT INTO `arno_accounts` VALUES (1,'root','21232F297A57A5A743894A0E4A801FC3','admi@administaction.com',_binary '','2019-06-17 10:00:48','2019-06-28 14:18:14'),(7,'Test acccount 101','FAE48D2EE52675BCD315CF2E07A594FB',NULL,_binary '\0','2019-06-28 14:45:55',NULL),(8,'Test acccount 102','9A4EE5922FEAC219F0F7946341B7B2CA',NULL,_binary '\0','2019-06-28 14:45:55',NULL),(9,'Test acccount 103','F1D03098F6F6A30171655D9E368D41C6',NULL,_binary '\0','2019-06-28 14:45:55',NULL),(10,'Test acccount 104','FDC9A90E7FBB844BC58A17AE718C12E0',NULL,_binary '\0','2019-06-28 14:45:55',NULL),(11,'Test acccount 105','E38A228749E01D753F5B439B10EFD4C7',NULL,_binary '\0','2019-06-28 14:45:55',NULL),(12,'arno','098F6BCD4621D373CADE4E832627B4F6','ppudwel@softsystem.pl',_binary '\0','2019-06-28 16:11:30',NULL),(13,'admin','21232F297A57A5A743894A0E4A801FC3','admin',_binary '\0','2019-07-01 12:45:27',NULL),(14,'test1','098F6BCD4621D373CADE4E832627B4F6','test',_binary '\0','2019-07-01 12:48:31',NULL),(15,'admin','21232F297A57A5A743894A0E4A801FC3','admin',_binary '\0','2019-07-02 10:59:47',NULL);
/*!40000 ALTER TABLE `arno_accounts` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `accounts_BEFORE_UPDATE` BEFORE UPDATE ON `arno_accounts` FOR EACH ROW BEGIN
	DECLARE acc_debug BIT(1);
    SET acc_debug = (SELECT set_value FROM ait.binary_settings WHERE set_name = 'SQL_DEBUG_MODE');
	SET NEW.acc_lastupdate = NOW();
    UPDATE usersdata SET udt_lastupdate = NOW() WHERE NEW.acc_id = udt_accid;
	
    IF acc_debug = TRUE THEN
		IF OLD.acc_id <> NEW.acc_id 
		OR OLD.acc_id IS NULL AND NEW.acc_id IS NOT NULL 
		OR OLD.acc_id IS NOT NULL AND NEW.acc_id IS NULL THEN
			INSERT INTO updates VALUES ((SELECT acc_login FROM accounts LIMIT 1), NOW(), 'accounts', 'acc_id', OLD.acc_id, NEW.acc_id);
		END IF;
		IF OLD.acc_login <> NEW.acc_login 
		OR OLD.acc_login IS NULL AND NEW.acc_login IS NOT NULL 
		OR OLD.acc_login IS NOT NULL AND NEW.acc_login IS NULL THEN
			INSERT INTO updates VALUES ((SELECT acc_login FROM accounts LIMIT 1), NOW(), 'accounts', 'acc_login', OLD.acc_login, NEW.acc_login);
		END IF;
		IF OLD.acc_pass <> NEW.acc_pass 
		OR OLD.acc_pass IS NULL AND NEW.acc_pass IS NOT NULL 
		OR OLD.acc_pass IS NOT NULL AND NEW.acc_pass IS NULL THEN
			INSERT INTO updates VALUES ((SELECT acc_login FROM accounts LIMIT 1), NOW(), 'accounts', 'acc_pass', OLD.acc_pass, NEW.acc_pass);
		END IF;
		IF OLD.acc_email <> NEW.acc_email 
		OR OLD.acc_email IS NULL AND NEW.acc_email IS NOT NULL 
		OR OLD.acc_email IS NOT NULL AND NEW.acc_email IS NULL THEN
			INSERT INTO updates VALUES ((SELECT acc_login FROM accounts LIMIT 1), NOW(), 'accounts', 'acc_email', OLD.acc_email, NEW.acc_email);
		END IF;
		IF OLD.acc_active <> NEW.acc_active 
		OR OLD.acc_active IS NULL AND NEW.acc_active IS NOT NULL 
		OR OLD.acc_active IS NOT NULL AND NEW.acc_active IS NULL THEN
			INSERT INTO updates VALUES ((SELECT acc_login FROM accounts LIMIT 1), NOW(), 'accounts', 'acc_active', OLD.acc_active, NEW.acc_active);
		END IF;
		IF OLD.acc_create <> NEW.acc_create 
		OR OLD.acc_create IS NULL AND NEW.acc_create IS NOT NULL 
		OR OLD.acc_create IS NOT NULL AND NEW.acc_create IS NULL THEN
			INSERT INTO updates VALUES ((SELECT acc_login FROM accounts LIMIT 1), NOW(), 'accounts', 'acc_create', OLD.acc_create, NEW.acc_create);
		END IF;
	END IF;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `accounts_AFTER_DELETE` AFTER DELETE ON `arno_accounts` FOR EACH ROW BEGIN
	DECLARE acc_size INT;
	DECLARE acc_debug BIT(1);
    SET acc_debug = (SELECT set_value FROM ait.binary_settings WHERE set_name = 'SQL_DEBUG_MODE');
    
    IF acc_debug = TRUE THEN
		SET acc_size = (SELECT COUNT(*) FROM ait.accounts);
		
		-- IF acc_size = 0 THEN
		-- 	 ALTER TABLE ait.accounts AUTO_INCREMENT = 1;
		-- END IF;
    END IF;
    
	UPDATE usersdata SET udt_accid = NULL WHERE old.acc_id = udt_accid;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `arno_files`
--

DROP TABLE IF EXISTS `arno_files`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `arno_files` (
  `fls_id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `fls_accid` int(11) unsigned NOT NULL,
  `fls_name` varchar(45) NOT NULL,
  `fls_filecontent` longblob NOT NULL,
  `fls_create` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `fls_lastupdate` datetime DEFAULT NULL,
  PRIMARY KEY (`fls_id`),
  KEY `fls_acc_fk_idx` (`fls_accid`),
  CONSTRAINT `fls_acc_fk` FOREIGN KEY (`fls_accid`) REFERENCES `arno_accounts` (`acc_id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `arno_files`
--

LOCK TABLES `arno_files` WRITE;
/*!40000 ALTER TABLE `arno_files` DISABLE KEYS */;
/*!40000 ALTER TABLE `arno_files` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `files_BEFORE_UPDATE` BEFORE UPDATE ON `arno_files` FOR EACH ROW BEGIN
	SET NEW.fls_lastupdate = NOW();
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `arno_usersdata`
--

DROP TABLE IF EXISTS `arno_usersdata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `arno_usersdata` (
  `udt_id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `udt_accid` int(11) unsigned DEFAULT NULL,
  `udt_nick` varchar(255) NOT NULL,
  `udt_firstname` varchar(45) DEFAULT NULL,
  `udt_middlename` varchar(45) DEFAULT NULL,
  `udt_lastname` varchar(45) DEFAULT NULL,
  `udt_birthdate` datetime DEFAULT NULL,
  `udt_lastupdate` datetime DEFAULT NULL,
  UNIQUE KEY `udt_id_UNIQUE` (`udt_id`),
  UNIQUE KEY `udt_accid_UNIQUE` (`udt_accid`),
  CONSTRAINT `udt_acc_fk` FOREIGN KEY (`udt_accid`) REFERENCES `arno_accounts` (`acc_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `arno_usersdata`
--

LOCK TABLES `arno_usersdata` WRITE;
/*!40000 ALTER TABLE `arno_usersdata` DISABLE KEYS */;
INSERT INTO `arno_usersdata` VALUES (1,1,'admin',NULL,NULL,NULL,NULL,'2019-06-28 14:18:14');
/*!40000 ALTER TABLE `arno_usersdata` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `usersdata_BEFORE_UPDATE` BEFORE UPDATE ON `arno_usersdata` FOR EACH ROW BEGIN
	SET NEW.udt_lastupdate = NOW();
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `arno_usershosts`
--

DROP TABLE IF EXISTS `arno_usershosts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `arno_usershosts` (
  `uhs_id` int(11) NOT NULL AUTO_INCREMENT,
  `uhs_accid` int(11) NOT NULL,
  `uhs_hostname` varchar(45) NOT NULL,
  `uhs_active` bit(1) DEFAULT b'0',
  `uhs_actualloggedin` bit(1) DEFAULT b'0',
  `uhs_create` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`uhs_id`),
  UNIQUE KEY `uhs_id_UNIQUE` (`uhs_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `arno_usershosts`
--

LOCK TABLES `arno_usershosts` WRITE;
/*!40000 ALTER TABLE `arno_usershosts` DISABLE KEYS */;
INSERT INTO `arno_usershosts` VALUES (1,1,'PCPPUDWEL',_binary '',_binary '',NULL);
/*!40000 ALTER TABLE `arno_usershosts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sys_identificators`
--

DROP TABLE IF EXISTS `sys_identificators`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `sys_identificators` (
  `idn_id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `idn_value` varchar(45) NOT NULL,
  `idn_create` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`idn_id`),
  UNIQUE KEY `idn_id_UNIQUE` (`idn_id`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_identificators`
--

LOCK TABLES `sys_identificators` WRITE;
/*!40000 ALTER TABLE `sys_identificators` DISABLE KEYS */;
INSERT INTO `sys_identificators` VALUES (1,'test','2019-07-10 11:59:51'),(3,'ACK-AIT','2019-07-10 12:03:11'),(4,'CR-AIT','2019-07-11 09:49:22'),(5,'CR-AIT','2019-07-11 10:02:18'),(13,'REQ-AIT','2019-07-11 10:05:06'),(14,'REQ-AIT','2019-07-11 10:05:27'),(15,'REQ-AIT','2019-07-11 10:06:58'),(16,'REQ-AIT-3','2019-07-11 11:43:30'),(17,'REQ-AIT','2019-07-11 11:47:03'),(18,'CR-AIT','2019-07-11 11:48:05'),(19,'CR-AIT','2019-07-12 10:33:53'),(20,'ISS-AIT','2019-07-15 11:15:52'),(21,'CR-AIT','2019-07-15 12:26:12');
/*!40000 ALTER TABLE `sys_identificators` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sys_settings`
--

DROP TABLE IF EXISTS `sys_settings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `sys_settings` (
  `set_id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `set_name` varchar(45) NOT NULL,
  `set_value` varchar(45) DEFAULT NULL,
  `set_lastupdate` datetime DEFAULT NULL,
  PRIMARY KEY (`set_id`),
  UNIQUE KEY `idnew_table_UNIQUE` (`set_id`),
  UNIQUE KEY `set_name_UNIQUE` (`set_name`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_settings`
--

LOCK TABLES `sys_settings` WRITE;
/*!40000 ALTER TABLE `sys_settings` DISABLE KEYS */;
INSERT INTO `sys_settings` VALUES (1,'SQL_DEBUG_MODE','0',NULL),(2,'CR-AIT','10','2019-07-15 12:26:12'),(3,'ISS-AIT','5','2019-07-15 11:15:52'),(4,'ACK-AIT','0',NULL),(5,'REQ-AIT','3','2019-07-11 11:47:03'),(6,'AIT_MAIL_ADDRESS','ait.wms.nano@gmail.com',NULL),(7,'AIT_MAIL_PASSWORD','NIJdkE+HyTr75zeR9vepuw==',NULL);
/*!40000 ALTER TABLE `sys_settings` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `settings_AFTER_INSERT` AFTER INSERT ON `sys_settings` FOR EACH ROW BEGIN
	IF NEW.set_name LIKE '%-%' THEN
		INSERT INTO `ait`.`identificators` (idn_value) VALUES (NEW.set_name);
    END IF;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `settings_BEFORE_UPDATE` BEFORE UPDATE ON `sys_settings` FOR EACH ROW BEGIN
	SET NEW.set_lastupdate = NOW();
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `settings_AFTER_UPDATE` AFTER UPDATE ON `sys_settings` FOR EACH ROW BEGIN
	IF NEW.set_name LIKE '%-%' THEN
		INSERT INTO `ait`.`identificators` (idn_value) VALUES (NEW.set_name);
    END IF;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `sys_tasks`
--

DROP TABLE IF EXISTS `sys_tasks`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `sys_tasks` (
  `tks_id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `tks_taskcode` varchar(45) NOT NULL,
  `tks_tasklive` datetime DEFAULT NULL,
  `tks_create` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`tks_id`),
  UNIQUE KEY `tks_id_UNIQUE` (`tks_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_tasks`
--

LOCK TABLES `sys_tasks` WRITE;
/*!40000 ALTER TABLE `sys_tasks` DISABLE KEYS */;
/*!40000 ALTER TABLE `sys_tasks` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sys_updates`
--

DROP TABLE IF EXISTS `sys_updates`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `sys_updates` (
  `upd_id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `upd_by` varchar(45) NOT NULL,
  `upd_at` datetime NOT NULL,
  `upd_table` varchar(45) NOT NULL,
  `upd_column` varchar(100) NOT NULL,
  `upd_oldval` varchar(100) DEFAULT NULL,
  `upd_newval` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`upd_id`),
  UNIQUE KEY `upd_id_UNIQUE` (`upd_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_updates`
--

LOCK TABLES `sys_updates` WRITE;
/*!40000 ALTER TABLE `sys_updates` DISABLE KEYS */;
/*!40000 ALTER TABLE `sys_updates` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'ait'
--

--
-- Dumping routines for database 'ait'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-07-15 13:02:47
