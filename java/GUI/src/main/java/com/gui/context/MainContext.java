package com.gui.context;

import com.gui.ArnoController;
import com.gui.DashboardController;
import com.gui.LoginController;
import com.gui.RegistraionController;
import com.gui.generic.IGenericController;
import com.gui.namespace.ArnoNamespace;
import com.gui.namespace.BaseNamespace;
import com.gui.namespace.ControllersName;
import com.gui.namespace.DashboardNamespace;
import com.gui.namespace.LoginNamespace;
import com.gui.namespace.RegistrationNamespace;
import com.hbm.datamodels.models.Account;
import com.hbm.hibernate.HibernateUtil;
import javafx.util.Pair;
import org.apache.log4j.Logger;
import org.hibernate.Session;

import java.util.HashMap;
import java.util.Map;

public class MainContext {
    private static Account user;
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

    public static void setUser(Account acc) {
            user = acc;
    }

    public static Account getUser() {
        return user;
    }
}
