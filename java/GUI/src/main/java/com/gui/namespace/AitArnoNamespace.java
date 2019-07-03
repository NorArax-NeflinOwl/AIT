package com.gui.namespace;

import com.gui.interfaces.AitNamespaceInterface;
import com.gui.strings.AitControllersNameConstStrings;
import com.gui.strings.AitFramesStrings;

public class AitArnoNamespace implements AitNamespaceInterface {
    private String name = AitFramesStrings.arno;

    @Override
    public String getControllerName() {
        return AitControllersNameConstStrings.ARNO_NAMESPACE;
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
        return AitFramesStrings.ait.toLowerCase() + name + AitFramesStrings.frame;
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
