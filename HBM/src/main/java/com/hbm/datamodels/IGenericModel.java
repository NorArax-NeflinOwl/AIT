package com.hbm.datamodels;

import java.io.Serializable;

public interface IGenericModel<T extends Serializable> {
    void saveOrUpdate();
    void delete();
}
