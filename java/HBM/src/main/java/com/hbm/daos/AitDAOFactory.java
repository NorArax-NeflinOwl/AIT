package com.hbm.daos;

import com.hbm.daos.models.AitAccountDAO;
import com.hbm.daos.models.AitNoteDAO;
import com.hbm.daos.models.AitSettingsDAO;
import com.hbm.daos.models.AitUserDataDAO;
import com.hbm.daos.models.AitUserHostDAO;
import com.hbm.generics.AitGenericDAO;
import com.hbm.managers.AitLogger;
import org.hibernate.Session;

import java.util.HashMap;
import java.util.Map;

public class AitDAOFactory {

    private Session session;
    private Map<Class<?>, AitGenericDAO<?,?>> createdDAOImpl;

    public AitDAOFactory(Session session) {
        AitLogger.getInstance().logInfoToFile("opening: AitDAOFactory.AitDAOFactory()");
        this.session = session;
        this.createdDAOImpl = new HashMap<>();
        AitLogger.getInstance().logInfoToFile("exiting: AitDAOFactory.AitDAOFactory()");
    }

    @SuppressWarnings({ "rawtypes", "unchecked" })
    private AitGenericDAO instantiateDAO(Class daoClass) {
        AitLogger.getInstance().logInfoToFile("opening: AitDAOFactory.instantiateDAO()");
        try {
            AitGenericDAO result = createdDAOImpl.get(daoClass);
            if(result == null) {
                result = (AitGenericDAO)daoClass.getConstructor(Session.class).newInstance(session);
                createdDAOImpl.put(daoClass, result);
            }
            AitLogger.getInstance().logInfoToFile("exiting: AitDAOFactory.instantiateDAO()");
            return result;
        } catch (Exception ex) {
            AitLogger.getInstance().logErrorToFile("exiting: AitDAOFactory.instantiateDAO()", ex);
            throw new RuntimeException("Can not instantiate DAO: " + daoClass, ex);
        }
    }

    public AitAccountDAO getAccountDAO() { return (AitAccountDAO) instantiateDAO(AitAccountDAO.class); }

    public AitUserDataDAO getUserDataDAO() { return (AitUserDataDAO) instantiateDAO(AitUserDataDAO.class); }

    public AitSettingsDAO getSettingsDAO() { return (AitSettingsDAO) instantiateDAO(AitSettingsDAO.class); }

    public AitUserHostDAO getUserHostDAO() { return (AitUserHostDAO) instantiateDAO(AitUserHostDAO.class); }

    public AitNoteDAO getNotesDAO() { return (AitNoteDAO) instantiateDAO(AitNoteDAO.class); }
}
