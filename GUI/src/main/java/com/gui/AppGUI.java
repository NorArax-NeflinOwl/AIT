package com.gui;

import com.gui.namespace.ArnoNamespace;
import com.gui.namespace.BaseNamespace;
import com.gui.namespace.Consts;
import javafx.application.Application;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.image.Image;
import javafx.stage.Stage;

import java.io.IOException;

public class AppGUI extends Application {

    private static Scene scene;
    private static Stage stage;

    @Override
    public void start(Stage stage) throws Exception {
        this.stage = stage;
        initStage();
    }

    static void setRoot(BaseNamespace namespace) throws Exception {
        scene.setRoot(loadFXML(namespace.getFrame()));
        stage.setTitle(namespace.getName());
        stage.setWidth(namespace.getWigth());
        stage.setHeight(namespace.getHeight());
    }

    private void initStage() throws Exception {
        BaseNamespace namespace = new ArnoNamespace();
        scene = new Scene(loadFXML(namespace.getFrame()));

        Image anotherIcon = new Image(getClass().getResource(Consts.logiPath).toExternalForm());
        stage.getIcons().add(anotherIcon);
        stage.setTitle(namespace.getName());
        stage.setResizable(false);
        stage.setScene(scene);
        stage.show();
    }

    private static Parent loadFXML(String fxml) throws IOException {
        FXMLLoader fxmlLoader = new FXMLLoader(AppGUI.class.getResource(fxml + Consts.fxmlExt));
        return fxmlLoader.load();
    }

    public static void main(String[] args) {
        // TODO
        //  1) connect to com.hbm
        //  2) connect to com.ptl.server and communicate to with
        //      a) own ports protocol
        //      b) create webservices and used web methods

        launch();
    }
}