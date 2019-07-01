package com.gui;

import com.gui.context.Initializer;
import com.gui.cultureResources.CultureManager;
import com.gui.generic.GenericController;
import com.gui.generic.IGenericController;
import com.gui.namespace.ControllersName;
import javafx.concurrent.Task;
import javafx.fxml.FXML;
import javafx.scene.control.Label;
import javafx.scene.control.ProgressBar;

import java.util.Random;

public class ArnoController extends GenericController<ArnoController, Integer>{

    @FXML
    public Label arnoFirstInfo;
    @FXML
    public Label arnoSecondInfo;
    @FXML
    public ProgressBar progressBar;

    @FXML
    public void initialize() {
        logger.info("opening: ArnoController.initialize()");
        BindableTask task = new BindableTask(this);
        progressBar.progressProperty().bind(task.progressProperty());
        arnoFirstInfo.textProperty().bind(task.messageProperty());

        Initializer.getInstance().registerAppSettings();
        new Thread(task).start();

        try {
            arnoSecondInfo.setText(CultureManager.getInstance().getLanguage().getSecondInfoProgress());
        } catch (Exception e) {
            logger.error("error: ArnoController.initialize()", e);
        }
        logger.info("exiting: ArnoController.initialize()");
    }

    private class BindableTask extends Task<Integer> {

        private IGenericController controller;

        BindableTask(IGenericController controller) {
            this.controller = controller;
        }

        @Override
        protected Integer call() throws Exception {
            return updateProgress();
        }

        @Override
        protected void succeeded() {
            try {
                AppGUI.setRoot(ControllersName.LOGIN_NAMESPACE, this.controller);
            } catch (Exception e) {
                e.printStackTrace();
            }
        }

        private Integer updateProgress() throws Exception {
            updateMessage(CultureManager.getInstance().getLanguage().getFirstInfoProgress());

            Thread.sleep(100);
            int value = 0;
            for(int i = 0; i <= 10 || value < 7; i++) {
                value += new Random().nextInt(4);
                updateProgress(value, 10);
                Thread.sleep(200);
                if(isCancelled()) {
                    return i;
                }
            }

            updateMessage(CultureManager.getInstance().getLanguage().getFinishInfoProgress());
            Thread.sleep(1000);

            updateProgress(10,10);
            Thread.sleep(500);
            return 10;
        }
    }
}
