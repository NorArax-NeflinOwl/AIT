package com.gui.namespace;

import com.gui.interfaces.AitNamespaceInterface;
import com.gui.managers.AitCultureManager;
import com.gui.strings.AitControllersNameConstStrings;
import com.gui.strings.AitFramesStrings;

public class AitRegistrationNamespace implements AitNamespaceInterface {
    private String name = AitFramesStrings.registration;

    @Override
    public String getControllerName() {
        return AitControllersNameConstStrings.REGISTRATION_NAMESPACE;
    }

    @Override
    public String getTitle() {
        return AitCultureManager.getInstance().getLanguage().getRegistrationFrameTitle();
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
        return 440;
    }
}
