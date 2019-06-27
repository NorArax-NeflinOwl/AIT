module GUI {
    requires javafx.controls;
    requires javafx.fxml;

    requires java.naming;
    //requires HBM;

    opens com.gui to javafx.fxml;
    exports com.gui;
}