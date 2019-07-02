package com.gui.namespace;

import com.gui.cultureResources.CultureManager;
import com.gui.strings.Consts;

public class DashboardNamespace implements BaseNamespace {
    private String name = Consts.dashboard;
    private String title = null;
    @Override
    public String getControllerName() {
        return ControllersName.DASHBOARD_NAMESPACE;
    }

    @Override
    public String getTitle() {
        String result = CultureManager.getInstance().getLanguage().getDashboardTitle();
        result += title != null ? " [" + title + "]" : "";
        return result;
    }

    @Override
    public void setTitle(String title) {
        this.title = title;
    }

    @Override
    public String getFrame() {
        return name.toLowerCase() + Consts.frame;
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
