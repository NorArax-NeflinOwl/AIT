package com.gui.frames;

import com.gui.AppGUI;
import com.gui.context.MainContext;
import com.gui.cultureResources.CultureManager;
import com.gui.generic.GenericController;
import com.gui.generic.IGenericController;
import com.gui.namespace.ControllersName;
import com.hbm.daos.DAOFactory;
import com.hbm.datamodels.models.Account;
import com.hbm.datamodels.models.UserData;
import com.hbm.entities.AccountEntity;
import com.hbm.entities.UserDataEntity;
import com.ptl.managers.AitCrypter;
import javafx.fxml.FXML;
import javafx.scene.control.*;

import java.time.ZoneId;
import java.util.Calendar;
import java.util.Date;
import java.util.List;

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
    public Button backButton;

    @FXML
    public void initialize() {
        logger.info("opening: RegistraionController.initialize()");
        try {
            reqLabel.setText(CultureManager.getInstance().getLanguage().getReqLabelContent());
            backButton.setText(CultureManager.getInstance().getLanguage().getBackButtonContent());
            backButton.setOnAction(actionEvent -> {
                try {
                    onBackAction();
                } catch (Exception e) {
                    e.printStackTrace();
                }
            });
            loginBox.setPromptText(CultureManager.getInstance().getLanguage().getReqLoginPrompt());
            passwordBox.setPromptText(CultureManager.getInstance().getLanguage().getReqPasswordPrompt());
            repeatPasswordBox.setPromptText(CultureManager.getInstance().getLanguage().getReqRepeatPasswordPrompt());
            emailBox.setPromptText(CultureManager.getInstance().getLanguage().getReqEmailPrompt());
            repeatEmailBox.setPromptText(CultureManager.getInstance().getLanguage().getReqRepeatEmailPrompt());
            optionalLabel.setText(CultureManager.getInstance().getLanguage().getOptionalLabelContent());
            nickBox.setPromptText(CultureManager.getInstance().getLanguage().getOptionalNickPrompt());
            firstNameBox.setPromptText(CultureManager.getInstance().getLanguage().getOptionalFirstNamePrompt());
            middleNameBox.setPromptText(CultureManager.getInstance().getLanguage().getOptionalMiddleNamePrompt());
            lastNameBox.setPromptText(CultureManager.getInstance().getLanguage().getOptionalLastNamePrompt());
            birthdateBox.setPromptText(CultureManager.getInstance().getLanguage().getBirthdayPrompt());
            registerButton.setText(CultureManager.getInstance().getLanguage().getRegisterButtonContent());
            registerButton.setOnAction(actionEvent -> onRegisterAction());

            if(AppGUI.peekStack() != MainContext.getNamespace(ControllersName.LOGIN_NAMESPACE)) {
                backButton.setVisible(false);
            }

        } catch (Exception e) {
            logger.error("error: RegistrationController.initialize()", e);
        }
        logger.info("exiting: RegistraionController.initialize()");
    }

    @FXML
    public void onRegisterAction() {
        logger.info("opening: RegistraionController.onRegisterAction()");
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

                    DAOFactory dao = new DAOFactory(MainContext.getSession(true));
                    List<UserData> datas = dao.getUserDataDAO().findUserDataByNick(nick);

                    if(nick.length() >= 8 && (datas.isEmpty())) {
                        createUserData = true;
                    } else if(nick.length() > 0) {
                        Alert alert = new Alert(Alert.AlertType.WARNING, "Nick must be >= 8 characters!", ButtonType.OK);
                        alert.show();
                        nickBox.requestFocus();
                        nickBox.clear();
                        logger.info("exiting: RegistraionController.onRegisterAction()");
                        return;
                    } else if (!datas.isEmpty()) {
                        Alert alert = new Alert(Alert.AlertType.WARNING, "Nick must be unique!", ButtonType.OK);
                        alert.show();
                        nickBox.requestFocus();
                        nickBox.clear();
                        logger.info("exiting: RegistraionController.onRegisterAction()");
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
                    logger.info("exiting: RegistraionController.onRegisterAction()");
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
                        logger.info("exiting: RegistraionController.onRegisterAction()");
                        return;
                    }
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

                AppGUI.setRoot(ControllersName.LOGIN_NAMESPACE, ControllersName.REGISTRATION_NAMESPACE, this);
                IGenericController controller = MainContext.getController(ControllersName.LOGIN_NAMESPACE);
                if(controller != null) {
                    LoginController loginController = (LoginController)controller;
                    loginController.loginBox.setText(login);
                    loginController.passwordBox.requestFocus();
                }
                // TODO send activation email


                Alert alert = new Alert(Alert.AlertType.INFORMATION, "Account was create successfull, check your email.", ButtonType.OK);
                alert.show();
            }
        } catch (Exception e) {
            if(MainContext.getSession(false) != null) {
                MainContext.getSession(false).getTransaction().rollback();
            }
            logger.error("error: RegistraionController.onRegisterAction()", e);
        } finally {
            if(MainContext.getSession(false) != null) {
                MainContext.getSession(false).close();
            }
        }

        logger.info("exiting: RegistraionController.onRegisterAction()");
    }

    public void onBackAction() {
        logger.info("opening: RegistraionController.onRegisterAction()");
        AppGUI.back();
        logger.info("exiting: RegistraionController.onRegisterAction()");
    }

}