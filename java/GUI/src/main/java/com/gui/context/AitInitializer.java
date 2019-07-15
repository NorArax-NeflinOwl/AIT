package com.gui.context;

import com.gui.managers.AitCultureManager;
import com.gui.strings.AitPolishStrings;
import com.hbm.managers.AitLogger;
import javafx.scene.control.Alert;
import javafx.scene.control.ButtonType;

public class AitInitializer {

    private static AitInitializer ourInstance = new AitInitializer();

    public static AitInitializer getInstance() {
        return ourInstance;
    }

    private AitInitializer() {
    }

    public void registerAppSettings() {
        AitLogger.getInstance().logInfoToFile("opening: AitInitializer.registerAppSettings()");
        AitCultureManager.getInstance().setLanguage(AitPolishStrings.locale);

        AitLogger.getInstance().logInfoToFile("exiting: AitInitializer.registerAppSettings()");
    }

    public boolean testDBConnection() {
        AitLogger.getInstance().logInfoToFile("opening: AitInitializer.testDBConnection()");
        try {
            AitMainContext.getSession(true);
        } catch (Exception e){
            Alert alert = new Alert(Alert.AlertType.ERROR, "Error durling db connection!", ButtonType.OK);
            alert.show();
            return false;
        }
        AitLogger.getInstance().logInfoToFile("exiting: AitInitializer.testDBConnection()");
        return true;
    }
}
