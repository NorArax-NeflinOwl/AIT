package com.gui.frames;

import com.gui.AppGUI;
import com.gui.abstracts.AitGenericController;
import com.gui.context.AitMainContext;
import com.gui.managers.AitCultureManager;
import com.gui.strings.AitControllersNameConstStrings;
import com.hbm.daos.AitDAOFactory;
import com.hbm.models.AitAccount;
import com.ptl.managers.AitCrypter;
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
        logger.info("opening: AitLoginController.initialize()");
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
                    logger.error("error: AitLoginController.setOnAction()", e);
                }
            });
        } catch (Exception e) {
            logger.error("error: AitLoginController.initialize()", e);
        }
        logger.info("exiting: AitLoginController.initialize()");
    }

    private void onKeyReleased(KeyEvent keyEvent) {
        if(keyEvent.getCode().equals(KeyCode.ENTER)) {
            loginAction();
        }
    }

    @FXML
    private void openRegisterFrame() throws Exception {
        logger.info("opening: AitLoginController.openRegisterFrame()");
        AppGUI.setRoot(AitControllersNameConstStrings.REGISTRATION_NAMESPACE, AitControllersNameConstStrings.LOGIN_NAMESPACE, this);
        logger.info("exiting: AitLoginController.openRegisterFrame()");
    }

    @FXML
    private void loginAction() {
        logger.info("opening: AitLoginController.loginAction()");
        try {
            // TODO show progress bar

            String login = loginBox.getText();
            String password = passwordBox.getText();
            boolean rememberMe = rememberCheckBox.isSelected();

            if(login == null || login.length() == 0) {
                Alert alert = new Alert(Alert.AlertType.ERROR, "Please enter login!", ButtonType.OK);
                alert.show();
                logger.info("Empty login!");
                loginBox.requestFocus();
            }
            else if(password == null || password.length() == 0) {
                Alert alert = new Alert(Alert.AlertType.ERROR, "Please enter password!", ButtonType.OK);
                alert.show();
                logger.info("Empty password!");
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
                                logger.info("AitAccount is not activate!");
                                passwordBox.requestFocus();
                                logger.info("exiting: AitLoginController.loginAction()");
                                return;
                            }
                            else {
                                try {
                                    AitMainContext.setUser(acc, rememberMe);
                                } catch (Exception se) {
                                    Alert alert = new Alert(Alert.AlertType.ERROR, "You dont have permition to use this app! Run app as administator!", ButtonType.OK);
                                    alert.showAndWait();
                                    if(alert.getResult() == ButtonType.OK) {
                                        AppGUI.exit();
                                    }
                                }
                                AppGUI.setRoot(AitControllersNameConstStrings.DASHBOARD_NAMESPACE, AitControllersNameConstStrings.LOGIN_NAMESPACE, this);

                                logger.info("exiting: AitLoginController.loginAction() Login Successfull");
                                return;
                            }
                        }
                    }
                    Alert alert = new Alert(Alert.AlertType.ERROR, "Incorect password!", ButtonType.OK);
                    alert.show();
                    logger.info("Incorect password!");
                    passwordBox.requestFocus();
                    logger.info("exiting: AitLoginController.loginAction()");
                    return;
                }
                else {
                    Alert alert = new Alert(Alert.AlertType.ERROR, "Login not found!", ButtonType.OK);
                    alert.show();
                    logger.info("Login not found!");
                    loginBox.requestFocus();
                }
            }
        } catch (Exception e) {
            logger.error("error: AitLoginController.loginAction()", e);
        } finally {
            if(AitMainContext.getSession(false) != null) {
                AitMainContext.getSession(false).close();
            }
        }
        logger.info("exiting: AitLoginController.loginAction()");
    }
}
