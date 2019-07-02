package com.gui.namespace;

import com.gui.strings.Consts;

public class ArnoNamespace implements BaseNamespace {
    private String name = Consts.arno;

    @Override
    public String getControllerName() {
        return ControllersName.ARNO_NAMESPACE;
    }

    @Override
    public String getTitle() {
        return name;
    }

    @Override
    public void setTitle(String title) {

    }

    @Override
    public String getFrame() {
        return name.toLowerCase() + Consts.frame;
    }

    @Override
    public double getWigth() {
        return 390;
    }

    @Override
    public double getHeight() {
        return 225;
    }
}
