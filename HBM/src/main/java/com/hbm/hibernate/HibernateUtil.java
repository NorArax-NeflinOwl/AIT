package com.hbm.hibernate;

import org.hibernate.SessionFactory;
import org.hibernate.cfg.Configuration;

import java.util.logging.Level;

public class HibernateUtil {

    private static HibernateUtil ourInstance = new HibernateUtil();

    public static HibernateUtil getInstance() {
        return ourInstance;
    }

    private HibernateUtil() { }

    private SessionFactory sessionFactory;
    public SessionFactory getSessionFactory() {
        if (sessionFactory == null) {
            try {
                java.util.logging.Logger.getLogger("org.hibernate").setLevel(Level.OFF);

                sessionFactory = new Configuration().configure("hibernate.cfg.xml").buildSessionFactory();
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
        return sessionFactory;
    }
}