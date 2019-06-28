package com.hbm.daos;

import java.io.Serializable;

public interface IGenericDAO<T, ID extends Serializable> {
    T findById(ID id);
    boolean deleteById(ID id);
}
