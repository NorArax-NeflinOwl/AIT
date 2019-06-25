package com.arno.daos;

import com.arno.hibernate.HibernateUtil;
import org.hibernate.Session;

import java.io.Serializable;
import java.lang.reflect.ParameterizedType;

public class GenericDAO<T, ID extends Serializable> implements IGenericDAO<T, ID> {

    private Class<T> persistentClass;
    private Session session;

    protected GenericDAO(Session session) {
        this.session = session;
        this.persistentClass = (Class<T>) ((ParameterizedType) getClass()
                .getGenericSuperclass()).getActualTypeArguments()[0];
    }

    public Session getSession() {
        if(session != null)
            return session;
        return HibernateUtil.getInstance().getSessionFactory().getCurrentSession();
    }


    protected Class<T> getPersistentClass() {
        return persistentClass;
    }

    @Override
    public T findById(ID id) {
        return (T) getSession().get(getPersistentClass(), id);
    }
}
