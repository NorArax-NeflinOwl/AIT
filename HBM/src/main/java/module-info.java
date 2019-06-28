module HBM {
    requires java.naming;
    requires java.persistence;
    requires java.logging;
    requires java.sql;
    requires java.xml.bind;

    requires jdk.xml.dom;
    requires net.bytebuddy;
    requires com.sun.xml.bind;
    requires org.hibernate.orm.core;
    requires mysql.connector.java;
    requires log4j;
    requires PTL;

    opens com.hbm.entities to org.hibernate.orm.core;
    exports com.hbm;
}