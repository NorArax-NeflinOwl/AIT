package com.gui.namespace;

import com.gui.managers.AitCultureManager;
import com.gui.interfaces.AitNamespaceInterface;
import com.gui.strings.AitFramesStrings;
import com.gui.strings.AitControllersNameConstStrings;

public class AitLoginNamespace implements AitNamespaceInterface {
    private String name = AitFramesStrings.login;

    @Override
    public String getControllerName() {
        return AitControllersNameConstStrings.LOGIN_NAMESPACE;
    }

    @Override
    public String getTitle() {
        return AitCultureManager.getInstance().getLanguage().getLoginFrameTitle();
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
        return 400;
    }

    @Override
    public double getHeight() {
        return 300;
    }
}


