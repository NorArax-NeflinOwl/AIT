module GUI {
    requires javafx.controls;
    requires javafx.fxml;

    requires java.naming;
    requires log4j;
    requires HBM;
    requires PTL;
    requires org.hibernate.orm.core;
    requires java.prefs;
    requires gson;

    opens com.gui.frames to javafx.fxml;
    opens com.gui.models to gson;
    exports com.gui.frames;
    exports com.gui;
}