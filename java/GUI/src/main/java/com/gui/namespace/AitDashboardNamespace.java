package com.gui.namespace;

import com.gui.interfaces.AitNamespaceInterface;
import com.gui.managers.AitCultureManager;
import com.gui.strings.AitControllersNameConstStrings;
import com.gui.strings.AitFramesStrings;

public class AitDashboardNamespace implements AitNamespaceInterface {
    private String name = AitFramesStrings.dashboard;
    private String title = null;
    @Override
    public String getControllerName() {
        return AitControllersNameConstStrings.DASHBOARD_NAMESPACE;
    }

    @Override
    public String getTitle() {
        String result = AitCultureManager.getInstance().getLanguage().getDashboardTitle();
        result += title != null ? " [" + title + "]" : "";
        return result;
    }

    @Override
    public void setTitle(String title) {
        this.title = title;
    }

    @Override
    public String getPanel() {
        return AitFramesStrings.ait.toLowerCase() + name + AitFramesStrings.panel;
    }

    @Override
    public double getWigth() {
        return 1000;
    }

    @Override
    public double getHeight() {
        return 700;
    }
}
