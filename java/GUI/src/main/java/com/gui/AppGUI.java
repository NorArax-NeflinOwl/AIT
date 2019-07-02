package com.gui;

import com.gui.context.MainContext;
import com.gui.generic.IGenericController;
import com.gui.namespace.BaseNamespace;
import com.gui.namespace.ControllersName;
import com.gui.strings.Consts;
import javafx.application.Application;
import javafx.fxml.FXMLLoader;
import javafx.geometry.Rectangle2D;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.Alert;
import javafx.scene.control.ButtonType;
import javafx.scene.image.Image;
import javafx.stage.Screen;
import javafx.stage.Stage;
import javafx.util.Pair;

import java.util.ArrayDeque;
import java.util.Queue;
import java.util.Stack;

public class AppGUI extends Application {

    private static Stack<Pair<Parent, BaseNamespace>> stack = new Stack<>();
    private static Scene scene;
    private static Stage stage;
    private static Queue<Stage> stages = new ArrayDeque<>();

    @Override
    public void start(Stage stage) throws Exception {
        AppGUI.stage = stage;
        initStage();
    }

    public static void setRoot(String to, String from, IGenericController controller) throws Exception {

        MainContext.setController(from, controller);
        BaseNamespace namespace = MainContext.getNamespace(to);

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
        stage.setTitle(namespace.getTitle());
        stage.setWidth(namespace.getWigth());
        stage.setHeight(namespace.getHeight());

        Rectangle2D primScreenBounds = Screen.getPrimary().getVisualBounds();
        stage.setX((primScreenBounds.getWidth() - stage.getWidth()) / 2);
        stage.setY((primScreenBounds.getHeight() - stage.getHeight()) / 2);
        if(namespace.getControllerName().equals(ControllersName.DASHBOARD_NAMESPACE)) {
            stage.setResizable(true);
        } else {
            stage.setResizable(false);
        }
    }

    private void initStage() throws Exception {
        BaseNamespace namespace = MainContext.getNamespace(ControllersName.ARNO_NAMESPACE);
        scene = new Scene(loadFXML(namespace));

        Image anotherIcon = new Image(getClass().getResource(Consts.logiPath).toExternalForm());
        stage.getIcons().add(anotherIcon);
        stage.setTitle(namespace.getTitle());
        stage.setResizable(false);
        stage.setScene(scene);
        stage.show();
        stage.setOnCloseRequest(windowEvent -> {
                    windowEvent.consume();

                    // FIXME
                    onClose(stage);
                });
    }

    private void onClose(Stage s) {
        Alert alert = new Alert(Alert.AlertType.NONE, "Do you want exit form application?", ButtonType.YES, ButtonType.NO);
        if (alert.showAndWait().orElse(ButtonType.NO) == ButtonType.YES) {
            s.close();
        }
    }

    public static Parent loadFXML(BaseNamespace namespace) throws Exception {
        FXMLLoader fxmlLoader = new FXMLLoader(AppGUI.class.getResource(Consts.frameSlash + namespace.getFrame() + Consts.fxmlExt));
        return fxmlLoader.load();
    }

    public static void main(String[] args) {
        MainContext.initFrames();
        launch();
    }

    public static void addStage(Stage stage) {
        stages.add(stage);
    }

    public static void closeAllStages() {
        while(!stages.isEmpty()) {
            if(stages.peek().isShowing()) {
                stages.peek().close();
            }
            stages.remove();
        }
    }

    public static void exit() {
        closeAllStages();
        stage.close();
    }

    public static BaseNamespace peekStack() {
        return !stack.empty() ? stack.peek().getValue() : null;
    }
}