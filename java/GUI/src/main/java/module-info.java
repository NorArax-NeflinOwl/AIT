module GUI {
    requires javafx.controls;
    requires javafx.fxml;

    requires java.prefs;
    requires java.naming;
    requires org.hibernate.orm.core;
    requires HBM;

    opens com.gui.panels to javafx.fxml;

    exports com.gui;
    exports com.gui.strings;
    exports com.gui.interfaces;
}