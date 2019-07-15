package com.hbm.interfaces;

import java.io.Serializable;

public interface AitGenericModelInterface<T extends Serializable> {
    void saveOrUpdate();
    void delete();
}
