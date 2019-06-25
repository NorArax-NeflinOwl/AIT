package com.arno.daos;

import java.io.Serializable;

public interface IGenericDAO<T, ID extends Serializable> {
    T findById(ID id);
}
