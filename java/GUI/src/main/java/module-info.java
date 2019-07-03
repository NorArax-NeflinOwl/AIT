module GUI {
    requires javafx.controls;
    requires javafx.fxml;

    requires java.prefs;
    requires java.naming;
    requires org.hibernate.orm.core;
    requires HBM;
    requires PTL;
    requires log4j;
    requires gson;

    opens com.gui.panels to javafx.fxml;
    opens com.gui.models to gson;

    exports com.gui;
    exports com.gui.strings;
    exports com.gui.interfaces;
}