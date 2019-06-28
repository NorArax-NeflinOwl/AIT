package com.hbm.daos;

import com.hbm.hibernate.HibernateUtil;
import org.apache.log4j.Logger;
import org.hibernate.Session;

import java.io.Serializable;
import java.lang.reflect.ParameterizedType;

public class GenericDAO<T, ID extends Serializable> implements IGenericDAO<T, ID> {

    protected static Logger logger = Logger.getLogger(GenericDAO.class);
    private Class<T> persistentClass;
    private Session session;

    protected GenericDAO(Session session) {
        logger.info("opening: GenericDAO.GenericDAO(Session)");
        this.session = session;
        this.persistentClass = (Class<T>) ((ParameterizedType) getClass()
                .getGenericSuperclass()).getActualTypeArguments()[0];
        logger.info("exiting: GenericDAO.GenericDAO(Session)");
    }

    protected Session getSession() {
        if(session != null)
            return session;
        return HibernateUtil.getInstance().getSessionFactory().getCurrentSession();
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
