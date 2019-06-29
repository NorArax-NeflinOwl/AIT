package com.hbm.hibernate;

import com.hbm.daos.DAOFactory;
import org.hibernate.Session;

public class MainContext {
    private Session session;
    private DAOFactory daoFactory;

    public MainContext(Session session) {
        this.session = session;
        daoFactory = new DAOFactory(session);
    }

    public Session getSession() {
        return session;
    }

    public DAOFactory getDaoFactory() {
        return daoFactory;
    }

    public void setDaoFactory(DAOFactory daoFactory) {
        this.daoFactory = daoFactory;
    }
}
