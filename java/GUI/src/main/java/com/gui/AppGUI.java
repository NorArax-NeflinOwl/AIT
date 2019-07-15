package com.gui;

import com.gui.context.AitInitializer;
import com.gui.context.AitMainContext;
import com.gui.interfaces.AitGenericControllerInterface;
import com.gui.interfaces.AitNamespaceInterface;
import com.gui.strings.AitControllersNameConstStrings;
import com.gui.strings.AitFramesStrings;
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
import java.util.Objects;
import java.util.Queue;
import java.util.Stack;

public class AppGUI extends Application {

    //<editor-fold desc="fields">
    private static Stack<Pair<Parent, AitNamespaceInterface>> stack = new Stack<>();
    private static Scene scene;
    private static Stage stage;
    private static Queue<Stage> stages = new ArrayDeque<>();
    //</editor-fold>

    //<editor-fold desc="override methods">
    @Override
    public void start(Stage stage) throws Exception {
        AppGUI.stage = stage;
        if(AitInitializer.getInstance().testDBConnection()) {
            initStage();
        } else {
            AppGUI.exit();
        }
    }
    //</editor-fold>

    //<editor-fold desc="public methods">
    public static void setRoot(String to, String from, AitGenericControllerInterface controller, boolean clearStack) throws Exception {
        setRoot(to, from, controller);
        if(clearStack) {
            stack.clear();
        }
    }

    public static void setRoot(String to, String from, AitGenericControllerInterface controller) throws Exception {
        AitMainContext.setController(from, controller);
        AitNamespaceInterface namespace = AitMainContext.getNamespace(to);

        if(namespace != null) {
            Parent parent = loadFXML(namespace);
            setRoot(parent, namespace);
        }
    }

    public static void back() {
        stack.pop();
        Pair<Parent, AitNamespaceInterface> pair = stack.peek();
        setRoot(pair.getKey(), pair.getValue());
    }

    public static Parent loadFXML(AitNamespaceInterface namespace) throws Exception {
        FXMLLoader fxmlLoader = new FXMLLoader(AppGUI.class.getResource(AitFramesStrings.panelsSlash + namespace.getPanel() + AitFramesStrings.fxmlExt));
        return fxmlLoader.load();
    }

    public static void main(String[] args) {
        AitMainContext.initFrames();
        launch();
    }

    public static void addStage(Stage stage) {
        stages.add(stage);
    }

    public static void closeAllStages() {
        while(!stages.isEmpty()) {
            if(stages.peek().isShowing()) {
                Objects.requireNonNull(stages.peek()).close();
            }
            stages.remove();
        }
    }

    public static void exit() {
        closeAllStages();
        stage.close();
    }

    public static AitNamespaceInterface peekStack() {
        return !stack.empty() ? stack.peek().getValue() : null;
    }
    //</editor-fold>

    //<editor-fold desc="private methods">
    private static void setRoot(Parent parent, AitNamespaceInterface namespace) {
        stack.push(new Pair<>(parent, namespace));

        scene.setRoot(parent);
        stage.setTitle(namespace.getTitle());
        stage.setWidth(namespace.getWigth());
        stage.setHeight(namespace.getHeight());

        Rectangle2D primScreenBounds = Screen.getPrimary().getVisualBounds();
        stage.setX((primScreenBounds.getWidth() - stage.getWidth()) / 2);
        stage.setY((primScreenBounds.getHeight() - stage.getHeight()) / 2);
        if(namespace.getControllerName().equals(AitControllersNameConstStrings.DASHBOARD_NAMESPACE)) {
            stage.setResizable(true);
        } else {
            stage.setResizable(false);
        }
    }

    private void initStage() throws Exception {
        AitNamespaceInterface namespace = AitMainContext.getNamespace(AitControllersNameConstStrings.ARNO_NAMESPACE);
        scene = new Scene(loadFXML(Objects.requireNonNull(namespace)));

        Image anotherIcon = new Image(getClass().getResource(AitFramesStrings.logiPath).toExternalForm());
        stage.getIcons().add(anotherIcon);
        stage.setTitle(namespace.getTitle());
        stage.setResizable(false);
        stage.setScene(scene);
        stage.show();
        stage.setOnCloseRequest(windowEvent -> {
            windowEvent.consume();

            // FIXME
            // analize problem and write what is going on :/
            onClose(stage);
        });
    }

    private void onClose(Stage s) {
        Alert alert = new Alert(Alert.AlertType.NONE, "Do you want exit form application?", ButtonType.YES, ButtonType.NO);
        if (alert.showAndWait().orElse(ButtonType.NO) == ButtonType.YES) {
            s.close();
        }
    }
    //</editor-fold>
}