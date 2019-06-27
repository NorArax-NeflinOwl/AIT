module HBM {
    requires java.naming;
    requires java.persistence;
    requires jdk.xml.dom;
    requires org.hibernate.orm.core;
    requires mysql.connector.java;

    opens com.hbm to org.hibernate.orm.core;
    exports com.hbm;
}