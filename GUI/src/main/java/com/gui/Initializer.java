package com.gui;

import com.gui.cultureResources.CultureManager;
import com.gui.strings.Polish;

public class Initializer {
    private static Initializer ourInstance = new Initializer();

    public static Initializer getInstance() {
        return ourInstance;
    }

    private Initializer() {
    }

    public void RegisterAppSettings() {
        CultureManager.getInstance().setLanguage(Polish.locale);
    }
}
