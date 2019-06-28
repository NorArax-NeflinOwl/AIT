package com.hbm.datamodels;

import com.hbm.hibernate.MainContext;
import org.hibernate.Session;

import java.io.Serializable;

public class GenericModel<T extends Serializable> extends MainContext implements IGenericModel<T> {
    protected T entity;

    public GenericModel(Session session) {
        super(session);
    }

    public T getEntity() { return entity; }

    @Override
    public void saveOrUpdate() {
        getSession().saveOrUpdate(entity);
    }

    @Override
    public void delete() {
        getSession().delete(entity);
    }
}
