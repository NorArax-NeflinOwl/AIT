package com.gui.models;

import com.hbm.datamodels.models.Account;

import java.io.Serializable;

public class AccountSerializableModel implements Serializable {

    private Integer id;
    private String login;
    private String nick;
    private String email;


    public AccountSerializableModel(Integer id, String login, String nick, String email) {
        this.id = id;
        this.login = login;
        this.nick = nick;
        this.email = email;
    }

    public AccountSerializableModel(Account account, String nick) {
        id = account.getID();
        login = account.getLogin();
        email = account.getEmail();

        this.nick = nick;
    }

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public String getLogin() {
        return login;
    }

    public void setLogin(String login) {
        this.login = login;
    }

    public String getNick() {
        return nick;
    }

    public void setNick(String nick) {
        this.nick = nick;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }
}
