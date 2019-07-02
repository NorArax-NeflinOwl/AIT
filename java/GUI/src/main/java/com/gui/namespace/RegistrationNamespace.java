package com.gui.namespace;

import com.gui.cultureResources.CultureManager;
import com.gui.strings.Consts;

public class RegistrationNamespace implements BaseNamespace {
    private String name = Consts.registration;

    @Override
    public String getControllerName() {
        return ControllersName.REGISTRATION_NAMESPACE;
    }

    @Override
    public String getTitle() {
        return CultureManager.getInstance().getLanguage().getRegistrationFrameTitle();
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
        return 440;
    }
}
