package com.arno;

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
    public void initialize() {
        try {
            Task<Void> task = showInit();
            Thread th = new Thread(task);
            th.setDaemon(true);
            th.start();
        } catch (InterruptedException e) {
            e.printStackTrace();
            Thread.interrupted();
        }
    }

    public Task<Void> showInit() throws InterruptedException {
        progressBar.setProgress(0.0);
        Thread.sleep(500);
        for(int i = 0; i < 10; i++) {
            double rand = new Random().nextDouble();
            if(progressBar.getProgress() <= 0.9) {
                progressBar.setProgress(progressBar.getProgress() + rand);
            }
            Thread.sleep(500);
        }
        arnoFirstInfo.setText("Finishing...");
        progressBar.setProgress(0.9);
        Thread.sleep(1000);
        progressBar.setProgress(1);
        Thread.sleep(500);
        return null;
    }
}
