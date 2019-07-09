package com.hbm.models;

import com.hbm.generics.AitGenericModel;
import com.hbm.models.entities.AitUserDataEntity;
import org.hibernate.Session;

import java.io.Serializable;
import java.util.Date;

public class AitUserData extends AitGenericModel<AitUserDataEntity> implements Serializable {

    public AitUserData(Session session) {
        super(session);
        entity = new AitUserDataEntity();
    }

    public AitUserData(Session session, AitUserDataEntity entity) {
        super(session);
        this.entity = entity;
    }

    public int getId() {
        return entity.getId();
    }

    public void setId(int id) {
        entity.setId(id);
    }

    public AitAccount getAccount() {
        return new AitAccount(getSession(), entity.getAccount());
    }

    public void setAccount(AitAccount account) {
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
