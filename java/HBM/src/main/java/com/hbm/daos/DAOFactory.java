package com.hbm.daos;

import com.hbm.daos.modeldao.AccountDAO;
import com.hbm.daos.modeldao.UserDataDAO;
import org.apache.log4j.Logger;
import org.hibernate.Session;

import java.util.HashMap;
import java.util.Map;

public class DAOFactory {

    private static Logger logger = Logger.getLogger(DAOFactory.class);
    private Session session;
    private Map<Class<?>, GenericDAO<?,?>> createdDAOImpl;

    public DAOFactory(Session session) {
        logger.info("opening: DAOFactory.DAOFactory()");
        this.session = session;
        this.createdDAOImpl = new HashMap<>();
        logger.info("exiting: DAOFactory.DAOFactory()");
    }

    @SuppressWarnings({ "rawtypes", "unchecked" })
    private GenericDAO instantiateDAO(Class daoClass) {
        logger.info("opening: DAOFactory.instantiateDAO()");
        try {
            GenericDAO result = createdDAOImpl.get(daoClass);
            if(result == null) {
                result = (GenericDAO)daoClass.getConstructor(Session.class).newInstance(session);
                createdDAOImpl.put(daoClass, result);
            }
            logger.info("exiting: DAOFactory.instantiateDAO()");
            return result;
        } catch (Exception ex) {
            logger.error("exiting: DAOFactory.instantiateDAO()", ex);
            throw new RuntimeException("Can not instantiate DAO: " + daoClass, ex);
        }
    }

    public AccountDAO getAccountDAO() { return (AccountDAO) instantiateDAO(AccountDAO.class); }

    public UserDataDAO getUserDataDAO() { return (UserDataDAO) instantiateDAO(UserDataDAO.class); }
}
