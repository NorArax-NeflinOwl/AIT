package com.hbm.models.entities;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import java.io.Serializable;
import java.util.Date;

@Entity(name = "accounts")
public class AitAccountEntity implements Serializable {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "acc_id")
    private int id;

    @Column(name = "acc_login")
    private String login;

    @Column(name = "acc_pass")
    private String password;

    @Column(name = "acc_email")
    private String email;

    @Column(name = "acc_active")
    private boolean isActive;

    @Column(name = "acc_create")
    private Date createDate;

    @Column(name = "acc_lastupdate")
    private Date lastUpdate;

    public AitAccountEntity() {}

    public AitAccountEntity(int id, String login, String password, String email, boolean isActive, Date createDate, Date lastUpdate) {
        this.id = id;
        this.login = login;
        this.password = password;
        this.email = email;
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

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
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
}
