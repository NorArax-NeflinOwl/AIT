package com.hbm.generics;

import com.hbm.hibernate.AitHibernateUtil;
import com.hbm.interfaces.AitGenericDAOInterface;
import com.hbm.managers.AitLogger;
import org.hibernate.Session;

import java.io.Serializable;
import java.lang.reflect.ParameterizedType;

public class AitGenericDAO<T extends Serializable, ID extends Serializable> implements AitGenericDAOInterface<T, ID> {

    private Class<T> persistentClass;
    private Session session;

    protected AitGenericDAO(Session session) {
        AitLogger.getInstance().logInfoToFile("opening: AitGenericDAO.AitGenericDAO(Session)");
        this.session = session;
        this.persistentClass = (Class<T>) ((ParameterizedType) getClass()
                .getGenericSuperclass()).getActualTypeArguments()[0];
        AitLogger.getInstance().logInfoToFile("exiting: AitGenericDAO.AitGenericDAO(Session)");
    }

    protected Session getSession() {
        if(session != null)
            return session;
        return AitHibernateUtil.getInstance().getSessionFactory().getCurrentSession();
    }


    private Class<T> getPersistentClass() {
        return persistentClass;
    }

    @Override
    public T findById(ID id) {
        return getSession().get(getPersistentClass(), id);
    }

    @Override
    public boolean deleteById(ID id) {
        T obj = findById(id);
        getSession().delete(obj);
        return findById(id) == null;
    }
}
