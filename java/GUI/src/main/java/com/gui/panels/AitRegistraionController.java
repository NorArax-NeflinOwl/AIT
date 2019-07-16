package com.gui.panels;

import com.gui.AppGUI;
import com.gui.abstracts.AitGenericController;
import com.gui.context.AitMainContext;
import com.gui.interfaces.AitGenericControllerInterface;
import com.gui.managers.AitCultureManager;
import com.gui.strings.AitControllersNameConstStrings;
import com.hbm.daos.AitDAOFactory;
import com.hbm.managers.AitCrypter;
import com.hbm.managers.AitLogger;
import com.hbm.models.entitiecovers.AitAccount;
import com.hbm.models.entitiecovers.AitUserData;
import com.hbm.models.entities.AitAccountEntity;
import com.hbm.models.entities.AitUserDataEntity;
import javafx.fxml.FXML;
import javafx.scene.control.Alert;
import javafx.scene.control.Button;
import javafx.scene.control.ButtonType;
import javafx.scene.control.DatePicker;
import javafx.scene.control.Label;
import javafx.scene.control.PasswordField;
import javafx.scene.control.TextField;

import java.time.ZoneId;
import java.util.Calendar;
import java.util.Date;
import java.util.List;

public class AitRegistraionController extends AitGenericController<AitRegistraionController, Integer> {

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
    public Button backButton;

    @FXML
    public void initialize() {
        AitLogger.getInstance().logInfoToFile("opening: AitRegistraionController.initialize()");
        try {
            reqLabel.setText(AitCultureManager.getInstance().getLanguage().getReqLabelContent());
            backButton.setText(AitCultureManager.getInstance().getLanguage().getBackButtonContent());
            backButton.setOnAction(actionEvent -> {
                try {
                    onBackAction();
                } catch (Exception e) {
                    e.printStackTrace();
                }
            });
            loginBox.setPromptText(AitCultureManager.getInstance().getLanguage().getReqLoginPrompt());
            passwordBox.setPromptText(AitCultureManager.getInstance().getLanguage().getReqPasswordPrompt());
            repeatPasswordBox.setPromptText(AitCultureManager.getInstance().getLanguage().getReqRepeatPasswordPrompt());
            emailBox.setPromptText(AitCultureManager.getInstance().getLanguage().getReqEmailPrompt());
            repeatEmailBox.setPromptText(AitCultureManager.getInstance().getLanguage().getReqRepeatEmailPrompt());
            optionalLabel.setText(AitCultureManager.getInstance().getLanguage().getOptionalLabelContent());
            nickBox.setPromptText(AitCultureManager.getInstance().getLanguage().getOptionalNickPrompt());
            firstNameBox.setPromptText(AitCultureManager.getInstance().getLanguage().getOptionalFirstNamePrompt());
            middleNameBox.setPromptText(AitCultureManager.getInstance().getLanguage().getOptionalMiddleNamePrompt());
            lastNameBox.setPromptText(AitCultureManager.getInstance().getLanguage().getOptionalLastNamePrompt());
            birthdateBox.setPromptText(AitCultureManager.getInstance().getLanguage().getBirthdayPrompt());
            registerButton.setText(AitCultureManager.getInstance().getLanguage().getRegisterButtonContent());
            registerButton.setOnAction(actionEvent -> onRegisterAction());

            if(AppGUI.peekStack() != AitMainContext.getNamespace(AitControllersNameConstStrings.LOGIN_NAMESPACE)) {
                backButton.setVisible(false);
            }

        } catch (Exception e) {
            AitLogger.getInstance().logErrorToFile("error: RegistrationController.initialize()", e);
        }
        AitLogger.getInstance().logInfoToFile("exiting: AitRegistraionController.initialize()");
    }

    @FXML
    public void onRegisterAction() {
        AitLogger.getInstance().logInfoToFile("opening: AitRegistraionController.onRegisterAction()");
        try {
            AitMainContext.getSession(true).beginTransaction();
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
                AitAccountEntity entity = new AitAccountEntity();
                entity.setLogin(login);
                entity.setPassword(AitCrypter.generateMD5Hash(password));
                entity.setEmail(email);
                entity.setCreateDate(new Date());
                AitAccount account = new AitAccount(AitMainContext.getSession(true), entity);

                boolean createUserData = false;
                if(nick != null) {

                    AitDAOFactory dao = new AitDAOFactory(AitMainContext.getSession(true));
                    List<AitUserData> datas = dao.getUserDataDAO().findUserDataByNick(nick);

                    if(nick.length() >= 8 && (datas.isEmpty())) {
                        createUserData = true;
                    } else if(nick.length() > 0) {
                        Alert alert = new Alert(Alert.AlertType.WARNING, "Nick must be >= 8 characters!", ButtonType.OK);
                        alert.show();
                        nickBox.requestFocus();
                        nickBox.clear();
                        AitLogger.getInstance().logInfoToFile("exiting: AitRegistraionController.onRegisterAction()");
                        return;
                    } else if (!datas.isEmpty()) {
                        Alert alert = new Alert(Alert.AlertType.WARNING, "Nick must be unique!", ButtonType.OK);
                        alert.show();
                        nickBox.requestFocus();
                        nickBox.clear();
                        AitLogger.getInstance().logInfoToFile("exiting: AitRegistraionController.onRegisterAction()");
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
                    AitLogger.getInstance().logInfoToFile("exiting: AitRegistraionController.onRegisterAction()");
                    return;
                }

                Calendar calendar = Calendar.getInstance();
                calendar.setTime(new Date());
                calendar.add(Calendar.YEAR, -18);

                if(birthday != null) {
                    if(birthday.before(calendar.getTime())) {
                        createUserData = true;
                    } else {
                        Alert alert = new Alert(Alert.AlertType.WARNING, "You must have over 18 year!", ButtonType.OK);
                        alert.show();
                        birthdateBox.requestFocus();
                        birthdateBox.setValue(null);
                        AitLogger.getInstance().logInfoToFile("exiting: AitRegistraionController.onRegisterAction()");
                        return;
                    }
                }

                if(createUserData) {
                    AitUserDataEntity dataEntity = new AitUserDataEntity();
                    dataEntity.setAccount(entity);
                    dataEntity.setNick(nick);
                    dataEntity.setFirstName(firstName);
                    dataEntity.setMiddleName(middleName);
                    dataEntity.setLastName(lastName);
                    dataEntity.setBirthdate(birthday);
                    AitUserData data = new AitUserData(AitMainContext.getSession(true), dataEntity);
                    data.saveOrUpdate();
                } else {
                    account.saveOrUpdate();
                }

                //TODO AitMailSender.sendTo(account.getEmail());

                Alert alert = new Alert(Alert.AlertType.INFORMATION, "AitAccount was create successfull, check your email.", ButtonType.OK);
                alert.show();

                if(AppGUI.peekStack() != AitMainContext.getNamespace(AitControllersNameConstStrings.LOGIN_NAMESPACE)) {
                    Alert dialog = new Alert(Alert.AlertType.NONE, "Do you want log out and switch user?", ButtonType.YES, ButtonType.NO);
                    dialog.showAndWait();
                    if(dialog.getResult() == ButtonType.YES) {
                        AitMainContext.setAccount(null, false);
                        AppGUI.setRoot(AitControllersNameConstStrings.LOGIN_NAMESPACE, AitControllersNameConstStrings.DASHBOARD_NAMESPACE, this);
                    }
                } else {
                    AppGUI.setRoot(AitControllersNameConstStrings.LOGIN_NAMESPACE, AitControllersNameConstStrings.REGISTRATION_NAMESPACE, this);
                    AitGenericControllerInterface controller = AitMainContext.getController(AitControllersNameConstStrings.LOGIN_NAMESPACE);
                    if(controller != null) {
                        AitLoginController loginController = (AitLoginController)controller;
                        loginController.loginBox.setText(login);
                        loginController.passwordBox.requestFocus();
                    }
                }
            }
        } catch (Exception e) {
            AitLogger.getInstance().logErrorToFile("error: AitRegistraionController.onRegisterAction()", e);
        }

        AitLogger.getInstance().logInfoToFile("exiting: AitRegistraionController.onRegisterAction()");
    }

    public void onBackAction() {
        AitLogger.getInstance().logInfoToFile("opening: AitRegistraionController.onRegisterAction()");
        AppGUI.back();
        AitLogger.getInstance().logInfoToFile("exiting: AitRegistraionController.onRegisterAction()");
    }

}