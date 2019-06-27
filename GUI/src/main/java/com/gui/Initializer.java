package com.gui;

import com.gui.cultureResources.CultureManager;
import com.gui.strings.Polish;

class Initializer {
    private static Initializer ourInstance = new Initializer();

    static Initializer getInstance() {
        return ourInstance;
    }

    private Initializer() {
    }

    void RegisterAppSettings() {
        CultureManager.getInstance().setLanguage(Polish.locale);
    }
}
