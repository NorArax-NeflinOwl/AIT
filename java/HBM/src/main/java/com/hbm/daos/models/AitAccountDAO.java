package com.hbm.daos.models;

import com.hbm.generics.AitGenericDAO;
import com.hbm.interfaces.AitAccountDAOInterface;
import com.hbm.managers.AitLogger;
import com.hbm.models.entitiecovers.AitAccount;
import com.hbm.models.entities.AitAccountEntity;
import org.hibernate.Session;
import org.hibernate.query.Query;

import java.util.ArrayList;
import java.util.List;

public class AitAccountDAO extends AitGenericDAO<AitAccountEntity, Integer> implements AitAccountDAOInterface {
    public AitAccountDAO(Session session) {
        super(session);
    }

    @Override
    public AitAccount findAccountById(int id) {
        AitLogger.getInstance().logInfoToFile("opening: AitAccountDAO.findAccountById(Integer)");
        AitLogger.getInstance().logInfoToFile("exiting: AitAccountDAO.findAccountById(Integer)");
        return new AitAccount(getSession(), findById(id));
    }

    @Override
    public List<AitAccount> findAccountByLogin(String login) {
        AitLogger.getInstance().logInfoToFile("opening: AitAccountDAO.findAccountByLogin(String)");

        Query query = getSession().createQuery("from accounts where acc_login = :login", AitAccountEntity.class);
        query.setParameter("login", login);

        List<AitAccount> result = new ArrayList<>();
        List<AitAccountEntity> accounts = query.list();
        if(accounts != null && accounts.size() > 0)
        {
            for (AitAccountEntity entity : accounts) {
                result.add(new AitAccount(getSession(), entity));
            }
        }

        AitLogger.getInstance().logInfoToFile("exiting: AitAccountDAO.findAccountByLogin(String)");
        return result;
    }

    @Override
    public AitAccount findAccountByPass(String pass) {
        AitLogger.getInstance().logInfoToFile("opening: AitAccountDAO.findAccountByPass(String)");

        Query query = getSession().createQuery("from accounts where acc_pass = :pass", AitAccountEntity.class);
        query.setParameter("pass", pass);

        List<AitAccountEntity> accounts = query.list();
        if(accounts != null && accounts.size() > 0)
        {
            AitLogger.getInstance().logInfoToFile("exiting: AitAccountDAO.findAccountByPass(String)");
            return new AitAccount(getSession(), accounts.get(0));
        }

        AitLogger.getInstance().logInfoToFile("exiting: AitAccountDAO.findAccountByPass(String)");
        return null;
    }

    @Override
    public List<AitAccount> findAccountByEmail(String email) {
        AitLogger.getInstance().logInfoToFile("opening: AitAccountDAO.findAccountByEmail(String)");

        Query query = getSession().createQuery("from accounts where acc_email = :email", AitAccountEntity.class);
        query.setParameter("email", email);

        List<AitAccount> result = new ArrayList<>();
        List<AitAccountEntity> accounts = query.list();
        if(accounts != null && accounts.size() > 0)
        {
            for (AitAccountEntity entity : accounts) {
                result.add(new AitAccount(getSession(), entity));
            }
        }

        AitLogger.getInstance().logInfoToFile("exiting: AitAccountDAO.findAccountByEmail(String)");
        return result;
    }

    @Override
    public List<AitAccount> findActiveAccounts() {
        AitLogger.getInstance().logInfoToFile("opening: AitAccountDAO.findActiveAccounts()");

        Query query = getSession().createQuery("from accounts where acc_active = 1", AitAccountEntity.class);

        List<AitAccount> result = new ArrayList<>();
        List<AitAccountEntity> accounts = query.list();
        if(accounts != null && accounts.size() > 0)
        {
            for (AitAccountEntity entity : accounts) {
                result.add(new AitAccount(getSession(), entity));
            }
        }

        AitLogger.getInstance().logInfoToFile("exiting: AitAccountDAO.findActiveAccounts()");
        return result;
    }

    @Override
    public List<AitAccount> findNotActiveAccounts() {
        AitLogger.getInstance().logInfoToFile("opening: AitAccountDAO.findNotActiveAccounts()");

        Query query = getSession().createQuery("from accounts where acc_active = 0", AitAccountEntity.class);

        List<AitAccount> result = new ArrayList<>();
        List<AitAccountEntity> accounts = query.list();
        if(accounts != null && accounts.size() > 0)
        {
            for (AitAccountEntity entity : accounts) {
                result.add(new AitAccount(getSession(), entity));
            }
        }

        AitLogger.getInstance().logInfoToFile("exiting: AitAccountDAO.findNotActiveAccounts()");
        return result;
    }

    @Override
    public List<AitAccount> findAllAccount() {
        AitLogger.getInstance().logInfoToFile("opening: AitAccountDAO.findAllAccount()");

        Query query = getSession().createQuery("from accounts", AitAccountEntity.class);

        List<AitAccount> result = new ArrayList<>();
        List<AitAccountEntity> accounts = query.list();
        if(accounts != null && accounts.size() > 0)
        {
            for (AitAccountEntity entity : accounts) {
                result.add(new AitAccount(getSession(), entity));
            }
        }

        AitLogger.getInstance().logInfoToFile("exiting: AitAccountDAO.findAllAccount()");
        return result;
    }
}
