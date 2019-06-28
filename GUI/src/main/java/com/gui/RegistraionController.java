package com.gui;

import com.gui.context.MainContext;
import com.gui.cultureResources.CultureManager;
import com.gui.generic.GenericController;
import com.gui.namespace.LoginNamespace;
import com.hbm.datamodels.models.Account;
import com.hbm.datamodels.models.UserData;
import com.hbm.entities.AccountEntity;
import com.hbm.entities.UserDataEntity;
import com.ptl.managers.AitCrypter;
import javafx.fxml.FXML;
import javafx.scene.control.Alert;
import javafx.scene.control.Button;
import javafx.scene.control.ButtonType;
import javafx.scene.control.DatePicker;
import javafx.scene.control.Label;
import javafx.scene.control.PasswordField;
import javafx.scene.control.TextField;

import java.time.LocalDate;
import java.time.ZoneId;
import java.util.Date;

public class RegistraionController  extends GenericController<RegistraionController, Integer> {

    @FXML
    public Label reqLabel;
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
    public Label optionalLabel;
    @FXML
    public TextField nickBox;
    @FXML
    public TextField firstNameBox;
    @FXML
    public TextField middleNameBox;
    @FXML
    public TextField lastNameBox;
    @FXML
    public DatePicker birthdateBox;
    @FXML
    public Button registerButton;

    @FXML
    public void initialize() throws Exception {
        logger.info("opening: RegistraionController.initialize()");
        reqLabel.setText(CultureManager.getInstance().getLanguage().getReqLabelContent());
        loginBox.setPromptText(CultureManager.getInstance().getLanguage().getReqLoginPrompt());
        passwordBox.setPromptText(CultureManager.getInstance().getLanguage().getReqPasswordPrompt());
        repeatPasswordBox.setPromptText(CultureManager.getInstance().getLanguage().getReqRepeatPasswordPrompt());
        emailBox.setPromptText(CultureManager.getInstance().getLanguage().getReqEmailPrompt());
        repeatPasswordBox.setPromptText(CultureManager.getInstance().getLanguage().getReqRepeatEmailPrompt());
        optionalLabel.setText(CultureManager.getInstance().getLanguage().getOptionalLabelContent());
        nickBox.setPromptText(CultureManager.getInstance().getLanguage().getOptionalNickPrompt());
        firstNameBox.setPromptText(CultureManager.getInstance().getLanguage().getOptionalFirstNamePrompt());
        middleNameBox.setPromptText(CultureManager.getInstance().getLanguage().getOptionalMiddleNamePrompt());
        lastNameBox.setPromptText(CultureManager.getInstance().getLanguage().getOptionalLastNamePrompt());
        birthdateBox.setPromptText(CultureManager.getInstance().getLanguage().getBirthdayPrompt());
        registerButton.setText(CultureManager.getInstance().getLanguage().getRegisterButtonContent());
        logger.info("exiting: RegistraionController.initialize()");
    }

    @FXML
    public void registerAction() {
        logger.info("opening: RegistraionController.registerAction()");
        try {
            MainContext.getSession(true).beginTransaction();
            // TODO show progress bar

            String login = loginBox.getText();
            String password = passwordBox.getText();
            String repeatPassword= repeatPasswordBox.getText();
            String email = emailBox.getText();
            String repeatEmail = repeatEmailBox.getText();

            String nick = nickBox.getText();
            String firstName = firstNameBox.getText();
            String middleName = middleNameBox.getText();
            String lastName = lastNameBox.getText();
            Date birthday = birthdateBox.getValue() != null ? Date.from(birthdateBox.getValue().atStartOfDay(ZoneId.systemDefault()).toInstant()) : null;

            if(login == null || login.length() == 0) {
                Alert alert = new Alert(Alert.AlertType.ERROR, "Please enter login!", ButtonType.OK);
                alert.show();
                loginBox.requestFocus();
            }
            else if(password == null || password.length() == 0) {
                Alert alert = new Alert(Alert.AlertType.ERROR, "Please enter password!", ButtonType.OK);
                alert.show();
                passwordBox.requestFocus();
            }
            else if (repeatPassword == null || repeatPassword.length() == 0) {
                Alert alert = new Alert(Alert.AlertType.ERROR, "Please repeat password!", ButtonType.OK);
                alert.show();
                repeatPasswordBox.requestFocus();
            }
            else if (!password.equals(repeatPassword)){
                Alert alert = new Alert(Alert.AlertType.ERROR, "Passwords not equals!", ButtonType.OK);
                alert.show();
                passwordBox.requestFocus();
                repeatPasswordBox.clear();
            }
            else if(email == null || email.length() == 0) {
                Alert alert = new Alert(Alert.AlertType.ERROR, "Please enter email!", ButtonType.OK);
                alert.show();
                emailBox.requestFocus();
            }
            else if(repeatEmail == null || repeatEmail.length() == 0) {
                Alert alert = new Alert(Alert.AlertType.ERROR, "Please repeat email!", ButtonType.OK);
                alert.show();
                repeatEmailBox.requestFocus();
            }
            else if(!email.equals(repeatEmail)) {
                Alert alert = new Alert(Alert.AlertType.ERROR, "Emails not equals!", ButtonType.OK);
                alert.show();
                emailBox.requestFocus();
                repeatEmailBox.clear();
            }
            else {
                // TODO create account
                AccountEntity entity = new AccountEntity();
                entity.setLogin(login);
                entity.setPassword(AitCrypter.generateMD5Hash(password));
                entity.setEmail(email);
                entity.setCreateDate(new Date());
                Account account = new Account(MainContext.getSession(true), entity);

                boolean createUserData = false;
                if(nick != null) {
                    if(nick.length() >= 8) {
                        createUserData = true;
                    } else if(nick.length() > 0) {
                        Alert alert = new Alert(Alert.AlertType.WARNING, "Nick must be >= 8 characters!", ButtonType.OK);
                        alert.show();
                        nickBox.requestFocus();
                        nickBox.clear();
                        logger.info("exiting: RegistraionController.registerAction()");
                        return;
                    }
                }
                if(firstName != null && firstName.length() > 0 && lastName != null && lastName.length() > 0) {
                    createUserData = true;
                } else if (middleName != null && middleName.length() > 0) {
                    Alert alert = new Alert(Alert.AlertType.WARNING, "You must enter first and last name!", ButtonType.OK);
                    alert.show();
                    middleNameBox.requestFocus();
                    middleNameBox.clear();
                    logger.info("exiting: RegistraionController.registerAction()");
                    return;
                }
                if(birthday != null) {
                    createUserData = true;
                }

                if(createUserData) {
                    UserDataEntity dataEntity = new UserDataEntity();
                    dataEntity.setAccount(entity);
                    dataEntity.setNick(nick);
                    dataEntity.setFirstName(firstName);
                    dataEntity.setMiddleName(middleName);
                    dataEntity.setLastName(lastName);
                    dataEntity.setBirthdate(birthday);
                    UserData data = new UserData(MainContext.getSession(true), dataEntity);
                    data.saveOrUpdate();
                } else {
                    account.saveOrUpdate();
                }

                MainContext.getSession(true).getTransaction().commit();

                AppGUI.setRoot(new LoginNamespace());
            }
        } catch (Exception e) {
            if(MainContext.getSession(false) != null) {
                MainContext.getSession(false).getTransaction().rollback();
            }
            logger.error("error: RegistraionController.registerAction()", e);
        } finally {
            if(MainContext.getSession(false) != null) {
                MainContext.getSession(false).close();
            }
        }
        // TODO send activation email


        Alert alert = new Alert(Alert.AlertType.INFORMATION, "Account was create successfull, check your email.", ButtonType.OK);
        alert.show();
        logger.info("exiting: RegistraionController.registerAction()");
    }
}