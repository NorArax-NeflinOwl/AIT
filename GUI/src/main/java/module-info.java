module com.gui {
    requires javafx.controls;
    requires javafx.fxml;

    requires java.logging;
    requires java.naming;

    opens com.gui to javafx.fxml;
    exports com.gui;
}