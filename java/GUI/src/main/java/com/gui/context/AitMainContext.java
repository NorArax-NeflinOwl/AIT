package com.gui.context;

import com.gui.interfaces.AitGenericControllerInterface;
import com.gui.interfaces.AitNamespaceInterface;
import com.gui.namespace.AitArnoNamespace;
import com.gui.namespace.AitDashboardNamespace;
import com.gui.namespace.AitLoginNamespace;
import com.gui.namespace.AitRegistrationNamespace;
import com.gui.panels.AitArnoController;
import com.gui.panels.AitDashboardController;
import com.gui.panels.AitLoginController;
import com.gui.panels.AitRegistraionController;
import com.gui.strings.AitControllersNameConstStrings;
import com.hbm.daos.AitDAOFactory;
import com.hbm.hibernate.AitHibernateUtil;
import com.hbm.managers.AitLogger;
import com.hbm.models.entitiecovers.AitAccount;
import com.hbm.models.entitiecovers.AitUserHost;
import com.hbm.models.entities.AitUserHostEntity;
import javafx.util.Pair;
import org.hibernate.Session;

import java.net.InetAddress;
import java.net.UnknownHostException;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class AitMainContext {
    private static AitAccount user;
    private static Session sessionObj;
    private static Map<String, Pair<AitNamespaceInterface, AitGenericControllerInterface>> frames;

    public static void initFrames() {
        AitLogger.getInstance().logInfoToFile("opening: AitIdManager.initFrames()");
        frames = new HashMap<>();
        frames.put(AitControllersNameConstStrings.ARNO_NAMESPACE, new Pair<>(new AitArnoNamespace(), new AitArnoController()));
        frames.put(AitControllersNameConstStrings.LOGIN_NAMESPACE, new Pair<>(new AitLoginNamespace(), new AitLoginController()));
        frames.put(AitControllersNameConstStrings.REGISTRATION_NAMESPACE, new Pair<>(new AitRegistrationNamespace(), new AitRegistraionController()));
        frames.put(AitControllersNameConstStrings.DASHBOARD_NAMESPACE, new Pair<>(new AitDashboardNamespace(), new AitDashboardController()));
        AitLogger.getInstance().logInfoToFile("exiting: AitIdManager.initFrames()");
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

    public static void setAccount(AitAccount acc, boolean rememberMe) throws UnknownHostException {
        // TODO valide multiple account on one machine by new column (look getAccount())

        InetAddress addr = InetAddress.getLocalHost();
        String hostname = addr.getHostName();
        if(acc != null) {
            user = acc;
            List<AitUserHost> userHosts = user.getUserHosts();
            if(userHosts.isEmpty()) {
                try {
                    getSession(true).beginTransaction();

                    AitUserHostEntity entity = new AitUserHostEntity();
                    entity.setHostName(hostname);
                    entity.setActive(true);
                    entity.setActualLoggedIn(rememberMe);

                    AitUserHost userHost = new AitUserHost(getSession(true), entity);
                    userHost.setAccount(acc);

                    userHost.saveOrUpdate();
                } catch (Exception e) {
                    AitLogger.getInstance().logErrorToFile("error: AitMainContext.setAccount(AitAccount, boolean)", e);
                }
            } else {
                for (AitUserHost host : userHosts) {
                    if(host.getHostName().equals(hostname) && host.isActive()) {
                        host.setActualLoggedIn(rememberMe);
                        host.saveOrUpdate();
                    }
                }
            }
        } else {
            user = null;
            List<AitUserHost> userHosts = new AitDAOFactory(getSession(true)).getUserHostDAO().findUserHostsByHostName(hostname);
            if(!userHosts.isEmpty()) {
                for (AitUserHost host : userHosts) {
                    if(host.getHostName().equals(hostname) && host.isActive()) {
                        host.setActualLoggedIn(rememberMe);
                        host.saveOrUpdate();
                    }
                }
            }
        }
    }

    public static AitAccount getAccount() throws UnknownHostException {
        if(user == null) {
            InetAddress addr = InetAddress.getLocalHost();
            String hostname = addr.getHostName();
            List<AitUserHost> userHosts = new AitDAOFactory(getSession(true)).getUserHostDAO().findUserHostsByHostName(hostname);

            // If actual log in user is more that one in the same computer that must log out all that user
            if(userHosts.size() > 1) {
                // TODO add new column with error about multiple login in one machine name is the same in db more that ones
                userHosts.forEach(q -> {
                    if(q.isActive()) {
                        q.setActualLoggedIn(false);
                        q.saveOrUpdate();
                    }
                });
            }
            if(userHosts.size() == 1 && userHosts.get(0).isActive() && userHosts.get(0).isActualLoggedIn()) {
                user = userHosts.get(0).getAccount();
            }
        }
        return user;
    }
}
