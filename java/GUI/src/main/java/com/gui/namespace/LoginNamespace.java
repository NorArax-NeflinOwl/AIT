package com.gui.namespace;

import com.gui.cultureResources.CultureManager;

public class LoginNamespace implements BaseNamespace {
    private String name = Consts.login;

    @Override
    public String getName() throws Exception {
        return CultureManager.getInstance().getLanguage().getLoginFrameTitle();
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


