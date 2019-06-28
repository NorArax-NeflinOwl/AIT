package com.hbm.datamodels;

import com.hbm.entities.UserDataEntity;

import java.util.Date;

public class UserData {
    private UserDataEntity entity;

    public UserData() {
        entity = new UserDataEntity();
    }

    public UserData(UserDataEntity entity) {
        this.entity = entity;
    }

    public int getId() {
        return entity.getId();
    }

    public void setId(int id) {
        entity.setId(id);
    }

    public Account getAccount() {
        return new Account(entity.getAccount());
    }

    public void setAccount(Account account) {
        entity.setAccount(account.getEntity());
    }

    public String getNick() {
        return entity.getNick();
    }

    public void setNick(String nick) {
        entity.setNick(nick);
    }

    public String getFirstName() {
        return entity.getFirstName();
    }

    public void setFirstName(String firstName) {
        entity.setFirstName(firstName);
    }

    public String getMiddleName() {
        return entity.getMiddleName();
    }

    public void setMiddleName(String middleName) {
        entity.setMiddleName(middleName);
    }

    public String getLastName() {
        return entity.getLastName();
    }

    public void setLastName(String lastName) {
        entity.setLastName(lastName);
    }

    public Date getBirthDate() { return entity.getBirthdate();}

    public void setBirthDate(Date birthDate) {
        entity.setBirthdate(birthDate);
    }

    public Date getLastUpdate() {
        return entity.getLastUpdate();
    }

    public void setLastUpdate(Date lastUpdate) {
        entity.setLastUpdate(lastUpdate);
    }
}
