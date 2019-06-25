package com.arno;

import com.arno.cultureResources.CultureManager;
import com.arno.namespace.LoginNamespace;
import javafx.concurrent.Task;
import javafx.fxml.FXML;
import javafx.scene.control.Label;
import javafx.scene.control.ProgressBar;

import java.util.Random;

public class ArnoController {

    @FXML
    public Label arnoFirstInfo;
    @FXML
    public Label arnoSecondInfo;
    @FXML
    public ProgressBar progressBar;

    @FXML
    public void initialize() throws Exception {
        BindableTask task = new BindableTask();
        progressBar.progressProperty().bind(task.progressProperty());
        arnoFirstInfo.textProperty().bind(task.messageProperty());

        Initializer.getInstance().RegisterAppSettings();
        new Thread(task).start();

        arnoSecondInfo.setText(CultureManager.getInstance().getLanguage().getSecondInfoProgress());
    }

    private class BindableTask extends Task<Integer> {

        @Override
        protected Integer call() throws Exception {
            return updateProgress();
        }

        @Override
        protected void succeeded() {
            try {
                AppFX.setRoot(new LoginNamespace());
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
