package managers;

import resources.AitConsoleColor;
import resources.AitLoggerPriority;

import java.io.*;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.concurrent.locks.ReentrantLock;

public class AitLogger {
    private static ReentrantLock m_Locker = new ReentrantLock();
    private static AitLogger m_Instance = new AitLogger();

    private String m_MainPath;

    private AitLogger() {
        m_MainPath = new File("").getAbsolutePath();
    }

    public static AitLogger getInstance() {
        m_Locker.lock();
        try {
            return m_Instance;
        } finally {
            m_Locker.unlock();
        }
    }

    public void logToConsole(Object[] params) {
        logToConsole(params, AitLoggerPriority.Information);
    }

    public void logToConsole(Object[] params, AitLoggerPriority priority)  {
        for (Object arg : params) {
            String color = convertPriorityToColor(priority);
            System.out.println(String.format(color, convertDateTimeToString() + "[LOG MSG]: " + arg.toString()));
        }
    }

    public void logToFile(Object[] params, String title) throws Exception {
        logToFile(params, title, AitLoggerPriority.Error);
    }

    private void logToFile(Object[] params, String title, AitLoggerPriority priority) throws Exception  {
        createLogDir();

        StringBuilder builder = new StringBuilder();

        File logFile = createLogFile();
        FileReader fr = new FileReader(logFile.getAbsoluteFile());
        BufferedReader br = new BufferedReader(fr);
        String line;

        do {
            line = br.readLine();
            if(line != null) {
                builder.append(line);
                builder.append(System.getProperty("line.separator"));
            }
        } while(line != null);

        builder.append(String.format(convertDateTimeToString() + convertPiorityToString(priority), title));
        builder.append(System.getProperty("line.separator"));

        for (Object arg : params) {
            String msg = arg.toString();
            builder.append(msg);
            builder.append(System.getProperty("line.separator"));
        }
        builder.append(System.getProperty("line.separator"));
        builder.append(System.getProperty("line.separator"));

        FileWriter fw = new FileWriter(logFile.getAbsoluteFile());
        BufferedWriter bw = new BufferedWriter(fw);
        bw.write(builder.toString());
        bw.close();
    }

    private String convertDateTimeToString(){
        return new SimpleDateFormat("[hh:mm:ss]").format(Calendar.getInstance().getTime());
    }

    private String convertPriorityToColor(AitLoggerPriority priority) {
        switch (priority) {
            case Error:
                return AitConsoleColor.ANSI_RED_LINE;
            case Warting:
                return AitConsoleColor.ANSI_PURPLE_LINE;
            case Information:
                return AitConsoleColor.ANSI_GREEN_LINE;
            default:
                return AitConsoleColor.ANSI_WHITE_LINE;
        }
    }

    private String convertPiorityToString(AitLoggerPriority priority) {
        switch (priority) {
            case Error:
                return "[ERR]: %s";
            case Warting:
                return "[WAR]: %s";
            case Information:
                return "[INF]: %s";
            default:
                return "%s";
        }
    }

    private void createLogDir() {
        String logDirPath = m_MainPath + "\\Log";
        File logDir = new File(logDirPath);

        if(!logDir.exists()) {
            if(logDir.mkdir()) {
                AitLogger.getInstance().logToConsole(new Object[]{"Create Log directory"}, AitLoggerPriority.Information);
            }
        }
        else {
            AitLogger.getInstance().logToConsole(new Object[]{"Log directory exists"}, AitLoggerPriority.Information);
        }
    }

    private File createLogFile() throws Exception {
        String timeStamp = new SimpleDateFormat("yyMMdd").format(Calendar.getInstance().getTime());
        String m_LogFileExt = ".log";
        String logFilePath = m_MainPath + "\\Log\\" + timeStamp + m_LogFileExt;
        File logFile = new File(logFilePath);
        if (!logFile.exists()) {
            if(logFile.createNewFile()) {
                AitLogger.getInstance().logToConsole(new Object[]{"Create Log file: " + timeStamp}, AitLoggerPriority.Information);
            }
        }
        else {
            AitLogger.getInstance().logToConsole(new Object[]{"Log file exists: " + timeStamp}, AitLoggerPriority.Information);
        }

        return logFile;
    }
}
