package com.arno.namespace;

import com.arno.cultureResources.CultureManager;

public class RegistrationNamespace implements BaseNamespace {
    private String name = Consts.registration;

    @Override
    public String getName() throws Exception {
        return CultureManager.getInstance().getLanguage().getRegistrationFrameTitle();
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
        return 280;
    }
}
