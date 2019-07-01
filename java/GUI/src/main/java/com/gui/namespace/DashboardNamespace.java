package com.gui.namespace;

public class DashboardNamespace implements BaseNamespace {
    @Override
    public String getControllerName() {
        return ControllersName.DASHBOARD_NAMESPACE;
    }

    @Override
    public String getName() throws Exception {
        return null;
    }

    @Override
    public String getFrame() {
        return null;
    }

    @Override
    public double getWigth() {
        return 0;
    }

    @Override
    public double getHeight() {
        return 0;
    }
}
