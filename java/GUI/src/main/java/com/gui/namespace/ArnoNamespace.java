package com.gui.namespace;

public class ArnoNamespace implements BaseNamespace {
    private String name = Consts.arno;

    @Override
    public String getName() {
        return name;
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
