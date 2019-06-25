package com.arno.datamodels;

import com.arno.daos.DAOFactory;
import com.arno.entities.AccountEntity;
import com.arno.hibernate.HibernateUtil;
import org.hibernate.Session;

public class Account {
    private AccountEntity entity;

    public Account() {}

    public Account(AccountEntity entity) {
        this.entity = entity;
    }

    public int getID() {
        return entity.getId();
    }

    public UserData getUserData() {
        Session session = HibernateUtil.getInstance().getSessionFactory().getCurrentSession();
        return new DAOFactory(session).getUserDataDAO().findUserDataById(entity.getId());
    }
}
