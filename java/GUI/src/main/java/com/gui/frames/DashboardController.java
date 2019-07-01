package com.gui.frames;

import com.gui.context.MainContext;
import com.gui.generic.GenericController;
import javafx.fxml.FXML;

public class DashboardController extends GenericController<DashboardController, Integer>{

    @FXML
    public void initialize() {
        logger.info("opening: DashboardController.initialize()");
        MainContext.getController("");

        logger.info("exiting: DashboardController.initialize()");
    }
}
