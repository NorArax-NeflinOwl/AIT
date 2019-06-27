package com.hbm.entities;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import java.util.Date;

@Entity(name = "accounts")
public class AccountEntity {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "acc_id")
    private int id;

    @Column(name = "acc_login")
    private String login;

    @Column(name = "acc_pass")
    private String password;

    @Column(name = "acc_email")
    private String mail;

    @Column(name = "acc_active")
    private boolean isActive;

    @Column(name = "acc_create")
    private Date createDate;

    @Column(name = "acc_lastupdate")
    private Date lastUpdate;

    public AccountEntity() {}

    public AccountEntity(int id, String login, String password, String mail, boolean isActive, Date createDate, Date lastUpdate) {
        this.id = id;
        this.login = login;
        this.password = password;
        this.mail = mail;
        this.isActive = isActive;
        this.createDate = createDate;
        this.lastUpdate = lastUpdate;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getLogin() {
        return login;
    }

    public void setLogin(String login) {
        this.login = login;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public String getMail() {
        return mail;
    }

    public void setMail(String mail) {
        this.mail = mail;
    }

    public boolean isActive() {
        return isActive;
    }

    public void setActive(boolean active) {
        isActive = active;
    }

    public Date getCreateDate() {
        return createDate;
    }

    public void setCreateDate(Date createDate) {
        this.createDate = createDate;
    }

    public Date getLastUpdate() {
        return lastUpdate;
    }

    public void setLastUpdate(Date lastUpdate) {
        this.lastUpdate = lastUpdate;
    }

    @Override
    public String toString() {
        return "[" + id + "] " + login + " created " + createDate + " last modificated " + lastUpdate;
    }
}
