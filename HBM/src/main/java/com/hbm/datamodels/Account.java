package com.hbm.datamodels;

import com.hbm.daos.DAOFactory;
import com.hbm.entities.AccountEntity;
import com.hbm.hibernate.HibernateUtil;
import org.hibernate.Session;

import java.util.Date;

public class Account {
    private AccountEntity entity;

    public Account() {
        entity = new AccountEntity();
    }

    public Account(AccountEntity entity) {
        this.entity = entity;
    }

    public static Account getAccountById(Integer id) {
        Session session = HibernateUtil.getInstance().getSessionFactory().getCurrentSession();
        return new Account(new DAOFactory(session).getAccountDAO().findById(id));
    }

    public AccountEntity getEntity() { return entity; }

    public int getID() {
        return entity.getId();
    }

    public void setID(long id) {
        entity.setId((int) id);
    }

    public UserData getUserData() {
        Session session = HibernateUtil.getInstance().getSessionFactory().getCurrentSession();
        return new DAOFactory(session).getUserDataDAO().findUserDataById(entity.getId());
    }

    public void setUserData(UserData data) {
        // TODO update userdata
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
