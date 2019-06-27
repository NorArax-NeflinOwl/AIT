module HBM {
    requires java.naming;
    requires java.persistence;
    requires jdk.xml.dom;
    requires org.hibernate.orm.core;
    requires mysql.connector.java;

    exports com.hbm.hibernate;
}