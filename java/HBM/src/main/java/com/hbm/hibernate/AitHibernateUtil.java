package com.hbm.hibernate;

import org.apache.log4j.Logger;
import org.hibernate.SessionFactory;
import org.hibernate.cfg.Configuration;
import org.hibernate.cfg.Environment;

import java.net.InetAddress;
import java.net.UnknownHostException;
import java.util.AbstractMap;
import java.util.Map;

public class AitHibernateUtil {

    private static Logger logger = Logger.getLogger(AitHibernateUtil.class);
    private Map.Entry<String,String> account;
    private SessionFactory sessionFactory;

    private static AitHibernateUtil ourInstance = new AitHibernateUtil();

    private AitHibernateUtil() {
        logger.info("opening: AitHibernateUtil.AitHibernateUtil()");
        try {
            String workStation = "pcppudwel";
            InetAddress addr = InetAddress.getLocalHost();
            String hostname = addr.getHostName();

            account = (workStation.equals(hostname.toLowerCase())) ?
                    new AbstractMap.SimpleEntry<>("root", "#_@rnn0I")
                    : new AbstractMap.SimpleEntry<>("admin", "admin");

        } catch (UnknownHostException e) {
            logger.error("AitHibernateUtil.AitHibernateUtil()", e);
        }
        logger.info("exiting: AitHibernateUtil.AitHibernateUtil()");
    }

    public static AitHibernateUtil getInstance() {
        return ourInstance;
    }

    public SessionFactory getSessionFactory() {
        logger.info("opening: AitHibernateUtil.getSessionFactory()");
        if (sessionFactory == null) {
            try {
                Configuration config = new Configuration();
                config.configure("hibernate.cfg.xml");

                config.setProperty(Environment.USER, account.getKey());
                config.setProperty(Environment.PASS, account.getValue());

                sessionFactory = config.buildSessionFactory();
            } catch (Exception e) {
                logger.error("AitHibernateUtil.getSessionFactory()", e);
            }
        }
        logger.info("exiting: AitHibernateUtil.getSessionFactory()");
        return sessionFactory;
    }
}