package com.hbm.generics;

import com.hbm.hibernate.AitMainContext;
import com.hbm.interfaces.AitGenericModelInterface;
import org.hibernate.Session;

import java.io.Serializable;

public class AitGenericModel<T extends Serializable> extends AitMainContext implements AitGenericModelInterface<T> {
    protected T entity;

    public AitGenericModel(Session session) {
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
