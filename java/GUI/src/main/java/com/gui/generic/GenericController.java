package com.gui.generic;

import org.apache.log4j.Logger;

import java.io.Serializable;

public class GenericController<T, ID extends Serializable> implements IGenericController {
    protected static Logger logger = Logger.getLogger(GenericController.class);
}
