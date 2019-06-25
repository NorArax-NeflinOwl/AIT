package com.arno;

import com.arno.cultureResources.CultureManager;
import com.arno.namespace.ArnoNamespace;
import com.arno.namespace.BaseNamespace;
import com.arno.namespace.Consts;
import com.arno.namespace.LoginNamespace;
import com.arno.strings.Polish;
import javafx.application.Application;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.image.Image;
import javafx.stage.Stage;

import java.io.IOException;

public class AppFX extends Application {

    private static Scene scene;
    private static Stage stage;

    @Override
    public void start(Stage stage) throws Exception {
        this.stage = stage;
        initStage();
    }

    public static void setRoot(BaseNamespace namespace) throws Exception {
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
        FXMLLoader fxmlLoader = new FXMLLoader(AppFX.class.getResource(fxml + Consts.fxmlExt));
        return fxmlLoader.load();
    }

    public static void main(String[] args) {
        //CultureManager.getInstance().init();
        CultureManager.getInstance().setLanguage(Polish.locale);

        // TODO
        //  1) connect to db
        //  2) connect to server and communicate to with
        //      a) own ports protocol
        //      b) webservices and used web methods

        launch();
    }
}