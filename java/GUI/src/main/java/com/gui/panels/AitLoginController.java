package com.gui.panels;

import com.gui.AppGUI;
import com.gui.abstracts.AitGenericController;
import com.gui.context.AitMainContext;
import com.gui.managers.AitCultureManager;
import com.gui.strings.AitControllersNameConstStrings;
import com.hbm.daos.AitDAOFactory;
import com.hbm.managers.AitCrypter;
import com.hbm.managers.AitLogger;
import com.hbm.models.entitiecovers.AitAccount;
import javafx.fxml.FXML;
import javafx.scene.control.Alert;
import javafx.scene.control.Button;
import javafx.scene.control.ButtonType;
import javafx.scene.control.CheckBox;
import javafx.scene.control.Label;
import javafx.scene.control.PasswordField;
import javafx.scene.control.TextField;
import javafx.scene.input.KeyCode;
import javafx.scene.input.KeyEvent;

import java.util.List;

public class AitLoginController extends AitGenericController<AitLoginController, Integer> {
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
    public void initialize() {
        AitLogger.getInstance().logInfoToFile("opening: AitLoginController.initialize()");
        try {
            loginBox.setPromptText(AitCultureManager.getInstance().getLanguage().getLoginPrompt());
            passwordBox.setPromptText(AitCultureManager.getInstance().getLanguage().getPasswordPrompt());
            passwordBox.setOnKeyReleased(this::onKeyReleased);
            rememberCheckBox.setText(AitCultureManager.getInstance().getLanguage().getRementberMeQuestion());
            loginButton.setText(AitCultureManager.getInstance().getLanguage().getLoginButtonContent());
            loginButton.setOnAction(actionEvent -> loginAction());
            registrationQuestion.setText(AitCultureManager.getInstance().getLanguage().getRegistrationQuestion());
            registrationButton.setText(AitCultureManager.getInstance().getLanguage().getRegistrationButtonContent());
            registrationButton.setOnAction(actionEvent -> {
                try {
                    openRegisterFrame();
                } catch (Exception e) {
                    AitLogger.getInstance().logErrorToFile("error: AitLoginController.setOnAction()", e);
                }
            });
        } catch (Exception e) {
            AitLogger.getInstance().logErrorToFile("error: AitLoginController.initialize()", e);
        }
        AitLogger.getInstance().logInfoToFile("exiting: AitLoginController.initialize()");
    }

    private void onKeyReleased(KeyEvent keyEvent) {
        if(keyEvent.getCode().equals(KeyCode.ENTER)) {
            loginAction();
        }
    }

    @FXML
    private void openRegisterFrame() throws Exception {
        AitLogger.getInstance().logInfoToFile("opening: AitLoginController.openRegisterFrame()");
        AppGUI.setRoot(AitControllersNameConstStrings.REGISTRATION_NAMESPACE, AitControllersNameConstStrings.LOGIN_NAMESPACE, this);
        AitLogger.getInstance().logInfoToFile("exiting: AitLoginController.openRegisterFrame()");
    }

    @FXML
    private void loginAction() {
        AitLogger.getInstance().logInfoToFile("opening: AitLoginController.loginAction()");
        try {
            // TODO show progress bar

            String login = loginBox.getText();
            String password = passwordBox.getText();
            boolean rememberMe = rememberCheckBox.isSelected();

            if(login == null || login.length() == 0) {
                Alert alert = new Alert(Alert.AlertType.ERROR, "Please enter login!", ButtonType.OK);
                alert.show();
                AitLogger.getInstance().logInfoToFile("Empty login!");
                loginBox.requestFocus();
            }
            else if(password == null || password.length() == 0) {
                Alert alert = new Alert(Alert.AlertType.ERROR, "Please enter password!", ButtonType.OK);
                alert.show();
                AitLogger.getInstance().logInfoToFile("Empty password!");
                passwordBox.requestFocus();
            }
            else {
                AitDAOFactory dao = new AitDAOFactory(AitMainContext.getSession(true));
                List<AitAccount> accounts = dao.getAccountDAO().findAccountByLogin(login);
                if(accounts != null && accounts.size() > 0) {
                    for (AitAccount acc : accounts) {
                        if(AitCrypter.generateMD5Hash(password).equals(acc.getPassword())) {
                            if(!acc.IsActive()) {
                                Alert alert = new Alert(Alert.AlertType.ERROR, "AitAccount is not activate!", ButtonType.OK);
                                alert.show();
                                AitLogger.getInstance().logInfoToFile("AitAccount is not activate!");
                                passwordBox.requestFocus();
                                AitLogger.getInstance().logInfoToFile("exiting: AitLoginController.loginAction()");
                                return;
                            }
                            else {
                                try {
                                    AitMainContext.setAccount(acc, rememberMe);
                                } catch (Exception se) {
                                    AitLogger.getInstance().logErrorToFile("",se);
                                    Alert alert = new Alert(Alert.AlertType.ERROR, "You dont have permition to use this app! Run app as administator!", ButtonType.OK);
                                    alert.showAndWait();
                                    if(alert.getResult() == ButtonType.OK) {
                                        AppGUI.exit();
                                    }
                                }
                                AppGUI.setRoot(AitControllersNameConstStrings.DASHBOARD_NAMESPACE, AitControllersNameConstStrings.LOGIN_NAMESPACE, this, true);

                                AitLogger.getInstance().logInfoToFile("exiting: AitLoginController.loginAction() Login Successfull");
                                return;
                            }
                        }
                    }
                    Alert alert = new Alert(Alert.AlertType.ERROR, "Incorect password!", ButtonType.OK);
                    alert.show();
                    AitLogger.getInstance().logInfoToFile("Incorect password!");
                    passwordBox.requestFocus();
                    AitLogger.getInstance().logInfoToFile("exiting: AitLoginController.loginAction()");
                    return;
                }
                else {
                    Alert alert = new Alert(Alert.AlertType.ERROR, "Login not found!", ButtonType.OK);
                    alert.show();
                    AitLogger.getInstance().logInfoToFile("Login not found!");
                    loginBox.requestFocus();
                }
            }
        } catch (Exception e) {
            AitLogger.getInstance().logErrorToFile("error: AitLoginController.loginAction()", e);
        }
        AitLogger.getInstance().logInfoToFile("exiting: AitLoginController.loginAction()");
    }
}
