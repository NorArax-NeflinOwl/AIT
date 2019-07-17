package com.hbm.contexts;

import com.hbm.managers.AitLogger;

import java.io.File;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;

public class AitDbContext {
    private static final String dbFileName = "nano.db";
    private static final String dbSqlUrl = new File("").getAbsolutePath() + "/_sqlite/";
    private static final String dbDirName = "db";
    private static final String url = "jdbc:sqlite:" + dbSqlUrl + "/" + dbDirName + "/";

    private Connection connection;
    private static AitDbContext instance = new AitDbContext();

    private AitDbContext() {
        createFolder();
    }

    public static AitDbContext getInstance() {
        return instance;
    }

    public Connection getConnection() {
        if(connection == null) {
            connection = connect();
        }
        return connection;
    }

    private void createFolder(){
        String logDirPath = dbSqlUrl;
        File logDir = new File(logDirPath);
        if(!logDir.exists() && logDir.mkdir()) {
            System.out.println("Create main log directory");
        }

        logDirPath = dbSqlUrl + dbDirName;
        logDir = new File(logDirPath);
        if(!logDir.exists() && logDir.mkdir()) {
            System.out.println("Create main log directory");
        }
    }

    private Connection connect() {
        Connection conn = null;
        try {
            conn = DriverManager.getConnection(url + dbFileName);
            System.out.println("Connection to SQLite has been established.");
        } catch (SQLException e) {
            AitLogger.getInstance().logErrorToFile(e);
        }
        return conn;
    }
}
