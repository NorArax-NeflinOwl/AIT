package com.gui;

import com.gui.context.MainContext;
import com.gui.generic.IGenericController;
import com.gui.namespace.ArnoNamespace;
import com.gui.namespace.BaseNamespace;
import com.gui.namespace.ControllersName;
import com.gui.strings.Consts;
import javafx.application.Application;
import javafx.fxml.FXMLLoader;
import javafx.geometry.Rectangle2D;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.image.Image;
import javafx.stage.Screen;
import javafx.stage.Stage;
import javafx.util.Pair;

import java.util.Stack;

public class AppGUI extends Application {

    private static Stack<Pair<Parent, BaseNamespace>> stack = new Stack<>();
    private static Scene scene;
    private static Stage stage;

    @Override
    public void start(Stage stage) throws Exception {
        AppGUI.stage = stage;
        initStage();
    }

    public static void setRoot(String from, String to, IGenericController controller) throws Exception {

        MainContext.setController(to, controller);
        BaseNamespace namespace = MainContext.getNamespace(from);

        if(namespace != null) {
            Parent parent = loadFXML(namespace);
            setRoot(parent, namespace);
        }
    }

    public static void back() {
        stack.pop();
        Pair<Parent, BaseNamespace> pair = stack.peek();
        setRoot(pair.getKey(), pair.getValue());
    }

    private static void setRoot(Parent parent, BaseNamespace namespace) {
        stack.push(new Pair<>(parent, namespace));

        scene.setRoot(parent);
        stage.setTitle(namespace.getName());
        stage.setWidth(namespace.getWigth());
        stage.setHeight(namespace.getHeight());

        if(namespace.getControllerName().equals(ControllersName.DASHBOARD_NAMESPACE)) {
            Rectangle2D primScreenBounds = Screen.getPrimary().getVisualBounds();
            stage.setX((primScreenBounds.getWidth() - stage.getWidth()) / 2);
            stage.setY((primScreenBounds.getHeight() - stage.getHeight()) / 2);
        }
    }

    private void initStage() throws Exception {
        BaseNamespace namespace = new ArnoNamespace();
        scene = new Scene(loadFXML(namespace));

        Image anotherIcon = new Image(getClass().getResource(Consts.logiPath).toExternalForm());
        stage.getIcons().add(anotherIcon);
        stage.setTitle(namespace.getName());
        stage.setResizable(false);
        stage.setScene(scene);
        stage.show();
    }

    private static Parent loadFXML(BaseNamespace namespace) throws Exception {
        FXMLLoader fxmlLoader = new FXMLLoader(AppGUI.class.getResource(Consts.frameSlash + namespace.getFrame() + Consts.fxmlExt));
        return fxmlLoader.load();
    }

    public static void main(String[] args) {
        MainContext.initFrames();
        launch();
    }
}