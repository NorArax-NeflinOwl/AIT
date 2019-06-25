package com.arno.hibernate;

import org.hibernate.SessionFactory;
import org.hibernate.boot.registry.StandardServiceRegistryBuilder;
import org.hibernate.cfg.Configuration;
import org.hibernate.service.ServiceRegistry;

import java.util.logging.Level;
import java.util.logging.Logger;

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
                Logger.getLogger("org.hibernate").setLevel(Level.OFF);
                Configuration configuration = new Configuration().configure("hibernate.cfg.xml");

                ServiceRegistry serviceRegistry = new StandardServiceRegistryBuilder()
                        .applySettings(configuration.getProperties()).build();

                sessionFactory = configuration.buildSessionFactory(serviceRegistry);
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
        return sessionFactory;
    }
}