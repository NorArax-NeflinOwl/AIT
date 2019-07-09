package com.hbm.daos;

import com.hbm.daos.models.AitAccountDAO;
import com.hbm.daos.models.AitSettingsDAO;
import com.hbm.daos.models.AitUserDataDAO;
import com.hbm.generics.AitGenericDAO;
import org.apache.log4j.Logger;
import org.hibernate.Session;

import java.util.HashMap;
import java.util.Map;

public class AitDAOFactory {

    private static Logger logger = Logger.getLogger(AitDAOFactory.class);
    private Session session;
    private Map<Class<?>, AitGenericDAO<?,?>> createdDAOImpl;

    public AitDAOFactory(Session session) {
        logger.info("opening: AitDAOFactory.AitDAOFactory()");
        this.session = session;
        this.createdDAOImpl = new HashMap<>();
        logger.info("exiting: AitDAOFactory.AitDAOFactory()");
    }

    @SuppressWarnings({ "rawtypes", "unchecked" })
    private AitGenericDAO instantiateDAO(Class daoClass) {
        logger.info("opening: AitDAOFactory.instantiateDAO()");
        try {
            AitGenericDAO result = createdDAOImpl.get(daoClass);
            if(result == null) {
                result = (AitGenericDAO)daoClass.getConstructor(Session.class).newInstance(session);
                createdDAOImpl.put(daoClass, result);
            }
            logger.info("exiting: AitDAOFactory.instantiateDAO()");
            return result;
        } catch (Exception ex) {
            logger.error("exiting: AitDAOFactory.instantiateDAO()", ex);
            throw new RuntimeException("Can not instantiate DAO: " + daoClass, ex);
        }
    }

    public AitAccountDAO getAccountDAO() { return (AitAccountDAO) instantiateDAO(AitAccountDAO.class); }

    public AitUserDataDAO getUserDataDAO() { return (AitUserDataDAO) instantiateDAO(AitUserDataDAO.class); }

    public AitSettingsDAO getSettingsDAO() { return (AitSettingsDAO) instantiateDAO(AitSettingsDAO.class); }
}
