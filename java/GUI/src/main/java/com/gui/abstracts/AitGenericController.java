package com.gui.abstracts;

import com.gui.interfaces.AitGenericControllerInterface;
import org.apache.log4j.Logger;

import java.io.Serializable;

public abstract class AitGenericController<T, ID extends Serializable> implements AitGenericControllerInterface {
    protected static Logger logger = Logger.getLogger(AitGenericController.class);
}
