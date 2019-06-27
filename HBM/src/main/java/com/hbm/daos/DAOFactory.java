package com.hbm.daos;

import com.hbm.daos.modeldao.AccountDAO;
import com.hbm.daos.modeldao.UserDataDAO;
import org.hibernate.Session;

import java.util.HashMap;
import java.util.Map;

public class DAOFactory {

    private Session session;
    private Map<Class<?>, GenericDAO<?,?>> createdDAOImpl;

    public DAOFactory(Session session) {
        this.session = session;
        this.createdDAOImpl = new HashMap<>();
    }

    @SuppressWarnings({ "rawtypes", "unchecked" })
    private GenericDAO instantiateDAO(Class daoClass) {
        try {
            GenericDAO result = createdDAOImpl.get(daoClass);
            if(result == null) {
                result = (GenericDAO)daoClass.getConstructor(Session.class).newInstance(session);
                createdDAOImpl.put(daoClass, result);
            }
            return result;
        } catch (Exception ex) {
            throw new RuntimeException("Can not instantiate DAO: " + daoClass, ex);
        }
    }

    public AccountDAO getAccountDAO() { return (AccountDAO) instantiateDAO(AccountDAO.class); }

    public UserDataDAO getUserDataDAO() { return (UserDataDAO) instantiateDAO(UserDataDAO.class); }
}
