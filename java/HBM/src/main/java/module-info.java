module HBM {
    requires java.naming;
    requires java.persistence;
    requires java.logging;
    requires java.sql;

    requires jdk.xml.dom;
    requires net.bytebuddy;
    requires com.sun.xml.bind;
    requires org.hibernate.orm.core;
    requires mysql.connector.java;
    requires log4j;
    requires PTL;

    opens com.hbm.entities to org.hibernate.orm.core;

    exports com.hbm;
    exports com.hbm.daos;
    exports com.hbm.models;
    exports com.hbm.daos.models;
    exports com.hbm.hibernate;
    exports com.hbm.entities;
}