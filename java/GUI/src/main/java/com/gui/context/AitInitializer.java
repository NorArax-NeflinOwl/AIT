package com.gui.context;

import com.gui.managers.AitCultureManager;
import com.gui.strings.AitPolishStrings;
import org.apache.log4j.Logger;

public class AitInitializer {
    private static Logger logger = Logger.getLogger(AitMainContext.class);

    private static AitInitializer ourInstance = new AitInitializer();

    public static AitInitializer getInstance() {
        return ourInstance;
    }

    private AitInitializer() {
    }

    public void registerAppSettings() {
        logger.info("opening: AitInitializer.registerAppSettings()");
        AitCultureManager.getInstance().setLanguage(AitPolishStrings.locale);

        logger.info("exiting: AitInitializer.registerAppSettings()");
    }
}
