package com.hbm.datamodels;

import org.hibernate.Session;

import java.io.Serializable;

public class GenericModel<T extends Serializable> implements IGenericModel<T> {
    protected T entity;

    public T getEntity() { return entity; }

    @Override
    public void saveOrUpdate(Session session) {
        session.saveOrUpdate(entity);
    }

    @Override
    public void delete(Session session) {
        session.delete(entity);
    }
}
