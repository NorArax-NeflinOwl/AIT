package com.gui;

import com.gui.generic.GenericController;
import javafx.fxml.FXML;

import java.net.URL;
import java.util.ResourceBundle;

public class DashboardController extends GenericController<DashboardController, Integer>{

    @FXML
    public void initialize(URL url, ResourceBundle resourceBundle) {
        logger.info("opening: DashboardController.initialize()");


        logger.info("exiting: DashboardController.initialize()");
    }
}
