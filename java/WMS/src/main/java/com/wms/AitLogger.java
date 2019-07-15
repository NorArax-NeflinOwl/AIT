package com.wms;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileReader;
import java.io.FileWriter;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.concurrent.locks.ReentrantLock;

public class AitLogger {
    private static ReentrantLock m_Locker = new ReentrantLock();
    private static AitLogger m_Instance = new AitLogger();

    private String m_MainPath;

    private AitLogger() {
        m_MainPath = new File("").getAbsolutePath() + "\\_log_files";
    }

    public static AitLogger getInstance() {
        m_Locker.lock();
        try {
            return m_Instance;
        } finally {
            m_Locker.unlock();
        }
    }

    void logErrorToFile(Exception exception) {
        try {
            logToFile(exception.getStackTrace(), exception.toString());
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    private void logToFile(Object[] params, String title) throws Exception  {
        String dicFolderName, fileExt;
        dicFolderName = "errors";
        fileExt = ".err";
        createDir(dicFolderName);
        File file = createFile(dicFolderName, fileExt);

        FileReader fr = new FileReader(file.getAbsoluteFile());
        BufferedReader br = new BufferedReader(fr);
        StringBuilder builder = new StringBuilder();
        String line;

        do {
            line = br.readLine();
            if(line != null) {
                builder.append(line);
                builder.append(System.getProperty("line.separator"));
            }
        } while(line != null);

        builder.append(String.format(convertDateTimeToString() + "[ERR]: %s", title));
        builder.append(System.getProperty("line.separator"));

        if(params != null && params.length > 0) {
            for (Object arg : params) {
                String msg = arg.toString();
                builder.append(msg);
                builder.append(System.getProperty("line.separator"));
            }
            builder.append(System.getProperty("line.separator"));
        }

        FileWriter fw = new FileWriter(file.getAbsoluteFile());
        BufferedWriter bw = new BufferedWriter(fw);
        bw.write(builder.toString());
        bw.close();
    }

    private String convertDateTimeToString(){
        return new SimpleDateFormat("[hh:mm:ss]").format(Calendar.getInstance().getTime());
    }

    private void createDir(String dicName) {
        String logDirPath = m_MainPath;
        File logDir = new File(logDirPath);
        try {
            Files.setAttribute(Paths.get(logDir.getPath()), "dos:hidden", true);
        } catch (Exception e) {
            e.printStackTrace();
        }

        if(!logDir.exists() && logDir.mkdir()) {
            System.out.println("Create main log directory");
        }

        logDirPath = m_MainPath + "\\" + dicName;
        logDir = new File(logDirPath);

        if(!logDir.exists() && logDir.mkdir()) {
            System.out.println("Create '\" + dicName + \"' directory");
        }
    }

    private File createFile(String dicName, String fileExt) throws Exception {
        String timeStamp = new SimpleDateFormat("yyMMdd").format(Calendar.getInstance().getTime());
        String logFilePath = m_MainPath + "\\" + dicName + "\\" + timeStamp + fileExt;
        File logFile = new File(logFilePath);
        if (!logFile.exists() && logFile.createNewFile()) {
            System.out.println("Create in \\" + dicName + "\\ file: " + timeStamp + fileExt);
        }

        return logFile;
    }
}
