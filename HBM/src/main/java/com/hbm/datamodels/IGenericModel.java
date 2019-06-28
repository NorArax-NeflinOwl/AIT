package com.hbm.datamodels;

import org.hibernate.Session;

import java.io.Serializable;

public interface IGenericModel<T extends Serializable> {
    void saveOrUpdate(Session session);
    void delete(Session session);
}
