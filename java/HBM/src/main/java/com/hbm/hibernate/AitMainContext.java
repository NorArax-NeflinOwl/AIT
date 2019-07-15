package com.hbm.hibernate;

import com.hbm.daos.AitDAOFactory;
import org.hibernate.Session;

public class AitMainContext {
    private Session session;
    private AitDAOFactory daoFactory;

    public AitMainContext(Session session) {
        this.session = session;
        daoFactory = new AitDAOFactory(session);
    }

    public Session getSession() {
        return session;
    }

    public AitDAOFactory getDaoFactory() {
        return daoFactory;
    }

    public void setDaoFactory(AitDAOFactory daoFactory) {
        this.daoFactory = daoFactory;
    }
}
