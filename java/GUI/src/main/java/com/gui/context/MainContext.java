package com.gui.context;

import com.google.gson.Gson;
import com.gui.frames.ArnoController;
import com.gui.frames.DashboardController;
import com.gui.frames.LoginController;
import com.gui.frames.RegistraionController;
import com.gui.generic.IGenericController;
import com.gui.models.AccountSerializableModel;
import com.gui.namespace.*;
import com.hbm.daos.DAOFactory;
import com.hbm.datamodels.models.Account;
import com.hbm.datamodels.models.UserData;
import com.hbm.hibernate.HibernateUtil;
import javafx.util.Pair;
import org.apache.log4j.Logger;
import org.hibernate.Session;

import java.util.HashMap;
import java.util.Map;
import java.util.prefs.Preferences;

public class MainContext {
    private static Preferences settings = Preferences.systemNodeForPackage(MainContext.class);
    private static AccountSerializableModel user;
    private static Session sessionObj;
    private static Map<String, Pair<BaseNamespace, IGenericController>> frames;
    private static Logger logger = Logger.getLogger(MainContext.class);

    public static void initFrames() {
        logger.info("opening: MainContext.initFrames()");
        frames = new HashMap<>();
        frames.put(ControllersName.ARNO_NAMESPACE, new Pair<>(new ArnoNamespace(), new ArnoController()));
        frames.put(ControllersName.LOGIN_NAMESPACE, new Pair<>(new LoginNamespace(), new LoginController()));
        frames.put(ControllersName.REGISTRATION_NAMESPACE, new Pair<>(new RegistrationNamespace(), new RegistraionController()));
        frames.put(ControllersName.DASHBOARD_NAMESPACE, new Pair<>(new DashboardNamespace(), new DashboardController()));
        logger.info("exiting: MainContext.initFrames()");
    }

    public static BaseNamespace getNamespace(String key) {
        return frames != null && frames.containsKey(key) ? frames.get(key).getKey() : null;
    }

    public static IGenericController getController(String key) {
        return frames != null && frames.containsKey(key) ? frames.get(key).getValue() : null;
    }

    public static void setController(String key, IGenericController controller) {
        if(frames.containsKey(key)) {
            Pair<BaseNamespace, IGenericController> entry = frames.get(key);
            entry = new Pair<>(entry.getKey(), controller);
            frames.put(key, entry);
        }
    }

    public static Session getSession(boolean createIfNotExists) {
        if(sessionObj == null && createIfNotExists || sessionObj != null && !sessionObj.isOpen()) {
            sessionObj = HibernateUtil.getInstance().getSessionFactory().openSession();
        }
        return sessionObj;
    }

    public static void setUser(Account acc, boolean rememberMe) {
        UserData data = new DAOFactory(getSession(true)).getUserDataDAO().findUserDataByAccountId(acc.getID());
        String nick = data != null ? data.getNick() : DEFAULT_RM;
        user = new AccountSerializableModel(acc, nick);
        if(rememberMe) {
            Gson gson = new Gson();
            settings.put(REMEMBER_ME, gson.toJson(user));
        }
    }

    public static AccountSerializableModel getUser() {
        if(user == null) {
            String acc = settings.get(REMEMBER_ME, DEFAULT_RM);
            if(!acc.equals(DEFAULT_RM)) {
                Gson gson = new Gson();
                user = gson.fromJson(acc, AccountSerializableModel.class);
            }
        }
        return user;
    }

    private static String DEFAULT_RM = "";
    private static String REMEMBER_ME  = "REMEMBER_ME";
}
