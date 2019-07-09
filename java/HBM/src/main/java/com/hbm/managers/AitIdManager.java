package com.hbm.managers;

import com.hbm.daos.AitDAOFactory;
import com.hbm.daos.models.AitSettingsDAO;
import com.hbm.hibernate.AitHibernateUtil;
import com.hbm.models.AitSetting;
import com.hbm.models.entities.AitSettingEntity;
import com.ptl.managers.AitLogger;
import com.ptl.resources.AitLoggerPriority;
import com.ptl.resources.Inerfix;
import com.ptl.resources.Postfix;
import com.ptl.resources.Prefix;
import org.hibernate.Session;

public class AitIdManager {
    private AitIdManager(){}
    private static Session sessionObj;
    private static AitIdManager instance = new AitIdManager();

    public static AitIdManager getInstance() {
        return instance;
    }

    /// Generator
    public String generateId(Prefix prefix) {
        return generateId(prefix, Inerfix.AIT);
    }
    public String generateId(Prefix prefix, Postfix postfix) {
        return generateId(prefix, Inerfix.AIT, postfix);
    }

    public String generateId(Prefix prefix, Inerfix inerfix) {
        return generateId(prefix, inerfix, Postfix.IT);
    }

    public String generateId(Prefix prefix, Inerfix inerfix, Postfix postfix) {
        int i = 0;
        char separator = '-';
        String key = prefix.toString() + separator + inerfix.toString();

        try {
            getSession(true).beginTransaction();
            AitSettingsDAO dao = new AitDAOFactory(getSession(true)).getSettingsDAO();
            AitSetting set = dao.findSettingByName(key);

            if(set != null) {
                i = set.getValue() + 1;

            } else {
                AitSettingEntity entity = new AitSettingEntity();
                entity.setName(key);
                set = new AitSetting(getSession(true), entity);
            }
            set.setValue(i);
            set.saveOrUpdate();
            getSession(true).getTransaction().commit();

        } catch (Exception ex) {
            AitLogger.getInstance().logToConsole(ex.toString(), AitLoggerPriority.ERROR);
            if(null != getSession(false).getTransaction()) {
                AitLogger.getInstance().logToConsole(".......Transaction Is Being Rolled Back.......");
                getSession(false).getTransaction().rollback();
            }
        }

        return key + separator + i + postfix.toString();
    }

    private static Session getSession(boolean createIfNotExists) {
        if(sessionObj == null && createIfNotExists || sessionObj != null && !sessionObj.isOpen()) {
            sessionObj = AitHibernateUtil.getInstance().getSessionFactory().openSession();
        }
        return sessionObj;
    }
}
