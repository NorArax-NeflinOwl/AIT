package com.gui.panels;

import com.gui.AppGUI;
import com.gui.abstracts.AitGenericController;
import com.gui.context.AitMainContext;
import com.gui.interfaces.AitNamespaceInterface;
import com.gui.managers.AitCultureManager;
import com.gui.strings.AitControllersNameConstStrings;
import com.gui.strings.AitFramesStrings;
import com.hbm.managers.AitLogger;
import com.hbm.models.entitiecovers.AitAccount;
import javafx.fxml.FXML;
import javafx.scene.Scene;
import javafx.scene.control.Alert;
import javafx.scene.control.ButtonType;
import javafx.scene.control.Menu;
import javafx.scene.control.MenuItem;
import javafx.scene.image.Image;
import javafx.stage.Stage;

import java.net.UnknownHostException;

public class AitDashboardController extends AitGenericController<AitDashboardController, Integer> {

    public Menu fileMenu;
    public MenuItem closeAllItem;
    public MenuItem settingsItem;
    public MenuItem logoutItem;
    public MenuItem exitItem;
    public Menu editMenu;
    public MenuItem undoItem;
    public MenuItem redoItem;
    public Menu viewMenu;
    public MenuItem dashboardItem;
    public MenuItem createAccountItem;
    public Menu helpMenu;
    public MenuItem reqisterProductItem;
    public MenuItem aboutItem;

    @FXML
    public void initialize() {
        AitLogger.getInstance().logInfoToFile("opening: AitDashboardController.initialize()");

        fileMenu.setText(AitCultureManager.getInstance().getLanguage().getFileMenuContent());
        editMenu.setText(AitCultureManager.getInstance().getLanguage().getEditMenuContent());
        viewMenu.setText(AitCultureManager.getInstance().getLanguage().getViewMenuContent());
        helpMenu.setText(AitCultureManager.getInstance().getLanguage().getHelpMenuContent());

        closeAllItem.setText(AitCultureManager.getInstance().getLanguage().getCloseAllContent());
        closeAllItem.setDisable(true);

        settingsItem.setText(AitCultureManager.getInstance().getLanguage().getSettingsContent());
        settingsItem.setDisable(true);

        logoutItem.setText(AitCultureManager.getInstance().getLanguage().getLogoutContent());
        logoutItem.setOnAction(actionEvent -> onLogoutAction());

        exitItem.setText(AitCultureManager.getInstance().getLanguage().getExitContent());
        exitItem.setOnAction(actionEvent -> onExitAction());

        undoItem.setText(AitCultureManager.getInstance().getLanguage().getUndoContent());
        undoItem.setOnAction(actionEvent -> onUndoAction());

        redoItem.setText(AitCultureManager.getInstance().getLanguage().getRedoContent());
        redoItem.setDisable(true);

        dashboardItem.setText(AitCultureManager.getInstance().getLanguage().getDashboardContent());
        dashboardItem.setOnAction(actionEvent -> onDashboardAction());

        createAccountItem.setText(AitCultureManager.getInstance().getLanguage().getCreateAccountContent());
        createAccountItem.setOnAction(actionEvent -> onCreateAccountAction());

        reqisterProductItem.setText(AitCultureManager.getInstance().getLanguage().getRegisterProductContent());
        reqisterProductItem.setDisable(true);

        aboutItem.setText(AitCultureManager.getInstance().getLanguage().getAboutContent());
        aboutItem.setDisable(true);

        try {
            AitAccount account = AitMainContext.getAccount();
            if(account != null) {
                AitMainContext.setNamespaceTitle(AitControllersNameConstStrings.DASHBOARD_NAMESPACE, account.getUserData().getNick());
            }
        } catch (UnknownHostException e) {
            AitLogger.getInstance().logErrorToFile("error: AitDashboardController.initialize()", e);
        }

        AitLogger.getInstance().logInfoToFile("exiting: AitDashboardController.initialize()");
    }

    private void onLogoutAction() {
        AitLogger.getInstance().logInfoToFile("opening: AitDashboardController.onLogoutAction()");
        // TODO show progress bar

        try {
            Alert dialog = new Alert(Alert.AlertType.NONE, "Do you want log out?", ButtonType.YES, ButtonType.NO);
            dialog.showAndWait();
            if(dialog.getResult() == ButtonType.YES) {
                AitMainContext.setAccount(null, false);
                AppGUI.closeAllStages();
                AppGUI.setRoot(AitControllersNameConstStrings.LOGIN_NAMESPACE, AitControllersNameConstStrings.DASHBOARD_NAMESPACE, this);
            }
        } catch (Exception e) {
            AitLogger.getInstance().logErrorToFile("error: AitDashboardController.onLogoutAction()", e);
        }
        AitLogger.getInstance().logInfoToFile("exiting: AitDashboardController.onLogoutAction()");
    }

    private void onExitAction() {
        Alert dialog = new Alert(Alert.AlertType.NONE, "Do you want exit form application?", ButtonType.YES, ButtonType.NO);
        dialog.showAndWait();
        if(dialog.getResult() == ButtonType.YES) {
            AppGUI.exit();
        }
    }

    private void onUndoAction() {
        // TODO before starting write code, change AppGUI.stage to list of stage and add id or samething like that to managment their.
    }

    private void onDashboardAction() {
        AitLogger.getInstance().logInfoToFile("exiting: AitDashboardController.onCreateAccountAction()");
        Alert dialog = new Alert(Alert.AlertType.NONE, "Do you want open new dashboard?", ButtonType.YES, ButtonType.NO);
        dialog.showAndWait();
        if(dialog.getResult() == ButtonType.YES) {
            try {
                Stage stage = new Stage();
                AitNamespaceInterface namespace = AitMainContext.getNamespace(AitControllersNameConstStrings.DASHBOARD_NAMESPACE);
                Scene scene = new Scene(AppGUI.loadFXML(namespace));

                Image anotherIcon = new Image(getClass().getResource(AitFramesStrings.logiPath).toExternalForm());
                stage.getIcons().add(anotherIcon);
                stage.setTitle(namespace.getTitle());
                stage.setResizable(false);
                stage.setScene(scene);
                stage.show();
                AppGUI.addStage(stage);
            } catch (Exception e) {
                AitLogger.getInstance().logErrorToFile("error: AitDashboardController.onCreateAccountAction()",e);
            }
        }
        AitLogger.getInstance().logInfoToFile("exiting: AitDashboardController.onLogoutAction()");
    }

    private void onCreateAccountAction() {
        AitLogger.getInstance().logInfoToFile("exiting: AitDashboardController.onCreateAccountAction()");
        try {
            Stage stage = new Stage();
            AitNamespaceInterface namespace = AitMainContext.getNamespace(AitControllersNameConstStrings.DASHBOARD_NAMESPACE);
            Scene scene = new Scene(AppGUI.loadFXML(namespace));

            Image anotherIcon = new Image(getClass().getResource(AitFramesStrings.logiPath).toExternalForm());
            stage.getIcons().add(anotherIcon);
            stage.setTitle(namespace.getTitle());
            stage.setResizable(false);
            stage.setScene(scene);
            stage.show();
            AppGUI.addStage(stage);
        } catch (Exception e) {
            AitLogger.getInstance().logErrorToFile("error: AitDashboardController.onCreateAccountAction()",e);
        }
        AitLogger.getInstance().logInfoToFile("exiting: AitDashboardController.onLogoutAction()");
    }
}
