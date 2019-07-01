package com.gui.context;

import com.gui.cultureResources.CultureManager;
import com.gui.strings.Polish;
import org.apache.log4j.Logger;

public class Initializer {
    private static Logger logger = Logger.getLogger(MainContext.class);

    private static Initializer ourInstance = new Initializer();

    public static Initializer getInstance() {
        return ourInstance;
    }

    private Initializer() {
    }

    public void registerAppSettings() {
        logger.info("opening: Initializer.registerAppSettings()");
        CultureManager.getInstance().setLanguage(Polish.locale);

        logger.info("exiting: Initializer.registerAppSettings()");
    }
}
