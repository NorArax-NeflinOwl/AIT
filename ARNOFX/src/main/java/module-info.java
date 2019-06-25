module com.arno {
    requires javafx.controls;
    requires javafx.fxml;

    requires java.logging;
    requires java.persistence;
    requires java.naming;

    requires org.hibernate.orm.core;
    requires org.jboss.logging;

    requires mysql.connector.java;
    requires gson;

    opens com.arno to javafx.fxml;
    exports com.arno;
}