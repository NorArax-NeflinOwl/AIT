package com.hbm.models;

import com.hbm.daos.AitDAOFactory;
import com.hbm.generics.AitGenericModel;
import com.hbm.entities.AitAccountEntity;
import org.hibernate.Session;

import java.io.Serializable;
import java.util.Date;

public class AitAccount extends AitGenericModel<AitAccountEntity> implements Serializable {

    public AitAccount(Session session) {
        super(session);
        entity = new AitAccountEntity();
    }

    public AitAccount(Session session, AitAccountEntity entity) {
        super(session);
        this.entity = entity;
    }

    public int getID() {
        return entity.getId();
    }

    public void setID(long id) {
        entity.setId((int) id);
    }

    public AitUserData getUserData() {
        return new AitDAOFactory(getSession()).getUserDataDAO().findUserDataById(entity.getId());
    }

    public void setUserData(AitUserData data) {
        data.saveOrUpdate();
    }

    public String getLogin() {
        return entity.getLogin();
    }

    public void setLogin(String login) {
        entity.setLogin(login);
    }

    public String getPassword() {
        return entity.getPassword();
    }

    public void setPassword(String password) {
        entity.setPassword(password);
    }

    public String getEmail() {
        return entity.getEmail();
    }

    public void setEmail(String email) {
        entity.setEmail(email);
    }

    public boolean IsActive() { return entity.isActive(); }

    public void setActive(boolean isActive) {
        entity.setActive(isActive);
    }

    public Date getCreateDate() {
        return entity.getCreateDate();
    }

    public void setCreateDate(Date createDate) {
        entity.setCreateDate(createDate);
    }

    public Date getLastUpdate() {
        return entity.getLastUpdate();
    }

    public void setLastUpdate(Date lastUpdate) {
        entity.setLastUpdate(lastUpdate);
    }

}
