package com.gui.namespace;

import com.gui.cultureResources.CultureManager;
import com.gui.strings.Consts;

public class DashboardNamespace implements BaseNamespace {
    private String name = Consts.dashboard;
    @Override
    public String getControllerName() {
        return ControllersName.DASHBOARD_NAMESPACE;
    }

    @Override
    public String getName() {
        return CultureManager.getInstance().getLanguage().getDashboardTitle();
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
