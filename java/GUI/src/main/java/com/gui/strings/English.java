package com.gui.strings;

import com.gui.cultureResources.Language;

public class English implements Language {

    public static String locale = "en_US";

    @Override
    public String getLocale() {
        return locale;
    }

    @Override
    public String getLoginPrompt() {
        return "Login";
    }

    @Override
    public String getPasswordPrompt() {
        return "Password";
    }

    @Override
    public String getRementberMeQuestion() {
        return "Remenber me?";
    }

    @Override
    public String getLoginButtonContent() {
        return "Login";
    }

    @Override
    public String getRegistrationQuestion() {
        return "If you don't have account?";
    }

    @Override
    public String getRegistrationButtonContent() {
        return "Register";
    }

    @Override
    public String getLoginFrameTitle() {
        return "Login";
    }

    @Override
    public String getRegistrationFrameTitle() {
        return "Registration";
    }

    @Override
    public String getReqLabelContent() {
        return "Required fields:";
    }

    @Override
    public String getBackButtonContent() {
        return "<- back";
    }

    @Override
    public String getReqLoginPrompt() {
        return "*Login";
    }

    @Override
    public String getReqPasswordPrompt() {
        return "*Password";
    }

    @Override
    public String getReqRepeatPasswordPrompt() {
        return "*Repeat Password";
    }

    @Override
    public String getReqEmailPrompt() {
        return "*E-mail";
    }

    @Override
    public String getReqRepeatEmailPrompt() {
        return "*Repeat E-mail";
    }

    @Override
    public String getOptionalLabelContent() {
        return "Optional fields:";
    }

    @Override
    public String getOptionalNickPrompt() {
        return "Nick";
    }

    @Override
    public String getOptionalFirstNamePrompt() {
        return "First name";
    }

    @Override
    public String getOptionalMiddleNamePrompt() {
        return "Middle name";
    }

    @Override
    public String getOptionalLastNamePrompt() {
        return "Last name";
    }

    @Override
    public String getBirthdayPrompt() {
        return "Birthday (dd.MM.yyyy)";
    }

    @Override
    public String getRegisterButtonContent() {
        return "Register";
    }

    @Override
    public String getFirstInfoProgress() {
        return "Start application...";
    }

    @Override
    public String getSecondInfoProgress() {
        return "Please wait...";
    }

    @Override
    public String getFinishInfoProgress() {
        return "Finalizing the settings...";
    }

    @Override
    public String getDashboardTitle() {
        return "Dashboard";
    }

    @Override
    public String getFileMenuContent() {
        return "File";
    }

    @Override
    public String getEditMenuContent() {
        return "Edit";
    }

    @Override
    public String getViewMenuContent() {
        return "View";
    }

    @Override
    public String getHelpMenuContent() {
        return "Help";
    }

    @Override
    public String getCloseAllContent() {
        return "Close All";
    }

    @Override
    public String getSettingsContent() {
        return "Settings";
    }

    @Override
    public String getLogoutContent() {
        return "Log Out";
    }

    @Override
    public String getExitContent() {
        return "Exit";
    }

    @Override
    public String getUndoContent() {
        return "Undo";
    }

    @Override
    public String getRedoContent() {
        return "Redo";
    }

    @Override
    public String getCreateAccountContent() {
        return "Create Account";
    }

    @Override
    public String getRegisterProductContent() {
        return "Register Product";
    }

    @Override
    public String getAboutContent() {
        return "About";
    }
}
