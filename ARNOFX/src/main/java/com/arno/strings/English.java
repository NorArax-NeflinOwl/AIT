package com.arno.strings;

import com.arno.cultureResources.Language;

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
    public String getBirthdayPrompt() {
        return "Birthday";
    }

    @Override
    public String getRegisterButtonContent() {
        return "Register";
    }
}
