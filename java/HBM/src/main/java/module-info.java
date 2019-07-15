module HBM {
    requires com.sun.xml.bind;          // org.hibernate.internal.util.config.ConfigurationException
    requires net.bytebuddy;             // java.lang.NoClassDefFoundError
    requires java.sql;                  // java.lang.NoClassDefFoundError

    requires gson;
    requires maven.model;
    requires java.persistence;
    requires java.naming;
    requires org.jsoup;
    requires org.hibernate.orm.core;

    opens com.hbm.models.entities to org.hibernate.orm.core;

    exports com.hbm;
    exports com.hbm.daos;
    exports com.hbm.models.entitiecovers;
    exports com.hbm.daos.models;
    exports com.hbm.hibernate;
    exports com.hbm.models.entities;
    exports com.hbm.managers;
    exports com.hbm.resources;
}