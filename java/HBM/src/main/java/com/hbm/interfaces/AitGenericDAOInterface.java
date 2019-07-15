package com.hbm.interfaces;


import java.io.Serializable;

public interface AitGenericDAOInterface<T extends Serializable, ID extends Serializable> {
    T findById(ID id);
    boolean deleteById(ID id);
}
