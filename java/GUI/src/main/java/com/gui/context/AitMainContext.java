package com.gui.context;

import com.google.gson.Gson;
import com.gui.frames.AitArnoController;
import com.gui.frames.AitDashboardController;
import com.gui.frames.AitLoginController;
import com.gui.frames.AitRegistraionController;
import com.gui.interfaces.AitGenericControllerInterface;
import com.gui.models.AitAccountSerializableModel;
import com.gui.namespace.AitArnoNamespace;
import com.gui.interfaces.AitNamespaceInterface;
import com.gui.strings.AitControllersNameConstStrings;
import com.gui.namespace.AitDashboardNamespace;
import com.gui.namespace.AitLoginNamespace;
import com.gui.namespace.AitRegistrationNamespace;
import com.hbm.daos.AitDAOFactory;
import com.hbm.models.AitAccount;
import com.hbm.models.AitUserData;
import com.hbm.hibernate.AitHibernateUtil;
import javafx.util.Pair;
import org.apache.log4j.Logger;
import org.hibernate.Session;

import java.util.HashMap;
import java.util.Map;
import java.util.prefs.Preferences;

public class AitMainContext {
    private static Preferences settings = Preferences.systemNodeForPackage(AitMainContext.class);
    private static AitAccountSerializableModel user;
    private static Session sessionObj;
    private static Map<String, Pair<AitNamespaceInterface, AitGenericControllerInterface>> frames;
    private static Logger logger = Logger.getLogger(AitMainContext.class);

    public static void initFrames() {
        logger.info("opening: AitMainContext.initFrames()");
        frames = new HashMap<>();
        frames.put(AitControllersNameConstStrings.ARNO_NAMESPACE, new Pair<>(new AitArnoNamespace(), new AitArnoController()));
        frames.put(AitControllersNameConstStrings.LOGIN_NAMESPACE, new Pair<>(new AitLoginNamespace(), new AitLoginController()));
        frames.put(AitControllersNameConstStrings.REGISTRATION_NAMESPACE, new Pair<>(new AitRegistrationNamespace(), new AitRegistraionController()));
        frames.put(AitControllersNameConstStrings.DASHBOARD_NAMESPACE, new Pair<>(new AitDashboardNamespace(), new AitDashboardController()));
        logger.info("exiting: AitMainContext.initFrames()");
    }

    public static AitNamespaceInterface getNamespace(String key) {
        return frames != null && frames.containsKey(key) ? frames.get(key).getKey() : null;
    }

    public static void setNamespaceTitle(String key, String title) {
        if(frames != null && frames.containsKey(key)) {
            frames.get(key).getKey().setTitle(title);
        }
    }

    public static AitGenericControllerInterface getController(String key) {
        return frames != null && frames.containsKey(key) ? frames.get(key).getValue() : null;
    }

    public static void setController(String key, AitGenericControllerInterface controller) {
        if(frames.containsKey(key)) {
            Pair<AitNamespaceInterface, AitGenericControllerInterface> entry = frames.get(key);
            entry = new Pair<>(entry.getKey(), controller);
            frames.put(key, entry);
        }
    }

    public static Session getSession(boolean createIfNotExists) {
        if(sessionObj == null && createIfNotExists || sessionObj != null && !sessionObj.isOpen()) {
            sessionObj = AitHibernateUtil.getInstance().getSessionFactory().openSession();
        }
        return sessionObj;
    }

    public static void setUser(AitAccount acc, boolean rememberMe) throws SecurityException {
        if(acc != null) {
            AitUserData data = new AitDAOFactory(getSession(true)).getUserDataDAO().findUserDataByAccountId(acc.getID());
            String nick = data != null ? data.getNick() : DEFAULT_RM;
            user = new AitAccountSerializableModel(acc, nick);
        } else {
            user = null;
        }
        if(rememberMe) {
            Gson gson = new Gson();
            settings.put(REMEMBER_ME, gson.toJson(user));
        } else {
            settings.put(REMEMBER_ME, DEFAULT_RM);
        }
    }

    public static AitAccountSerializableModel getUser() throws SecurityException {
        if(user == null) {
            // if WARNING then RUN PROJECT AS administrator;
            String acc = settings.get(REMEMBER_ME, DEFAULT_RM);
            if(!acc.equals(DEFAULT_RM)) {
                Gson gson = new Gson();
                user = gson.fromJson(acc, AitAccountSerializableModel.class);
            }
        }
        return user;
    }

    private static String DEFAULT_RM = "";
    private static String REMEMBER_ME  = "REMEMBER_ME";
}
