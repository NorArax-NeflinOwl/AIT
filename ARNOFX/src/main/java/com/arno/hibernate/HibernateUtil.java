package com.arno.hibernate;

import com.arno.entities.AccountEntity;
import com.arno.entities.UserDataEntity;
import org.hibernate.SessionFactory;
import org.hibernate.boot.registry.StandardServiceRegistryBuilder;
import org.hibernate.cfg.Configuration;
import org.hibernate.cfg.Environment;
import org.hibernate.service.ServiceRegistry;

import java.util.Properties;
import java.util.TimeZone;
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
                Class.forName("com.mysql.jdbc.Driver");
                Logger.getLogger("org.hibernate").setLevel(Level.OFF);

                Properties settings = new Properties();
                settings.put(Environment.URL, "jdbc:mysql://localhost:3306/ait?serverTimezone="
                        + TimeZone.getDefault().getID());
                settings.put(Environment.USER, "admin");
                settings.put(Environment.PASS, "admin");
                settings.put(Environment.HBM2DDL_AUTO, "update");
                settings.put(Environment.SHOW_SQL, "true");
                settings.put(Environment.DIALECT, "org.hibernate.dialect.MySQL5Dialect");

                Configuration configuration = new Configuration();
                configuration.setProperties(settings);

                configuration.addAnnotatedClass(AccountEntity.class);
                configuration.addAnnotatedClass(UserDataEntity.class);

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