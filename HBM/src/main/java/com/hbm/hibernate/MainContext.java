package com.hbm.hibernate;

import org.hibernate.Session;

public class MainContext {
    private Session session;

    public MainContext(Session session) {
        this.session = session;
    }

    public Session getSession() {
        return session;
    }
}
