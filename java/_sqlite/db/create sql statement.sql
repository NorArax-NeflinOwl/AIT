DROP TABLE IF EXISTS ait_accounts;
CREATE TABLE ait_accounts (
    acc_id TEXT PRIMARY KEY NOT NULL,
    acc_login VARCHAR(45) NOT NULL,
    acc_password VARCHAR(45) DEFAULT NULL,
    acc_email VARCHAR(45) DEFAULT NULL,
    acc_active INT(1) DEFAULT '0',
    acc_create DATETIME DEFAULT CURRENT_TIMESTAMP,
    acc_lastupdate DATETIME DEFAULT NULL
);

DROP TRIGGER TRG_ACC_AFT_UPD;
CREATE TRIGGER TRG_ACC_AFT_UPD AFTER UPDATE ON ait_accounts
BEGIN
   UPDATE ait_accounts SET acc_lastupdate = CURRENT_TIMESTAMP;
END;

INSERT INTO ait_accounts (acc_id, acc_login) VALUES ('AIT-ACC-0000000', 'admin');

UPDATE ait_accounts SET acc_email = 'ait.wms.nano@gmail.com' WHERE acc_id = 'AIT-ACC-0000000';
UPDATE ait_accounts SET acc_active = '1' WHERE acc_id = 'AIT-ACC-0000000';

SELECT * FROM ait_accounts;