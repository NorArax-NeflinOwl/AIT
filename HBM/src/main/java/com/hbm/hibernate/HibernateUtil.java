package com.hbm.hibernate;

import org.hibernate.SessionFactory;
import org.hibernate.cfg.Configuration;
import org.hibernate.cfg.Environment;

import java.net.InetAddress;
import java.net.UnknownHostException;
import java.util.AbstractMap;
import java.util.Map;
import java.util.logging.Level;

public class HibernateUtil {

    private Map.Entry<String,String> account;
    private SessionFactory sessionFactory;

    private static HibernateUtil ourInstance = new HibernateUtil();

    private HibernateUtil() {

        try {
            String workStation = "pcppudwel";
            InetAddress addr = InetAddress.getLocalHost();
            String hostname = addr.getHostName();

            account = (workStation.equals(hostname.toLowerCase())) ?
                    new AbstractMap.SimpleEntry<>("root", "#_@rnn0I")
                    : new AbstractMap.SimpleEntry<>("admin", "admin");

        } catch (UnknownHostException e) {
            e.printStackTrace();
        }
    }

    public static HibernateUtil getInstance() {
        return ourInstance;
    }

    public SessionFactory getSessionFactory() {
        if (sessionFactory == null) {
            try {
                java.util.logging.Logger.getLogger("org.hibernate").setLevel(Level.OFF);

                Configuration config = new Configuration();
                config.configure("hibernate.cfg.xml");

                config.setProperty(Environment.USER, account.getKey());
                config.setProperty(Environment.PASS, account.getValue());

                sessionFactory = config.buildSessionFactory();
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
        return sessionFactory;
    }
}