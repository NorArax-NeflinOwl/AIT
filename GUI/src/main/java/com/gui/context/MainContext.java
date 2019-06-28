package com.gui.context;

import com.hbm.hibernate.HibernateUtil;
import org.hibernate.Session;

public class MainContext {
    private static Session sessionObj;

    public static Session getSession(boolean createIfNotExists) {
        if(sessionObj == null && createIfNotExists || sessionObj != null && !sessionObj.isOpen()) {
            sessionObj = HibernateUtil.getInstance().getSessionFactory().openSession();
        }
        return sessionObj;
    }
}
