package com.hbm.hibernate;

import org.apache.log4j.Logger;
import org.hibernate.SessionFactory;
import org.hibernate.cfg.Configuration;
import org.hibernate.cfg.Environment;

import java.net.InetAddress;
import java.net.UnknownHostException;
import java.util.AbstractMap;
import java.util.Map;

public class HibernateUtil {

    private static Logger logger = Logger.getLogger(HibernateUtil.class);
    private Map.Entry<String,String> account;
    private SessionFactory sessionFactory;

    private static HibernateUtil ourInstance = new HibernateUtil();

    private HibernateUtil() {
        logger.info("opening: HibernateUtil.HibernateUtil()");
        try {
            String workStation = "pcppudwel";
            InetAddress addr = InetAddress.getLocalHost();
            String hostname = addr.getHostName();

            account = (workStation.equals(hostname.toLowerCase())) ?
                    new AbstractMap.SimpleEntry<>("root", "#_@rnn0I")
                    : new AbstractMap.SimpleEntry<>("admin", "admin");

        } catch (UnknownHostException e) {
            logger.error("HibernateUtil.HibernateUtil()", e);
        }
        logger.info("exiting: HibernateUtil.HibernateUtil()");
    }

    public static HibernateUtil getInstance() {
        return ourInstance;
    }

    public SessionFactory getSessionFactory() {
        logger.info("opening: HibernateUtil.getSessionFactory()");
        if (sessionFactory == null) {
            try {
                Configuration config = new Configuration();
                config.configure("hibernate.cfg.xml");

                config.setProperty(Environment.USER, account.getKey());
                config.setProperty(Environment.PASS, account.getValue());

                sessionFactory = config.buildSessionFactory();
            } catch (Exception e) {
                logger.error("HibernateUtil.getSessionFactory()", e);
            }
        }
        logger.info("exiting: HibernateUtil.getSessionFactory()");
        return sessionFactory;
    }
}