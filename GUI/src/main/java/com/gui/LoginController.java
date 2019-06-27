package com.gui;

import com.gui.cultureResources.CultureManager;
import com.gui.generic.GenericController;
import com.gui.namespace.RegistrationNamespace;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.scene.control.Alert;
import javafx.scene.control.Button;
import javafx.scene.control.ButtonType;
import javafx.scene.control.CheckBox;
import javafx.scene.control.Label;
import javafx.scene.control.PasswordField;
import javafx.scene.control.TextField;

public class LoginController extends GenericController<LoginController, Integer> {

    @FXML
    public TextField loginBox;
    @FXML
    public PasswordField passwordBox;
    @FXML
    public CheckBox rememberCheckBox;
    @FXML
    public Button loginButton;
    @FXML
    public Label registrationQuestion;
    @FXML
    public Button registrationButton;

    @FXML
    public void initialize() throws Exception {
        logger.info("opening: LoginController.initialize()");
        loginBox.setPromptText(CultureManager.getInstance().getLanguage().getLoginPrompt());
        passwordBox.setPromptText(CultureManager.getInstance().getLanguage().getPasswordPrompt());
        rememberCheckBox.setText(CultureManager.getInstance().getLanguage().getRementberMeQuestion());
        loginButton.setText(CultureManager.getInstance().getLanguage().getLoginButtonContent());
        registrationQuestion.setText(CultureManager.getInstance().getLanguage().getRegistrationQuestion());
        registrationButton.setText(CultureManager.getInstance().getLanguage().getRegistrationButtonContent());
        logger.info("exiting: LoginController.initialize()");
    }

    @FXML
    private void openRegisterFrame() throws Exception {
        logger.info("opening: LoginController.openRegisterFrame()");
        AppFX.setRoot(new RegistrationNamespace());
        logger.info("exiting: LoginController.openRegisterFrame()");
    }

    @FXML
    private void loginAction() {
        logger.info("opening: LoginController.loginAction()");
        String login = loginBox.getText();
        String password = passwordBox.getText();

        if(login == null || login.length() == 0) {
            Alert alert = new Alert(Alert.AlertType.ERROR, "Please enter login!", ButtonType.OK);
            alert.show();
        }
        else if(password == null || password.length() == 0) {
            Alert alert = new Alert(Alert.AlertType.ERROR, "Please enter password!", ButtonType.OK);
            alert.show();
        }
        // TODO login user
        // else {
        // }
        logger.info("exiting: LoginController.loginAction()");
    }
}
