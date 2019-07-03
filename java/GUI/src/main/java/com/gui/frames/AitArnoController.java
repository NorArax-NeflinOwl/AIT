package com.gui.frames;

import com.gui.AppGUI;
import com.gui.abstracts.AitGenericController;
import com.gui.context.AitInitializer;
import com.gui.context.AitMainContext;
import com.gui.interfaces.AitGenericControllerInterface;
import com.gui.managers.AitCultureManager;
import com.gui.strings.AitControllersNameConstStrings;
import javafx.concurrent.Task;
import javafx.fxml.FXML;
import javafx.scene.control.Alert;
import javafx.scene.control.ButtonType;
import javafx.scene.control.Label;
import javafx.scene.control.ProgressBar;

import java.util.Random;

public class AitArnoController extends AitGenericController<AitArnoController, Integer> {

    @FXML
    public Label arnoFirstInfo;
    @FXML
    public Label arnoSecondInfo;
    @FXML
    public ProgressBar progressBar;

    @FXML
    public void initialize() {
        logger.info("opening: AitArnoController.initialize()");
        BindableTask task = new BindableTask(this);
        progressBar.progressProperty().bind(task.progressProperty());
        arnoFirstInfo.textProperty().bind(task.messageProperty());

        AitInitializer.getInstance().registerAppSettings();
        new Thread(task).start();

        try {
            arnoSecondInfo.setText(AitCultureManager.getInstance().getLanguage().getSecondInfoProgress());
        } catch (Exception e) {
            logger.error("error: AitArnoController.initialize()", e);
        }
        logger.info("exiting: AitArnoController.initialize()");
    }

    private class BindableTask extends Task<Integer> {

        private AitGenericControllerInterface controller;

        BindableTask(AitGenericControllerInterface controller) {
            this.controller = controller;
        }

        @Override
        protected Integer call() throws Exception {
            return updateProgress();
        }

        @Override
        protected void succeeded() {
            try {
                if (AitMainContext.getUser() != null) {
                    AppGUI.setRoot(AitControllersNameConstStrings.DASHBOARD_NAMESPACE, AitControllersNameConstStrings.ARNO_NAMESPACE, controller);
                } else {
                    AppGUI.setRoot(AitControllersNameConstStrings.LOGIN_NAMESPACE, AitControllersNameConstStrings.ARNO_NAMESPACE, controller);
                }
            } catch (SecurityException se) {
                logger.error("eror: ArnoControllerBandableTask.succeeded()", se);
                Alert alert = new Alert(Alert.AlertType.ERROR, "You dont have permition to use this app! Run app as administator!", ButtonType.OK);
                alert.show();
            } catch (Exception e) {
                e.printStackTrace();
            }
        }

        private Integer updateProgress() throws Exception {
            updateMessage(AitCultureManager.getInstance().getLanguage().getFirstInfoProgress());

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

            updateMessage(AitCultureManager.getInstance().getLanguage().getFinishInfoProgress());
            Thread.sleep(1000);

            updateProgress(10,10);
            Thread.sleep(500);
            return 10;
        }
    }
}
