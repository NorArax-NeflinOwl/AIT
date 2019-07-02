package com.gui.namespace;

import com.gui.cultureResources.CultureManager;
import com.gui.strings.Consts;

public class LoginNamespace implements BaseNamespace {
    private String name = Consts.login;

    @Override
    public String getControllerName() {
        return ControllersName.LOGIN_NAMESPACE;
    }

    @Override
    public String getTitle() {
        return CultureManager.getInstance().getLanguage().getLoginFrameTitle();
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
        return 400;
    }

    @Override
    public double getHeight() {
        return 300;
    }
}


