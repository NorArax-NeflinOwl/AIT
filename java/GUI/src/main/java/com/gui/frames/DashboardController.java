package com.gui.frames;

import com.gui.AppGUI;
import com.gui.context.MainContext;
import com.gui.cultureResources.CultureManager;
import com.gui.generic.GenericController;
import com.gui.namespace.BaseNamespace;
import com.gui.namespace.ControllersName;
import com.gui.namespace.RegistrationNamespace;
import com.gui.strings.Consts;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.scene.Scene;
import javafx.scene.control.Alert;
import javafx.scene.control.ButtonType;
import javafx.scene.control.Menu;
import javafx.scene.control.MenuItem;
import javafx.scene.image.Image;
import javafx.stage.Stage;

public class DashboardController extends GenericController<DashboardController, Integer>{

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
        logger.info("opening: DashboardController.initialize()");

        fileMenu.setText(CultureManager.getInstance().getLanguage().getFileMenuContent());
        editMenu.setText(CultureManager.getInstance().getLanguage().getEditMenuContent());
        viewMenu.setText(CultureManager.getInstance().getLanguage().getViewMenuContent());
        helpMenu.setText(CultureManager.getInstance().getLanguage().getHelpMenuContent());

        closeAllItem.setText(CultureManager.getInstance().getLanguage().getCloseAllContent());
        closeAllItem.setDisable(true);

        settingsItem.setText(CultureManager.getInstance().getLanguage().getSettingsContent());
        settingsItem.setDisable(true);

        logoutItem.setText(CultureManager.getInstance().getLanguage().getLogoutContent());
        logoutItem.setOnAction(actionEvent -> onLogoutAction());

        exitItem.setText(CultureManager.getInstance().getLanguage().getExitContent());
        exitItem.setOnAction(actionEvent -> onExitAction());

        undoItem.setText(CultureManager.getInstance().getLanguage().getUndoContent());
        undoItem.setOnAction(actionEvent -> onUndoAction());

        redoItem.setText(CultureManager.getInstance().getLanguage().getRedoContent());
        redoItem.setDisable(true);

        dashboardItem.setText(CultureManager.getInstance().getLanguage().getDashboardTitle());
        dashboardItem.setOnAction(actionEvent -> onDoshboardAction());

        createAccountItem.setText(CultureManager.getInstance().getLanguage().getCreateAccountContent());
        createAccountItem.setOnAction(actionEvent -> onCreateAccountAction());

        reqisterProductItem.setText(CultureManager.getInstance().getLanguage().getRegisterProductContent());
        reqisterProductItem.setDisable(true);

        aboutItem.setText(CultureManager.getInstance().getLanguage().getAboutContent());
        aboutItem.setDisable(true);

        String nick = MainContext.getUser().getNick();
        if(nick != null && nick != "") {
            MainContext.setNamespaceTitle(ControllersName.DASHBOARD_NAMESPACE, nick);
        }

        logger.info("exiting: DashboardController.initialize()");
    }

    private void onLogoutAction() {
        logger.info("opening: DashboardController.onLogoutAction()");
        try {
            Alert dialog = new Alert(Alert.AlertType.CONFIRMATION, "Do you want log out?", ButtonType.OK, ButtonType.CANCEL);
            dialog.showAndWait();
            if(dialog.getResult() == ButtonType.OK) {
                MainContext.setUser(null, false);
                AppGUI.setRoot(ControllersName.LOGIN_NAMESPACE, ControllersName.DASHBOARD_NAMESPACE, this);
            }
        } catch (Exception e) {
            logger.error("error: DashboardController.onLogoutAction()");
        }
        logger.info("exiting: DashboardController.onLogoutAction()");
    }

    private void onExitAction() {
        Alert dialog = new Alert(Alert.AlertType.CONFIRMATION, "Do you want exit form application?", ButtonType.OK, ButtonType.CANCEL);
        dialog.showAndWait();
        if(dialog.getResult() == ButtonType.OK) {
            AppGUI.exit();
        }
    }

    private void onUndoAction() {

    }

    private void onDoshboardAction() {

    }

    private void onCreateAccountAction() {
        logger.info("exiting: DashboardController.onCreateAccountAction()");
        try {
            Stage stage = new Stage();
            BaseNamespace namespace = new RegistrationNamespace();
            Scene scene = new Scene(AppGUI.loadFXML(namespace));

            Image anotherIcon = new Image(getClass().getResource(Consts.logiPath).toExternalForm());
            stage.getIcons().add(anotherIcon);
            stage.setTitle(namespace.getTitle());
            stage.setResizable(false);
            stage.setScene(scene);
            stage.show();
        } catch (Exception e) {
            logger.error("error: DashboardController.onCreateAccountAction()",e);
        }
        logger.info("exiting: DashboardController.onLogoutAction()");
    }
}
