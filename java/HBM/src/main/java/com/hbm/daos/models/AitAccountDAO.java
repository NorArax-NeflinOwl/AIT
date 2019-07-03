package com.hbm.daos.models;

import com.hbm.models.entities.AitAccountEntity;
import com.hbm.generics.AitGenericDAO;
import com.hbm.interfaces.AitAccountDAOInterface;
import com.hbm.models.AitAccount;
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
        logger.info("opening: AitAccountDAO.findAccountById(Integer)");
        logger.info("exiting: AitAccountDAO.findAccountById(Integer)");
        return new AitAccount(getSession(), findById(id));
    }

    @Override
    public List<AitAccount> findAccountByLogin(String login) {
        logger.info("opening: AitAccountDAO.findAccountByLogin(String)");

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

        logger.info("exiting: AitAccountDAO.findAccountByLogin(String)");
        return result;
    }

    @Override
    public AitAccount findAccountByPass(String pass) {
        logger.info("opening: AitAccountDAO.findAccountByPass(String)");

        Query query = getSession().createQuery("from accounts where acc_pass = :pass", AitAccountEntity.class);
        query.setParameter("pass", pass);

        List<AitAccountEntity> accounts = query.list();
        if(accounts != null && accounts.size() > 0)
        {
            logger.info("exiting: AitAccountDAO.findAccountByPass(String)");
            return new AitAccount(getSession(), accounts.get(0));
        }

        logger.info("exiting: AitAccountDAO.findAccountByPass(String)");
        return null;
    }

    @Override
    public List<AitAccount> findAccountByEmail(String email) {
        logger.info("opening: AitAccountDAO.findAccountByEmail(String)");

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

        logger.info("exiting: AitAccountDAO.findAccountByEmail(String)");
        return result;
    }

    @Override
    public List<AitAccount> findActiveAccounts() {
        logger.info("opening: AitAccountDAO.findActiveAccounts()");

        Query query = getSession().createQuery("from accounts where acc_active = 1", AitAccountEntity.class);

        List<AitAccount> result = new ArrayList<>();
        List<AitAccountEntity> accounts = query.list();
        if(accounts != null && accounts.size() > 0)
        {
            for (AitAccountEntity entity : accounts) {
                result.add(new AitAccount(getSession(), entity));
            }
        }

        logger.info("exiting: AitAccountDAO.findActiveAccounts()");
        return result;
    }

    @Override
    public List<AitAccount> findNotActiveAccounts() {
        logger.info("opening: AitAccountDAO.findNotActiveAccounts()");

        Query query = getSession().createQuery("from accounts where acc_active = 0", AitAccountEntity.class);

        List<AitAccount> result = new ArrayList<>();
        List<AitAccountEntity> accounts = query.list();
        if(accounts != null && accounts.size() > 0)
        {
            for (AitAccountEntity entity : accounts) {
                result.add(new AitAccount(getSession(), entity));
            }
        }

        logger.info("exiting: AitAccountDAO.findNotActiveAccounts()");
        return result;
    }

    @Override
    public List<AitAccount> findAllAccount() {
        logger.info("opening: AitAccountDAO.findAllAccount()");

        Query query = getSession().createQuery("from accounts", AitAccountEntity.class);

        List<AitAccount> result = new ArrayList<>();
        List<AitAccountEntity> accounts = query.list();
        if(accounts != null && accounts.size() > 0)
        {
            for (AitAccountEntity entity : accounts) {
                result.add(new AitAccount(getSession(), entity));
            }
        }

        logger.info("exiting: AitAccountDAO.findAllAccount()");
        return result;
    }
}
