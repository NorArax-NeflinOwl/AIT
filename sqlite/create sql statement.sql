BEGIN TRANSACTION

--CREATE DATABASE IF NO EXISTS NANO;

DROP TABLE IF EXISTS sys_stsgenids;
CREATE TABLE sys_stsgenids (
    sgi_id VARCHAR(15) PRIMARY KEY NOT NULL,
    sgi_create DATETIME DEFAULT CURRENT_TIMESTAMP,
    sgi_delete DATETIME DEFAULT NULL
);

--######################################################################################################################

DROP TABLE IF EXISTS sys_permitiontypes;
CREATE TABLE sys_permitiontypes (
    pts_id VARCHAR(15) PRIMARY KEY NOT NULL,
    pts_key VARCHAR(10) UNIQUE NOT NULL,
    pts_description VARCHAR(2048) NOT NULL
);

DROP TRIGGER IF EXISTS TGR_PTS_BFR_INS;
CREATE TRIGGER TGR_PTS_BFR_INS BEFORE INSERT ON sys_permitiontypes
BEGIN
    INSERT INTO sys_stsgenids (sgi_id) VALUES (NEW.pts_id);
END;
DROP TRIGGER IF EXISTS TGR_PTS_AFT_DEL;
CREATE TRIGGER TGR_PTS_AFT_DEL AFTER DELETE ON sys_permitiontypes
BEGIN
    UPDATE sys_stsgenids SET sgi_delete = CURRENT_TIMESTAMP WHERE sgi_id = OLD.pts_id;
END;

--######################################################################################################################

DROP TABLE IF EXISTS sys_languagesresources;
CREATE TABLE sys_languagesresources (
    lgr_id VARCHAR(15) PRIMARY KEY NOT NULL,
    lgr_language VARCHAR(3) NOT NULL,
    lgr_key VARCHAR(45) NOT NULL,
    lgr_value VARCHAR(45) NOT NULL
);

DROP TRIGGER IF EXISTS TGR_LGR_BFR_INS;
CREATE TRIGGER TGR_LGR_BFR_INS BEFORE INSERT ON sys_languagesresources
BEGIN
    INSERT INTO sys_stsgenids (sgi_id) VALUES (NEW.lgr_id);
END;
DROP TRIGGER IF EXISTS TGR_LGR_AFT_DEL;
CREATE TRIGGER TGR_LGR_AFT_DEL AFTER DELETE ON sys_languagesresources
BEGIN
    UPDATE sys_stsgenids SET sgi_delete = CURRENT_TIMESTAMP WHERE sgi_id = OLD.lgr_id;
END;

--######################################################################################################################

DROP TABLE IF EXISTS ait_accounts;
CREATE TABLE ait_accounts (
    acc_id VARCHAR(15) PRIMARY KEY NOT NULL,            -- XXX-XXX-???????
    acc_login VARCHAR(20) NOT NULL,
    acc_password VARCHAR(64) DEFAULT NULL,              -- sha256
    acc_email VARCHAR(320) DEFAULT NULL,                -- {64}@{255}
    acc_active INT(1) DEFAULT '0',                      -- 0 - false; 1 - true
    acc_permition VARCHAR(10) NOT NULL,                 -- default is simple permition from sys_permitiontypes
    acc_create DATETIME DEFAULT CURRENT_TIMESTAMP,      -- datetime now
    acc_lastupdate DATETIME DEFAULT NULL
);

DROP TRIGGER IF EXISTS TGR_ACC_BFR_INS;
CREATE TRIGGER TGR_ACC_BFR_INS BEFORE INSERT ON ait_accounts
BEGIN
    INSERT INTO sys_stsgenids (sgi_id) VALUES (NEW.acc_id);
END;
DROP TRIGGER IF EXISTS TRG_ACC_AFT_UPD;
CREATE TRIGGER TRG_ACC_AFT_UPD AFTER UPDATE ON ait_accounts
BEGIN
   UPDATE ait_accounts SET acc_lastupdate = CURRENT_TIMESTAMP WHERE acc_id = NEW.acc_id;
END;
DROP TRIGGER IF EXISTS TGR_ACC_AFT_DEL;
CREATE TRIGGER TGR_ACC_AFT_DEL AFTER DELETE ON ait_accounts
BEGIN
    UPDATE sys_stsgenids SET sgi_delete = CURRENT_TIMESTAMP WHERE sgi_id = OLD.acc_id;
END;

--######################################################################################################################

DROP TABLE IF EXISTS ait_files;
CREATE TABLE ait_files (
    fls_id VARCHAR(15) PRIMARY KEY NOT NULL,
    fls_accid VARCHAR(15) NOT NULL,
    fls_asgaccid VARCHAR(15) DEFAULT NULL,
    fls_name VARCHAR(45) NOT NULL,
    fls_type VARCHAR(15) NOT NULL,                      -- file extension or if file is big then in here is part[x] x - number of package
    fls_content VARCHAR(2048) DEFAULT NULL,
    fls_create DATETIME DEFAULT CURRENT_TIMESTAMP,      -- datetime now
    fls_lastupdate DATETIME DEFAULT NULL,
    FOREIGN KEY(fls_accid) REFERENCES ait_accounts(acc_id),
    FOREIGN KEY(fls_asgaccid) REFERENCES ait_accounts(acc_id)
);

DROP TRIGGER IF EXISTS TGR_FLS_BFR_INS;
CREATE TRIGGER TGR_FLS_BFR_INS BEFORE INSERT ON ait_files
BEGIN
    INSERT INTO sys_stsgenids (sgi_id) VALUES (NEW.fls_id);
END;
DROP TRIGGER IF EXISTS TRG_FLS_BFR_UPD;
CREATE TRIGGER TRG_FLS_BFR_UPD BEFORE UPDATE ON ait_files
BEGIN
   UPDATE ait_files SET fls_lastupdate = CURRENT_TIMESTAMP WHERE fls_id = NEW.fls_id;
END;
DROP TRIGGER IF EXISTS TGR_FLS_AFT_DEL;
CREATE TRIGGER TGR_FLS_AFT_DEL AFTER DELETE ON ait_files
BEGIN
    UPDATE sys_stsgenids SET sgi_delete = CURRENT_TIMESTAMP WHERE sgi_id = OLD.fls_id;
END;

--######################################################################################################################

DROP TABLE IF EXISTS ait_usersdata;
CREATE TABLE ait_usersdata (
    usd_id VARCHAR(15) PRIMARY KEY NOT NULL,
    usd_accid VARCHAR(15) DEFAULT NULL,
    usd_nick VARCHAR(255) NOT NULL,
    usd_firstname VARCHAR(45) DEFAULT NULL,
    usd_middlename VARCHAR(45) DEFAULT NULL,
    usd_lastname VARCHAR(45) DEFAULT NULL,
    usd_birthday DATETIME DEFAULT NULL,
    usd_lastupdate DATETIME DEFAULT NULL,
    FOREIGN KEY(usd_accid) REFERENCES ait_accounts(acc_id)
);

DROP TRIGGER IF EXISTS TGR_USD_BFR_INS;
CREATE TRIGGER TGR_USD_BFR_INS BEFORE INSERT ON ait_usersdata
BEGIN
    INSERT INTO sys_stsgenids (sgi_id) VALUES (NEW.usd_id);
END;
DROP TRIGGER IF EXISTS TGR_USD_BFR_UPD;
CREATE TRIGGER TGR_USD_BFR_UPD BEFORE UPDATE ON ait_usersdata
BEGIN
    UPDATE ait_usersdata SET usd_lastupdate = CURRENT_TIMESTAMP WHERE usd_id = NEW.usd_id;
END;
DROP TRIGGER IF EXISTS TGR_USD_AFT_DEL;
CREATE TRIGGER TGR_USD_AFT_DEL AFTER DELETE ON ait_usersdata
BEGIN
    UPDATE sys_stsgenids SET sgi_delete = CURRENT_TIMESTAMP WHERE sgi_id = OLD.usd_id;
END;

--######################################################################################################################

DROP TABLE IF EXISTS ait_usershosts;
CREATE TABLE ait_usershosts (
    ush_id VARCHAR(15) PRIMARY KEY NOT NULL,
    ush_accid VARCHAR(15) NOT NULL,
    ush_hostname VARCHAR(63) NOT NULL,
    ush_active INT(1) DEFAULT '0',
    ush_loggedin INT(1) DEFAULT '0',
    ush_create DATETIME DEFAULT CURRENT_TIMESTAMP,
    ush_lastupdate DATETIME DEFAULT NULL,
    FOREIGN KEY(ush_accid) REFERENCES ait_accounts(acc_id)
);

DROP TRIGGER IF EXISTS TGR_USH_BFR_INS;
CREATE TRIGGER TGR_USH_BFR_INS BEFORE INSERT ON ait_usershosts
BEGIN
    INSERT INTO sys_stsgenids (sgi_id) VALUES (NEW.ush_id);
END;
DROP TRIGGER IF EXISTS TGR_USH_BRF_UPD;
CREATE TRIGGER TGR_USH_BRF_UPD BEFORE UPDATE ON ait_usershosts
BEGIN
    UPDATE ait_usershosts SET ush_lastupdate = CURRENT_TIMESTAMP WHERE ush_id = NEW.ush_id;
END;
DROP TRIGGER IF EXISTS TGR_USH_AFT_DEL;
CREATE TRIGGER TGR_USH_AFT_DEL AFTER DELETE ON ait_usershosts
BEGIN
    UPDATE sys_stsgenids SET sgi_delete = CURRENT_TIMESTAMP WHERE sgi_id = OLD.ush_id;
END;


--######################################################################################################################
--######################################################################################################################

INSERT INTO sys_permitiontypes (pts_id, pts_key, pts_description) VALUES ('AIT-PTS-0000000','ADMIN','User with this permition have full access to application
view all account, all tables, query history and more');
INSERT INTO sys_permitiontypes (pts_id, pts_key, pts_description) VALUES ('AIT-PTS-0000001','SIMPLE','User with this permition have access only to own date');
INSERT INTO sys_permitiontypes (pts_id, pts_key, pts_description) VALUES ('AIT-PTS-0000002','MANAGER','User with this permition have
accounts managment allowing to view all account, modifide another account (but not account with "ADMIN" permition)
meta data like change permition, block usershost etc and delete accounts');
INSERT INTO sys_permitiontypes (pts_id, pts_key, pts_description) VALUES ('AIT-PTS-0000003','BLOCKED','User with this permition have blocked access to aplication,
he can log in but only he can do is view dialog that he is block and mangment on meta data');
INSERT INTO sys_permitiontypes (pts_id, pts_key, pts_description) VALUES ('AIT-PTS-0000004','NONE','User with this permition have no access');

INSERT INTO ait_accounts (acc_id, acc_login, acc_permition) VALUES ('AIT-ACC-0000000', 'admin', (SELECT pts_key FROM sys_permitiontypes WHERE pts_id = 'AIT-PTS-0000000'));
UPDATE ait_accounts SET acc_email = 'ait.wms.nano@gmail.com' WHERE acc_id = 'AIT-ACC-0000000';
UPDATE ait_accounts SET acc_active = '1' WHERE acc_id = 'AIT-ACC-0000000';
--SELECT * FROM ait_accounts;

INSERT INTO ait_files (fls_id, fls_accid, fls_name, fls_type) VALUES ('AIT-FLS-0000000', 'AIT-ACC-0000000', 'admin', 'note');
--UPDATE ait_files SET fls_type = 'note' WHERE fls_id = 'AIT-FLS-0000002';
--SELECT * FROM ait_files;

INSERT INTO ait_usersdata (usd_id, usd_nick) VALUES ('AIT-USD-0000000', 'Administrator');
UPDATE ait_usersdata SET usd_accid = 'AIT-ACC-0000000' WHERE usd_id = 'AIT-USD-0000000';
UPDATE ait_usersdata SET usd_firstname = 'Patryk' WHERE usd_id = 'AIT-USD-0000000';
UPDATE ait_usersdata SET usd_middlename = 'Norbert' WHERE usd_id = 'AIT-USD-0000000';
UPDATE ait_usersdata SET usd_lastname = 'Pudwel' WHERE usd_id = 'AIT-USD-0000000';
--SELECT * FROM ait_usersdata;

INSERT INTO ait_usershosts (ush_id, ush_accid, ush_hostname) VALUES ('AIT-USH-0000000', 'AIT-ACC-0000000', 'pcppudwel');
UPDATE ait_usershosts SET ush_active = '1' WHERE ush_id = 'AIT-USH-0000000';
--SELECT * FROM ait_usershosts;

COMMIT;

SELECT * FROM sys_stsgenids ORDER BY sgi_id;

SELECT
    acc_id AS "Id",
    acc_login AS "Login",
    acc_password AS "Password",
    acc_email AS "E-Mail",
    acc_active AS "Acount is active",
    usd_nick AS "Nick",
    usd_firstname || ' ' || usd_middlename || ' ' || usd_lastname AS "User name",
    usd_birthday AS "B-Day",
    fls_asgaccid AS "File assigned to",
    fls_name AS "File name",
    fls_type AS "File type",
    fls_content AS "File content",
    ush_hostname AS "Host name",
    ush_active AS "Host is active",
    ush_loggedin AS "User is actual logged in on this host",
    acc_create AS "Create date",
    acc_lastupdate AS "Last update date"
FROM ait_accounts
LEFT JOIN ait_usersdata ON acc_id = usd_accid
LEFT JOIN ait_files ON acc_id = fls_accid
LEFT JOIN ait_usershosts ON acc_id = ush_accid;