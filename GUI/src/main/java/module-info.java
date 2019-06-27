module GUI {
    requires javafx.controls;
    requires javafx.fxml;

    requires java.naming;
    requires log4j;
    requires HBM;

    opens com.gui to javafx.fxml;
    exports com.gui;
}