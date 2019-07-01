package com.gui;

import com.gui.context.MainContext;
import com.gui.cultureResources.CultureManager;
import com.gui.generic.GenericController;
import com.gui.namespace.ControllersName;
import com.hbm.daos.DAOFactory;
import com.hbm.datamodels.models.Account;
import com.ptl.managers.AitCrypter;
import javafx.fxml.FXML;
import javafx.scene.control.Alert;
import javafx.scene.control.Button;
import javafx.scene.control.ButtonType;
import javafx.scene.control.CheckBox;
import javafx.scene.control.Label;
import javafx.scene.control.PasswordField;
import javafx.scene.control.TextField;

import java.util.List;

public class LoginController extends GenericController<LoginController, Integer>{
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
        logger.info("opening: LoginController.initialize()");
        try {
            loginBox.setPromptText(CultureManager.getInstance().getLanguage().getLoginPrompt());
            passwordBox.setPromptText(CultureManager.getInstance().getLanguage().getPasswordPrompt());
            rememberCheckBox.setText(CultureManager.getInstance().getLanguage().getRementberMeQuestion());
            loginButton.setText(CultureManager.getInstance().getLanguage().getLoginButtonContent());
            loginButton.setOnAction(actionEvent -> loginAction());
            registrationQuestion.setText(CultureManager.getInstance().getLanguage().getRegistrationQuestion());
            registrationButton.setText(CultureManager.getInstance().getLanguage().getRegistrationButtonContent());
            registrationButton.setOnAction(actionEvent -> {
                try {
                    openRegisterFrame();
                } catch (Exception e) {
                    logger.error("error: LoginController.setOnAction()", e);
                }
            });
        } catch (Exception e) {
            logger.error("error: LoginController.initialize()", e);
        }
        logger.info("exiting: LoginController.initialize()");
    }

    @FXML
    private void openRegisterFrame() throws Exception {
        logger.info("opening: LoginController.openRegisterFrame()");
        AppGUI.setRoot(ControllersName.REGISTRATION_NAMESPACE, this);
        logger.info("exiting: LoginController.openRegisterFrame()");
    }

    @FXML
    private void loginAction() {
        logger.info("opening: LoginController.loginAction()");
        try {
            // TODO show progress bar

            String login = loginBox.getText();
            String password = passwordBox.getText();

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
                DAOFactory dao = new DAOFactory(MainContext.getSession(true));
                List<Account> accounts = dao.getAccountDAO().findAccountByLogin(login);
                if(accounts != null && accounts.size() > 0) {
                    for (Account acc : accounts) {
                        if(AitCrypter.generateMD5Hash(password).equals(acc.getPassword())) {
                            if(!acc.IsActive()) {
                                Alert alert = new Alert(Alert.AlertType.ERROR, "Account is not activate!", ButtonType.OK);
                                alert.show();
                                logger.info("Account is not activate!");
                                passwordBox.requestFocus();
                                logger.info("exiting: LoginController.loginAction()");
                                return;
                            }
                            else {
                                // TODO login successfull
                                MainContext.setUser(acc);
                                AppGUI.setRoot(ControllersName.DASHBOARD_NAMESPACE, this);

                                logger.info("exiting: LoginController.loginAction() Login Successfull");
                                return;
                            }
                        }
                    }
                    Alert alert = new Alert(Alert.AlertType.ERROR, "Incorect password!", ButtonType.OK);
                    alert.show();
                    logger.info("Incorect password!");
                    passwordBox.requestFocus();
                    logger.info("exiting: LoginController.loginAction()");
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
            logger.error("error: LoginController.loginAction()", e);
        } finally {
            if(MainContext.getSession(false) != null) {
                MainContext.getSession(false).close();
            }
        }
        logger.info("exiting: LoginController.loginAction()");
    }
}
