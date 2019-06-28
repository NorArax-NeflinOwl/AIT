package com.hbm.daos.modeldao;

import com.hbm.daos.GenericDAO;
import com.hbm.daos.imodeldao.IAccountDAO;
import com.hbm.datamodels.models.Account;
import com.hbm.entities.AccountEntity;
import org.hibernate.Session;
import org.hibernate.query.Query;

import java.util.ArrayList;
import java.util.List;

public class AccountDAO extends GenericDAO<AccountEntity, Integer> implements IAccountDAO {
    public AccountDAO(Session session) {
        super(session);
    }

    @Override
    public Account findAccountById(int id) {
        logger.info("opening: AccountDAO.findAccountById(Integer)");
        logger.info("exiting: AccountDAO.findAccountById(Integer)");
        return new Account(getSession(), findById(id));
    }

    @Override
    public List<Account> findAccountByLogin(String login) {
        logger.info("opening: AccountDAO.findAccountByLogin(String)");

        Query query = getSession().createQuery("from accounts where acc_login = :login", AccountEntity.class);
        query.setParameter("login", login);

        List<Account> result = new ArrayList<>();
        List<AccountEntity> accounts = query.list();
        if(accounts != null && accounts.size() > 0)
        {
            for (AccountEntity entity : accounts) {
                result.add(new Account(getSession(), entity));
            }
        }

        logger.info("exiting: AccountDAO.findAccountByLogin(String)");
        return result;
    }

    @Override
    public Account findAccountByPass(String pass) {
        logger.info("opening: AccountDAO.findAccountByPass(String)");

        Query query = getSession().createQuery("from accounts where acc_pass = :pass", AccountEntity.class);
        query.setParameter("pass", pass);

        List<AccountEntity> accounts = query.list();
        if(accounts != null && accounts.size() > 0)
        {
            logger.info("exiting: AccountDAO.findAccountByPass(String)");
            return new Account(getSession(), accounts.get(0));
        }

        logger.info("exiting: AccountDAO.findAccountByPass(String)");
        return null;
    }

    @Override
    public List<Account> findAccountByEmail(String email) {
        logger.info("opening: AccountDAO.findAccountByEmail(String)");

        Query query = getSession().createQuery("from accounts where acc_email = :email", AccountEntity.class);
        query.setParameter("email", email);

        List<Account> result = new ArrayList<>();
        List<AccountEntity> accounts = query.list();
        if(accounts != null && accounts.size() > 0)
        {
            for (AccountEntity entity : accounts) {
                result.add(new Account(getSession(), entity));
            }
        }

        logger.info("exiting: AccountDAO.findAccountByEmail(String)");
        return result;
    }

    @Override
    public List<Account> findActiveAccounts() {
        logger.info("opening: AccountDAO.findActiveAccounts()");

        Query query = getSession().createQuery("from accounts where acc_active = 1", AccountEntity.class);

        List<Account> result = new ArrayList<>();
        List<AccountEntity> accounts = query.list();
        if(accounts != null && accounts.size() > 0)
        {
            for (AccountEntity entity : accounts) {
                result.add(new Account(getSession(), entity));
            }
        }

        logger.info("exiting: AccountDAO.findActiveAccounts()");
        return result;
    }

    @Override
    public List<Account> findNotActiveAccounts() {
        logger.info("opening: AccountDAO.findNotActiveAccounts()");

        Query query = getSession().createQuery("from accounts where acc_active = 0", AccountEntity.class);

        List<Account> result = new ArrayList<>();
        List<AccountEntity> accounts = query.list();
        if(accounts != null && accounts.size() > 0)
        {
            for (AccountEntity entity : accounts) {
                result.add(new Account(getSession(), entity));
            }
        }

        logger.info("exiting: AccountDAO.findNotActiveAccounts()");
        return result;
    }

    @Override
    public List<Account> findAllAccount() {
        logger.info("opening: AccountDAO.findAllAccount()");

        Query query = getSession().createQuery("from accounts", AccountEntity.class);

        List<Account> result = new ArrayList<>();
        List<AccountEntity> accounts = query.list();
        if(accounts != null && accounts.size() > 0)
        {
            for (AccountEntity entity : accounts) {
                result.add(new Account(getSession(), entity));
            }
        }

        logger.info("exiting: AccountDAO.findAllAccount()");
        return result;
    }
}
