package com.gui;

import com.gui.cultureResources.CultureManager;
import com.gui.generic.GenericController;
import com.gui.namespace.LoginNamespace;
import javafx.fxml.FXML;
import javafx.scene.control.Alert;
import javafx.scene.control.Button;
import javafx.scene.control.ButtonType;
import javafx.scene.control.DatePicker;
import javafx.scene.control.PasswordField;
import javafx.scene.control.TextField;

import java.time.LocalDate;

public class RegistraionController  extends GenericController<RegistraionController, Integer> {

    @FXML
    public TextField loginBox;
    @FXML
    public PasswordField passwordBox;
    @FXML
    public PasswordField repeatPasswordBox;
    @FXML
    public TextField emailBox;
    @FXML
    public TextField repeatEmailBox;
    @FXML
    public DatePicker birthdayBox;
    @FXML
    public Button registerButton;

    @FXML
    public void initialize() throws Exception {
        logger.info("opening: RegistraionController.initialize()");
        loginBox.setPromptText(CultureManager.getInstance().getLanguage().getReqLoginPrompt());
        passwordBox.setPromptText(CultureManager.getInstance().getLanguage().getReqPasswordPrompt());
        repeatPasswordBox.setPromptText(CultureManager.getInstance().getLanguage().getReqRepeatPasswordPrompt());
        emailBox.setPromptText(CultureManager.getInstance().getLanguage().getReqEmailPrompt());
        repeatPasswordBox.setPromptText(CultureManager.getInstance().getLanguage().getReqRepeatEmailPrompt());
        birthdayBox.setPromptText(CultureManager.getInstance().getLanguage().getBirthdayPrompt());
        registerButton.setText(CultureManager.getInstance().getLanguage().getRegisterButtonContent());
        logger.info("exiting: RegistraionController.initialize()");
    }

    @FXML
    public void registerAction() throws Exception {
        logger.info("opening: RegistraionController.registerAction()");
        String login = loginBox.getText();
        String password = passwordBox.getText();
        String repeatPassword= repeatPasswordBox.getText();
        String email = emailBox.getText();
        String repeatEmail = repeatEmailBox.getText();
        LocalDate birthday = birthdayBox.getValue();

        if(login == null || login.length() == 0) {
            Alert alert = new Alert(Alert.AlertType.ERROR, "Please enter login!", ButtonType.OK);
            alert.show();
        }
        else if(password == null || password.length() == 0) {
            Alert alert = new Alert(Alert.AlertType.ERROR, "Please enter password!", ButtonType.OK);
            alert.show();
        }
        else if (repeatPassword == null || repeatPassword.length() == 0) {
            Alert alert = new Alert(Alert.AlertType.ERROR, "Please repeat password!", ButtonType.OK);
            alert.show();
        }
        else if (!password.equals(repeatPassword)){
            Alert alert = new Alert(Alert.AlertType.ERROR, "Passwords not equals!", ButtonType.OK);
            alert.show();
        }
        else if(email == null || email.length() == 0) {
            Alert alert = new Alert(Alert.AlertType.ERROR, "Please enter email!", ButtonType.OK);
            alert.show();
        }
        else if(repeatEmail == null || repeatEmail.length() == 0) {
            Alert alert = new Alert(Alert.AlertType.ERROR, "Please repeat email!", ButtonType.OK);
            alert.show();
        }
        else if(!email.equals(repeatEmail)) {
            Alert alert = new Alert(Alert.AlertType.ERROR, "Emails not equals!", ButtonType.OK);
            alert.show();
        }
        else {
            // TODO create account

            AppGUI.setRoot(new LoginNamespace());
        }
        logger.info("exiting: RegistraionController.registerAction()");
    }
}