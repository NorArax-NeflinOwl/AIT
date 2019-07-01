package com.gui.strings;

import com.gui.cultureResources.Language;

public class Polish implements Language {

    public static String locale = "pl_PL";

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
        return "Hasło";
    }

    @Override
    public String getRementberMeQuestion() {
        return "Automatyczne logowanie?";
    }

    @Override
    public String getLoginButtonContent() {
        return "Zaloguj";
    }

    @Override
    public String getRegistrationQuestion() {
        return "Nie masz konta?";
    }

    @Override
    public String getRegistrationButtonContent() {
        return "Zarejestruj się";
    }

    @Override
    public String getLoginFrameTitle() {
        return "Logowanie";
    }

    @Override
    public String getRegistrationFrameTitle() {
        return "Rejestracja";
    }

    @Override
    public String getReqLabelContent() {
        return "Pola wymagane:";
    }

    @Override
    public String getBackButtonContent() {
        return "<- powrót";
    }

    @Override
    public String getReqLoginPrompt() {
        return "*Login";
    }

    @Override
    public String getReqPasswordPrompt() {
        return "*Hasło";
    }

    @Override
    public String getReqRepeatPasswordPrompt() {
        return "*Powtórz hasło";
    }

    @Override
    public String getReqEmailPrompt() {
        return "*E-mail";
    }

    @Override
    public String getReqRepeatEmailPrompt() {
        return "*Powtórz E-mail";
    }

    @Override
    public String getOptionalLabelContent() {
        return "Pola opcjonalne:";
    }

    @Override
    public String getOptionalNickPrompt() {
        return "Nazwa użytkownika";
    }

    @Override
    public String getOptionalFirstNamePrompt() {
        return "Imię";
    }

    @Override
    public String getOptionalMiddleNamePrompt() {
        return "Drugie imię";
    }

    @Override
    public String getOptionalLastNamePrompt() {
        return "Nazwisko";
    }

    @Override
    public String getBirthdayPrompt() {
        return "Data urodzenia (dd.MM.yyyy)";
    }

    @Override
    public String getRegisterButtonContent() {
        return "Rejestruj";
    }

    @Override
    public String getFirstInfoProgress() {
        return "Uruchamianie aplikacji...";
    }

    @Override
    public String getSecondInfoProgress() {
        return "Proszę czekać...";
    }

    @Override
    public String getFinishInfoProgress() {
        return "Finalizowanie ustawień...";
    }
}
