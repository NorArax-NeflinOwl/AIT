package database;

import managers.AitCrypter;
import managers.AitLogger;
import resources.AitLoggerPriority;
import structures.AitClientData;
import structures.AitMap;
import com.google.gson.Gson;

import java.io.*;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.concurrent.locks.ReentrantLock;

public class AitLocalDB {
    private static ReentrantLock m_Locker = new ReentrantLock();
    private static AitLocalDB m_Instance;

    private String dbFileExt = ".db";
    private String dbFile = "\\gui.database\\gui.database";
    private String mainPath;

    private AitMap clients;

    private AitLocalDB() throws Exception {
        mainPath = new File("").getAbsolutePath();
        clients = new AitMap();
        init();
    }

    public static AitLocalDB getInstance() {
        m_Locker.lock();
        try {
            if(m_Instance == null)
                m_Instance = new AitLocalDB();
            return m_Instance;
        } catch (Exception e) {
            try {
                AitLogger.getInstance().logToFile(e.getStackTrace(), e.toString());
            } catch (Exception exc) {
                e.printStackTrace();
                exc.printStackTrace();
            }
            return null;
        } finally {
            m_Locker.unlock();
        }
    }

    public boolean canRegister(String login) {
        return !clients.containsKey(login);
    }

    public void registerClient(AitClientData data) throws Exception {
        clients.put(data.getLogin(), data);
        saveDb();
    }

    public void deleteAccount(String login) throws Exception {
        clients.remove(login);
        saveDb();
    }

    public boolean validLogin(String login) {
        return clients.containsKey(login);
    }

    public boolean validPass(String login, String password) {
        return clients.get(login).getPassword().equals(password);
    }

    public void changeData(String login, AitClientData newData) throws Exception {
        //AitClientData odlData = clients.get(login);
        //odlData = newData;
        //saveDb()
        throw new Exception("Please implement method changeData for " + newData.getId() + " " + login);
    }

    private String loadDb() throws Exception {
        String dbFilePath = mainPath + dbFile + dbFileExt;
        File dbFile = new File(dbFilePath);

        FileReader fr = new FileReader(dbFile.getAbsoluteFile());
        BufferedReader br = new BufferedReader(fr);
        return  br.readLine();
    }

    private void saveDb() throws Exception {
        Gson gson = new Gson();
        String db = gson.toJson(clients);
        String cryptDb = AitCrypter.encrypt(db);
        saveDb(cryptDb);
    }

    private void saveDb(String param) throws Exception {
        createDbDir();
        File dbFile = createDbFile();

        FileWriter fw = new FileWriter(dbFile.getAbsoluteFile());
        BufferedWriter bw = new BufferedWriter(fw);
        bw.write(param);
        bw.close();
    }

    private void createDbDir() {
        String logDirPath = mainPath + "\\gui.database";
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

    private File createDbFile() throws Exception {
        String timeStamp = new SimpleDateFormat("yy_MM_dd").format(Calendar.getInstance().getTime());
        String dbFilePath = mainPath + dbFile + dbFileExt;
        File dbFile = new File(dbFilePath);
        if (!dbFile.exists()) {
            if(dbFile.createNewFile()) {
                AitLogger.getInstance().logToConsole(new Object[]{"Create gui.database file: " + timeStamp}, AitLoggerPriority.Information);
            }
        }
        else {
            AitLogger.getInstance().logToConsole(new Object[]{"Log gui.database exists: " + timeStamp}, AitLoggerPriority.Information);
        }

        return dbFile;
    }

    private boolean dbExist() {
        String dbFilePath = mainPath + dbFile + dbFileExt;
        File dbFile = new File(dbFilePath);
        return dbFile.exists();
    }

    private void init() throws Exception {
        if(dbExist()) {
            String cryptDb = loadDb();
            if(cryptDb != null && !cryptDb.isEmpty()){

                Gson gson = new Gson();
                clients = gson.fromJson(AitCrypter.decrypt(cryptDb), clients.getClass());
            }
        } else {
            createDbDir();
            createDbFile();
        }
    }
}
